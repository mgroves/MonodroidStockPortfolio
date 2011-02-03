using Android.Content;
using PostSharp.Aspects;

namespace MonoStockPortfolio.Framework
{
    public class IoCAttribute : LocationInterceptionAspect
    {
        public sealed override void OnGetValue(LocationInterceptionArgs args)
        {
            if (ServiceLocator.Context == null)
            {
                var activityContext = (Context)args.Instance;
                ServiceLocator.Context = activityContext.ApplicationContext.ApplicationContext;
            }

            var locationType = args.Location.LocationType;
            var instantiation = ServiceLocator.Get(locationType);

            if (instantiation != null)
            {
                args.SetNewValue(instantiation);
            }
            args.ProceedGetValue();
        }
    }
}