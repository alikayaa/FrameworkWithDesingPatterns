using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAI.Cache.Test
{
    [TestClass]
    public class CacheContextTest
    {
        #region Contexti manipule eden fonksiyonların testi

        [TestMethod]
        public void ContextiEkleyipSilmeBasariliMi()
        {
            Assert.IsTrue(CacheManager.Instance().AddContext("Context"));
            Assert.IsTrue(CacheManager.Instance().AddContext("Context2"));

            Assert.IsTrue(CacheManager.Instance().ContainsContext("Context"));
            Assert.IsTrue(CacheManager.Instance().ContainsContext("Context2"));

            Assert.IsTrue(CacheManager.Instance().RemoveContext("Context"));
            Assert.IsTrue(CacheManager.Instance().RemoveContext("Context2"));

            Assert.IsFalse(CacheManager.Instance().ContainsContext("Context"));
            Assert.IsFalse(CacheManager.Instance().ContainsContext("Context2"));
        }

        [TestMethod]
        public void OlmayanBirContextiSilmeyeCalismakFalseCiktiVeriyorMu()
        {
            Assert.IsFalse(CacheManager.Instance().RemoveContext("Context"));
        }

        [TestMethod]
        public void MukerrerContextEklenmeyeCalisildigindaFalseCiktiVeriyorMu()
        {
            Assert.IsTrue(CacheManager.Instance().ContainsContext("default"));//default kafadan var
            Assert.IsFalse(CacheManager.Instance().AddContext("default"));//aynı isimde bi daha eklenmeye çalışıldığında eklememeli
        }

        [TestMethod]
        public void OlmayanBirContextSilinmeyeCalisildigindaFalseDonuyorMu()
        {
            Assert.IsFalse(CacheManager.Instance().ClearContext("nonexistent-context"));
        }



        #endregion

        #region Utility functions
        private void Init()
        {
            if (!CacheManager.Instance().ContainsContext("Context"))
            {
                CacheManager.Instance().AddContext("Context");
            }
            else
            {
                CacheManager.Instance().ClearContext("Context");
            }
        }
        #endregion

        #region Datayı manipüle eden fonksiyonların testi

        [TestMethod]
        public void EkleSilGuncelleIslemleriDuzgunCalisiyorMu()
        {
            Init();
            Assert.IsTrue(CacheManager.Instance().Add("Context", "testkey", "testvalue"));

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.AreEqual("testvalue", CacheManager.Instance().Get("Context", "testkey"));

            Assert.IsTrue(CacheManager.Instance().Update("Context", "testkey", "testvalue2"));

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.AreEqual("testvalue2", CacheManager.Instance().Get("Context", "testkey"));

            Assert.IsTrue(CacheManager.Instance().Remove("Context", "testkey"));
            Assert.IsFalse(CacheManager.Instance().Contains("Context", "testkey"));
        }

        [TestMethod]
        public void SonTarihBelirtilerekEklenmisBirCacheGerekliZamanGecinceSiliniyorMu()
        {
            Init();
            Assert.IsTrue(CacheManager.Instance().Add("Context", "testkey", "testvalue", DateTime.Now.AddMilliseconds(100)));

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.AreEqual("testvalue", CacheManager.Instance().Get("Context", "testkey"));

            //gerekli süre beklenildiğinde silinecek mi testi
            System.Threading.Thread.Sleep(1101);

            Assert.IsFalse(CacheManager.Instance().Contains("Context", "testkey"));
        }

        [TestMethod]
        public void MaksimumBellekteDurmaSuresiBelirtilerekEklenmisBirCacheGerekliZamanGecinceSiliniyorMu()
        {
            Init();

            Assert.IsTrue(CacheManager.Instance().Add("Context", "testkey", "testvalue", TimeSpan.FromMilliseconds(100)));

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.AreEqual("testvalue", CacheManager.Instance().Get("Context", "testkey"));

            System.Threading.Thread.Sleep(1101);

            Assert.IsFalse(CacheManager.Instance().Contains("Context", "testkey"));

            Assert.IsTrue(CacheManager.Instance().Add("Context", "testkey", "testvalue", TimeSpan.FromMilliseconds(1100)));

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));

            System.Threading.Thread.Sleep(1000);

            CacheManager.Instance().Get("Context", "testkey");

            System.Threading.Thread.Sleep(1000);

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));

        }

        [TestMethod]
        public void SonTarihVerilierekEkleVeGuncelleIslemleriCalisiyorMu()
        {
            Init();
            Assert.IsTrue(CacheManager.Instance().Add("Context", "testkey", "testvalue", DateTime.Now.AddMilliseconds(1100)));

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.AreEqual("testvalue", CacheManager.Instance().Get("Context", "testkey"));

            System.Threading.Thread.Sleep(1000);

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.IsTrue(CacheManager.Instance().Update("Context", "testkey", "testvalue2"));
            Assert.AreEqual("testvalue2", CacheManager.Instance().Get("Context", "testkey"));

        }

        [TestMethod]
        public void MaksimumBellekteDurmaSuresiBelirtilerekOlusturulanCacheIcinEkleVeGuncelleCalisiyorMu()
        {
            Init();
            Assert.IsTrue(CacheManager.Instance().Add("Context", "testkey", "testvalue", TimeSpan.FromMilliseconds(1100)));

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.AreEqual("testvalue", CacheManager.Instance().Get("Context", "testkey"));

            System.Threading.Thread.Sleep(1000);

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.IsTrue(CacheManager.Instance().Update("Context", "testkey", "testvalue2"));
            Assert.AreEqual("testvalue2", CacheManager.Instance().Get("Context", "testkey"));

            System.Threading.Thread.Sleep(1000);

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
        }

        [TestMethod]
        public void CachedeOlupOlmadigininKontrolunuYapanFonksiyonIslevselMi()
        {
            Init();
            Assert.IsTrue(CacheManager.Instance().Add("Context", "key", "value"));
            Assert.IsTrue(CacheManager.Instance().Contains("Context", "key"));
            Assert.IsTrue(CacheManager.Instance().Remove("Context", "key"));
            Assert.IsFalse(CacheManager.Instance().Contains("Context", "key"));
        }

        [TestMethod]
        public void EklemeVeyaGuncellemeBasariliBirSekildeYapiliyorMu()
        {
            Init();
            Assert.IsTrue(CacheManager.Instance().AddOrUpdate("Context", "testkey", "testvalue"));
            Assert.AreEqual("testvalue", CacheManager.Instance().Get("Context", "testkey"));
            Assert.IsFalse(CacheManager.Instance().AddOrUpdate("Context", "testkey", "testvalue2"));
            Assert.AreEqual("testvalue2", CacheManager.Instance().Get("Context", "testkey"));
            Assert.IsTrue(CacheManager.Instance().Remove("Context", "testkey"));
        }

        [TestMethod]
        public void SonTarihVerilerekEklemeVeyaGuncellemeBasariliBirSekildeCalisiyorMu()
        {
            Init();
            Assert.IsTrue(CacheManager.Instance().AddOrUpdate("Context", "testkey", "testvalue", DateTime.Now.AddYears(1)));

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.AreEqual("testvalue", CacheManager.Instance().Get("Context", "testkey"));

            System.Threading.Thread.Sleep(1000);

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.IsFalse(CacheManager.Instance().AddOrUpdate("Context", "testkey", "testvalue", DateTime.Now.AddMilliseconds(1)));

            System.Threading.Thread.Sleep(1010);
            Assert.IsFalse(CacheManager.Instance().Contains("Context", "testkey"));
        }

        [TestMethod]
        public void MaksimumBellekteDurmaSuresiBelirtilerekEklemeVeyaGuncellemeCalisiyorMu()
        {
            Init();
            Assert.IsTrue(CacheManager.Instance().AddOrUpdate("Context", "testkey", "testvalue", TimeSpan.FromMilliseconds(1100)));

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.AreEqual("testvalue", CacheManager.Instance().Get("Context", "testkey"));

            System.Threading.Thread.Sleep(1000);

            Assert.IsTrue(CacheManager.Instance().Contains("Context", "testkey"));
            Assert.IsFalse(CacheManager.Instance().AddOrUpdate("Context", "testkey", "testvalue2", TimeSpan.FromMilliseconds(1)));
            Assert.AreEqual("testvalue2", CacheManager.Instance().Get("Context", "testkey"));

            System.Threading.Thread.Sleep(1100);

            Assert.IsFalse(CacheManager.Instance().Contains("Context", "testkey"));
        }

        [TestMethod]
        public void GetIslemiCailisyorMu()
        {
            Init();
            Assert.IsTrue(CacheManager.Instance().Add("Context", "key", "value"));
            Assert.AreEqual(CacheManager.Instance().Get("Context", "key"), "value");
            CacheManager.Instance().Remove("Context", "key");
        }

        #endregion

        #region Beklenen hataların testi
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void OlmayanBirContexteVeriEklemeyeCalisildigindaGerekliHataVeriliyorMu()
        {
            CacheManager.Instance().Add("olmayan-context", "key", "value");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void OlmayanBirContexteVeriEklemeyeCalisildigindaGerekliHataVeriliyorMu2()
        {
            CacheManager.Instance().Add("nonexistent-context", "key", "value", DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void OlmayanBirContexteVeriEklemeyeCalisildigindaGerekliHataVeriliyorMu3()
        {
            CacheManager.Instance().Add("nonexistent-context", "key", "value", TimeSpan.FromMilliseconds(10));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void OlmayanBirContexteVeriEklemeyeVeyaGuncellenmeyeCalisildigindaGerekliHataVeriliyorMu()
        {
            CacheManager.Instance().AddOrUpdate("nonexistent-context", "key", "value");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void OlmayanBirContexteVeriEklemeyeVeyaGuncellenmeyeCalisildigindaGerekliHataVeriliyorMu2()
        {
            CacheManager.Instance().AddOrUpdate("nonexistent-context", "key", "value", DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void OlmayanBirContexteVeriEklemeyeVeyaGuncellenmeyeCalisildigindaGerekliHataVeriliyorMu3()
        {
            CacheManager.Instance().AddOrUpdate("nonexistent-context", "key", "value", TimeSpan.FromMilliseconds(10));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void OlmayanBirContextteVarlikYoklukKontroluYapildigindaGerekliHataVeriliyorMu()
        {
            CacheManager.Instance().Contains("nonexistent-context", "key");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void OlmayanBirContextteGetIslemiYapildigindaGerekliHataVeriliyorMu()
        {
            CacheManager.Instance().Get("nonexistent-context", "key");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void OlmayanBirContexttenHerhangiBirDegerSilinmeyeCalisildigiZamanGerekliHataVeriliyorMu()
        {
            CacheManager.Instance().Remove("nonexistent-context", "key");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void OlmayanBirContextiUpdateEtmeyeCalistigimizZamanGerekliHataVeriliyorMu()
        {
            CacheManager.Instance().Update("nonexistent-context", "key", "value");
        }
        #endregion
    }
}
