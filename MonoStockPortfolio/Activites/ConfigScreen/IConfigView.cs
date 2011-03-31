using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Activites.ConfigScreen
{
    public interface IConfigView
    {
        void PrepopulateConfiguration(IList<StockDataItem> allitems, IEnumerable<StockDataItem> checkeditems);
        void ShowToastMessage(string message);
    }
}