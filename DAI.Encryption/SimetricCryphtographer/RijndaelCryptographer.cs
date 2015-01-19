using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Cryptography
{
    //Rijndael simetrik şifreleme
    public class RijndaelCryptographer : ISimetricCryphtographer
    {
        #region Private Variables

        private string initVector; // 16 byte olmalı
        private string saltValue; // herhangi bir string değer olabilir
        private string passPhrase;  // herhangi bir string değer olabilir
        private const string hashAlgorithm = "SHA1"; //Değiştirilemez
        private int passwordIterations; // herhangi bir numara olabilir
        private const string _pwd = "ICBRG3618TRH(/";

        #endregion

        #region Yapıcılar

        /// <summary>
        /// Rijndael algorithm spesification
        /// </summary>
        /// <param name="keySize">Şifreci anahtar boyutu (192, 128 ,256) bitleri</param>
        /// /// <param name="key">Anahtar</param>
        /// <param name="keySize">Anahtarın boyutu</param>
        public RijndaelCryptographer(int keySize, string key)//key size must be 192 or 128 or 256
            : base(keySize, key)
        {
            this.initVector = "ICBRG199VAN(,)%t";
            this.saltValue = base.GetKey();
            this.passPhrase = base.GetKey() + "*!'";
            this.passwordIterations = 2;
        }

        /// <summary>
        /// Rijndael algorithm spesification
        /// </summary>
        /// <param name="parameters">Rijndael şifreleme algoritması parametreleri</param>
        public RijndaelCryptographer(CryphtographerParameter parameters)
            : base(parameters.KeySize, parameters.Key)
        {
            this.initVector = parameters.InitVector;
            this.saltValue = base.GetKey();
            this.passPhrase = base.GetKey() + "*!'";
            this.passwordIterations = parameters.PasswordIterations;
        }

        #endregion

        /// <summary>
        /// Rijndael algotimasıyla şifreleme
        /// </summary>
        /// <param name="plainText">Şifrelenecek metin</param>
        /// <returns>Şifreli metin</returns>
        public override string Encrypt(string plainText)
        {
            //Şifreleme algoritmasının uygulanışı
            string cipherText = "";

            try
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                byte[] keyBytes = password.GetBytes(base.GetKeySize() / 8);

                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                MemoryStream memoryStream = new MemoryStream();

                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                cryptoStream.FlushFinalBlock();

                byte[] cipherTextBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();

                cipherText = Convert.ToBase64String(cipherTextBytes);
            }
            catch { }

            return cipherText;
        }

        /// <summary>
        /// Şifreli metni çözer
        /// </summary>
        /// <param name="textToDecrypt">Çözülecek metin</param>
        /// <returns>Orjinal metin</returns>
        public override string Decrypt(string textToDecrypt)
        {
            //Rijndael algoritmasına göre şifreyi çözme
            string plainText = "";
            try
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                byte[] cipherTextBytes = Convert.FromBase64String(textToDecrypt);

                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                byte[] keyBytes = password.GetBytes(base.GetKeySize() / 8);

                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                memoryStream.Close();
                cryptoStream.Close();

                plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);


            }
            catch { }

            return plainText;

        }
    }
}
