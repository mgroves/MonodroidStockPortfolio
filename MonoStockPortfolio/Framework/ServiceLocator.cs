using System;
using Android.Content;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Core.StockData;

namespace MonoStockPortfolio.Framework
{
    public static class ServiceLocator
    {
        public static Context Context { get; set; }

        static ServiceLocator()
        {
            IttyBittyIoC.Register<IStockDataProvider>(() => new YahooStockDataProvider());
            IttyBittyIoC.Register<IPortfolioService>(() => new PortfolioService(new AndroidSqlitePortfolioRepository(Context), new YahooStockDataProvider()));
            IttyBittyIoC.Register<IPortfolioRepository>(() => new AndroidSqlitePortfolioRepository(Context));
            IttyBittyIoC.Register<IConfigRepository>(() => new AndroidSqliteConfigRepository(Context));
        }

        public static object Get(Type serviceType)
        {
            return IttyBittyIoC.Resolve(serviceType);
        }
    }
}