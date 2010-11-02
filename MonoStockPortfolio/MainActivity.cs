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
    [Activity(Label = "Stock Portfolio", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public static string ClassName { get { return "monoStockPortfolio.MainActivity"; } }
        public MainActivity(IntPtr handle) : base(handle) { }

        private IPortfolioService _svc;
        private IList<Portfolio> _portfolios;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.layout.main);

            RefreshList();

            WireUpEvents();
        }

        private Button AddPortfolioButton { get { return FindViewById<Button>(Resource.id.btnAddPortfolio); } }
        private ListView PortfolioListView { get { return FindViewById<ListView>(Resource.id.portfolioList); } }

        private void RefreshList()
        {
            _svc = new PortfolioService(this);
            _portfolios = _svc.GetAllPortfolios();

            var listAdapter = new ArrayAdapter<string>(this, Android.R.Layout.SimpleListItem1, _portfolios.Select(p => p.Name).ToList());
            PortfolioListView.Adapter = listAdapter;
        }

        private void WireUpEvents()
        {
            AddPortfolioButton.Click += addPortfolioButton_Click;
            PortfolioListView.ItemClick += listView_ItemClick;
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

