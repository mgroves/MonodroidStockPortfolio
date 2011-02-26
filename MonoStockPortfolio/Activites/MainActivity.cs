﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Stock Portfolio", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        [IoC] private IPortfolioRepository _repo;

        [LazyView(Resource.Id.btnAddPortfolio)] protected Button AddPortfolioButton;
        [LazyView(Resource.Id.portfolioList)] protected ListView PortfolioListView;

        private IList<Portfolio> _portfolios;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.main);

            RefreshList();

            WireUpEvents();
        }

        private void RefreshList()
        {
            _portfolios = _repo.GetAllPortfolios();

            var listAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, _portfolios.Select(p => p.Name).ToList());
            PortfolioListView.Adapter = listAdapter;
        }

        private void WireUpEvents()
        {
            AddPortfolioButton.Click += addPortfolioButton_Click;
            PortfolioListView.ItemClick += listView_ItemClick;
            RegisterForContextMenu(PortfolioListView);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);

            var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
            var selectedPortfolioName = ((TextView)info.TargetView).Text.ToS();
            var selectedPortfolio = _repo.GetPortfolioByName(selectedPortfolioName);
            var id = (int)(selectedPortfolio.ID ?? -1);

            menu.SetHeaderTitle("Options".ToJ());
            menu.Add(0, id, 1, "Rename".ToJ());
            menu.Add(0, id, 2, "Delete".ToJ());
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            if (item.TitleFormatted.ToS() == "Rename")
            {
                // Edit
                var intent = EditPortfolioActivity.EditIntent(this, item.ItemId);
                StartActivityForResult(intent, 0);
                return true;
            }
            if (item.TitleFormatted.ToS() == "Delete")
            {
                // Delete
                _repo.DeletePortfolioById(item.ItemId);
                RefreshList();
                return true;
            }
            return base.OnContextItemSelected(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var configItem = menu.Add(0, 1, 1, "Config".ToJ());
            configItem.SetIcon(Android.Resource.Drawable.IcMenuPreferences);
            var exitItem = menu.Add(0, 1, 1, "Exit".ToJ());
            exitItem.SetIcon(Android.Resource.Drawable.IcMenuCloseClearCancel);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.TitleFormatted.ToS())
            {
                case "Config":
                    var intent = ConfigActivity2.GotoIntent(this);
//                    var intent = new Intent();
//                    intent.SetClassName(this, ConfigActivity.ClassName);
                    StartActivityForResult(intent, 0);
                    return true;
                case "Exit":
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void listView_ItemClick(object sender, ItemEventArgs e)
        {
            var intent = PortfolioActivity.ViewIntent(this, _portfolios[e.Position].ID ?? -1);
            StartActivityForResult(intent, 0);
        }

        private void addPortfolioButton_Click(object sender, EventArgs e)
        {
            var intent = EditPortfolioActivity.AddIntent(this);
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            RefreshList();
        }
    }
}

