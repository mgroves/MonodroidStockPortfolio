using System;
using Android.Content;
using PostSharp.Aspects;

namespace MonoStockPortfolio.Framework
{
    public class IoCAttribute : LocationInterceptionAspect
    {
        public sealed override void OnGetValue(LocationInterceptionArgs args)
        {
            args.ProceedGetValue();
            if (args.Value == null)     // lazy loading
            {
                var context = args.Instance as Context;
                if(context == null) throw new Exception("The IoC Aspect can only be used on a field within an Activity (or Context) object");

                ResolveContextDependency((Context)args.Instance);

                var dependencyType = args.Location.LocationType;
                var instantiation = ServiceLocator.Get(dependencyType);

                if (instantiation != null)
                {
                    args.SetNewValue(instantiation);
                }
                args.ProceedGetValue();
            }
        }

        private static void ResolveContextDependency(Context contextObject)
        {
            if (ServiceLocator.Context == null)
            {
                // note the double ApplicationContext
                // is because the context's reference could get garbage collected while the app is still goin
                // for whatever reason, but it's reference's reference is long-running
                // and since this context dependency is mainly used for Sqlite, that's the most ideal one
                ServiceLocator.Context = contextObject.ApplicationContext.ApplicationContext;
            }
        }
    }
}