﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Text.Util;
using Android.Views;
using Android.Widget;
using MonoStockPortfolio.Activites.ConfigScreen;
using MonoStockPortfolio.Activites.EditPortfolioScreen;
using MonoStockPortfolio.Activites.PortfolioScreen;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites.MainScreen
{
    [Activity(Label = "Stock Portfolio", MainLauncher = false, Icon = "@drawable/icon", Name = "monostockportfolio.activites.mainscreen.MainActivity")]
    public class MainActivity : Activity, IMainView
    {
        [LazyView(Resource.Id.btnAddPortfolio)] protected Button AddPortfolioButton;
        [LazyView(Resource.Id.portfolioList)] protected ListView PortfolioListView;

        [IoC] IMainPresenter _presenter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.main);

            _presenter.Initialize(this);

            WireUpEvents();
        }

        #region IMainView implementation

        public void RefreshList(IList<Portfolio> portfolios)
        {
            var portfolioLabels = portfolios.Select(p => p.Name).ToArray();
            var listAdapter = new PortfolioArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, portfolios, portfolioLabels);
            PortfolioListView.Adapter = listAdapter;
        }

        public void ExitApplication()
        {
            Finish();
        }

        public void ShowAboutInfo(string message)
        {
            var spannable = new SpannableString(message);
            Linkify.AddLinks(spannable, MatchOptions.All);

            var alertBox = new AlertDialog.Builder(this)
                .SetNeutralButton("Ok", (x, y) => { })
                .SetMessage(spannable)
                .SetTitle("About Mono Stock Portfolio")
                .Create();

            alertBox.Show();

            ((TextView)alertBox.FindViewById(Android.Resource.Id.Message)).MovementMethod = LinkMovementMethod.Instance;
        }

        public void StartEditPortfolioActivity(int itemId)
        {
            var intent = EditPortfolioActivity.EditIntent(this, itemId);
            StartActivityForResult(intent, 0);
        }

        public void StartConfigActivity()
        {
            var intent = ConfigActivity.GotoIntent(this);
            StartActivityForResult(intent, 0);
        }

        public void StartViewPortfolioActivity(long portfolioId)
        {
            var intent = PortfolioActivity.ViewIntent(this, portfolioId);
            StartActivityForResult(intent, 0);
        }

        public void StartAddPortfolioActivity()
        {
            var intent = EditPortfolioActivity.AddIntent(this);
            StartActivityForResult(intent, 0);
        }

        #endregion

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
            var id = (int)info.Id;

            menu.SetHeaderTitle("Options".ToJ());
            menu.Add(0, id, 1, "Rename".ToJ());
            menu.Add(0, id, 2, "Delete".ToJ());
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            if (item.TitleFormatted.ToS() == "Rename")
            {
                // Edit
                _presenter.EditPortfolio(item.ItemId);
                return true;
            }
            if (item.TitleFormatted.ToS() == "Delete")
            {
                // Delete
                _presenter.DeletePortfolio(item.ItemId);
                _presenter.RefreshPortfolioList();
                return true;
            }
            return base.OnContextItemSelected(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var configItem = menu.Add(0, 1, 1, "Config".ToJ());
            configItem.SetIcon(Resource.Drawable.ic_menu_preferences);
            var aboutItem = menu.Add(0, 1, 1, "About".ToJ());
            aboutItem.SetIcon(Resource.Drawable.ic_menu_info_details); // IcMenuInfoDetails);
            var exitItem = menu.Add(0, 1, 1, "Exit".ToJ());
            exitItem.SetIcon(Resource.Drawable.ic_menu_close_clear_cancel);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.TitleFormatted.ToS())
            {
                case "Config":
                    _presenter.GotoConfig();
                    return true;
                case "Exit":
                    _presenter.ExitApplication();
                    return true;
                case "About":
                    _presenter.GotoAboutInfo();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void listView_ItemClick(object sender, ItemEventArgs e)
        {
            _presenter.ViewPortfolio(e.Position);
        }

        private void addPortfolioButton_Click(object sender, EventArgs e)
        {
            _presenter.AddNewPortfolio();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            _presenter.RefreshPortfolioList();
        }
    }
}

