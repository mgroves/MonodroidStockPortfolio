using Machine.Specifications;
using MonoStockPortfolio.Core.StockData;

namespace MonoStockPortfolio.Tests.Services
{
    [Tags("IntegrationTest")]
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