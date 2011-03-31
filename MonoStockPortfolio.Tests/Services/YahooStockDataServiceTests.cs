using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Machine.Specifications.Runner.Impl;
using MonoStockPortfolio.Core.StockData;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Tests.Services
{
    [Tags("IntegrationTest")]
    public class When_using_the_Yahoo_stock_data_service_to_get_quotes
    {
        static YahooStockDataProvider _svc;
        static IList<StockQuote> _quotes;

        Establish context = () =>
            {
                _svc = new YahooStockDataProvider();
            };

        Because of = () =>
            {
                _quotes = _svc.GetStockQuotes(new[] { "GOOG", "AMZN", "AAPL", "MSFT", "NOVL", "S", "VZ", "T" })
                    .ToList();
            };

        It should_get_volumes_from_the_web = () =>
            _quotes.ForEach(q => string.IsNullOrEmpty(q.Volume).ShouldBeFalse());
        It should_get_last_trade_prices_from_the_web = () =>
            _quotes.ForEach(q => q.LastTradePrice.ShouldNotEqual(0.0M));
        It should_get_price_change_from_the_web = () =>
            _quotes.ForEach(q => q.Change.ShouldNotEqual(0.0M));
    }
}