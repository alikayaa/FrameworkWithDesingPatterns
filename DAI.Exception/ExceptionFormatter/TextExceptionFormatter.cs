using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Exception
{
    //yakalanan hata geriye string olarak handle edilmek istendiğinde
    //kullanılması gereken sınıf
    public class TextExceptionFormatter : IExceptionFormatter
    {
        /// <summary>
        /// Kodun patladığı yolu formatlamak için
        /// </summary>
        /// <param name="methodbase">Çağıran metod bilgisi. </param>
        /// <param name="errorDesc">Hata için bizim açıklamamız</param>
        /// <param name="ex">Hata sınıfı</param>
        /// <param name="exType">Hata tipi</param>
        /// <returns>Geriye formatlanmış hatayı döner</returns>
        public string FormatException(MethodBase methodbase, string errorDesc, System.Exception ex, ExceptionTypes exType)
        {
            //istenildiği takdirde hatanın daha derin detayları burada tek bir noktadan 
            //eklenebilir veya formatlanabilir.
            string formattedException = string.Format(@"
                                        Namaspace: {0}, \n
                                        Sınıf: {1} ,\n
                                        Metod: {2} ,\n
                                        Exception Type: {3} ,\n 
                                        Message: {4} , \n
                                        Our Details: {5} ",
                            methodbase.ReflectedType.Namespace, methodbase.ReflectedType.Name, methodbase.Name, exType.ToString(), ex.Message, errorDesc == null ? "Yok" : errorDesc);
            return formattedException;
        }

        /// <summary>
        /// Kodun patladığı yeri formatlamak için
        /// </summary>
        /// <param name="methodbase">Çağıran metod bilgisi. </param>
        /// <param name="errorDesc">Hata için bizim açıklamamız</param>
        /// <param name="ex">Hata sınıfı</param>
        /// <param name="exType">Hata tipi</param>
        /// <returns>Geriye formatlanmış hatayı döner</returns>
        public string FormatExceptionThrowCatch(MethodBase methodbase, string errorDesc, System.Exception ex, ExceptionTypes exType)
        {
            //istenildiği takdirde hatanın daha derin detayları burada tek bir noktadan 
            //eklenebilir veya formatlanabilir.
            string formattedException = string.Format(@"
                            Sınıf: {0} ,\n
                            Metod: {1} ,\n
                            Exception Type: {2} ,\n 
                            Our Details: {3}",
                 methodbase.ReflectedType.Name, methodbase.Name, exType.ToString(), errorDesc == null ? "Yok" : errorDesc);
            return formattedException;
        }
    }
}
