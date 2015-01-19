using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Cryptography
{
    //AES simetrik şifreleme 
    public class AESCryptographer : ISimetricCryphtographer
    {
        #region Private variables
        //buradaki değişkenler AES şifreleme algoritması uygulanırken kullanılacaktır.
        private int BlockBitSize;
        private int SaltBitSize;
        private int Iterations;
        private int MinPasswordLength;
        private byte[] cryptKey;
        private byte[] authKey;

        #endregion

        #region Yapıcı metodlar

        /// <summary>
        /// AES algoritması default yapıcı
        /// </summary>
        /// <param name="keySize">Anahtar Boyutu (256 bit veya 128 bit olmalı)</param>
        /// <param name="key">Şifrelemede kullanılacak anahtar</param>
        internal AESCryptographer(int keySize, string key)
            : base(keySize, key)//default values
        {
            //default değerler atanır
            this.BlockBitSize = 128;
            this.SaltBitSize = 64;
            this.Iterations = 10000;
            this.MinPasswordLength = 12;

            this.cryptKey = Encoding.UTF8.GetBytes(key);
            this.authKey = Encoding.UTF8.GetBytes(key);
        }
        /// <summary>
        /// AES algoritması parametreli yapıcı
        /// </summary>
        /// <param name="keySize">Anahtar Boyutu (256 bit veya 128 bit olmalı)</param>
        /// <param name="parameter">Algoritmanın parametrik değerleri</param>
        internal AESCryptographer(CryphtographerParameter parameter)
            : base(parameter.KeySize, parameter.Key)
        {
            //değerler initialize edilir
            this.BlockBitSize = parameter.BlockBitSize;
            this.SaltBitSize = parameter.SaltBitSize;
            this.Iterations = parameter.Iterations;
            this.MinPasswordLength = parameter.MinPasswordLength;
            this.cryptKey = Encoding.UTF8.GetBytes(parameter.Key);//bakılacak
            this.authKey = Encoding.UTF8.GetBytes(parameter.Key);//bakılcak
        }


        #endregion

        /// <summary>
        /// AES Basit Şifreleme
        /// </summary>
        /// <param name="secretMessage">Şifrelenecek mesaj</param>
        /// <param name="cryptKey">Şifreleme anahtarı</param>
        /// <param name="authKey">Kimlik anahtarı</param>
        /// <returns>
        /// Şifreli mesaj
        /// </returns>
        /// <exception cref="System.ArgumentException">Şifrelenecek mesaj olmak zorunda</exception>
        public override string Encrypt(string plainText)//UTF 8 plain text
        {
            #region Kontroller
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Şifrelenecek mesajı giriniz!", "şifrelecekMesaj");
            //girilen plainText byte array e çevirilir.
            var byteArray = Encoding.UTF8.GetBytes(plainText);

            //Kullanıcı hata kontrolleri yapılır
            if (cryptKey == null || cryptKey.Length != base.GetKeySize() / 8)
                throw new ArgumentException(String.Format("Şifreci Anahtar {0} bit olmalı!", base.GetKeySize()), "cryptKey");

            if (authKey == null || authKey.Length != base.GetKeySize() / 8)
                throw new ArgumentException(String.Format("Yetki anahtarı {0} bit olmalı!", base.GetKeySize()), "authKey");

            if (byteArray == null || byteArray.Length < 1)
                throw new ArgumentException("Şifrelenecek mesajı giriniz!", "şifrelecekMesaj");

            #endregion

            byte[] cipherText;//byte array tipinden şifreli metnin tutulacağı değişken
            byte[] iv;

            using (var aes = new AesManaged
            {
                KeySize = base.GetKeySize(),
                BlockSize = BlockBitSize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            })
            {

                //Simetrik algoritmayı kullanmak için rastgele IV üretilir
                aes.GenerateIV();
                iv = aes.IV;

                using (var encrypter = aes.CreateEncryptor(cryptKey, iv))//aes şifreci oluşturulur.
                using (var cipherStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write))
                    using (var binaryWriter = new BinaryWriter(cryptoStream))
                    {
                        binaryWriter.Write(plainText);
                    }
                    cipherText = cipherStream.ToArray();//burada şifreli mesaj oluşturulur
                }

            }

            //Şifreli mesajı oluşturmak ve kimlik eklemek authentication
            using (var hmac = new HMACSHA256(authKey))
            using (var encryptedStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(encryptedStream))
                {
                    binaryWriter.Write(new byte[] { });
                    binaryWriter.Write(iv);
                    binaryWriter.Write(cipherText);
                    binaryWriter.Flush();

                    //Tüm verilerin kimlik doğrulamasını yap
                    var tag = hmac.ComputeHash(encryptedStream.ToArray());
                    binaryWriter.Write(tag);
                }
                return Convert.ToBase64String(encryptedStream.ToArray());
            }

        }

        /// <summary>
        /// AES Basti Şifre Çözme
        /// </summary>
        /// <param name="encryptedMessage">Şifreli mesaj</param>
        /// <param name="cryptKey">Şifreleme anahtarı</param>
        /// <param name="authKey">Kimlik anahtarı</param>
        /// <returns>
        /// Çözülmüş metin
        /// </returns>
        public override string Decrypt(string encryptedText)
        {
            #region Parametre Kontrolü
            if (string.IsNullOrWhiteSpace(encryptedText))
                throw new ArgumentException("Şifreli mesaj gerekli", "sifreliMesaj");

            var cipherText = Convert.FromBase64String(encryptedText);

            //Basit kullanım hata kontrolleri
            if (cryptKey == null || cryptKey.Length != base.GetKeySize() / 8)
                throw new ArgumentException(String.Format("Şifreci anahtar {0} bit olmalı!", base.GetKeySize()), "cryptKey");

            if (authKey == null || authKey.Length != base.GetKeySize() / 8)
                throw new ArgumentException(String.Format("Kimlik anahtarı {0} bit olmalı!", base.GetKey()), "authKey");

            if (cipherText == null || cipherText.Length == 0)
                throw new ArgumentException("Şifreli mesaj gerekli", "sifreliMesaj");
            #endregion

            //algoritmayı kullanarak şifreli olan metnin çözülmesi
            using (var hmac = new HMACSHA256(authKey))
            {
                var sentTag = new byte[hmac.HashSize / 8];

                var calcTag = hmac.ComputeHash(cipherText, 0, cipherText.Length - sentTag.Length);
                var ivLength = (BlockBitSize / 8);

                if (cipherText.Length < sentTag.Length + ivLength)
                    return null;

                Array.Copy(cipherText, cipherText.Length - sentTag.Length, sentTag, 0, sentTag.Length);

                var compare = 0;
                for (var i = 0; i < sentTag.Length; i++)
                    compare |= sentTag[i] ^ calcTag[i];

                if (compare != 0)
                    return null;

                using (var aes = new AesManaged
                {
                    KeySize = base.GetKeySize(),
                    BlockSize = BlockBitSize,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                })
                {

                    var iv = new byte[ivLength];
                    Array.Copy(cipherText, 0, iv, 0, iv.Length);

                    using (var decrypter = aes.CreateDecryptor(cryptKey, iv))
                    using (var plainTextStream = new MemoryStream())
                    {
                        using (var decrypterStream = new CryptoStream(plainTextStream, decrypter, CryptoStreamMode.Write))
                        using (var binaryWriter = new BinaryWriter(decrypterStream))
                        {
                            binaryWriter.Write(
                              cipherText,
                              iv.Length,
                              cipherText.Length - iv.Length - sentTag.Length
                            );
                        }
                        string plainText = Encoding.UTF8.GetString(plainTextStream.ToArray());
                        return plainText.Substring(1, plainText.Length - 1);
                    }
                }
            }
        }
    }
}
