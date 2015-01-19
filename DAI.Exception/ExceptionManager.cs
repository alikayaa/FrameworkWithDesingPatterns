using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAI.Exception
{
    public class ExceptionManager
    {
        //Manager sınıfının içerisinde kullanılacak olan formatter
        private IExceptionFormatter exceptionFormatter;

        internal ExceptionHandling ExHandler { 
            get 
            {
                return ExceptionHandling.getInstance();
            } 
        }
        #region Singleton

        private static ExceptionManager instance = null;
        private static object objectlock = new object();
        //yapıcıda ile dependency injection yapılır.
        private ExceptionManager(IExceptionFormatter exceptionFormatter)
        {
            this.exceptionFormatter = exceptionFormatter;
        }

        //bu metod standart double check singleton defaultta htmlformatter set eder.
        public static ExceptionManager getInstance()
        {
            if (instance == null)
            {
                lock (objectlock)
                {
                    if (instance == null)
                    {
                        instance = new ExceptionManager(new HtmlExceptionFormatter());
                    }
                }
            }
            return instance;
        }
        //aynı metodun overload edilmiş hali, kullanıcı isterse parametre olarak
        //IExceptionformatter' ı kendisi verebilir.
        public static ExceptionManager getInstance(IExceptionFormatter exceptionFormatter)
        {
            if (instance == null)
            {
                lock (objectlock)
                {
                    if (instance == null)
                    {
                        instance = new ExceptionManager(exceptionFormatter);
                    }
                }
            }
            return instance;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// web uygulamalarında bütün hatayı handle etmek için kullanılır.
        /// Gelen httprequestbase nesnesini de alarak, istenilen bilgileri kaydeder.
        /// Burası istenildiği kadar geniş yazılabilir.
        /// </summary>
        /// <param name="ex">Exception nesnemiz</param>
        /// <param name="request">Request nesnemiz</param>
        /// <param name="errorCaption">Hata başlığımız. Bunu hataları gruplamak için kullanabilrsiniz.</param>
        /// <returns>Geriye handle edilip formatlanmış hatayı döner</returns>
        public string HandleTheError(System.Exception ex, HttpRequestBase request, string errorCaption)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(errorCaption);
            sb.Append("URL:" + request.Url.ToString() + "<br />");

            string referer = "";
            if (request.UrlReferrer != null)
                referer = request.UrlReferrer.ToString();
            sb.Append("URL Referrer: " + referer);
            sb.Append("<br />");
            sb.Append(this.GetException(ex, sb.ToString()));

            return sb.ToString();
        }
        /// <summary>
        /// request olmadan handle etmek için kullanılır.
        /// </summary>
        /// <param name="ex">Exception nesnesi</param>
        /// <param name="errorCaption">Hata başlığı. Hataları gruplamak için kullanılır.</param>
        /// <returns>Geriye handle edilip formatlanmış hatayı döner.</returns>
        public string HandleTheError(System.Exception ex, string errorCaption)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(errorCaption);

            sb.Append("<br />");
            sb.Append(this.GetException(ex, sb.ToString()));

            return sb.ToString();
        }

        //client tarafında yazılan geri dönüşlü kodlarımızı bu metodu kullanarak yazarız. 
        //Bu sayede daha sadece bir kullanım olur.
        public T TryCatch<T>(Func<T> func, MethodBase methodbase, string errorDesc = null,Action<System.Exception> failCallback=null,bool throwup=true)
        {
            return
            ExHandler.SurroundWithTryCatch(func, methodbase, exceptionFormatter, errorDesc, failCallback, throwup);
        }

        //client tarafında yazılan geri dönüşü olmayan kodlarımızı bu metodu kullanarak yazarız.
        //bu sayede daha sade bir kullanım olur.
        public void TryCatch(Action method, MethodBase methodbase, string errorDesc = null, Action<System.Exception> failCallback = null, bool throwup = true)
        {
            ExHandler.SurroundWithTryCatch(method, methodbase, exceptionFormatter, errorDesc, failCallback, throwup);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Mevcut hatamız zaten formatlanarak bir exception nesnesinin içerisine
        /// innerException' ları da doldurularak yazıldığı için bu metod recursive olarak
        /// kendi kendini çağırarak tek bir hata stringi döner.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetException(System.Exception ex, string result)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(ex.Message);

                if (ex.InnerException != null)
                    return sb.Append(GetException(ex.InnerException, sb.ToString())).ToString();
                else
                    return sb.ToString();
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Hata logu alınırken hata oluştu :)", e);
            }
        }

        #endregion
    }
}
