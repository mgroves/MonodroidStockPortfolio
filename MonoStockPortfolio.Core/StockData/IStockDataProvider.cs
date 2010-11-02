using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.StockData
{
    public interface IStockDataProvider
    {
        IEnumerable<StockQuote> GetStockQuotes(IEnumerable<string> enumerable);
    }
}