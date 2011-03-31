using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
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