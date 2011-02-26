using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MonoStockPortfolio.Core;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Portfolio", Name = "monostockportfolio.activites.PortfolioActivity")]
    public class PortfolioActivity : Activity
    {
        [IoC] private IPortfolioService _svc;
        [IoC] private IPortfolioRepository _repo;
        [IoC] private IConfigRepository _config;

        [LazyView(Resource.Id.quoteListview)] protected ListView QuoteListview;
        [LazyView(Resource.Id.btnAddPosition)] protected Button AddPositionButton;
        [LazyView(Resource.Id.quoteHeaderLayout)] protected LinearLayout QuoteListviewHeader;

        private const string PORTFOLIOIDEXTRA = "monoStockPortfolio.PortfolioActivity.PortfolioID";

        public static Intent ViewIntent(Context context, long portfolioId)
        {
            var intent = new Intent();
            intent.SetClassName(context, ManifestNames.GetName<PortfolioActivity>());
            intent.PutExtra(PORTFOLIOIDEXTRA, portfolioId);
            return intent;
        }
        public long ThisPortofolioId { get { return Intent.GetLongExtra(PORTFOLIOIDEXTRA, -1); } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.portfolio);

            Refresh();

            WireUpEvents();

            SetTitle();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var item = menu.Add(0,1,1, "Refresh".ToJ());
            item.SetIcon(Resource.Drawable.ic_menu_refresh);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.TitleFormatted.ToS())
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

            menu.SetHeaderTitle("Options".ToJ());
            menu.Add(0, selectedPositionId, 1, "Edit".ToJ());
            menu.Add(0, selectedPositionId, 2, "Delete".ToJ());
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            if (item.TitleFormatted.ToS() == "Edit")
            {
                // Edit
                var intent = EditPositionActivity.EditIntent(this, item.ItemId, ThisPortofolioId);
                StartActivityForResult(intent, 0);
                return true;
            }
            if (item.TitleFormatted.ToS() == "Delete")
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
            RefreshWorker();

            UpdateHeader(GetStockItemsFromConfig());
        }

        [OnWorkerThread]
        private void RefreshWorker()
        {
            var tickers = _svc.GetDetailedItems(ThisPortofolioId, GetStockItemsFromConfig());
            if (tickers.Any())
            {
                RefreshUI(tickers);
            }
            else
            {
                ShowMessage("Please add positions!");
            }
        }

        [OnGuiThread]
        private void RefreshUI(IEnumerable<PositionResultsViewModel> tickers)
        {
            var listAdapter = new PositionArrayAdapter(this, tickers.ToArray());
            QuoteListview.Adapter = listAdapter;
        }

        [OnGuiThread]
        private void ShowMessage(string message)
        {
            var listAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new[] { message });
            QuoteListview.Adapter = listAdapter;
        }

        [OnGuiThread]
        private void UpdateHeader(IEnumerable<StockDataItem> items)
        {
            QuoteListviewHeader.RemoveAllViews();
            var cellwidth = this.GetScreenWidth()/items.Count();
            foreach (var stockDataItem in items)
            {
                var textItem = new TextView(this);
                textItem.Text = stockDataItem.GetStringValue();
                textItem.SetWidth(cellwidth);
                textItem.SetTextColor(Resources.GetColor(Android.Resource.Color.Black));
                QuoteListviewHeader.AddView(textItem);
            }
            QuoteListviewHeader.SetBackgroundResource(Android.Resource.Color.BackgroundLight);
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
                var portfolioActivity = (PortfolioActivity) Context;
                foreach (var stockDataItem in portfolioActivity.GetStockItemsFromConfig())
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
            var portfolio = _repo.GetPortfolioById(ThisPortofolioId);
            this.Title = "Portfolio: " + portfolio.Name;
        }

        void addPositionButton_Click(object sender, EventArgs e)
        {
            var intent = EditPositionActivity.AddIntent(this, ThisPortofolioId);
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Refresh();
        }

        public IEnumerable<StockDataItem> GetStockItemsFromConfig()
        {
            return _config.GetStockItems();
        }
    }
}