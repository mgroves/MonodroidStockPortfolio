using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.PortfolioRepositories
{
    public interface IPortfolioRepository
    {
        IList<Entities.Portfolio> GetAllPortfolios();
        void SavePortfolio(Entities.Portfolio portfolio);
        void DeletePortfolio(Entities.Portfolio portfolio);
        IList<Position> GetAllPositions(long portfolioId);
        Portfolio GetPortfolioById(long portfolioId);
    }
}