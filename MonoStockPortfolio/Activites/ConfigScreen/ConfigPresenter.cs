using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Core;

namespace MonoStockPortfolio.Activites.ConfigScreen
{
    [Preserve(AllMembers = true)]
    public class ConfigPresenter : IConfigPresenter
    {
        private IConfigView _currentView;
        private readonly IConfigRepository _configRepository;

        public ConfigPresenter(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public void Initialize(IConfigView configView)
        {
            _currentView = configView;

            var allitems = StockDataItem.Volume.GetValues<StockDataItem>().ToList();
            var checkeditems = _configRepository.GetStockItems();

            _currentView.PrepopulateConfiguration(allitems, checkeditems);
        }

        public void SaveConfig(List<StockDataItem> checkedItems)
        {
            _configRepository.UpdateStockItems(checkedItems);

            _currentView.ShowToastMessage("Configuration updated!");
        }
    }
}