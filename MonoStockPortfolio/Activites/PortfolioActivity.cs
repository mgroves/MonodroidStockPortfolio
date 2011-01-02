using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MonoStockPortfolio.Core;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;
using Orientation = Android.Widget.Orientation;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Portfolio")]
    public partial class PortfolioActivity : Activity
    {
        [IoC] private IPortfolioService _svc;

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
                                     RunOnUiThread(pd.Dismiss);
                                 };
            var background = new Thread(() => refresh());
            background.Start();
            UpdateHeader(GetStockItemsFromConfig());
        }

        private void ShowMessage(string message)
        {
            var listAdapter = new ArrayAdapter<string>(this, Android.R.Layout.SimpleListItem1, new[] {message});
            QuoteListview.Adapter = listAdapter;
        }

        private void RefreshUI(IEnumerable<PositionResultsViewModel> tickers)
        {
            var listAdapter = new PositionArrayAdapter(this, tickers.ToArray());
            QuoteListview.Adapter = listAdapter;
        }

        private void UpdateHeader(ICollection<StockDataItem> items)
        {
            QuoteListviewHeader.RemoveAllViews();
            var cellwidth = this.GetScreenWidth()/items.Count;
            foreach (var stockDataItem in items)
            {
                var textItem = new TextView(this);
                textItem.Text = stockDataItem.GetStringValue();
                textItem.SetWidth(cellwidth);
                textItem.SetTextColor(Resources.GetColor(Android.R.Color.Black));
                QuoteListviewHeader.AddView(textItem);
            }
            QuoteListviewHeader.SetBackgroundResource(Android.R.Color.BackgroundLight);
        }

        public class PositionArrayAdapter : GenericArrayAdapter<PositionResultsViewModel>
        {
            public PositionArrayAdapter(Context context, IEnumerable<PositionResultsViewModel> results)
                : base(context, results) { }

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
                foreach (var stockDataItem in GetStockItemsFromConfig())
                {
                    var cell = new TextView(Context);
                    cell.Text = item.Items[stockDataItem];
                    cell.SetWidth(columnWidth);
                    row.AddView(cell, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
                }
                return row;
            }
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

        public static List<StockDataItem> GetStockItemsFromConfig()
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
    }
}