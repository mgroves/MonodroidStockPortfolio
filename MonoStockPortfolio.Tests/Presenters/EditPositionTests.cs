using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Machine.Specifications;
using MonoStockPortfolio.Activites.EditPositionScreen;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.StockData;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    public class EditPositionTests
    {
        protected static EditPositionPresenter _presenter;
        protected static IPortfolioRepository _mockPortfolioRepository;
        protected static IStockDataProvider _mockStockService;
        protected static IEditPositionView _mockView;

        Establish context = () =>
            {
                _mockPortfolioRepository = Mock.Create<IPortfolioRepository>();
                _mockStockService = Mock.Create<IStockDataProvider>();
                _mockView = Mock.Create<IEditPositionView>();

                _presenter = new EditPositionPresenter(_mockPortfolioRepository, _mockStockService);
            };

    }

    public class When_initializing_the_edit_position_presenter_with_no_id : EditPositionTests
    {
        Because of = () =>
            {
                _presenter.Initialize(_mockView, 1);
            };

        It should_set_the_title_to_Add_Position = () =>
            Mock.Assert(() => _mockView.SetTitle("Add Position"), Occurs.Exactly(1));
        It shouldnt_prepopulate_the_form_with_anything = () =>
            Mock.Assert(() => _mockView.PopulateForm(Arg.IsAny<Position>()),Occurs.Never());
    }

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
            Mock.Assert(() => _mockView.GoBackToMainActivity(), Occurs.Exactly(1));
    }

    public class When_the_user_wants_to_save_an_invalid_position : EditPositionTests
    {
        Establish context = () =>
            {
                _presenter.Initialize(_mockView, 1);

                Mock.Arrange(() => _mockStockService.IsValidTicker(Arg.AnyString)).Returns(false);
            };

        Because of = () =>
            {
                var fakeInputModel = new PositionInputModel {PriceText = "cows", SharesText = "WALRUS!!", TickerText = "fail"};
                _presenter.Save(fakeInputModel);
            };

        It should_not_try_to_save_the_portfolio = () =>
            Mock.Assert(() => _mockPortfolioRepository.SavePosition(Arg.IsAny<Position>()), Occurs.Never());
        It should_send_the_validation_errors_to_the_view = () =>
            Mock.Assert(() => _mockView.ShowErrorMessages(Arg.IsAny<IList<string>>()), Occurs.Exactly(1));
        It should_send_an_invalid_ticker_error_to_the_view = () =>
            MockAssertPositionMatches(x => x.Any(p => p == "Invalid Ticker Name"));
        It should_send_an_invalid_shares_number_error_to_the_view = () =>
            MockAssertPositionMatches(x => x.Any(p => p == "Please enter a valid, positive number of shares"));
        It should_send_an_invalid_price_per_share_error_to_the_view = () =>
            MockAssertPositionMatches(x => x.Any(p => p == "Please enter a valid, positive price per share"));
        It should_not_tell_the_view_to_go_back_to_the_main_activity = () =>
            Mock.Assert(() => _mockView.GoBackToMainActivity(), Occurs.Never());

        private static void MockAssertPositionMatches(Expression<Predicate<IList<string>>> match)
        {
            Mock.Assert(() => _mockView.ShowErrorMessages(Arg.Matches(match)), Occurs.Exactly(1));
        }
    }

    public class When_the_user_wants_to_save_an_invalid_position_with_blank_fields : EditPositionTests
    {
        Establish context = () =>
        {
            _presenter.Initialize(_mockView, 1);

            Mock.Arrange(() => _mockStockService.IsValidTicker(Arg.AnyString)).Returns(false);
        };

        Because of = () =>
        {
            var fakeInputModel = new PositionInputModel { PriceText = "", SharesText = "", TickerText = "" };
            _presenter.Save(fakeInputModel);
        };

        It should_not_try_to_save_the_portfolio = () =>
            Mock.Assert(() => _mockPortfolioRepository.SavePosition(Arg.IsAny<Position>()), Occurs.Never());
        It should_send_the_validation_errors_to_the_view = () =>
            Mock.Assert(() => _mockView.ShowErrorMessages(Arg.IsAny<IList<string>>()), Occurs.Exactly(1));
        It should_send_an_invalid_ticker_error_to_the_view = () =>
            MockPositionMatches(x => x.Any(p => p == "Please enter a ticker"));
        It should_send_an_invalid_shares_number_error_to_the_view = () =>
            MockPositionMatches(x => x.Any(p => p == "Please enter a valid, positive number of shares"));
        It should_send_an_invalid_price_per_share_error_to_the_view = () =>
            MockPositionMatches(x => x.Any(p => p == "Please enter a valid, positive price per share"));
        It should_not_tell_the_view_to_go_back_to_the_main_activity = () =>
            Mock.Assert(() => _mockView.GoBackToMainActivity(), Occurs.Never());

        private static void MockPositionMatches(Expression<Predicate<IList<string>>> match)
        {
            Mock.Assert(() => _mockView.ShowErrorMessages(Arg.Matches(match)), Occurs.Exactly(1));
        }
    }
}