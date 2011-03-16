using System.Collections.Generic;
using System.Threading;
using Machine.Specifications;
using MonoStockPortfolio.Activites.PortfolioScreen;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    public class Given_an_initialized_Portfolio_Presenter
    {
        protected static IPortfolioPresenter _presenter;
        protected static IPortfolioRepository _mockPortfolioRepository;
        protected static IPortfolioService _mockPortfolioService;
        protected static IConfigRepository _mockConfigRepository;
        protected static IPortfolioView _mockView;

        Establish context = () =>
        {
            OnWorkerThreadAttribute.ThreadingService = new DoNotThreadService();

            _mockPortfolioRepository = Mock.Create<IPortfolioRepository>();
            Mock.Arrange(() => _mockPortfolioRepository.GetPortfolioById(999)).Returns(new Portfolio(123) { Name = "Test Portfolio" });

            _mockPortfolioService = Mock.Create<IPortfolioService>();
            Mock.Arrange(() => _mockPortfolioService.GetDetailedItems(999, Arg.IsAny<IEnumerable<StockDataItem>>())).
                Returns(new List<PositionResultsViewModel> { new PositionResultsViewModel(123) });

            _mockConfigRepository = Mock.Create<IConfigRepository>();
            _mockView = Mock.Create<IPortfolioView>();

            _presenter = new PortfolioPresenter(_mockPortfolioRepository, _mockPortfolioService, _mockConfigRepository);

            _presenter.Initialize(_mockView, 999);
        };
        
    }

    internal class DoNotThreadService : IThreadingService
    {
        public void QueueUserWorkItem(WaitCallback func)
        {
            func.Invoke(null);
        }
    }

    public class When_done_initializing_a_Portfolio_Presenter : Given_an_initialized_Portfolio_Presenter
    {
        It should_show_the_progress_bar_with_a_nice_message = () =>
            Mock.Assert(() => _mockView.ShowProgressDialog("Loading...Please wait..."), Occurs.Exactly(1));
        It should_use_the_service_to_get_the_positions = () =>
            Mock.Assert(() => _mockPortfolioService.GetDetailedItems(999, Arg.IsAny<IEnumerable<StockDataItem>>()), Occurs.Exactly(1));
        It should_hide_the_progress_bar_message = () =>
            Mock.Assert(() => _mockView.HideProgressDialog(), Occurs.Exactly(1));
        It should_refresh_the_view = () =>
            Mock.Assert(() => _mockView.RefreshList(Arg.IsAny<IEnumerable<PositionResultsViewModel>>(), Arg.IsAny<IEnumerable<StockDataItem>>()), Occurs.Exactly(1));
        It should_get_the_portfolio_name_from_the_repository_and_set_the_title_with_that = () =>
            Mock.Assert(() => _mockView.SetTitle("Portfolio: Test Portfolio"), Occurs.Exactly(1));
    }

    public class When_the_user_wants_to_add_a_new_position : Given_an_initialized_Portfolio_Presenter
    {
        Because of = () =>
                _presenter.AddNewPosition();

        It should_tell_the_view_to_bring_up_the_Add_new_portfolio_screen = () =>
            Mock.Assert(() => _mockView.StartAddNewPosition(999), Occurs.Exactly(1));
    }

    public class When_the_user_selects_edit_context_option : Given_an_initialized_Portfolio_Presenter
    {
        Because of = () =>
            _presenter.ContextOptionSelected("Edit", 123);

        It should_bring_up_the_edit_screen = () =>
            Mock.Assert(() => _mockView.StartEditPosition(123, 999), Occurs.Exactly(1));
    }

    public class When_the_user_selects_delete_context_option : Given_an_initialized_Portfolio_Presenter
    {
        Because of = () =>
            _presenter.ContextOptionSelected("Delete", 123);

        It should_use_the_repo_to_delete_the_position = () =>
            Mock.Assert(() => _mockPortfolioRepository.DeletePositionById(123), Occurs.Exactly(1));
    }

    public class When_the_user_wants_to_refresh_the_positions : Given_an_initialized_Portfolio_Presenter
    {
        Because of = () =>
            _presenter.RefreshPositions();

        It should_show_the_progress_bar_with_a_nice_message_again = () =>
            Mock.Assert(() => _mockView.ShowProgressDialog("Loading...Please wait..."), Occurs.Exactly(2));
        It should_use_the_service_to_get_the_positions_again = () =>
            Mock.Assert(() => _mockPortfolioService.GetDetailedItems(999, Arg.IsAny<IEnumerable<StockDataItem>>()), Occurs.Exactly(2));
        It should_hide_the_progress_bar_message_again = () =>
            Mock.Assert(() => _mockView.HideProgressDialog(), Occurs.Exactly(2));
        It should_refresh_the_view_again = () =>
            Mock.Assert(() => _mockView.RefreshList(Arg.IsAny<IEnumerable<PositionResultsViewModel>>(), Arg.IsAny<IEnumerable<StockDataItem>>()), Occurs.Exactly(2));
    }
}