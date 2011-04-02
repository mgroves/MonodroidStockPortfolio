using Machine.Specifications;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_view_a_portfolio : Given_an_initialized_Main_Presenter
    {
        Because of = () =>
                     _presenter.ViewPortfolio(1);

        It should_tell_the_view_to_bring_up_the_View_Portfolio_screen_with_the_given_position = () =>
            {
                var id = _portfolioList[1].ID ?? -1;
                Mock.Assert(() => _mockView.StartViewPortfolioActivity(id), Occurs.Exactly(1));
            };

    }
}