using System.Collections.Generic;
using System.Linq;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites.PortfolioScreen
{
    public class PortfolioPresenter : IPortfolioPresenter
    {
        private IPortfolioView _currentView;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IPortfolioService _portfolioSvc;
        private readonly IConfigRepository _configRepo;
        private long _portfolioId;

        private IEnumerable<PositionResultsViewModel> _positions;

        private IEnumerable<StockDataItem> GetConfigItems()
        {
            return _configRepo.GetStockItems();
        }

        public PortfolioPresenter(IPortfolioRepository portfolioRepository, IPortfolioService portfolioService, IConfigRepository configRepository)
        {
            _portfolioRepo = portfolioRepository;
            _configRepo = configRepository;
            _portfolioSvc = portfolioService;
        }

        public void Initialize(IPortfolioView view, long thisPortofolioId)
        {
            _currentView = view;
            _portfolioId = thisPortofolioId;
            
            RefreshPositions();

            UpdateHeader();

            SetTitle();
        }

        private void UpdateHeader()
        {
            _currentView.UpdateHeader(GetConfigItems());
        }

        public void AddNewPosition()
        {
            _currentView.StartAddNewPosition(_portfolioId);
        }

        public void MenuOptionSelected(string optionName)
        {
            switch(optionName)
            {
                case "Refresh":
                    RefreshPositions();
                    break;
            }
        }

        public IEnumerable<MenuOption> GetOptions()
        {
            var options = new List<MenuOption>();
            options.Add(new MenuOption {Id = 1, Order = 1, Title = "Refresh", IconResource = Resource.Drawable.ic_menu_refresh});
            return options;
        }

        public IEnumerable<MenuOption> GetContextItems()
        {
            var options = new List<MenuOption>();
            options.Add(new MenuOption {Order = 1, Title = "Edit"});
            options.Add(new MenuOption {Order = 2, Title = "Delete"});
            return options;
        }

        public void ContextOptionSelected(string contextName, int positionId)
        {
            switch (contextName)
            {
                case "Edit":
                    _currentView.StartEditPosition(positionId, _portfolioId);
                    break;
                case "Delete":
                    _portfolioRepo.DeletePositionById(positionId);
                    RefreshPositions();
                    break;
            }
        }

        public void SetTitle()
        {
            var portfolio = _portfolioRepo.GetPortfolioById(_portfolioId);
            var title = "Portfolio: " + portfolio.Name;
            _currentView.SetTitle(title);
        }

        [OnWorkerThread]
        public void RefreshPositions()
        {
            _currentView.ShowProgressDialog("Loading...Please wait...");

            _positions = GetPositions();
            if (_positions.Any())
            {
                _currentView.RefreshList(_positions, GetConfigItems());
            }
            else
            {
                _currentView.ShowMessage("Please add positions!");
            }

            _currentView.HideProgressDialog();
        }

        private IEnumerable<PositionResultsViewModel> GetPositions()
        {
            return _portfolioSvc.GetDetailedItems(_portfolioId, GetConfigItems());
        }
    }
}