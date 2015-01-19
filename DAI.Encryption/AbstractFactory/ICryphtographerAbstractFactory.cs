using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Cryptography
{
    //Abstract Factory Tasarım Deseni Kullanılmıştır.
    //Şifreleme tipleri Simetrik ve Asimetrik olmak üzere
    //iki şifreci grubu olarak düşünülmüştür.
    public abstract class ICryphtographerAbstractFactory
    {
        public abstract IAsimetricCryphtographer GetAsimetricCryphtographer();
        public abstract ISimetricCryphtographer GetSimetricCryphtographer();
    }
}
