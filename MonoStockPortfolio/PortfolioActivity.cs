using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Widget;
using MonoStockPortfolio.Core;
using MonoStockPortfolio.Core.Services;

namespace MonoStockPortfolio
{
    [Activity(Label = "Portfolio")]
    public class PortfolioActivity : Activity
    {
        public static string ClassName { get { return "monostockportfolio.PortfolioActivity"; } }
        public static string Extra_PortfolioID { get { return "monoStockPortfolio.PortfolioActivity.PortfolioID"; } }
        private IPortfolioService _svc;
        private IEnumerable<char>[] longClickOptions;

        private long ThisPortofolioId { get { return Intent.GetLongExtra(Extra_PortfolioID, -1); } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            try
            {
                SetContentView(Resource.layout.portfolio);

                _svc = new PortfolioService(this);

                Refresh();

                WireUpEvents();

                SetTitle();
            }
            catch (Exception ex)
            {
                Log.E("EXCEPTION", ex.ToString());
                Toast.MakeText(this, ex.ToString(), ToastLength.Long);
            }
        }

        private void Refresh()
        {
            var tickers = _svc.GetDetailedItems(ThisPortofolioId, GetStockItemsFromConfig());
            if (tickers.Any())
            {
                var tableLayout = FindViewById<TableLayout>(Resource.id.quoteTable);
                tableLayout.RemoveAllViews();

                WriteTickerHeader(tickers.First());
                foreach (var ticker in tickers)
                {
                    WriteTickerRow(ticker);
                }
            }
        }

        private void WireUpEvents()
        {
            var addPositionButton = FindViewById<Button>(Resource.id.btnAddPosition);
            addPositionButton.Click += addPositionButton_Click;
        }

        private void SetTitle()
        {
            var portfolio = _svc.GetPortolioById(ThisPortofolioId);
            this.Title = "Portfolio: " + portfolio.Name;
        }

        private List<StockDataItem> GetStockItemsFromConfig()
        {
            // TODO: load this from a config
            var items = new List<StockDataItem>();
            items.Add(StockDataItem.Ticker);
            items.Add(StockDataItem.LastTradePrice);
            items.Add(StockDataItem.GainLossRealTime);
            items.Add(StockDataItem.Time);
            return items;
        }

        void addPositionButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClassName(this, AddPositionActivity.ClassName);
            intent.PutExtra(AddPositionActivity.Extra_PortfolioID, ThisPortofolioId);
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Refresh();
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
            Toast.MakeText(this, "TODO!", ToastLength.Long);
        }
    }
}