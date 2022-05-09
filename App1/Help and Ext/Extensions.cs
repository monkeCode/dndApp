using System.Collections.Generic;
using System.Text;

namespace App1
{
    internal static class Extensions
    {
        public static string GetAllElemets<T>(this ICollection<T> collection, string separator = null)
        {
            StringBuilder str = new StringBuilder();
            var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!string.IsNullOrEmpty(str.ToString()) && !string.IsNullOrEmpty(enumerator.Current?.ToString()))
                    str.Append(separator ?? " ");
                str.Append(enumerator.Current);
            }
            return str.ToString();
        }

        public static string GetAllElemets<T>(this object[] collection, string separator = null)
        {
            StringBuilder str = new StringBuilder();
            var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!string.IsNullOrEmpty(str.ToString()) && enumerator.Current != null)
                    str.Append(separator ?? " ");
                str.Append(enumerator.Current);
            }
            return str.ToString();
        }
    }
}