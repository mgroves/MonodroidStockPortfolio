using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_initializing_the_Main_Presenter : Given_an_initialized_Main_Presenter
    {
        It should_get_the_portfolio_list = () =>
            Mock.Assert(() => _mockPortfolioRepository.GetAllPortfolios(), Occurs.Exactly(1));
        It should_refresh_the_view = () =>
            Mock.Assert(() => _mockView.RefreshList(Arg.IsAny<IList<string>>()), Occurs.Exactly(1));
        It should_refresh_the_view_with_the_portfolio_list = () =>
            Mock.Assert(() => _mockView.RefreshList(Arg.Matches<IList<string>>(stringList => stringList.SequenceEqual(_portfolioList.Select(p => p.Name)))));
    }
}