using System.Collections.Generic;
using Machine.Specifications;
using MonoStockPortfolio.Activites.ConfigScreen;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    public class Given_a_Config_Presenter
    {
        protected static ConfigPresenter _presenter;
        protected static IConfigRepository _configRepository;
        protected static IConfigView _configView;

        Establish context = () =>
            {
                _configView = Mock.Create<IConfigView>();
                
                _configRepository = Mock.Create<IConfigRepository>();
                Mock.Arrange(() => _configRepository.GetStockItems())
                    .Returns(new List<StockDataItem>
                                 {
                                     StockDataItem.GainLoss,
                                     StockDataItem.Change
                                 });

                _presenter = new ConfigPresenter(_configRepository);
            };
    }
}