using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.Services
{
    public interface IPortfolioService
    {
        IList<Portfolio> GetAllPortfolios();
        IEnumerable<IDictionary<StockDataItem, string>> GetDetailedItems(long portfolioID, IEnumerable<StockDataItem> items);
        Portfolio GetPortolioById(long portfolioId);
    }
}