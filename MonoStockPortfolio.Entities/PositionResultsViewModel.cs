using System.Collections.Generic;

namespace MonoStockPortfolio.Entities
{
    public class PositionResultsViewModel
    {
        public PositionResultsViewModel(long positionId)
        {
            PositionId = positionId;
        }
        public IDictionary<StockDataItem, string> Items { get; set; }
        public long PositionId { get; private set; }
    }
}