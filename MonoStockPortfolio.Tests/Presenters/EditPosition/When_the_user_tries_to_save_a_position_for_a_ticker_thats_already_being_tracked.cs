using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Activites.EditPositionScreen;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters.EditPosition
{
    [Tags("UnitTest")]
    public class When_the_user_tries_to_save_a_position_for_a_ticker_thats_already_being_tracked : EditPositionTests
    {
        Establish context = () =>
            {
                _presenter.Initialize(_mockView, 1);

                Mock.Arrange(() => _mockStockService.IsValidTicker(Arg.AnyString)).Returns(true);
                Mock.Arrange(() => _mockPortfolioRepository.IsTickerAlreadyBeingTracked(Arg.AnyString, Arg.AnyLong)).Returns(true);
            };

        Because of = () =>
            {
                var fakeInputModel = new PositionInputModel { PriceText = "5", SharesText = "5", TickerText = "ABC" };
                _presenter.Save(fakeInputModel);
            };

        It should_not_try_to_save_the_portfolio = () =>
            Mock.Assert(() => _mockPortfolioRepository.SavePosition(Arg.IsAny<Position>()), Occurs.Never());
        It should_send_the_validation_errors_to_the_view = () =>
            Mock.Assert(() => _mockView.ShowErrorMessages(Arg.IsAny<IList<string>>()), Occurs.Exactly(1));
        It should_send_a_duplicate_ticker_error_to_the_view = () =>
            MockPositionMatches(x => x.Any(p => p == "You are already tracking that ticker in this portfolio"));
    }
}