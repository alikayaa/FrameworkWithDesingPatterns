using System;
using DAI.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAI.Cryptography.Test
{
    [TestClass]
    public class SimpleTest
    {
        [TestMethod]
        public void SimpleAsimetricCalisiyorMu()
        {
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Simple);
            string cipherText = manager.EncryptAsimetric("EnginBulut_AliKaya");
            string cipherText2 = manager.EncryptAsimetric("EnginBulut_AliKaya");
            Assert.AreEqual(cipherText, cipherText2);
        }

        [TestMethod]
        public void SimpleSimetricCalisiyorMu()
        {
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Simple);
            string cipherText = manager.EncryptSimetric("EnginBulut_AliKaya");
            string plainText = manager.DecryptSimetric(cipherText);
            Assert.AreEqual(plainText, "EnginBulut_AliKaya");
        }

        [TestMethod]
        public void SimpleSimetricParametreliCalisiyorMu()
        {
            //parameterik aes
            CryphtographerParameter parameter = new CryphtographerParameter();
            parameter.BlockBitSize = 128;
            parameter.Iterations = 40;
            parameter.MinPasswordLength = 12;
            parameter.SaltBitSize = 128;
            parameter.Key = "123456789012345678901234567890AE";
            parameter.KeySize = 256;

            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Simple, parameter);
            string cipherText = manager.EncryptSimetric("EnginBulut_AliKaya");
            string plainText = manager.DecryptSimetric(cipherText);
            Assert.AreEqual(plainText, "EnginBulut_AliKaya");
        }
    }
}
