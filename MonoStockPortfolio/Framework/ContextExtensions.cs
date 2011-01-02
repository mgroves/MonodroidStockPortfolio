using Android.App;
using Android.Content;

namespace MonoStockPortfolio.Framework
{
    public static class ContextExtensions
    {
        public static int GetScreenWidth(this Context @this)
        {
            var activity = (Activity)@this;
            return activity.WindowManager.DefaultDisplay.Width;
        }
    }
}