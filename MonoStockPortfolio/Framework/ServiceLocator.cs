using System;
using System.Collections.Generic;
using Android.Content;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Core.StockData;
using MonoStockPortfolio.Entities;
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
            container.Register<IStockDataProvider, DummyStockDataProvider>().AsMultiInstance();     // works
            container.Register<IPortfolioService, PortfolioService>().AsMultiInstance();            // works
            container.Register<IPortfolioRepository, Whatever>().AsMultiInstance();     // error
            //container.Register<IConfigRepository, AndroidSqliteConfigRepository>().AsMultiInstance();               // error

            return container;
        }
    }

    public class Whatever : IPortfolioRepository
    {
        public IList<Portfolio> GetAllPortfolios()
        {
            throw new NotImplementedException();
        }

        public void SavePortfolio(Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public void DeletePortfolioById(int portfolioId)
        {
            throw new NotImplementedException();
        }

        public Portfolio GetPortfolioById(long portfolioId)
        {
            throw new NotImplementedException();
        }

        public Portfolio GetPortfolioByName(string portfolioName)
        {
            throw new NotImplementedException();
        }

        public IList<Position> GetAllPositions(long portfolioId)
        {
            throw new NotImplementedException();
        }

        public void SavePosition(Position position)
        {
            throw new NotImplementedException();
        }

        public void DeletePositionById(long positionId)
        {
            throw new NotImplementedException();
        }

        public Position GetPositionById(long positionId)
        {
            throw new NotImplementedException();
        }
    }
}