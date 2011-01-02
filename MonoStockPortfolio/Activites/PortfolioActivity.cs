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
using MonoStockPortfolio.Core.PortfolioRepositories;
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
        [IoC] private IPortfolioRepository _repo;

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

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);

            var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
            var selectedPositionId = int.Parse(info.TargetView.Tag.ToString());

            menu.SetHeaderTitle("Options");
            menu.Add(0, selectedPositionId, 1, "Edit");
            menu.Add(0, selectedPositionId, 2, "Delete");
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            if (item.Title.ToS() == "Edit")
            {
                // Edit
                var intent = new Intent();
                intent.SetClassName(this, EditPositionActivity.ClassName);
                intent.PutExtra(EditPositionActivity.Extra_PositionID, (long)item.ItemId);
                intent.PutExtra(EditPositionActivity.Extra_PortfolioID, ThisPortofolioId);
                StartActivityForResult(intent, 0);
                return true;
            }
            if (item.Title.ToS() == "Delete")
            {
                // Delete
                _repo.DeletePositionById(item.ItemId);
                Refresh();
                return true;
            }
            return base.OnContextItemSelected(item);
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
                    row.Tag = item.PositionId;
                    row.AddView(cell);
                }
                return row;
            }
        }

        private void WireUpEvents()
        {
            AddPositionButton.Click += addPositionButton_Click;
            RegisterForContextMenu(QuoteListview);
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
            intent.SetClassName(this, EditPositionActivity.ClassName);
            intent.PutExtra(EditPositionActivity.Extra_PortfolioID, ThisPortofolioId);
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Refresh();
        }
    }
}