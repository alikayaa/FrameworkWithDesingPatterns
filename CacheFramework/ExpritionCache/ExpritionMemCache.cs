using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CacheFramework
{
    public class ExpritionMemCache:IExpritionLeoCache
    {
        protected SortedDictionary<CacheKey, object> timedStorage = new SortedDictionary<CacheKey, object>();
        protected Dictionary<object, CacheKey> timedStorageIndex = new Dictionary<object, CacheKey>();


        object isPurging = new object();

        protected override void PurgeCache(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReaderWriterLock readWriteLock = new ReaderWriterLock();
            //Öncelik olarak ortanın altında seçiyoruz. Çünkü öncelik ekleme, güncelleme gibi işlerde.
            //Varsın bi kaç saniye geç temizlensin cache
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
            if (!Monitor.TryEnter(isPurging))
                return;

            try
            {
                readWriteLock.AcquireWriterLock(500);
                try
                {
                    List<object> expiredItems = new List<object>();

                    foreach (CacheKey timedKey in timedStorage.Keys)
                    {
                        if (timedKey.ExpirationDate < e.SignalTime)
                        {
                            // Temizlenmesi için birnevi nesneler işaretlenir.
                            expiredItems.Add(timedKey.Key);
                        }
                        else
                            break;
                    }

                    foreach (object key in expiredItems)
                    {
                        CacheKey timedKey = timedStorageIndex[key];
                        timedStorageIndex.Remove(timedKey.Key);
                        timedStorage.Remove(timedKey);
                    }
                }
                catch (ApplicationException ae)
                {
                    System.Console.WriteLine("Şu an lock lı.");
                }
                finally
                {
                    readWriteLock.ReleaseWriterLock();
                }
            }
            finally { Monitor.Exit(isPurging); }
        }

        protected override bool AddCache(object key, object value, DateTime expritionDate)
        {
            if (timedStorageIndex.ContainsKey(key))
                return false;
            else//toksa time storage olanlara ekle ve true dön
            {
                CacheKey internalKey = new CacheKey(key, expritionDate);
                timedStorage.Add(internalKey, value);//daha sonra bu oluşturduğumuz yeni key nesnesiyle ekleriz
                timedStorageIndex.Add(key, internalKey);
                return true;
            }
        }

        protected override bool RemoveCache(object key)
        {
            if (timedStorageIndex.ContainsKey(key))//timed storage da varsa sil ve true dön
            {
                timedStorage.Remove(timedStorageIndex[key]);
                timedStorageIndex.Remove(key);
                return true;
            }
            else//ikisinde de yoksa false dön
                return false;
        }

        protected override object GetCache(object key)
        {
            object o;
            if (timedStorageIndex.ContainsKey(key))//time storage da varsa 
            {
                CacheKey tkey = timedStorageIndex[key];//ilgili key in CacheKey propertisini al.
                o = timedStorage[tkey];//ondaki değeri ayrı bir değişkene at
                timedStorage.Remove(tkey);//time storage dan at.
                tkey.Accessed();//key e erişildi işaretini ekle
                timedStorage.Add(tkey, o);//bu keyle tekrar time storagelara ekleme yap
                return o;//time storage dan alınıp değişkene atılan değeri dön
            }
            else return null;
        }

        public override object this[object key,DateTime exprition]
        {
            get
            {
                return GetCache(key);
            }
            set
            {
                Add(key,value, exprition);
            }
        }

    }
}
