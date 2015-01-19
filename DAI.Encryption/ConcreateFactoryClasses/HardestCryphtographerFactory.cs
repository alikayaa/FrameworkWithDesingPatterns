using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Cryptography
{
    //Abstract factory'nin uygulanışı gereği
    //İçeriğinde simetrik ve asimetrik şifreci dönen ICryphtographerAbstractFactory den inherit
    //alan bir fabrika classı yazılır.
    //Rijndael ve SHA512 algoritmaları çözülmesi çok daha zor olduğundan Bu gruba koyduk.
    //Siz de kendi gruplarınızı oluşturmak için ICryphtographerAbstractFactory sınıfından 
    //inherit alan classlar yazabilirsiniz
    public class HardestCryphtographerFactory : ICryphtographerAbstractFactory
    {
        #region Private Members

        private int keySize;//Anahtar boyutu
        private string key;//anahtar
        private CryphtographerParameter parameter;//şifreleme parametreleri

        #endregion

        #region Yapıcılar

        public HardestCryphtographerFactory(CryphtographerParameter parameter = null)
        {
            this.parameter = parameter;
        }

        public HardestCryphtographerFactory(int keySize, string key)
        {
            this.keySize = keySize;
            this.key = key;
        }

        #endregion

        public override IAsimetricCryphtographer GetAsimetricCryphtographer()
        {
            return new SHACryphtographer();//IAsimetricCryphtographer tipinden bir asimetrik dönmesi gerekir.
        }

        public override ISimetricCryphtographer GetSimetricCryphtographer()
        {
            //ISimetricCryphtographer tipinden dönmesi gerekir.
            if (parameter == null)
                return new RijndaelCryptographer(keySize, key);
            else
                return new RijndaelCryptographer(parameter);
        }
    }
}
