using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core
{
    public static class EnumExtensions
    {
        /// Will get the string value attribute for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());

            var attributes = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            if (attributes != null)
            {
                return attributes.Length > 0 ? attributes[0].StringValue : string.Empty;
            }
            return string.Empty;
        }

        public static IEnumerable<T> GetValues<T>(this Enum value)
        {
            var enumerations = new List<Enum>();
            var fields = value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var fieldInfo in fields)
            {
                enumerations.Add((Enum)fieldInfo.GetValue(value));
            }
            return enumerations.Cast<T>();
        }
    }
}
