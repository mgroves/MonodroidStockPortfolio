using Machine.Specifications;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_delete_a_portfolio : Given_an_initialized_Main_Presenter
    {
        Because of = () =>
                     _presenter.DeletePortfolio(990099);

        It should_use_the_repo_to_delete_the_portfolio_with_the_given_ID = () =>
            Mock.Assert(() => _mockPortfolioRepository.DeletePortfolioById(990099), Occurs.Exactly(1));
    }
}