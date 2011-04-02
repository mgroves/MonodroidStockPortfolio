using Machine.Specifications;
using MonoStockPortfolio.Activites.EditPortfolioScreen;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

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
}