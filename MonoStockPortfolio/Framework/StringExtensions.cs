using System.Collections.Generic;
using Java.Lang;
using StringBuilder = System.Text.StringBuilder;

namespace MonoStockPortfolio.Framework
{
    public static class StringExtensions
    {
        public static string ToS(this IEnumerable<char> @this)
        {
            var sb = new StringBuilder();
            foreach (char c in @this)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static ICharSequence ToJ(this string @this)
        {
            return new String(@this);
        }
    }
}