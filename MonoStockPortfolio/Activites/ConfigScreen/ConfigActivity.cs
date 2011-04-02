using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;
using MonoStockPortfolio.Core;

namespace MonoStockPortfolio.Activites.ConfigScreen
{
    [Activity(Label = "Config", Name = "monostockportfolio.activites.ConfigActivity")]
    public class ConfigActivity : Activity, IConfigView
    {
        [LazyView(Resource.Id.configList)] private ListView ConfigList;
        [LazyView(Resource.Id.btnSaveConfig)] private Button SaveConfigButton;

        [IoC] IConfigPresenter _presenter;

        public static Intent GotoIntent(Context context)
        {
            var intent = new Intent();
            intent.SetClassName(context, ManifestNames.GetName<ConfigActivity>());
            return intent;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.config);

            _presenter.Initialize(this);

            WireUpEvents();
        }

        void WireUpEvents()
        {
            SaveConfigButton.Click += SaveConfigButton_Click;
        }

        #region IConfigView members

        public void PrepopulateConfiguration(IList<StockDataItem> allitems, IEnumerable<StockDataItem> checkeditems)
        {
            var allitemsLabels = allitems.Select(i => i.GetStringValue()).ToList();
            var configAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemMultipleChoice, allitemsLabels);
            ConfigList.Adapter = configAdapter;
            ConfigList.ChoiceMode = ChoiceMode.Multiple;

            for (int i = 0; i < ConfigList.Count; i++)
            {
                if (checkeditems.Contains(allitems[i]))
                {
                    ConfigList.SetItemChecked(i, true);
                }
            }
        }

        public void ShowToastMessage(string message)
        {
            this.LongToast(message);
        }

        #endregion

        void SaveConfigButton_Click(object sender, System.EventArgs e)
        {
            var checkedItems = new List<StockDataItem>();
            for(int i =0;i<ConfigList.Count;i++)
            {
                if (ConfigList.IsItemChecked(i))
                {
                    checkedItems.Add((StockDataItem) i);
                }
            }
            _presenter.SaveConfig(checkedItems);
        }
    }
}