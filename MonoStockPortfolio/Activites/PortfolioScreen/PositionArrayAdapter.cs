using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites.PortfolioScreen
{
    public class PositionArrayAdapter : BaseAdapter<PositionResultsViewModel>
    {
        private readonly IEnumerable<StockDataItem> _configItems;
        private readonly List<PositionResultsViewModel> _items;
        private readonly Context _context;

        public PositionArrayAdapter(Context context, IEnumerable<PositionResultsViewModel> results, IEnumerable<StockDataItem> configItems)
        {
            _configItems = configItems;
            _items = results.ToList();
            _context = context;
        }

        public override int Count
        {
            get { return _items.Count(); }
        }

        public override PositionResultsViewModel this[int position]
        {
            get { return _items[position]; }
        }

        public override long GetItemId(int position)
        {
            return this[position].PositionId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this[position];

            var width = _context.GetScreenWidth();
            var columnWidth = width / item.Items.Count;

            var row = new LinearLayout(_context);
            row.Orientation = Orientation.Horizontal;
            foreach (var stockDataItem in _configItems)
            {
                var cell = new TextView(_context);
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