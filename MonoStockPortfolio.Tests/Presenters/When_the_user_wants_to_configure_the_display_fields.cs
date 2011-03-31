using Machine.Specifications;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_configure_the_display_fields : Given_an_initialized_Main_Presenter
    {
        Because of = () =>
                     _presenter.GotoConfig();

        It should_tell_the_view_to_start_up_the_config_activity = () =>
            Mock.Assert(() => _mockView.StartConfigActivity(), Occurs.Exactly(1));
    }
}