using Machine.Specifications;
using MonoStockPortfolio.Tests.Presenters;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Activities
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_see_the_context_menu : Given_an_initialized_Main_Presenter
    {
        static int _id;

        Because of = () =>
                     _id = _presenter.GetPortfolioIdForContextMenu(_portfolio1.Name);

        It should_use_the_given_name_to_lookup_the_ID = () =>
            {
                Mock.Assert(() => _mockPortfolioRepository.GetPortfolioByName(_portfolio1.Name), Occurs.Exactly(1));
                _portfolio1.ID.ShouldEqual(_id);
            };
    }
}