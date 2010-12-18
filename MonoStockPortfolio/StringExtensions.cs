using System.Collections.Generic;
using System.Text;

namespace MonoStockPortfolio
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
    }
}