using Android.App;
using Android.Content;
using Android.Widget;

namespace MonoStockPortfolio.Framework
{
    public static class ActivityExtensions
    {
        public static void EndActivity(this Activity @this)
        {
            var intent = new Intent();
            @this.SetResult(Result.Ok, intent);
            @this.Finish();
        }

        public static void LongToast(this Activity @this, string message)
        {
            Toast.MakeText(@this, message, ToastLength.Long).Show();
        }

    }
}