using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Core;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters
{
    [Tags("UnitTest")]
    public class When_initialize_the_config_presenter : Given_a_Config_Presenter
    {
        static List<StockDataItem> _allStockDataItems;

        Establish context = () =>
            {
                _allStockDataItems = StockDataItem.Volume.GetValues<StockDataItem>().ToList<StockDataItem>();
            };

        Because of = () =>
            {
                _presenter.Initialize(_configView);
            };

        It should_send_two_checked_items = () =>
            Mock.Assert(() => _configView.PrepopulateConfiguration(Arg.IsAny<IList<StockDataItem>>(), Arg.Matches<IEnumerable<StockDataItem>>(i => i.Count() == 2)), Occurs.Exactly(1));
        It should_send_GainLoss_as_a_checked_item = () =>
            Mock.Assert(() => _configView.PrepopulateConfiguration(Arg.IsAny<IList<StockDataItem>>(), Arg.Matches<IEnumerable<StockDataItem>>(i => i.Any(p => p == StockDataItem.GainLoss))), Occurs.Exactly(1));
        It should_send_Change_as_a_checked_item = () =>
            Mock.Assert(() => _configView.PrepopulateConfiguration(Arg.IsAny<IList<StockDataItem>>(), Arg.Matches<IEnumerable<StockDataItem>>(i => i.Any(p => p == StockDataItem.Change))), Occurs.Exactly(1));
        It should_send_an_enumerated_list_of_all_stock_items = () =>
            Mock.Assert(() => _configView.PrepopulateConfiguration(Arg.Matches<IList<StockDataItem>>(i => i.Count == _allStockDataItems.Count), Arg.IsAny<IEnumerable<StockDataItem>>()), Occurs.Exactly(1));
    }
}