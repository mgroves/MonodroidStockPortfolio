using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Stock Portfolio", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity : Activity
    {
        [IoC] private IPortfolioRepository _repo;

        private IList<Portfolio> _portfolios;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.layout.main);

            RefreshList();

            WireUpEvents();
        }

        private void RefreshList()
        {
            _portfolios = _repo.GetAllPortfolios();

            var listAdapter = new ArrayAdapter<string>(this, Android.R.Layout.SimpleListItem1, _portfolios.Select(p => p.Name).ToList());
            PortfolioListView.Adapter = listAdapter;
        }

        private void WireUpEvents()
        {
            AddPortfolioButton.Click += addPortfolioButton_Click;
            PortfolioListView.ItemClick += listView_ItemClick;
            RegisterForContextMenu(PortfolioListView);
        }

        public override void OnCreateContextMenu(Android.Views.IContextMenu menu, Android.Views.View v, Android.Views.IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);

            var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
            var selectedPortfolioName = ((TextView)info.TargetView).Text.ToS();
            var selectedPortfolio = _repo.GetPortfolioByName(selectedPortfolioName);
            var id = (int)(selectedPortfolio.ID ?? -1);

            menu.SetHeaderTitle("Options");
            menu.Add(0, id, 1, "Rename");
            menu.Add(0, id, 2, "Delete");
        }

        public override bool OnContextItemSelected(Android.Views.IMenuItem item)
        {
            if (item.Title.ToS() == "Rename")
            {
                // Edit
                var intent = new Intent();
                intent.SetClassName(this, EditPortfolioActivity.ClassName);
                intent.PutExtra(EditPortfolioActivity.Extra_PortfolioID, (long)item.ItemId);
                StartActivityForResult(intent, 0);
                return true;
            }
            if (item.Title.ToS() == "Delete")
            {
                // Delete
                _repo.DeletePortfolioById(item.ItemId);
                RefreshList();
                return true;
            }
            return base.OnContextItemSelected(item);
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
            intent.SetClassName(this, EditPortfolioActivity.ClassName);
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            RefreshList();
        }
    }
}

