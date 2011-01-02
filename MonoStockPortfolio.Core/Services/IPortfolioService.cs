using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.Services
{
    public interface IPortfolioService
    {
        IList<Portfolio> GetAllPortfolios();
        IEnumerable<PositionResultsViewModel> GetDetailedItems(long portfolioID, IEnumerable<StockDataItem> items);
        Portfolio GetPortolioById(long portfolioId);
    }
}