using System.Collections.Generic;
using System.Threading;
using Machine.Specifications;
using MonoStockPortfolio.Activites.PortfolioScreen;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    public class Given_an_initialized_Portfolio_Presenter
    {
        protected static IPortfolioPresenter _presenter;
        protected static IPortfolioRepository _mockPortfolioRepository;
        protected static IPortfolioService _mockPortfolioService;
        protected static IConfigRepository _mockConfigRepository;
        protected static IPortfolioView _mockView;

        Establish context = () =>
        {
            OnWorkerThreadAttribute.ThreadingService = new DoNotThreadService();

            _mockPortfolioRepository = Mock.Create<IPortfolioRepository>();
            Mock.Arrange(() => _mockPortfolioRepository.GetPortfolioById(999)).Returns(new Portfolio(123) { Name = "Test Portfolio" });

            _mockPortfolioService = Mock.Create<IPortfolioService>();
            Mock.Arrange(() => _mockPortfolioService.GetDetailedItems(999, Arg.IsAny<IEnumerable<StockDataItem>>())).
                Returns(new List<PositionResultsViewModel> { new PositionResultsViewModel(123) });

            _mockConfigRepository = Mock.Create<IConfigRepository>();
            _mockView = Mock.Create<IPortfolioView>();

            _presenter = new PortfolioPresenter(_mockPortfolioRepository, _mockPortfolioService, _mockConfigRepository);

            _presenter.Initialize(_mockView, 999);
        };
        
    }

    internal class DoNotThreadService : IThreadingService
    {
        public void QueueUserWorkItem(WaitCallback func)
        {
            func.Invoke(null);
        }
    }
}