using Machine.Specifications;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_edit_a_portfolio : Given_an_initialized_Main_Presenter
    {
        Because of = () =>
                     _presenter.EditPortfolio(909);

        It should_tell_the_view_to_start_up_an_edit_activity_for_the_given_portfolio_id = () =>
            Mock.Assert(() => _mockView.StartEditPortfolioActivity(909), Occurs.Exactly(1));
    }
}