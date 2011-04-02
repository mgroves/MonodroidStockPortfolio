using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_initializing_the_Main_Presenter : Given_an_initialized_Main_Presenter
    {
        It should_get_the_portfolio_list = () =>
            Mock.Assert(() => _mockPortfolioRepository.GetAllPortfolios(), Occurs.Exactly(1));
        It should_refresh_the_view = () =>
            Mock.Assert(() => _mockView.RefreshList(Arg.IsAny<IList<Portfolio>>()), Occurs.Exactly(1));
        It should_refresh_the_view_with_the_portfolio_list = () =>
            Mock.Assert(() => _mockView.RefreshList(Arg.Matches<IList<Portfolio>>(p => p.SequenceEqual(_portfolioList))));
    }
}