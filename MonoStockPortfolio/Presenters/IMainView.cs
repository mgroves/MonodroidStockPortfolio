using System.Collections.Generic;

namespace MonoStockPortfolio.Presenters
{
    public interface IMainView
    {
        void RefreshList(IList<string> portfolioNames);
        void StartAddPortfolioActivity();
        void StartViewPortfolioActivity(long portfolioId);
        void StartEditPortfolioActivity(int itemId);
        void StartConfigActivity();
        void ExitApplication();
    }
}