using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Exception
{
    //yakalanan hata html olarak handle edilmek istendiğinde 
    //bu sınıf kullanılmalıdır
    public class HtmlExceptionFormatter : IExceptionFormatter
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
                                        <div style='margin-left:30px; border:1px solid black; background:yellow;color:red;'><br />
                                        [
                                        <br /><b>Namaspace:</b> {0} 
                                        <br /><b>Sınıf:</b> {1} 
                                        <br /> <b>Metod:</b> {2} 
                                        <br /> <b>Exception Type:</b> {3} 
                                        <br /> <b>Message:</b> {4} 
                                        <br /> <b>Our Details:</b> {5} <br />]<br /><div>",
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
            string formattedException = string.Format(@"<div style='margin-left:30px; border:1px solid black;background:yellow;color:red;'><br />
                                        [
,<br /><b>Sınıf:</b> {0} <br /> <b>Metod:</b> {1} 
                                        <br /> <b>Exception Type:</b> {2} <br /> <b>Our Details:</b> {3}<br />]<br /><div>",
                 methodbase.ReflectedType.Name, methodbase.Name, exType.ToString(), errorDesc == null ? "Yok" : errorDesc);
            return formattedException;
        }
    }
}
