using System.Collections.Generic;
using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_refresh_the_positions : Given_an_initialized_Portfolio_Presenter
    {
        Because of = () =>
                     _presenter.RefreshPositions();

        It should_show_the_progress_bar_with_a_nice_message_again = () =>
            Mock.Assert(() => _mockView.ShowProgressDialog("Loading...Please wait..."), Occurs.Exactly(2));
        It should_use_the_service_to_get_the_positions_again = () =>
            Mock.Assert(() => _mockPortfolioService.GetDetailedItems(999, Arg.IsAny<IEnumerable<StockDataItem>>()), Occurs.Exactly(2));
        It should_hide_the_progress_bar_message_again = () =>
            Mock.Assert(() => _mockView.HideProgressDialog(), Occurs.Exactly(2));
        It should_refresh_the_view_again = () =>
            Mock.Assert(() => _mockView.RefreshList(Arg.IsAny<IEnumerable<PositionResultsViewModel>>(), Arg.IsAny<IEnumerable<StockDataItem>>()), Occurs.Exactly(2));
    }
}