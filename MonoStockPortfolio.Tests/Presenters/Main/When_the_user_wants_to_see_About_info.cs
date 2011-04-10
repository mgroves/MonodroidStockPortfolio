using Machine.Specifications;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters.Main
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_see_About_info : Given_an_initialized_Main_Presenter
    {
        static string _expectedMessage;

        Establish context = () =>
            {
                _expectedMessage = "Matthew D. Groves © 2011\n" +
                              "Source code:\n" +
                              "\n" +
                              "http://tinyurl.com/mspSource\n" +
                              "\n" +
                              "Contact me:\n" +
                              "\n" +
                              "http://mgroves.com\n" +
                              "http://twitter.com/mgroves\n" +
                              "webmaster@mgroves.com";
            };

        Because of = () =>
            _presenter.GotoAboutInfo();

        It should_tell_the_view_toshow_the_about_info = () =>
            Mock.Assert(() => _mockView.ShowAboutInfo(Arg.AnyString), Occurs.Exactly(1));
        It should_pass_the_expected_message_to_the_about_info = () =>
            Mock.Assert(() => _mockView.ShowAboutInfo(_expectedMessage), Occurs.Exactly(1));
    }
}