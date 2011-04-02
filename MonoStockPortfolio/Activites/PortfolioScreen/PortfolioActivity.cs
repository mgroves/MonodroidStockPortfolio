using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MonoStockPortfolio.Activites.EditPositionScreen;
using MonoStockPortfolio.Core;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites.PortfolioScreen
{
    [Activity(Label = "Portfolio", Name = "monostockportfolio.activites.mainscreen.PortfolioActivity")]
    public class PortfolioActivity : Activity, IPortfolioView
    {
        [LazyView(Resource.Id.quoteListview)] protected ListView QuoteListview;
        [LazyView(Resource.Id.btnAddPosition)] protected Button AddPositionButton;
        [LazyView(Resource.Id.quoteHeaderLayout)] protected LinearLayout QuoteListviewHeader;

        [IoC] private IPortfolioPresenter _presenter;

        private const string PORTFOLIOIDEXTRA = "monoStockPortfolio.PortfolioActivity.PortfolioID";
        public static Intent ViewIntent(Context context, long portfolioId)
        {
            var intent = new Intent();
            intent.SetClassName(context, ManifestNames.GetName<PortfolioActivity>());
            intent.PutExtra(PORTFOLIOIDEXTRA, portfolioId);
            return intent;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.portfolio);

            _presenter.Initialize(this, Intent.GetLongExtra(PORTFOLIOIDEXTRA, -1));

            WireUpEvents();
        }

        #region IPortfolioView members

        [OnGuiThread]
        public void RefreshList(IEnumerable<PositionResultsViewModel> positions, IEnumerable<StockDataItem> getConfigItems)
        {
            var listAdapter = new PositionArrayAdapter(this, positions.ToArray(), getConfigItems);
            QuoteListview.Adapter = listAdapter;
        }

        [OnGuiThread]
        public void ShowMessage(string message)
        {
            var listAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new[] { message });
            QuoteListview.Adapter = listAdapter;
        }

        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public void StartAddNewPosition(long portfolioId)
        {
            var intent = EditPositionActivity.AddIntent(this, portfolioId);
            StartActivityForResult(intent, 0);
        }

        public void StartEditPosition(int positionId, long portfolioId)
        {
            var intent = EditPositionActivity.EditIntent(this, positionId, portfolioId);
            StartActivityForResult(intent, 0);
        }

        public void UpdateHeader(IEnumerable<StockDataItem> configItems)
        {
            QuoteListviewHeader.RemoveAllViews();
            var cellwidth = this.GetScreenWidth() / configItems.Count();
            foreach (var stockDataItem in configItems)
            {
                var textItem = new TextView(this);
                textItem.Text = stockDataItem.GetStringValue();
                textItem.SetWidth(cellwidth);
                textItem.SetTextColor(Resources.GetColor(Android.Resource.Color.Black));
                QuoteListviewHeader.AddView(textItem);
            }
            QuoteListviewHeader.SetBackgroundResource(Android.Resource.Color.BackgroundLight);
        }

        private ProgressDialog _progressDialog;

        [OnGuiThread]
        public void ShowProgressDialog(string loadingMessage)
        {
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetMessage(loadingMessage);
            _progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progressDialog.Show();
        }

        [OnGuiThread]
        public void HideProgressDialog()
        {
            if (_progressDialog != null)
            {
                _progressDialog.Hide();
            }
        }

        #endregion

        private void WireUpEvents()
        {
            AddPositionButton.Click += AddPositionButton_Click;
            RegisterForContextMenu(QuoteListview);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            foreach (var option in _presenter.GetOptions())
            {
                var item = menu.Add(0, option.Id, option.Order, option.Title.ToJ());
                item.SetIcon(option.IconResource);
            }
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            _presenter.MenuOptionSelected(item.TitleFormatted.ToS());
            return base.OnOptionsItemSelected(item);
        }

        void AddPositionButton_Click(object sender, EventArgs e)
        {
            _presenter.AddNewPosition();
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
        
            var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
            var selectedPositionId = int.Parse(info.TargetView.Tag.ToString());
        
            menu.SetHeaderTitle("Options".ToJ());
            foreach (var contextItem in _presenter.GetContextItems())
            {
                menu.Add(0, selectedPositionId, contextItem.Order, contextItem.Title.ToJ());
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            _presenter.ContextOptionSelected(item.TitleFormatted.ToS(), item.ItemId);
            return base.OnContextItemSelected(item);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            _presenter.RefreshPositions();
        }
    }
}
