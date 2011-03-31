using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Activites.ConfigScreen;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;
using MonoStockPortfolio.Core;

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
                Mock.Arrange(() => _configRepository.GetStockItems()).Returns(new List<StockDataItem>
                                                                                  {
                                                                                      StockDataItem.GainLoss,
                                                                                      StockDataItem.Change
                                                                                  });

                _presenter = new ConfigPresenter(_configRepository);
            };
    }

    public class When_initialize_the_config_presenter : Given_a_Config_Presenter
    {
        static List<StockDataItem> _allStockDataItems;

        Establish context = () =>
            {
                _allStockDataItems = StockDataItem.Volume.GetValues<StockDataItem>().ToList();
            };

        Because of = () =>
        {
            _presenter.Initialize(_configView);
        };

        It should_send_two_checked_items = () =>
                                                    Mock.Assert(() =>
                                                        _configView.PrepopulateConfiguration(
                                                            Arg.IsAny<IList<StockDataItem>>(),
                                                            Arg.Matches<IEnumerable<StockDataItem>>(i => i.Count() == 2)),
                                                        Occurs.Exactly(1));

        It should_send_GainLoss_as_a_checked_item = () =>
                                                    Mock.Assert(() =>
                                                        _configView.PrepopulateConfiguration(
                                                            Arg.IsAny<IList<StockDataItem>>(),
                                                            Arg.Matches<IEnumerable<StockDataItem>>(i => i.Any(p => p == StockDataItem.GainLoss))),
                                                        Occurs.Exactly(1));
        It should_send_Change_as_a_checked_item = () =>
                                                    Mock.Assert(() =>
                                                        _configView.PrepopulateConfiguration(
                                                            Arg.IsAny<IList<StockDataItem>>(),
                                                            Arg.Matches<IEnumerable<StockDataItem>>(i => i.Any(p => p == StockDataItem.Change))),
                                                        Occurs.Exactly(1));

        It should_send_an_enumerated_list_of_all_stock_items = () =>
                                                                Mock.Assert(() =>
                                                        _configView.PrepopulateConfiguration(
                                                            Arg.Matches<IList<StockDataItem>>(i => i.Count == _allStockDataItems.Count),
                                                            Arg.IsAny<IEnumerable<StockDataItem>>()),
                                                        Occurs.Exactly(1));
    }

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