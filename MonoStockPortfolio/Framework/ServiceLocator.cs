using System;
using Android.Content;
using Android.Util;
using MonoStockPortfolio.Activites.EditPortfolioScreen;
using MonoStockPortfolio.Activites.MainScreen;
using MonoStockPortfolio.Activites.PortfolioScreen;
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
            // services/repositories
            IttyBittyIoC.Register<Context>(() => Context);
            IttyBittyIoC.Register<IStockDataProvider, YahooStockDataProvider>();
            IttyBittyIoC.Register<IPortfolioRepository,AndroidSqlitePortfolioRepository>();
            IttyBittyIoC.Register<IPortfolioService, PortfolioService>();
            IttyBittyIoC.Register<IConfigRepository, AndroidSqliteConfigRepository>();

            // presenters
            IttyBittyIoC.Register<IMainPresenter, MainPresenter>();
            IttyBittyIoC.Register<IPortfolioPresenter, PortfolioPresenter>();
            IttyBittyIoC.Register<IEditPortfolioPresenter, EditPortfolioPresenter>();
        }

        public static object Get(Type serviceType)
        {
            try
            {
                return IttyBittyIoC.Resolve(serviceType);
            }
            catch (Exception)
            {
                Log.Error("ServiceLocatorGet", "Unable to resolve type: " + serviceType.Name);
                throw;
            }
        }
    }
}