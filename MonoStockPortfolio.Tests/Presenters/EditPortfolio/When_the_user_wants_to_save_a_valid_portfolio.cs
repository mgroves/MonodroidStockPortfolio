using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_save_a_valid_portfolio : EditPortfolioTests
    {
        Establish context = () =>
            {
                _presenter.Initialize(_mockEditPortfolioView, null);
            };

        Because of = () =>
            {
                _presenter.SavePortfolio(new Portfolio(999) {Name = "Whatever Portfolio"});
            };

        It should_use_the_repository_to_save_the_portfolio = () =>
            Mock.Assert(() => _mockPortfolioRepository.SavePortfolio(Arg.Matches<Portfolio>(x => x.ID == 999 && x.Name == "Whatever Portfolio")), Occurs.Exactly(1));
        It should_tell_the_view_to_show_a_nice_saved_message = () =>
            Mock.Assert(() => _mockEditPortfolioView.ShowSaveSuccessMessage("You saved: Whatever Portfolio"), Occurs.Exactly(1));
        It should_tell_the_view_to_go_back_to_the_main_activity = () =>
            Mock.Assert(() => _mockEditPortfolioView.GoBackToMainActivity(), Occurs.Exactly(1));
    }
}