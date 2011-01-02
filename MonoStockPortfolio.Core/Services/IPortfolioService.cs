using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.Services
{
    public interface IPortfolioService
    {
        IEnumerable<PositionResultsViewModel> GetDetailedItems(long portfolioID, IEnumerable<StockDataItem> items);
    }
}