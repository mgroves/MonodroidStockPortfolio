namespace MonoStockPortfolio.Activites.MainScreen
{
    public interface IMainPresenter
    {
        void Initialize(IMainView view);
        void RefreshPortfolioList();
        void AddNewPortfolio();
        void ViewPortfolio(int portfolioPosition);
        void DeletePortfolio(int itemId);
        void EditPortfolio(int itemId);
        void GotoConfig();
        void ExitApplication();
        int GetPortfolioIdForContextMenu(string selectedPortfolioName);
    }
}