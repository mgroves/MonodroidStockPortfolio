using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.PortfolioRepositories
{
    public interface IPortfolioRepository
    {
        IList<Portfolio> GetAllPortfolios();
        void SavePortfolio(Portfolio portfolio);
        void DeletePortfolioById(int portfolioId);
        Portfolio GetPortfolioById(long portfolioId);
        Portfolio GetPortfolioByName(string portfolioName);

        IList<Position> GetAllPositions(long portfolioId);
        void SavePosition(Position position);
        void DeletePositionById(long positionId);
        Position GetPositionById(long positionId);
    }
}