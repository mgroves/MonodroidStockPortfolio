using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Activites.EditPortfolioScreen
{
    public interface IEditPortfolioPresenter
    {
        void Initialize(IEditPortfolioView editPortfolioView, long? portfolioId = null);
        void SavePortfolio(Portfolio portfolioToSave);
    }
}