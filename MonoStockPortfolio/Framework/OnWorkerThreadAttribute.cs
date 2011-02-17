using System;
using System.Threading;
using Android.App;
using PostSharp.Aspects;

namespace MonoStockPortfolio.Framework
{
    public class OnWorkerThreadAttribute : MethodInterceptionAspect
    {
        private ProgressDialog _progressDialog;

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var activity = args.Instance as Activity;
            if(activity == null) throw new Exception("OnWorkerThread can only be used on methods in Activity classes");

            ShowProgressDialog(activity);
            ThreadPool.QueueUserWorkItem(delegate
                                             {
                                                 args.Proceed();
                                                 activity.RunOnUiThread(DismissProgressDialog);
                                             });
        }

        private void ShowProgressDialog(Activity activity)
        {
            if (_progressDialog == null)
            {
                var pd = new ProgressDialog(activity);
                pd.SetMessage("Loading...Please wait...");
                pd.SetProgressStyle(ProgressDialogStyle.Spinner);
                _progressDialog = pd;
            }
            _progressDialog.Show();
        }

        private void DismissProgressDialog()
        {
            _progressDialog.Dismiss();
        }
    }
}