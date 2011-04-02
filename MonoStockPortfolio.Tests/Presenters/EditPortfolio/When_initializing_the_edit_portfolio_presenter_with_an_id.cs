using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
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
}