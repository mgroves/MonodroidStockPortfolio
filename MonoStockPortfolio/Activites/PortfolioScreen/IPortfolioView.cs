using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Activites.PortfolioScreen
{
    public interface IPortfolioView
    {
        void RefreshList(IEnumerable<PositionResultsViewModel> positions, IEnumerable<StockDataItem> getConfigItems);
        void ShowMessage(string message);
        void SetTitle(string title);
        void StartAddNewPosition(long portfolioId);
        void StartEditPosition(int positionId, long portfolioId);
        void UpdateHeader(IEnumerable<StockDataItem> configItems);
        void ShowProgressDialog(string loadingMessage);
        void HideProgressDialog();
    }
}