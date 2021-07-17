using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    static class Extensions
    {
        public static string GetAllElemets<T>(this ICollection<T> collection, string separator = null)
        {
            StringBuilder str = new StringBuilder();
            var enumerator = collection.GetEnumerator();
            while(enumerator.MoveNext())
            {
                if (!string.IsNullOrEmpty(str.ToString()) && enumerator.Current !=null)
                    str.Append(separator ?? " ");
                str.Append(enumerator.Current);
            }
            return str.ToString();
        } 
        public static string GetAllElemets<T>(this object[] collection, string separator = null)
        {
            StringBuilder str = new StringBuilder();
            var enumerator = collection.GetEnumerator();
            while(enumerator.MoveNext())
            {
                if (!string.IsNullOrEmpty(str.ToString()) && enumerator.Current !=null)
                    str.Append(separator ?? " ");
                str.Append(enumerator.Current);
            }
            return str.ToString();
        }
    }
}
