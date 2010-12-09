using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio
{
    [Activity(Label = "Stock Portfolio", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static string ClassName { get { return "monostockportfolio.MainActivity"; } }

        private IPortfolioService _svc;
        private IList<Portfolio> _portfolios;
        private string[] _longClickOptions;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _svc = new PortfolioService(this);

            SetContentView(Resource.layout.main);

            RefreshList();

            WireUpEvents();
        }

        private Button AddPortfolioButton { get { return FindViewById<Button>(Resource.id.btnAddPortfolio); } }
        private ListView PortfolioListView { get { return FindViewById<ListView>(Resource.id.portfolioList); } }

        private void RefreshList()
        {
            _portfolios = _svc.GetAllPortfolios();

            var listAdapter = new ArrayAdapter<string>(this, Android.R.Layout.SimpleListItem1, _portfolios.Select(p => p.Name).ToList());
            PortfolioListView.Adapter = listAdapter;
        }

        private void WireUpEvents()
        {
            AddPortfolioButton.Click += addPortfolioButton_Click;
            PortfolioListView.ItemLongClick += PortfolioListView_ItemLongClick;
            PortfolioListView.ItemClick += listView_ItemClick;
        }

        void PortfolioListView_ItemLongClick(object sender, ItemEventArgs e)
        {
            _longClickOptions = new[] { "Edit", "Delete" };
            var dialogBuilder = new AlertDialog.Builder(this);
            dialogBuilder.SetTitle("Options");
            dialogBuilder.SetItems(_longClickOptions, tr_LongClick_Options);
            dialogBuilder.Create().Show();
        }

        private void tr_LongClick_Options(object sender, DialogClickEventArgs e)
        {
            Toast.MakeText(this, "Option: " + _longClickOptions[e.Which], ToastLength.Long).Show();
        }

        private void listView_ItemClick(object sender, ItemEventArgs e)
        {
            var intent = new Intent();
            intent.SetClassName(this, PortfolioActivity.ClassName);
            intent.PutExtra(PortfolioActivity.Extra_PortfolioID, _portfolios[e.Position].ID ?? -1);
            StartActivityForResult(intent, 0);
        }

        private void addPortfolioButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClassName(this, AddPortfolioActivity.ClassName);
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            RefreshList();
        }
    }
}

