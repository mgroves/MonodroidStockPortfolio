using System.Linq;
using MonoStockPortfolio.Core.StockData;
using Xunit;

namespace MonoStockPortfolio.Tests
{
    public class CsvParserTests
    {
        [Fact]
        public void Can_parse_a_single_line_with_a_single_token()
        {
            var line = "XIN";
            var result = CsvParser.ParseCsvIntoStockQuotes(line);

            Assert.Equal(result.Count(), 1);
            Assert.Equal(result.First()[0], "XIN");
        }

        [Fact]
        public void Can_parse_two_lines_with_a_single_token_each()
        {
            var line = "XIN\nMSFT";
            var result = CsvParser.ParseCsvIntoStockQuotes(line);

            Assert.Equal(result.Count(), 2);
            Assert.Equal(result.First()[0], "XIN");
            Assert.Equal(result.ElementAt(1)[0], "MSFT");
        }

        [Fact]
        public void Can_parse_a_more_complex_set()
        {
            var line = @"""XIN"",2.41,""Xinyuan Real Esta"",269244,-0.03,""4:00pm"",""N/A - <b>2.41</b>"",""-0.03""";
            var result = CsvParser.ParseCsvIntoStockQuotes(line);

            Assert.Equal(result.Count(), 1);
            Assert.Equal(result.First()[0], "XIN");
            Assert.Equal(result.First()[1], "2.41");
            Assert.Equal(result.First()[2], "Xinyuan Real Esta");
            Assert.Equal(result.First()[3], "269244");
            Assert.Equal(result.First()[4], "-0.03");
            Assert.Equal(result.First()[5], "4:00pm");
            Assert.Equal(result.First()[6], "N/A - <b>2.41</b>");
            Assert.Equal(result.First()[7], "-0.03");
        }
    }
}
