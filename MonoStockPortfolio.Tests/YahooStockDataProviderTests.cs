using System.Linq;
using MonoStockPortfolio.Core.StockData;
using Xunit;

namespace MonoStockPortfolio.Tests
{
    public class YahooStockDataProviderTests
    {
        [Fact]
        public void Can_get_volume()
        {
            var svc = new YahooStockDataProvider();
            var quote = svc.GetStockQuotes(new[] {"XIN"}).Single();
            Assert.True(!string.IsNullOrEmpty(quote.Volume));
        }
    }
}