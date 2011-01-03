using System.Collections.Generic;
using System.Linq;
using Android.Content;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.Config
{
    public class AndroidSqliteConfigRepository : AndroidSqliteBase, IConfigRepository
    {
        public AndroidSqliteConfigRepository(Context context) : base(context)
        { }

        public IEnumerable<StockDataItem> GetStockItems()
        {
            var cursor = Db.Query(CONFIG_TABLE_NAME, new[] { "StockItems" }, null, null, null, null, null);
            string stockItemsCsv = null;
            if (cursor.Count > 0)
            {
                cursor.MoveToNext();
                stockItemsCsv = cursor.GetString(0);
                if (!cursor.IsClosed) cursor.Close();
            }

            if (string.IsNullOrEmpty(stockItemsCsv))
            {
                return DefaultItems();
            }

            return stockItemsCsv.Split(',').Select(i => (StockDataItem)int.Parse(i));
        }

        // this should never be called, but it's here anyway in case of some catastrophe
        private static IEnumerable<StockDataItem> DefaultItems()
        {
            var items = new List<StockDataItem>();
            items.Add(StockDataItem.Ticker);
            items.Add(StockDataItem.LastTradePrice);
            items.Add(StockDataItem.GainLossRealTime);
            items.Add(StockDataItem.Time);
            return items;
        }
    }
}