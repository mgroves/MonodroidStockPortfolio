using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Activites.MainScreen
{
    public interface IMainView
    {
        void RefreshList(IList<Portfolio> portfolios);
        void StartAddPortfolioActivity();
        void StartViewPortfolioActivity(long portfolioId);
        void StartEditPortfolioActivity(int itemId);
        void StartConfigActivity();
        void ExitApplication();
        void ShowAboutInfo(string message);
    }
}