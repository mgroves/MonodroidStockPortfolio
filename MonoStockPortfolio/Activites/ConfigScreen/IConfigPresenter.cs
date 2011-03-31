using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Activites.ConfigScreen
{
    public interface IConfigPresenter
    {
        void Initialize(IConfigView configView);
        void SaveConfig(List<StockDataItem> checkedItems);
    }
}