using System.Collections.Generic;
using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_done_initializing_a_Portfolio_Presenter : Given_an_initialized_Portfolio_Presenter
    {
        It should_show_the_progress_bar_with_a_nice_message = () =>
            Mock.Assert(() => _mockView.ShowProgressDialog("Loading...Please wait..."), Occurs.Exactly(1));
        It should_use_the_service_to_get_the_positions = () =>
            Mock.Assert(() => _mockPortfolioService.GetDetailedItems(999, Arg.IsAny<IEnumerable<StockDataItem>>()), Occurs.Exactly(1));
        It should_hide_the_progress_bar_message = () =>
            Mock.Assert(() => _mockView.HideProgressDialog(), Occurs.Exactly(1));
        It should_refresh_the_view = () =>
            Mock.Assert(() => _mockView.RefreshList(Arg.IsAny<IEnumerable<PositionResultsViewModel>>(), Arg.IsAny<IEnumerable<StockDataItem>>()), Occurs.Exactly(1));
        It should_get_the_portfolio_name_from_the_repository_and_set_the_title_with_that = () =>
            Mock.Assert(() => _mockView.SetTitle("Portfolio: Test Portfolio"), Occurs.Exactly(1));
    }
}