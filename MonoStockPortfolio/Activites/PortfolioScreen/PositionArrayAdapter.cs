using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites.PortfolioScreen
{
    public class PositionArrayAdapter : GenericArrayAdapter<PositionResultsViewModel>
    {
        private IEnumerable<StockDataItem> _configItems;

        public PositionArrayAdapter(Context context, IEnumerable<PositionResultsViewModel> results, IEnumerable<StockDataItem> configItems)
            : base(context, results)
        {
            _configItems = configItems;
        }

        public override long GetItemId(int position)
        {
            return this[position].PositionId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this[position];

            var width = Context.GetScreenWidth();
            var columnWidth = width / item.Items.Count;

            var row = new LinearLayout(Context);
            row.Orientation = Orientation.Horizontal;
            foreach (var stockDataItem in _configItems)
            {
                var cell = new TextView(Context);
                cell.Text = item.Items[stockDataItem];
                cell.SetWidth(columnWidth);
                RedGreenHighlighting(cell, item.Items);
                row.Tag = item.PositionId;
                row.AddView(cell);
            }
            return row;
        }

        private static void RedGreenHighlighting(TextView cell, IDictionary<StockDataItem, string> items)
        {
            if (items.ContainsKey(StockDataItem.GainLoss))
            {
                cell.SetTextColor(decimal.Parse(items[StockDataItem.GainLoss]) < 0 ? Color.Red : Color.Green);
            }
        }
    }
}