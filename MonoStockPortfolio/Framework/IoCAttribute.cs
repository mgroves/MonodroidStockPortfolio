using Android.Content;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Core.StockData;
using PostSharp.Aspects;
using TinyIoC;

namespace MonoStockPortfolio.Framework
{
    public class IoCAttribute : LocationInterceptionAspect
    {
        private static Context _context;
        private static TinyIoCContainer _container;

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            if(_context == null)
            {
                var activityContext= (Context)args.Instance;
                _context = activityContext.ApplicationContext.ApplicationContext;
            }

            if (_container == null)
            {
                _container = RegisterTypes();
            }

            var locationType = args.Location.LocationType;

            var instantiation = _container.Resolve(locationType);
            if(instantiation != null)
            {
                args.SetNewValue(instantiation);
            }
            args.ProceedGetValue();
        }

        private TinyIoCContainer RegisterTypes()
        {
            var container = TinyIoCContainer.Current;

            container.Register<Context>(_context);
            container.Register<IStockDataProvider, YahooStockDataProvider>().AsMultiInstance();
            container.Register<IPortfolioService, PortfolioService>().AsMultiInstance();
            container.Register<IPortfolioRepository, AndroidSqlitePortfolioRepository>().AsMultiInstance();

            return container;
        }

//        private object GetInstance(Type locationType)
//        {
//            if (DependencyMap.ContainsKey(locationType))
//            {
//                return DependencyMap[locationType]();
//            }
//            return null;
//        }
//
//        private IDictionary<Type, Func<object>> DependencyMap
//        {
//            get { return _dependencyMap ?? (_dependencyMap = DefaultDependencies()); }
//        }
//
//        private static IDictionary<Type, Func<object>> _dependencyMap;
//        private IDictionary<Type, Func<object>> DefaultDependencies()
//        {
//            var map = new Dictionary<Type, Func<object>>();
//            map.Add(typeof(IPortfolioService), () => new PortfolioService(_context));
//            map.Add(typeof(IPortfolioRepository), () => new AndroidSqlitePortfolioRepository(_context));
//            return map;
//        }
//
//        public void LoadDependencies(IDictionary<Type, Func<object>> dependencies)
//        {
//            _dependencyMap = dependencies;
//        }
    }
}