using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI.Extensions
{
    public static class ListExtensions
    {
        public static string GetCacheKey<T>(this List<T> list)//int, long, decimal, gibi tipler not custom list
        {
            string result = "";
            foreach (var item in list)
            {
                result += item.ToString() + ",";
            }
            return result;
        }

        public static string Join<T>(this List<T> a, string splitChar)
        {
            string result = "";
            result = string.Join(splitChar, a.Select(n => n.ToString()).ToArray());
            return result;
        }
    }
}
