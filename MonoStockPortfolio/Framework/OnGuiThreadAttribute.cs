using Android.App;
using Android.Util;
using PostSharp.Aspects;

namespace MonoStockPortfolio.Framework
{
    public class OnGuiThreadAttribute : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var activity = args.Instance as Activity;
            if (activity == null)
            {
                Log.Error("OnGuiThreadAttribute", "OnGuiThreadAttribute can only be used on methods within an Activity");
                args.Proceed();
            }
            else
            {
                activity.RunOnUiThread(args.Proceed);
            }
        }
    }
}