using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.PortfolioRepositories
{
    public interface IPortfolioRepository
    {
        IList<Entities.Portfolio> GetAllPortfolios();
        void SavePortfolio(Entities.Portfolio portfolio);
        void DeletePortfolio(string portfolioName);
        IList<Position> GetAllPositions(long portfolioId);
        Portfolio GetPortfolioById(long portfolioId);
        void SavePosition(Position position);
        Portfolio GetPortfolioByName(string portfolioName);
    }
}