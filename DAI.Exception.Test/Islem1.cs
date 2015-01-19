using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Exception.Test
{
    public static class Islem1
    {
        public static void IslemYap()
        {
            ExceptionManager.getInstance().TryCatch(() =>
            {
                int a = 1;
                int b = 2;
                int c = a + b;
                string d = IslemYap2(c);
            }, MethodBase.GetCurrentMethod(), "Benim hata mesajım");
        }

        public static string IslemYap2(int c)
        {
            return
            ExceptionManager.getInstance().TryCatch(() =>
            {
                return IslemYap3(c *= 2);
            }, MethodBase.GetCurrentMethod());
        }

        public static string IslemYap3(int d)
        {
            return
            ExceptionManager.getInstance().TryCatch(() =>
            {
                ThrowException();
                return d.ToString();
            }, MethodBase.GetCurrentMethod());
        }

        public static void ThrowException()
        {
            ExceptionManager.getInstance().TryCatch(() =>
            {
                int x = 1;
                int y = 0;
                int z = x / y;
            }, MethodBase.GetCurrentMethod(),null,i=>
                Console.WriteLine(i.Message.ToString()));
        }
    }
}
