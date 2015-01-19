using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Cryptography
{
    //MD5 asimetrik şifreleme 
    public class MD5Cryptographer : IAsimetricCryphtographer
    {
        public override string Encrypt(string plainText)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // string i byte array e çevir ve hash hesapla
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));
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
