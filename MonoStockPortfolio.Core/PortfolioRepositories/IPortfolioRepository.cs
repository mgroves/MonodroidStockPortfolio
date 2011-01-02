using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.PortfolioRepositories
{
    public interface IPortfolioRepository
    {
        IList<Portfolio> GetAllPortfolios();
        void SavePortfolio(Portfolio portfolio);
        void DeletePortfolioById(int portfolioId);
        IList<Position> GetAllPositions(long portfolioId);
        Portfolio GetPortfolioById(long portfolioId);
        void SavePosition(Position position);
        Portfolio GetPortfolioByName(string portfolioName);
        void DeletePositionById(long positionId);
        Position GetPositionById(long positionId);
    }
}