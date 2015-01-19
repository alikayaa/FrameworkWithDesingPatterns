using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Exception
{
    //yakalanan exceptionlar handle edilirken çıktı formatını soyutlamak 
    //adına bir interface yazılır. Çıkan exception formatını soyutlamak 
    //için yeni sınıf yazılmak istendiğinde bu interfaceden türetilir.
    public interface IExceptionFormatter
    {
        //bu metod kodun geçtiği yolları formatlamak için kullanılacak.
        //yani kod herhangi bir yerde patladığından o kodu çağıran kodlar
        //ağaç şeklinde gösterilmeli. 
        string FormatException(MethodBase methodbase, string errorDesc, System.Exception ex, ExceptionTypes exType);
        //burası da kodun asıl patladığı yeri formatlamak içindir.
        string FormatExceptionThrowCatch(MethodBase methodbase, string errorDesc, System.Exception ex, ExceptionTypes exType);
    }
}
