using System;
using System.Threading;
using Android.App;
using PostSharp.Aspects;

namespace MonoStockPortfolio.Framework
{
    public class OnWorkerThreadAttribute : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var activity = args.Instance as Activity;
            if(activity == null) throw new Exception("OnWorkerThread can only be used on methods in Activity classes");

            var pd = ShowProgressDialog(activity);
            pd.Show();
            ThreadPool.QueueUserWorkItem(delegate
                                             {
                                                 args.Proceed();
                                                 activity.RunOnUiThread(pd.Dismiss);
                                             });
        }

        private static ProgressDialog ShowProgressDialog(Activity activity)
        {
            var pd = new ProgressDialog(activity);
            pd.SetMessage("Loading...Please wait...");
            pd.SetProgressStyle(ProgressDialogStyle.Spinner);
            return pd;
        }
    }
}