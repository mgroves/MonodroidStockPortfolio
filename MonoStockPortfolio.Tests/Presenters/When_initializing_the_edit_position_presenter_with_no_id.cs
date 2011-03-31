using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
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
}