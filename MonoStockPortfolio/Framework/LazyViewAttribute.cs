using System;
using Android.App;
using PostSharp.Aspects;

namespace MonoStockPortfolio.Framework
{
    public class LazyViewAttribute : LocationInterceptionAspect
    {
        private readonly int _viewId;

        public LazyViewAttribute(int viewId)
        {
            _viewId = viewId;
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            args.ProceedGetValue(); // this actually fetches the field and populates the args.Value
            if (args.Value == null)
            {
                var activity = args.Instance as Activity;
                if (activity == null) throw new ArgumentException("LazyViewAttribute can only be used within Activities");
                args.SetNewValue(activity.FindViewById(_viewId));
                args.ProceedGetValue();
            }
        }
    }
}