using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Extensions
{
    public static class DateTimeExtensions
    {
        public static TimeSpan GetTime(this DateTime a)
        {
            TimeSpan ts = new TimeSpan();
            ts = new TimeSpan(a.Hour, a.Minute, a.Second);
            return ts;
        }

        public static string GetCultureDate(this DateTime date, bool showDate)
        {
            return date.ToLongDateString().Replace(date.DayOfWeek.ToString(), date.ToString("ddd")).Replace(date.ToString("MMMM"), date.ToString("MMM")) + (showDate ? " " + date.ToString("HH:mm")  /*date.ToShortTimeString()*/ : "");
        }

    }
}
