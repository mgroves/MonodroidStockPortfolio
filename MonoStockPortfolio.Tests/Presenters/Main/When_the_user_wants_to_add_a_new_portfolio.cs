using Machine.Specifications;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_add_a_new_portfolio : Given_an_initialized_Main_Presenter
    {
        Because of = () =>
                     _presenter.AddNewPortfolio();

        It should_tell_the_view_to_bring_up_the_Add_new_portfolio_screen = () =>
            Mock.Assert(() => _mockView.StartAddPortfolioActivity(), Occurs.Exactly(1));
    }
}