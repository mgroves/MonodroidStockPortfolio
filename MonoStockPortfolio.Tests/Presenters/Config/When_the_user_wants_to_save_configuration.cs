using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_save_configuration : Given_a_Config_Presenter
    {
        Establish context = () =>
            {
                _presenter.Initialize(_configView);
            };

        Because of = () =>
            {
                _presenter.SaveConfig(new List<StockDataItem> {StockDataItem.Ticker, StockDataItem.Time});
            };

        It should_use_the_repo_to_update_to_2_stock_items = () =>
            Mock.Assert(() => _configRepository.UpdateStockItems(Arg.Matches<List<StockDataItem>>(i => i.Count == 2)));
        It should_use_the_repo_to_update_stock_items_with_Time = () =>
            Mock.Assert(() => _configRepository.UpdateStockItems(Arg.Matches<List<StockDataItem>>(i => i.Any(s => s == StockDataItem.Time))));
        It should_use_the_repo_to_update_stock_items_with_Ticker = () =>
            Mock.Assert(() => _configRepository.UpdateStockItems(Arg.Matches<List<StockDataItem>>(i => i.Any(s => s == StockDataItem.Ticker))));
    }
}