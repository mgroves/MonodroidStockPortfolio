using System;
using System.Collections.Generic;
using Android.Content;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using PostSharp.Aspects;

namespace MonoStockPortfolio
{
    public class IoCAttribute : LocationInterceptionAspect
    {
        private static Context _context;

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            if(_context == null)
            {
                var activityContext= (Context)args.Instance;
                _context = activityContext.ApplicationContext.ApplicationContext;
            }

            var locationType = args.Location.LocationType;
            var instantiation = GetInstance(locationType);
            if(instantiation != null)
            {
                args.SetNewValue(instantiation);
            }
            args.ProceedGetValue();
        }

        private object GetInstance(Type locationType)
        {
            if (DependencyMap.ContainsKey(locationType))
            {
                return DependencyMap[locationType]();
            }
            return null;
        }

        private IDictionary<Type, Func<object>> DependencyMap
        {
            get { return _dependencyMap ?? (_dependencyMap = DefaultDependencies()); }
        }

        private static IDictionary<Type, Func<object>> _dependencyMap;
        private IDictionary<Type, Func<object>> DefaultDependencies()
        {
            var map = new Dictionary<Type, Func<object>>();
            map.Add(typeof(IPortfolioService), () => new PortfolioService(_context));
            map.Add(typeof(IPortfolioRepository), () => new AndroidSqlitePortfolioRepository(_context));
            return map;
        }

        public void LoadDependencies(IDictionary<Type, Func<object>> dependencies)
        {
            _dependencyMap = dependencies;
        }
    }
}