using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CacheFramework
{
    public abstract class IExpritionLeoCache
    {

        private System.Timers.Timer timer = new System.Timers.Timer(TimeSpan.FromSeconds(1.0).TotalMilliseconds);

        public IExpritionLeoCache()
        {
            //timer tetikleniyor
            timer.Elapsed += new System.Timers.ElapsedEventHandler(PurgeCache);
            timer.Start();
        }

        
        public bool Add(object key, object value,DateTime expritionDate)
        {
            return
            WriterLockOperations(() =>
            {
                return AddCache(key, value, expritionDate);
            });
        }

        public bool Remove(object key)
        {
            return
            WriterLockOperations(() =>
            {
                return RemoveCache(key);
            });
        }

        public object Get(object key)
        {
            return
            ReaderLockOperations(() =>
            {
                return GetCache(key);
            });
        }

        protected abstract void PurgeCache(object sender, ElapsedEventArgs e);
        protected abstract bool AddCache(object key, object value, DateTime expritionDate);
        protected abstract bool RemoveCache(object key);
        protected abstract object GetCache(object key);
        public abstract object this[object key,DateTime exprition] { get; set; }


        #region Lock Operations
        /// <summary>
        /// thread safety için. Yani multiple process kullanımlarda örneğin; web. Veri bütünlüğünün sağlanması için lock lanır.
        /// O sırada kullanılan değişken
        /// </summary>
        private ReaderWriterLock readWriteLock = new ReaderWriterLock();
        //Thread leri kilitleme işlemi en fazla kaç saniye sürecek
        private const int MAX_LOCK_WAIT = 5000; // milliseconds

        protected T WriterLockOperations<T>(Func<T> func)
        {
            bool LockUpgraded = readWriteLock.IsReaderLockHeld;
            LockCookie lc = new LockCookie();
            if (LockUpgraded)
                lc = readWriteLock.UpgradeToWriterLock(MAX_LOCK_WAIT);
            else
                readWriteLock.AcquireWriterLock(MAX_LOCK_WAIT);

            try
            {
                return func();
            }
            finally
            {
                //lock durumunu eski haline getir.
                if (LockUpgraded)
                {
                    readWriteLock.DowngradeFromWriterLock(ref lc);
                }
                else
                {
                    readWriteLock.ReleaseWriterLock();
                }
            }
        }

        protected T ReaderLockOperations<T>(Func<T> func)
        {
            readWriteLock.AcquireReaderLock(MAX_LOCK_WAIT);//reader ı lockla
            try
            {
                return func();
            }
            finally
            {
                //lock durumunu eski haline getir.
                readWriteLock.ReleaseReaderLock();
            }
        }

        #endregion

    }
}
