using Machine.Specifications;
using MonoStockPortfolio.Activites.EditPositionScreen;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters.EditPosition
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_save_a_valid_position : EditPositionTests
    {
        Establish context = () =>
            {
                _presenter.Initialize(_mockView, 1);

                Mock.Arrange(() => _mockStockService.IsValidTicker(Arg.AnyString)).Returns(true);
            };

        Because of = () =>
            {
                var fakeInputModel = new PositionInputModel {PriceText = "2.34", SharesText = "671", TickerText = "LOL"};
                _presenter.Save(fakeInputModel);
            };

        It should_save_a_position_with_the_portfolio_repository = () =>
            Mock.Assert(() => _mockPortfolioRepository.SavePosition(Arg.IsAny<Position>()), Occurs.Exactly(1));
        It should_save_a_position_with_the_correct_Price = () =>
            Mock.Assert(() => _mockPortfolioRepository.SavePosition(Arg.Matches<Position>(p => p.PricePerShare == 2.34M)), Occurs.Exactly(1));
        It should_save_a_position_with_the_correct_Shares = () =>
            Mock.Assert(() => _mockPortfolioRepository.SavePosition(Arg.Matches<Position>(p => p.Shares == 671M)), Occurs.Exactly(1));
        It should_save_a_position_with_the_correct_Ticker = () =>
            Mock.Assert(() => _mockPortfolioRepository.SavePosition(Arg.Matches<Position>(p => p.Ticker == "LOL")), Occurs.Exactly(1));
        It should_save_a_position_with_the_correct_Containing_Portfolio_ID = () =>
            Mock.Assert(() => _mockPortfolioRepository.SavePosition(Arg.Matches<Position>(p => p.ContainingPortfolioID == 1)), Occurs.Exactly(1));
        It should_tell_the_view_to_go_back_to_the_main_activity = () =>
            Mock.Assert(() => _mockView.GoBackToPortfolioActivity(), Occurs.Exactly(1));
    }
}