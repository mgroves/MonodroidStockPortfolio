using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using MonoStockPortfolio.Core;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Config")]
    public class ConfigActivity : PreferenceActivity
    {
        [IoC] private IConfigRepository _repo;
        private StockItemPreference[] _stockItemsConfig;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            AddPreferencesFromResource(Resource.Layout.config);

            _stockItemsConfig = StockItemPreference.BuildList(_repo.GetStockItems()).ToArray();

            var customPref = FindPreference("customStockItems");
            customPref.PreferenceClick = customPref_PreferenceClick;
        }

        private bool customPref_PreferenceClick(Preference preference)
        {
            IEnumerable<char>[] stockItemsDisplay = _stockItemsConfig.OrderBy(i => i.StockDataItem).Select(i => i.StockDataItem.GetStringValue()).ToArray();
            bool[] allitemschecked = _stockItemsConfig.OrderBy(i => i.StockDataItem).Select(i => i.IsChecked).ToArray();
            
            var dialog = new AlertDialog.Builder(this);
            dialog.SetMultiChoiceItems(stockItemsDisplay, allitemschecked, clickCallback);
            dialog.SetTitle("Select columns");
            dialog.SetPositiveButton("Save", saveCallback);
            dialog.Create().Show();
            return true;
        }

        private void saveCallback(object sender, DialogClickEventArgs e)
        {
            var list = _stockItemsConfig.Where(i => i.IsChecked).Select(i => i.StockDataItem).ToList();
            _repo.UpdateStockItems(list);
        }

        private void clickCallback(object sender, DialogMultiChoiceClickEventArgs e)
        {
            var which = int.Parse(e.Which.ToString());
            _stockItemsConfig[which].IsChecked = e.IsChecked;
        }

        public static string ClassName { get { return "monostockportfolio.activites.ConfigActivity"; } }

        private class StockItemPreference
        {
            public static IEnumerable<StockItemPreference> BuildList(IEnumerable<StockDataItem> checkedItems)
            {
                var allitems = StockDataItem.Change.GetValues<StockDataItem>();

                return allitems.Select(item => new StockItemPreference {StockDataItem = item, IsChecked = checkedItems.Contains(item)});
            }
            public StockDataItem StockDataItem { get; private set; }
            public bool IsChecked { get; set; }
        }
    }
}