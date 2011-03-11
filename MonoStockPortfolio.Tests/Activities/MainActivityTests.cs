using System.Collections.Generic;
using System.Linq;
using MonoStockPortfolio.Activites.Main;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;
using Xunit;

namespace MonoStockPortfolio.Tests.Activities
{
    public abstract class MainActivityTest
    {
        protected IMainPresenter _presenter;
        protected IPortfolioRepository _mockPortfolioRepository;
        protected IMainView _mockView;
        protected IList<Portfolio> _portfolioList;
        protected Portfolio _portfolio1;
        protected Portfolio _portfolio2;

        // startup
        protected MainActivityTest()
        {
            _portfolio1 = new Portfolio(555) {Name = "portfolio1"};
            _portfolio2 = new Portfolio(777) {Name = "portfolio2"};
            _portfolioList = new List<Portfolio>();
            _portfolioList.Add(_portfolio1);
            _portfolioList.Add(_portfolio2);

            _mockPortfolioRepository = Mock.Create<IPortfolioRepository>();
            Mock.Arrange(() => _mockPortfolioRepository.GetAllPortfolios()).Returns(_portfolioList);
            Mock.Arrange(() => _mockPortfolioRepository.GetPortfolioByName("portfolio1")).Returns(_portfolio1);
            _mockView = Mock.Create<IMainView>();

            _presenter = new MainPresenter(_mockPortfolioRepository);
            _presenter.Initialize(_mockView);
        }
    }

    public class When_initializing_the_Main_Presenter : MainActivityTest
    {
        [Fact]
        public void Initialization_should_get_the_portfolio_list_and_refresh_the_view()
        {
            Mock.Assert(() => _mockPortfolioRepository.GetAllPortfolios(), Occurs.Exactly(1));
            Mock.Assert(() => _mockView.RefreshList(Arg.IsAny<IList<string>>()), Occurs.Exactly(1));

            var exp = Arg.Matches<IList<string>>(
                stringList => stringList.SequenceEqual(_portfolioList.Select(p => p.Name))
            );

            Mock.Assert(() => _mockView.RefreshList(exp));
        }
    }

    public class When_the_user_wants_to_add_a_new_portfolio : MainActivityTest
    {
        public When_the_user_wants_to_add_a_new_portfolio()
        {
            _presenter.AddNewPortfolio();
        }

        [Fact]
        public void Then_the_Presenter_should_tell_the_view_to_bring_up_the_Add_new_portfolio_screen()
        {
            Mock.Assert(() => _mockView.StartAddPortfolioActivity(),Occurs.Exactly(1));
        }
    }

    public class When_the_user_wants_to_view_a_portfolio : MainActivityTest
    {
        public When_the_user_wants_to_view_a_portfolio()
        {
            _presenter.ViewPortfolio(1);
        }

        [Fact]
        public void Then_the_Presenter_should_tell_the_view_to_bring_up_the_View_Portfolio_screen_with_the_given_position()
        {
            var id = _portfolioList[1].ID ?? -1;
            Mock.Assert(() => _mockView.StartViewPortfolioActivity(id), Occurs.Exactly(1));
        }
    }

    public class When_the_user_wants_to_delete_a_portfolio : MainActivityTest
    {
        public When_the_user_wants_to_delete_a_portfolio()
        {
            _presenter.DeletePortfolio(990099);
        }

        [Fact]
        public void Then_the_Presenter_should_use_the_repo_to_delete_the_portfolio_with_the_given_ID()
        {
            Mock.Assert(() => _mockPortfolioRepository.DeletePortfolioById(990099), Occurs.Exactly(1));
        }
    }

    public class When_the_user_wants_to_edit_a_portfolio : MainActivityTest
    {
        public When_the_user_wants_to_edit_a_portfolio()
        {
            _presenter.EditPortfolio(909);
        }

        [Fact]
        public void Then_the_presenter_should_tell_the_view_to_start_up_an_edit_activity_for_the_given_portfolio_id()
        {
            Mock.Assert(() => _mockView.StartEditPortfolioActivity(909), Occurs.Exactly(1));
        }
    }

    public class When_the_user_wants_to_configure_the_display_fields : MainActivityTest
    {
        public When_the_user_wants_to_configure_the_display_fields()
        {
            _presenter.GotoConfig();
        }

        [Fact]
        public void Then_the_presenter_should_tell_the_view_to_start_up_the_config_activity()
        {
            Mock.Assert(() => _mockView.StartConfigActivity(), Occurs.Exactly(1));
        }
    }
    
    public class When_the_user_wants_to_exit_the_app : MainActivityTest
    {
        public When_the_user_wants_to_exit_the_app()
        {
            _presenter.ExitApplication();
        }

        [Fact]
        public void Then_the_presenter_should_tell_the_view_to_start_up_the_config_activity()
        {
            Mock.Assert(() => _mockView.ExitApplication(), Occurs.Exactly(1));
        }
    }

    public class When_the_user_wants_to_see_the_context_menu : MainActivityTest
    {
        private int _id;

        public When_the_user_wants_to_see_the_context_menu()
        {
            _id = _presenter.GetPortfolioIdForContextMenu(_portfolio1.Name);
        }

        [Fact]
        public void Then_the_presenter_should_use_the_given_name_to_lookup_the_ID_and_return_it()
        {
            Mock.Assert(() => _mockPortfolioRepository.GetPortfolioByName(_portfolio1.Name), Occurs.Exactly(1));
            Assert.Equal(_portfolio1.ID, _id);
        }
    }
}