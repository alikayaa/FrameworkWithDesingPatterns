using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string FormatTime(this TimeSpan a)
        {
            string result = "";
            if (a.Minutes.ToString() == "0")
                result = a.Hours.ToString() + ".00";
            else
                result = a.Hours.ToString() + "." + a.Minutes.ToString();
            return result;
        }
    }
}
