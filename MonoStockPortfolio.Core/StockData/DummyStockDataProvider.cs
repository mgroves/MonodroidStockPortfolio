using System;
using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.StockData
{
    public class DummyStockDataProvider : IStockDataProvider
    {
        public IEnumerable<StockQuote> GetStockQuotes(IEnumerable<string> enumerable)
        {
            throw new NotImplementedException();
        }
    }
}