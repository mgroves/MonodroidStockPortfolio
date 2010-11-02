using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Core;
using MonoStockPortfolio.Core.Services;

namespace MonoStockPortfolio
{
    [Activity(Label = "Portfolio")]
    public class PortfolioActivity : Activity
    {
        public PortfolioActivity(IntPtr handle) : base(handle) { }

        public static string ClassName { get { return "monoStockPortfolio.PortfolioActivity"; } }
        public static string Extra_PortfolioID { get { return "monoStockPortfolio.PortfolioActivity.PortfolioID"; } }
        private IPortfolioService _svc;
        private IEnumerable<char>[] longClickOptions;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.layout.portfolio);

            var portfolioId = Intent.GetLongExtra(Extra_PortfolioID, -1);

            _svc = new PortfolioService(this);

            var portfolio = _svc.GetPortolioById(portfolioId);
            this.Title = "Portfolio: " + portfolio.Name;

            var items = new List<StockDataItem>();
            items.Add(StockDataItem.Ticker);
            items.Add(StockDataItem.LastTradePrice);
            var tickers = _svc.GetDetailedItems(portfolioId, items);

            if (tickers.Any())
            {
                WriteTickerHeader(tickers.First());
                foreach (var ticker in tickers)
                {
                    WriteTickerRow(ticker);
                }
            }
        }

        private void WriteTickerHeader(IDictionary<StockDataItem, string> ticker)
        {
            var tr = new TableRow(this);
            tr.SetPadding(5, 5, 0, 5);
            tr.SetBackgroundColor(Color.Gray);
            tr.LayoutParameters = new TableRow.LayoutParams(TableRow.LayoutParams.FillParent, TableRow.LayoutParams.WrapContent);

            foreach (var item in ticker)
            {
                var column = new TextView(this);
                column.Text = item.Key.GetStringValue();
                column.SetPadding(0, 0, 5, 0);
                column.LayoutParameters = new TableRow.LayoutParams(TableRow.LayoutParams.FillParent, TableRow.LayoutParams.WrapContent);
                column.SetTextSize(2, 22);
                column.SetTextColor(Color.Black);
                tr.AddView(column);
            }

            var tableLayout = FindViewById<TableLayout>(Resource.id.quoteTable);
            tableLayout.AddView(tr, new TableRow.LayoutParams(TableRow.LayoutParams.FillParent, TableRow.LayoutParams.WrapContent));
        }

        private void WriteTickerRow(IDictionary<StockDataItem, string> ticker)
        {
            var tr = new TableRow(this);
            tr.SetPadding(5,0,0,5);
            tr.LayoutParameters = new TableRow.LayoutParams(TableRow.LayoutParams.FillParent, TableRow.LayoutParams.WrapContent);
            tr.LongClick += tr_LongClick;

            foreach (var item in ticker)
            {
                var column = new TextView(this);
                column.Text = item.Value;
                column.SetPadding(0,0,5,0);
                column.LayoutParameters = new TableRow.LayoutParams(TableRow.LayoutParams.FillParent, TableRow.LayoutParams.WrapContent);
                column.SetTextSize(2, 22);
                tr.AddView(column);
            }

            var tableLayout = FindViewById<TableLayout>(Resource.id.quoteTable);
            tableLayout.AddView(tr, new TableRow.LayoutParams(TableRow.LayoutParams.FillParent, TableRow.LayoutParams.WrapContent));
        }

        void tr_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            longClickOptions = new IList<char>[] {"Edit".ToCharArray(), "Delete".ToCharArray()};
            var dialogBuilder = new AlertDialog.Builder(this);
            dialogBuilder.SetTitle("Options");
            dialogBuilder.SetItems(longClickOptions, tr_LongClick_Options);
            dialogBuilder.Create().Show();
        }

        private void tr_LongClick_Options(object sender, DialogClickEventArgs e)
        {
            Toast.MakeText(this, "Option: " + longClickOptions[e.Which], ToastLength.Long).Show();
        }
    }
}