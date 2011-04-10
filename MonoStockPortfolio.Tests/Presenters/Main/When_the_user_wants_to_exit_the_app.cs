using Machine.Specifications;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters.Main
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_exit_the_app : Given_an_initialized_Main_Presenter
    {
        Because of = () =>
            _presenter.ExitApplication();

        It should_tell_the_view_to_start_up_the_config_activity = () =>
            Mock.Assert(() => _mockView.ExitApplication(), Occurs.Exactly(1));
    }
}