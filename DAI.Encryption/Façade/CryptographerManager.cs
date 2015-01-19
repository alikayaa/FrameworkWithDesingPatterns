using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Cryptography
{
    //Bu sınıf Facede tasarım kalıbına uygun olarak dış sistemin ayrıntılarını
    //developerlardan gizlemek ve bir nevi kullanım kolaylığı sağlayabilmek 
    //adına yazılmıştır.
    public class CryptographerManager
    {
        #region Private Members
        //default değerler
        private const string keyConst = "ENGINBULUT_ALIKAYA%^2678?*}{<>||";
        private const int keySizeConst = 256;
        private ICryphtographerAbstractFactory criptographerFactory;

        private IAsimetricCryphtographer AsimetricCryphtographer
        {
            get
            {
                if (criptographerFactory != null)
                    return this.criptographerFactory.GetAsimetricCryphtographer();
                return null;
            }
        }
        private ISimetricCryphtographer SimetricCryphtographer
        {
            get
            {
                if (criptographerFactory != null)
                    return this.criptographerFactory.GetSimetricCryphtographer();
                return null;
            }
        }

        #endregion

        #region Yapıcılar

        /// <summary>
        /// Varsayılan olarak Simple Cryphtographer Factory gelir=> AES Algrorithm- MD5 ikilisi
        /// </summary>
        public CryptographerManager()
        {
            this.criptographerFactory = new SimpleCryphtographerFactory(keySizeConst, keyConst);
        }
        /// <summary>
        /// Verilen parametreye göre bir sonuç döndürür.
        /// </summary>
        /// <param name="algorithmType">Şifreci tipleri</param>
        public CryptographerManager(CryptoAlgorithmType algorithmType)
        {
            if (algorithmType == CryptoAlgorithmType.Simple)
                this.criptographerFactory = new SimpleCryphtographerFactory(keySizeConst, keyConst);
            if (algorithmType == CryptoAlgorithmType.Complicate)
                this.criptographerFactory = new HardestCryphtographerFactory(keySizeConst, keyConst);
        }
        /// <summary>
        /// Hem verilen parametreye göre hem de şifreci tipine göre bir sonuç döndürür.
        /// </summary>
        /// <param name="algorithmType"></param>
        /// <param name="parameter"></param>
        public CryptographerManager(CryptoAlgorithmType algorithmType, CryphtographerParameter parameter)
        {
            if (algorithmType == CryptoAlgorithmType.Simple)
                this.criptographerFactory = new SimpleCryphtographerFactory(parameter);
            if (algorithmType == CryptoAlgorithmType.Complicate)
                this.criptographerFactory = new HardestCryphtographerFactory(parameter);
        }

        #endregion

        //Simetric şifreleme için
        public string EncryptSimetric(string plainText)
        {
            return this.SimetricCryphtographer.Encrypt(plainText);
        }
        //asimetric şifreleme için
        public string EncryptAsimetric(string plainText)
        {
            return this.AsimetricCryphtographer.Encrypt(plainText);
        }
        //Şifre çözmek için
        public string DecryptSimetric(string cipherText)
        {
            return this.SimetricCryphtographer.Decrypt(cipherText);
        }

    }
}
