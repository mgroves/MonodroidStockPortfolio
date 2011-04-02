using System.Collections.Generic;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Activites.MainScreen
{
    public class MainPresenter : IMainPresenter
    {
        private IPortfolioRepository _repo;

        private IMainView _currentView;

        private IList<Portfolio> _portfolios;
        private IList<Portfolio> Portfolios
        {
            get
            {
                return _portfolios ?? (_portfolios = _repo.GetAllPortfolios());
            }
        }

        public MainPresenter(IPortfolioRepository portfolioRepository)
        {
            _repo = portfolioRepository;
        }

        public void Initialize(IMainView view)
        {
            _currentView = view;
            RefreshPortfolioList();
        }

        public void RefreshPortfolioList()
        {
            _portfolios = null;
            _currentView.RefreshList(Portfolios);
        }

        public void AddNewPortfolio()
        {
            _currentView.StartAddPortfolioActivity();
        }

        public void ViewPortfolio(int portfolioPosition)
        {
            _currentView.StartViewPortfolioActivity(Portfolios[portfolioPosition].ID ?? -1);
        }

        public void DeletePortfolio(int itemId)
        {
            _repo.DeletePortfolioById(itemId);
        }

        public void EditPortfolio(int itemId)
        {
            _currentView.StartEditPortfolioActivity(itemId);
        }

        public void GotoConfig()
        {
            _currentView.StartConfigActivity();
        }

        public void ExitApplication()
        {
            _currentView.ExitApplication();
        }
    }
}