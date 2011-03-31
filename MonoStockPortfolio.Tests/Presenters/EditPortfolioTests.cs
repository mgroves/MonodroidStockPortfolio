using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Activites.EditPortfolioScreen;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace MonoStockPortfolio.Tests.Presenters
{
    public class EditPortfolioTests
    {
        protected static EditPortfolioPresenter _presenter;
        protected static IPortfolioRepository _mockPortfolioRepository;
        protected static IEditPortfolioView _mockEditPortfolioView;

        Establish context = () =>
            {
                _mockPortfolioRepository = Mock.Create<IPortfolioRepository>();
                Mock.Arrange(() => _mockPortfolioRepository.GetPortfolioById(999)).Returns(
                    new Portfolio(999) {Name = "Testing Portfolio!"});

                _mockEditPortfolioView = Mock.Create<IEditPortfolioView>();

                _presenter = new EditPortfolioPresenter(_mockPortfolioRepository);
            };
    }

    public class When_initializing_the_edit_portfolio_presenter_with_no_id : EditPortfolioTests
    {
        Because of = () =>
            {
                _presenter.Initialize(_mockEditPortfolioView, null);
            };

        It should_set_the_title_to_Add_New_Portfolio = () =>
            Mock.Assert(() => _mockEditPortfolioView.SetTitle("Add New Portfolio"), Occurs.Exactly(1));
        It shouldnt_prepopulate_the_form_with_anything = () =>
            Mock.Assert(() => _mockEditPortfolioView.PopulateForm(Arg.IsAny<Portfolio>()), Occurs.Never());
    }

    public class When_initializing_the_edit_portfolio_presenter_with_an_id : EditPortfolioTests
    {
        Because of = () =>
            {
                _presenter.Initialize(_mockEditPortfolioView, 999);
            };

        It should_set_the_title_to_Edit_Portfolio = () =>
            Mock.Assert(() => _mockEditPortfolioView.SetTitle("Edit Portfolio"), Occurs.Exactly(1));
        It should_prepopulate_the_form_with_a_portfolio_name = () =>
            Mock.Assert(() => _mockEditPortfolioView.PopulateForm(Arg.Matches<Portfolio>(x => x.Name == "Testing Portfolio!")), Occurs.Exactly(1));
    }

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

    public class When_the_user_tries_to_save_a_new_portfolio_with_a_blank_name : EditPortfolioTests
    {
        Establish context = () =>
            {
                _presenter.Initialize(_mockEditPortfolioView);
            };

        Because of = () =>
            {
                _presenter.SavePortfolio(new Portfolio {Name = ""});
            };

        It should_return_1_validation_error = () =>
            Mock.Assert(() => _mockEditPortfolioView.ShowValidationErrors(Arg.Matches<IEnumerable<string>>(x => x.Count() == 1)), Occurs.Exactly(1));
        It should_return_a_nice_required_validation_error_message = () =>
            Mock.Assert(() => _mockEditPortfolioView.ShowValidationErrors(Arg.Matches<IEnumerable<string>>(x => x.Single() == "Please enter a portfolio name")), Occurs.Exactly(1));
    } 
    
    public class When_the_user_tries_to_save_a_portfolio_with_a_duplicated_name : EditPortfolioTests
    {
        Establish context = () =>
            {
                _presenter.Initialize(_mockEditPortfolioView);
            };

        Because of = () =>
            {
                Mock.Arrange(() => _mockPortfolioRepository.GetPortfolioByName(Arg.AnyString)).Returns(
                    new Portfolio(998) {Name = "Some Name"});
                _presenter.SavePortfolio(new Portfolio {Name = "Some Name"});
            };

        It should_return_1_validation_error = () =>
            Mock.Assert(() => _mockEditPortfolioView.ShowValidationErrors(Arg.Matches<IEnumerable<string>>(x => x.Count() == 1)), Occurs.Exactly(1));
        It should_return_a_nice_duplication_error_message = () =>
            Mock.Assert(() => _mockEditPortfolioView.ShowValidationErrors(Arg.Matches<IEnumerable<string>>(x => x.Single() == "Portfolio name is already taken")), Occurs.Exactly(1));
    }
}