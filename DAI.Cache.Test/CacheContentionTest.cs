using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAI.Cache.Test
{
    [TestClass]
    public class CacheContentionTest
    {
        const int TESTCOUNT = 10000;
        const int BACKGROUND_SMALL_VALUE = 1000;
        const int BACKGROUND_LARGE_VALUE = 1010;
        const int FOREGROUND_SMALL_VALUE = 0;
        const int FOREGROUND_LARGE_VALUE = 100;
        const int MAX_THREAD_LIFE = 20; // seconds

        //Bu fonksiyon maximum thread zamanı içinde cache e veri ekler
        void ForegroundAdderThread(object input)
        {
            DateTime starttime = DateTime.Now;//başlangıç zamanı alınır.

            List<int> addints = input as List<int>;
            while (addints.Count > 0 && DateTime.Now < starttime.AddSeconds(MAX_THREAD_LIFE))
            {
                while (CacheManager.Instance().Contains("default", addints[0].ToString()))
                {
                    Thread.Sleep(10);
                }
                CacheManager.Instance().Add("default", addints[0].ToString(), 0);
                addints.RemoveAt(0);
            }
            if (DateTime.Now >= starttime.AddSeconds(MAX_THREAD_LIFE))
            {
                throw new Exception("ForegroundAdderThread: Thread işini zamanında bitirmedi");
            }
        }
        //Bu fonksiyon maximum thread zamanı içinde cache den veri siler
        void ForegroundDeleterThread(object input)
        {
            DateTime starttime = DateTime.Now;

            List<int> delints = input as List<int>;

            while (delints.Count > 0 &&
                DateTime.Now < starttime.AddSeconds(MAX_THREAD_LIFE)
            )
            {
                if (CacheManager.Instance().Contains("default", delints[0].ToString()))
                {
                    CacheManager.Instance().Remove("default", delints[0].ToString());
                    delints.RemoveAt(0);
                }
                else
                {
                    Thread.Sleep(10);
                }
            }

            if (DateTime.Now >= starttime.AddSeconds(MAX_THREAD_LIFE))
            {
                throw new Exception("ForegroundDeleterThread: Thread işini zamanında bitirmedi");
            }
        }
        //Bu fonksiyon maximum thread zamanı içinde cache den veri get eder.
        void BackgroundReaderThread()
        {
            DateTime starttime = DateTime.Now;

            Random r = new Random((int)DateTime.Now.Ticks);
            int i;
            object o;
            while (true && DateTime.Now < starttime.AddSeconds(MAX_THREAD_LIFE))
            {
                i = r.Next(BACKGROUND_SMALL_VALUE, BACKGROUND_LARGE_VALUE + 1);
                o=CacheManager.Instance().Get("default", i.ToString());
            }
        }

        //Değerleri ekler
        void BackgroundWriterThread()
        {
            DateTime starttime = DateTime.Now;

            Random r = new Random((int)DateTime.Now.Ticks);
            int i;
            while (true && DateTime.Now < starttime.AddSeconds(MAX_THREAD_LIFE))
            {
                i = r.Next(BACKGROUND_SMALL_VALUE, BACKGROUND_LARGE_VALUE + 1);
                if (!CacheManager.Instance().Contains("default", i.ToString()))
                {
                    CacheManager.Instance().Add("default", i.ToString(), i);
                }
                Thread.Sleep(10);
            }
        }
        //Değerleri siler
        void BackgroundDeleterThread()
        {
            DateTime starttime = DateTime.Now;

            Random r = new Random((int)DateTime.Now.Ticks);
            int i;
            while (true && DateTime.Now < starttime.AddSeconds(MAX_THREAD_LIFE))
            {
                i = r.Next(BACKGROUND_SMALL_VALUE, BACKGROUND_LARGE_VALUE + 1);
                if (CacheManager.Instance().Contains("default", i.ToString()))
                {
                    CacheManager.Instance().Remove("default", i.ToString());
                }
                Thread.Sleep(10);
            }
        }
        //similasyonu baslatan method
        void execute(int readers, int writers, int deleters)
        {
            //gelen parametre sayısı kadar reader,writer ve deleter threadleri oluşturur.
            Random r = new Random((int)DateTime.Now.Ticks);

            // Test için rastgele sayılar üretilir
            List<int> addints = new List<int>();
            List<int> delints = new List<int>();

            // Rastgele sayılar cache e eklenir
            CacheManager.Instance().AddContext("lists");
            CacheManager.Instance().Add("lists", "addints", addints);
            CacheManager.Instance().Add("lists", "delints", delints);

            for (int i = 0; i < TESTCOUNT; i++)
            {
                int add = r.Next(FOREGROUND_SMALL_VALUE, FOREGROUND_LARGE_VALUE);
                addints.Add(add);
                delints.Add(add);
            }

            Thread[] readerthreads = new Thread[readers];
            for (int i = 0; i < readers; i++)
            {
                readerthreads[i] = new Thread(new ThreadStart(BackgroundReaderThread));
                readerthreads[i].Start();
            }

            Thread[] writerthreads = new Thread[writers];
            for (int i = 0; i < writers; i++)
            {
                writerthreads[i] = new Thread(new ThreadStart(BackgroundWriterThread));
                writerthreads[i].Start();
            }

            Thread[] deleterthreads = new Thread[deleters];
            for (int i = 0; i < deleters; i++)
            {
                deleterthreads[i] = new Thread(new ThreadStart(BackgroundDeleterThread));
                deleterthreads[i].Start();
            }


            new Thread(ForegroundAdderThread).Start(addints);
            new Thread(ForegroundDeleterThread).Start(delints);

            DateTime starttime = DateTime.Now;

            // Ekleyici thread in işinin bitmesi için izin verilir.
            while (addints.Count > 0 && DateTime.Now < starttime.AddSeconds(MAX_THREAD_LIFE))
            {
                Thread.Sleep(10);
            }

            // Silici thread e işinin bitmesi için izin verilir.
            while (delints.Count > 0 && DateTime.Now < starttime.AddSeconds(MAX_THREAD_LIFE))
            {
                Thread.Sleep(10);
            }
            if (DateTime.Now < starttime.AddSeconds(MAX_THREAD_LIFE))
            {
                for (int i = 0; i < readers; i++) { readerthreads[i].Abort(); }
                for (int i = 0; i < writers; i++) { writerthreads[i].Abort(); }
                for (int i = 0; i < deleters; i++) { deleterthreads[i].Abort(); }

                if (addints.Count > 0)
                {
                    throw new Exception("Ana Thread: addints.Count > 0 after " + MAX_THREAD_LIFE.ToString() + " seconds.");
                }

            }

            bool found = false; string baz = "";
            for (int i = FOREGROUND_SMALL_VALUE; i < FOREGROUND_LARGE_VALUE; i++)
            {
                if (CacheManager.Instance().Contains("default", i.ToString()))
                {
                    if (!found)
                    {
                        found = true;
                    }
                    baz += i.ToString() + ",";
                }
            }

            for (int i = 0; i < readers; i++) { readerthreads[i].Abort(); }
            for (int i = 0; i < writers; i++) { writerthreads[i].Abort(); }
            for (int i = 0; i < deleters; i++) { deleterthreads[i].Abort(); }
        }


        [TestMethod]
        public void AyniAndaBirerOkumaYazmaSilmeIslemiOldugundaDogruCalisiyorMu()
        {
            const int READERTHREADS = 1;
            const int WRITERTHREADS = 1;
            const int DELTETERTHREADS = 1;

            execute(READERTHREADS, WRITERTHREADS, DELTETERTHREADS);
        }

        [TestMethod]
        public void AyniAndaBeserAdetOkumaYazmaSilmeIslemiOldugundaDogruCalisiyorMu()
        {
            const int READERTHREADS = 5;
            const int WRITERTHREADS = 5;
            const int DELTETERTHREADS = 5;

            execute(READERTHREADS, WRITERTHREADS, DELTETERTHREADS);
        }





    }
}
