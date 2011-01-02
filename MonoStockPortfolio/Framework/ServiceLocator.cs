using System;
using Android.Content;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Core.StockData;
using TinyIoC;

namespace MonoStockPortfolio.Framework
{
    public static class ServiceLocator
    {
        public static Context Context { get; set; }
        private static TinyIoCContainer _container;

        public static object Get(Type serviceType)
        {
            if (_container == null)
            {
                _container = RegisterTypes();
            }
            return _container.Resolve(serviceType);
        }

        private static TinyIoCContainer RegisterTypes()
        {
            var container = TinyIoCContainer.Current;

            container.Register<Context>(Context);
            container.Register<IStockDataProvider, YahooStockDataProvider>().AsMultiInstance();
            container.Register<IPortfolioService, PortfolioService>().AsMultiInstance();
            container.Register<IPortfolioRepository, AndroidSqlitePortfolioRepository>().AsMultiInstance();

            return container;
        }
    }
}