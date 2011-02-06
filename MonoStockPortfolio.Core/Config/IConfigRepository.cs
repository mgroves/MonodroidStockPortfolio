using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.Config
{
    public interface IConfigRepository
    {
        IEnumerable<StockDataItem> GetStockItems();
        void UpdateStockItems(List<StockDataItem> stockDataItems);
    }
}