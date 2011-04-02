using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
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
}