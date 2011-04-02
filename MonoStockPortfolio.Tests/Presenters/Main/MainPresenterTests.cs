using System.Collections.Generic;
using Machine.Specifications;
using MonoStockPortfolio.Activites.MainScreen;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    public class Given_an_initialized_Main_Presenter
    {
        protected static IMainPresenter _presenter;
        protected static IPortfolioRepository _mockPortfolioRepository;
        protected static IMainView _mockView;
        protected static IList<Portfolio> _portfolioList;
        protected static Portfolio _portfolio1;
        protected static Portfolio _portfolio2;

        Establish context = () =>
            {
                _portfolio1 = new Portfolio(555) {Name = "portfolio1"};
                _portfolio2 = new Portfolio(777) {Name = "portfolio2"};
                _portfolioList = new List<Portfolio>();
                _portfolioList.Add(_portfolio1);
                _portfolioList.Add(_portfolio2);

                _mockPortfolioRepository = Mock.Create<IPortfolioRepository>();
                Mock.Arrange(() => _mockPortfolioRepository.GetAllPortfolios()).Returns(_portfolioList);
                Mock.Arrange(() => _mockPortfolioRepository.GetPortfolioByName("portfolio1")).Returns(_portfolio1);
                _mockView = Mock.Create<IMainView>();

                _presenter = new MainPresenter(_mockPortfolioRepository);
                _presenter.Initialize(_mockView);
            };
    }
}