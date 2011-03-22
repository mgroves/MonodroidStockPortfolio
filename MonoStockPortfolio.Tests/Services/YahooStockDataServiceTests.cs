using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Machine.Specifications.Runner.Impl;
using MonoStockPortfolio.Core.StockData;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Tests.Services
{
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

    public class When_using_the_Yahoo_stock_data_service_to_validate_tickers
    {
        static YahooStockDataProvider _svc;
        static bool _goodTicker;
        static bool _badTicker;

        Establish context = () =>
            {
                _svc = new YahooStockDataProvider();
            };

        Because of = () =>
            {
                _goodTicker = _svc.IsValidTicker("GOOG");
                _badTicker = _svc.IsValidTicker("GOOGAMOOGA");
            };

        It should_validate_the_good_ticker = () =>
            _goodTicker.ShouldBeTrue();
        It shouldnt_validate_the_bad_ticker = () =>
            _badTicker.ShouldBeFalse();
    }
}