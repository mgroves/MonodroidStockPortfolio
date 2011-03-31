using System.Collections.Generic;

namespace MonoStockPortfolio.Activites.MainScreen
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