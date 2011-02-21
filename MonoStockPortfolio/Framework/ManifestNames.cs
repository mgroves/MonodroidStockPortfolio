using System.Reflection;
using Android.App;

namespace MonoStockPortfolio.Framework
{
    public class ManifestNames
    {
        public static string GetName<T>()
        {
            var attrs = typeof(T).GetCustomAttributes(typeof(ActivityAttribute), false);
            if (attrs.Length == 0) throw new CustomAttributeFormatException("Activity attribute must specify name");
            foreach (var attr in attrs)
            {
                var activityAttr = attr as ActivityAttribute;
                if (activityAttr != null)
                {
                    return activityAttr.Name;
                }
            }
            throw new CustomAttributeFormatException("Activity attribute name not found");
        }        
    }
}