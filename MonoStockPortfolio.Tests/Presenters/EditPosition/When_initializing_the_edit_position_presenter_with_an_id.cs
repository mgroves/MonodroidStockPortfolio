using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters.EditPosition
{
    [Tags("UnitTest")]
    public class When_initializing_the_edit_position_presenter_with_an_id : EditPositionTests
    {
        Establish context = () =>
            {
                var fakePosition = new Position(999) { ContainingPortfolioID = 1, PricePerShare = 5.99M, Shares = 50M, Ticker = "FAKE" };
                Mock.Arrange(() => _mockPortfolioRepository.GetPositionById(999)).Returns(fakePosition);
            };

        Because of = () =>
            {
                _presenter.Initialize(_mockView, 1, 999);
            };

        It should_set_the_title_to_Edit_Position = () =>
            Mock.Assert(() => _mockView.SetTitle("Edit Position"), Occurs.Exactly(1));
        It should_prepopulate_the_PricePerShare_on_the_form = () =>
            Mock.Assert(() => _mockView.PopulateForm(Arg.Matches<Position>(p => p.PricePerShare == 5.99M)), Occurs.Exactly(1));
        It should_prepopulate_the_Shares_on_the_form = () =>
            Mock.Assert(() => _mockView.PopulateForm(Arg.Matches<Position>(p => p.Shares == 50M)), Occurs.Exactly(1));
        It should_prepopulate_the_Ticker_on_the_form = () =>
            Mock.Assert(() => _mockView.PopulateForm(Arg.Matches<Position>(p => p.Ticker == "FAKE")), Occurs.Exactly(1));
    }
}