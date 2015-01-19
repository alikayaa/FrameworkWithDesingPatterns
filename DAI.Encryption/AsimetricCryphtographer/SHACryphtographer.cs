using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DAI.Cryptography
{
    //SHA asimetrik şifreleme
    public class SHACryphtographer : IAsimetricCryphtographer
    {
        public override string Encrypt(string plainText)
        {
            using (SHA512 shaHash = SHA512.Create())
            {
                // string i byte array e çevir ve hash hesapla
                byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
