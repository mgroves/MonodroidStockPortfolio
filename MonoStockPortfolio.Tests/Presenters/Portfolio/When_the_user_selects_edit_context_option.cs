using Machine.Specifications;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_the_user_selects_edit_context_option : Given_an_initialized_Portfolio_Presenter
    {
        Because of = () =>
                     _presenter.ContextOptionSelected("Edit", 123);

        It should_bring_up_the_edit_screen = () =>
            Mock.Assert(() => _mockView.StartEditPosition(123, 999), Occurs.Exactly(1));
    }
}