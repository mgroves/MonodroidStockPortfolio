using System;
using System.Collections.Generic;
using Android.Runtime;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Activites.MainScreen
{
    [Preserve(AllMembers = true)]
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

        public void GotoAboutInfo()
        {
            var message = "Matthew D. Groves © 2011\n" +
                          "Source code:\n" +
                          "\n" +
                          "http://tinyurl.com/mspSource\n" +
                          "\n" +
                          "Contact me:\n" +
                          "\n" +
                          "http://mgroves.com\n" +
                          "http://twitter.com/mgroves\n" +
                          "webmaster@mgroves.com";

            _currentView.ShowAboutInfo(message);
        }
    }
}