using System.Collections.Generic;

namespace MonoStockPortfolio.Activites.PortfolioScreen
{
    public interface IPortfolioPresenter
    {
        void Initialize(IPortfolioView view, long thisPortofolioId);
        void AddNewPosition();
        void MenuOptionSelected(string optionName);
        IEnumerable<MenuOption> GetOptions();
        IEnumerable<MenuOption> GetContextItems();
        void ContextOptionSelected(string contextName, int positionId);
        void RefreshPositions();
    }
}