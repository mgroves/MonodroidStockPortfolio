using Machine.Specifications;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_the_user_selects_delete_context_option : Given_an_initialized_Portfolio_Presenter
    {
        Because of = () =>
                     _presenter.ContextOptionSelected("Delete", 123);

        It should_use_the_repo_to_delete_the_position = () =>
            Mock.Assert(() => _mockPortfolioRepository.DeletePositionById(123), Occurs.Exactly(1));
    }
}