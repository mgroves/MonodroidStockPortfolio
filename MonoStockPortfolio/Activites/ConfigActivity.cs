using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;
using MonoStockPortfolio.Core;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Config")]
    public class ConfigActivity : PreferenceActivity
    {
        [IoC] private IConfigRepository _repo;

        private bool[] _allitemschecked;
        private StockDataItem[] _allStockItems;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            AddPreferencesFromResource(Resource.layout.config);

            _allStockItems = StockDataItem.Change.GetValues<StockDataItem>().ToArray();
            var checkedItems = _repo.GetStockItems();
            _allitemschecked = _allStockItems.Select(i => checkedItems.Contains(i)).ToArray();

            var customPref = FindPreference("customStockItems");
            customPref.PreferenceClick += customPref_PreferenceClick;
        }

        bool customPref_PreferenceClick(Preference preference)
        {
            var stockItemsDisplay = _allStockItems.Select(i => i.GetStringValue()).Cast<IEnumerable<char>>().ToArray();

            var dialog = new AlertDialog.Builder(this)
                .SetTitle("Select columns")
                .SetMultiChoiceItems(stockItemsDisplay, _allitemschecked, clickCallback)
                .SetPositiveButton("Save", okCallback)
                .Create();
            dialog.Show();
            return true;
        }

        private void okCallback(object sender, DialogClickEventArgs e)
        {
            var list = new List<StockDataItem>();
            for (int i = 0; i < _allitemschecked.Length; i++)
            {
                if (_allitemschecked[i])
                {
                    list.Add(_allStockItems[i]);
                }
            }
            _repo.UpdateStockItems(list);
        }

        private void clickCallback(object sender, DialogMultiChoiceClickEventArgs e)
        {
            _allitemschecked[e.Which] = e.IsChecked;
        }

        public static string ClassName { get { return "monostockportfolio.activites.ConfigActivity"; } }
    }
}