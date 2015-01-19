using System;
using DAI.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAI.Cryptography.Test
{
    [TestClass]
    public class ComplicateTest
    {
        [TestMethod]
        public void ComplicateAsimetricCalisiyorMu()
        {
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Complicate);
            string cipherText = manager.EncryptAsimetric("EnginBulut_AliKaya");
            string cipherText2 = manager.EncryptAsimetric("EnginBulut_AliKaya");
            Assert.AreEqual(cipherText, cipherText2);
        }

        [TestMethod]
        public void ComplicateSimetricCalisiyorMu()
        {
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Complicate);
            string cipherText = manager.EncryptSimetric("EnginBulut_AliKaya");
            string plainText = manager.DecryptSimetric(cipherText);
            Assert.AreEqual(plainText, "EnginBulut_AliKaya");
        }

        [TestMethod]
        public void ComplicateSimetrikParametreliCalisiyorMu()
        {
            CryphtographerParameter parameter = new CryphtographerParameter();
            parameter.InitVector = "1234567890123456";
            parameter.PasswordIterations = 3;
            parameter.KeySize = 256;
            parameter.Key = "123456789012345678901234567890AE";

            //defaultta rijndal geliyor
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Complicate, parameter);
            string cipherText = manager.EncryptSimetric("EnginBulut_AliKaya");
            string plainText = manager.DecryptSimetric(cipherText);
            Assert.AreEqual(plainText, "EnginBulut_AliKaya");
        }

    }
}
