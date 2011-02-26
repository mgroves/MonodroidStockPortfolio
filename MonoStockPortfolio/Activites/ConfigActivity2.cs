using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;
using MonoStockPortfolio.Core;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Config", Name = "monostockportfolio.activites.ConfigActivity2")]
    public class ConfigActivity2 : Activity
    {
        [IoC] private IConfigRepository _repo;

        [LazyView(Resource.Id.configList)] private ListView ConfigList;
        [LazyView(Resource.Id.btnSaveConfig)] private Button SaveConfigButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.config2);

            var allitems = StockDataItem.Volume.GetValues<StockDataItem>().ToList();
            var allitemsLabels = allitems.Select(i => i.GetStringValue()).ToList();
            var checkeditems = _repo.GetStockItems();

            var configAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemMultipleChoice, allitemsLabels);
            ConfigList.Adapter = configAdapter;
            ConfigList.ChoiceMode = ChoiceMode.Multiple;

            for(int i=0;i<ConfigList.Count;i++)
            {
                if (checkeditems.Contains(allitems[i]))
                {
                    ConfigList.SetItemChecked(i, true);
                }
            }

            SaveConfigButton.Click += SaveConfigButton_Click;
        }

        void SaveConfigButton_Click(object sender, System.EventArgs e)
        {
            var allitems = StockDataItem.Volume.GetValues<StockDataItem>().ToList();
            var newConfig = new List<StockDataItem>();
            for (int i = 0; i < ConfigList.Count; i++)
            {
                if(ConfigList.IsItemChecked(i))
                {
                    newConfig.Add(allitems[i]);
                }
            }
            _repo.UpdateStockItems(newConfig);

            this.LongToast("Configuration updated!");
        }

        public static Intent GotoIntent(Context context)
        {
            var intent = new Intent();
            intent.SetClassName(context, ManifestNames.GetName<ConfigActivity2>());
            return intent;
        }
    }
}