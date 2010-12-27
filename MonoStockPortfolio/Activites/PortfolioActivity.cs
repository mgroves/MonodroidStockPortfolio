using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MonoStockPortfolio.Core;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Portfolio")]
    public partial class PortfolioActivity : Activity
    {
        [IoC] private IPortfolioService _svc;
        private IEnumerable<char>[] longClickOptions;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.layout.portfolio);

            Refresh();

            WireUpEvents();

            SetTitle();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var item = menu.Add(0,1,1,"Refresh");
            item.SetIcon(Resource.drawable.ic_menu_refresh);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.Title.ToS())
            {
                case "Refresh": Refresh();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void Refresh()
        {
            QuoteTable.RemoveAllViews();

            var pd = new ProgressDialog(this);
            pd.SetMessage("Loading...Please wait...");
            pd.SetProgressStyle(ProgressDialogStyle.Spinner);
            pd.Show();

            Action refresh = () =>
                                 {
                                     var tickers = _svc.GetDetailedItems(ThisPortofolioId, GetStockItemsFromConfig());
                                     if (tickers.Any())
                                     {
                                         RunOnUiThread(() => RefreshUI(tickers));
                                     }
                                     else
                                     {
                                         RunOnUiThread(() => ShowMessage("Please add positions!"));
                                     }
                                     pd.Dismiss();
                                 };
            var background = new Thread(() => refresh());
            background.Start();
        }

        private void ShowMessage(string message)
        {
            QuoteTable.RemoveAllViews();
            var pleaseWaitMessage = new TextView(this);
            pleaseWaitMessage.Text = message;
            QuoteTable.AddView(pleaseWaitMessage);
        }

        private void RefreshUI(IEnumerable<IDictionary<StockDataItem, string>> tickers)
        {
            QuoteTable.RemoveAllViews();

            WriteTickerHeader(tickers.First());
            foreach (var ticker in tickers)
            {
                WriteTickerRow(ticker);
            }

            this.Window.SetFeatureInt(WindowFeatures.IndeterminateProgress, 10000);
        }

        private void WireUpEvents()
        {
            AddPositionButton.Click += addPositionButton_Click;
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
                column.SetTextSize(2, 18);
                column.SetTextColor(Color.Black);
                tr.AddView(column);
            }

            QuoteTable.AddView(tr, new TableRow.LayoutParams(TableRow.LayoutParams.FillParent, TableRow.LayoutParams.WrapContent));
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
                column.SetTextSize(2, 18);
                tr.AddView(column);
            }

            QuoteTable.AddView(tr, new TableRow.LayoutParams(TableRow.LayoutParams.FillParent, TableRow.LayoutParams.WrapContent));
        }

        void tr_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            Toast.MakeText(this, "TODO!", ToastLength.Long);
        }
    }
}