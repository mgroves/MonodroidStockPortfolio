using System.Collections.Generic;
using System.Linq;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.PortfolioRepositories
{
    public class FakePortfolioRepository : IPortfolioRepository
    {
        public IList<Portfolio> GetAllPortfolios()
        {
            return new List<Portfolio> {new Portfolio(1) {Name = "test portfolio"}};
        }

        public void SavePortfolio(Portfolio portfolio)
        {
            ;
        }

        public void DeletePortfolioById(int portfolioId)
        {
            ;
        }

        public IList<Position> GetAllPositions(long portfolioId)
        {
            var list = new List<Position>();
            list.Add(new Position(1) { ContainingPortfolioID = 1, PricePerShare = 5, Shares = 280, Ticker = "XIN"});
            list.Add(new Position(2) { ContainingPortfolioID = 1, PricePerShare = 3, Shares = 100, Ticker = "DENN"});
            list.Add(new Position(3) { ContainingPortfolioID = 1, PricePerShare = 25, Shares = 300, Ticker = "MSFT"});
            list.Add(new Position(4) { ContainingPortfolioID = 1, PricePerShare = 590.18M, Shares = 400, Ticker = "GOOG"});
            list.Add(new Position(5) { ContainingPortfolioID = 1, PricePerShare = 330.10M, Shares = 500, Ticker = "AAPL"});
            list.Add(new Position(6) { ContainingPortfolioID = 1, PricePerShare = 15.10M, Shares = 600, Ticker = "YHOO"});
            return list;
        }

        public Portfolio GetPortfolioById(long portfolioId)
        {
            return GetAllPortfolios().First();
        }

        public void SavePosition(Position position)
        {
            ;
        }

        public Portfolio GetPortfolioByName(string portfolioName)
        {
            return GetAllPortfolios().First(p => p.Name == portfolioName);
        }
    }
}