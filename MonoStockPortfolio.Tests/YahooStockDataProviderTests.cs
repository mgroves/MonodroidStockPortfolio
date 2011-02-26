using System;
using System.Collections.Generic;
using System.Linq;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Core.StockData;
using MonoStockPortfolio.Entities;
using Xunit;

namespace MonoStockPortfolio.Tests
{
    public class YahooStockDataProviderTests
    {
        [Fact]
        public void Can_get_volume_from_web()
        {
            var svc = new YahooStockDataProvider();
            var quotes = svc.GetStockQuotes(new[] {"GOOG", "AMZN", "AAPL", "MSFT", "NOVL", "S", "VZ", "T"});
            foreach (var stockQuote in quotes)
            {
                Assert.True(!string.IsNullOrEmpty(stockQuote.Volume));
            }
            Assert.True(quotes.Any());
        }

        [Fact]
        public void Can_get_volume_from_service()
        {
            var svc = new PortfolioService(BuildStubPortfolioRepo(), new YahooStockDataProvider());
            var items = svc.GetDetailedItems(1, new List<StockDataItem> {StockDataItem.Ticker, StockDataItem.Volume});
            foreach (var positionResultsViewModel in items)
            {
                Assert.True(positionResultsViewModel.Items[StockDataItem.Volume] != null);
            }
            Assert.True(items.Any());
        }

        private IPortfolioRepository BuildStubPortfolioRepo()
        {
            return new StubPortfolioRepo();
        }

        public class StubPortfolioRepo : IPortfolioRepository
        {
            public IList<Position> GetAllPositions(long portfolioId)
            {
                return new List<Position>
                           {
                               new Position(1)
                                   {ContainingPortfolioID = 1, PricePerShare = 5, Shares = 100, Ticker = "GOOG"}
                           };
            }

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
}