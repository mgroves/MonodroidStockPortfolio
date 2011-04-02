using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
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
}