using System;
using System.Collections.Generic;
using Machine.Specifications;
using MonoStockPortfolio.Activites.PortfolioScreen;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    public class Given_an_initialized_Portfolio_Presenter
    {
        protected static PortfolioPresenter _presenter;
        protected static IPortfolioRepository _mockPortfolioRepository;
        protected static IPortfolioService _mockPortfolioService;
        protected static IConfigRepository _mockConfigRepository;
        protected static IPortfolioView _mockView;

        Establish context = () =>
        {
            _mockPortfolioRepository = Mock.Create<IPortfolioRepository>();
            Mock.Arrange(() => _mockPortfolioRepository.GetPortfolioById(999)).Returns(new Portfolio(123) { Name = "Test Portfolio" });

            _mockPortfolioService = Mock.Create<IPortfolioService>();
            Mock.Arrange(() => _mockPortfolioService.GetDetailedItems(999, Arg.IsAny<IEnumerable<StockDataItem>>())).
                Returns(new List<PositionResultsViewModel> { new PositionResultsViewModel(123) });

            _mockConfigRepository = Mock.Create<IConfigRepository>();
            _mockView = Mock.Create<IPortfolioView>();
            Mock.Arrange(() => _mockView.StartEditPosition(Arg.AnyInt, Arg.AnyLong)).DoNothing();       // i don't know why this has to be here to pass the "edit" context option test

            _presenter = new PortfolioPresenter(_mockPortfolioRepository, _mockPortfolioService, _mockConfigRepository);
            _presenter.Initialize(_mockView, 999);
        };
        
    }

    public class When_done_initializing_a_Portfolio_Presenter : Given_an_initialized_Portfolio_Presenter
    {
        It should_get_the_positions_list_and_refresh_the_view = () =>
            Mock.Assert(() => _mockPortfolioService.GetDetailedItems(999, Arg.IsAny<IEnumerable<StockDataItem>>()), Occurs.Exactly(1));

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
}