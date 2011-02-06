using MonoStockPortfolio.Core.StockData;
using Xunit;

namespace MonoStockPortfolio.Tests
{
    public class GoogleStockQuoteTests : GoogleStockDataProvider
    {
        #region ExampleJson : Example Json Result
        private string ExampleJson =
            @"
// [ {
""id"": ""720780""
,""t"" : ""XIN""
,""e"" : ""NYSE""
,""l"" : ""2.41""
,""l_cur"" : ""2.41""
,""s"": ""0""
,""ltt"":""4:00PM EST""
,""lt"" : ""Feb 4, 4:00PM EST""
,""c"" : ""-0.03""
,""cp"" : ""-1.23""
,""ccol"" : ""chr""
,""eo"" : """"
,""delay"": """"
,""op"" : ""2.45""
,""hi"" : ""2.45""
,""lo"" : ""2.39""
,""vo"" : ""269,244.00""
,""avvo"" : ""352,270.00""
,""hi52"" : ""4.30""
,""lo52"" : ""2.20""
,""mc"" : ""182.78M""
,""pe"" : ""3.47""
,""fwpe"" : """"
,""beta"" : ""1.30""
,""eps"" : ""0.69""
,""name"" : ""Xinyuan Real Estate Co., Ltd. (ADR)""
,""type"" : ""Company""
}
,{
""id"": ""358464""
,""t"" : ""MSFT""
,""e"" : ""NASDAQ""
,""l"" : ""27.77""
,""l_cur"" : ""27.77""
,""s"": ""2""
,""ltt"":""4:01PM EST""
,""lt"" : ""Feb 4, 4:01PM EST""
,""c"" : ""+0.12""
,""cp"" : ""0.43""
,""ccol"" : ""chg""
,""el"": ""27.72""
,""el_cur"": ""27.72""
,""elt"" : ""Feb 4, 7:39PM EST""
,""ec"" : ""-0.05""
,""ecp"" : ""-0.18""
,""eccol"" : ""chr""
,""div"" : ""0.16""
,""yld"" : ""2.30""
,""eo"" : """"
,""delay"": """"
,""op"" : ""27.73""
,""hi"" : ""27.84""
,""lo"" : ""27.51""
,""vo"" : ""40.42M""
,""avvo"" : ""55.50M""
,""hi52"" : ""31.58""
,""lo52"" : ""22.73""
,""mc"" : ""233.33B""
,""pe"" : ""11.77""
,""fwpe"" : """"
,""beta"" : ""1.06""
,""eps"" : ""2.36""
,""name"" : ""Microsoft Corporation""
,""type"" : ""Company""
}
,{
""id"": ""22144""
,""t"" : ""AAPL""
,""e"" : ""NASDAQ""
,""l"" : ""346.50""
,""l_cur"" : ""346.50""
,""s"": ""2""
,""ltt"":""4:02PM EST""
,""lt"" : ""Feb 4, 4:02PM EST""
,""c"" : ""+3.06""
,""cp"" : ""0.89""
,""ccol"" : ""chg""
,""el"": ""346.48""
,""el_cur"": ""346.48""
,""elt"" : ""Feb 4, 7:59PM EST""
,""ec"" : ""-0.02""
,""ecp"" : ""-0.01""
,""eccol"" : ""chr""
,""div"" : """"
,""yld"" : """"
,""eo"" : """"
,""delay"": """"
,""op"" : ""343.76""
,""hi"" : ""346.70""
,""lo"" : ""343.51""
,""vo"" : ""11.49M""
,""avvo"" : ""15.58M""
,""hi52"" : ""348.60""
,""lo52"" : ""190.85""
,""mc"" : ""319.22B""
,""pe"" : ""19.35""
,""fwpe"" : """"
,""beta"" : ""1.38""
,""eps"" : ""17.91""
,""name"" : ""Apple Inc.""
,""type"" : ""Company""
}
]
";
        #endregion

        private string[] SplitResults
        {
            get
            {
                return ExampleJson.Split('}');
            }
        }

        [Fact]
        public void Test_ticker()
        {
            var results = base.MapJsonToStockitems(SplitResults[0]);
            Assert.Equal(results.Ticker,"XIN");
            results = base.MapJsonToStockitems(SplitResults[1]);
            Assert.Equal(results.Ticker,"MSFT");
            results = base.MapJsonToStockitems(SplitResults[2]);
            Assert.Equal(results.Ticker,"AAPL");
        }

        [Fact]
        public void Test_change()
        {
            var results = base.MapJsonToStockitems(SplitResults[0]);
            Assert.Equal(results.Change, -0.03M);
            results = base.MapJsonToStockitems(SplitResults[1]);
            Assert.Equal(results.Change, 0.12M);
            results = base.MapJsonToStockitems(SplitResults[2]);
            Assert.Equal(results.Change, 3.06M);
        }

        [Fact]
        public void Test_last_price()
        {
            var results = base.MapJsonToStockitems(SplitResults[0]);
            Assert.Equal(results.LastTradePrice, 2.41M);
            results = base.MapJsonToStockitems(SplitResults[1]);
            Assert.Equal(results.LastTradePrice, 27.77M);
            results = base.MapJsonToStockitems(SplitResults[2]);
            Assert.Equal(results.LastTradePrice, 346.50M);
        }

        [Fact]
        public void Test_time()
        {
            var results = base.MapJsonToStockitems(SplitResults[0]);
            Assert.Equal(results.LastTradeTime, "4:00PM");
            results = base.MapJsonToStockitems(SplitResults[1]);
            Assert.Equal(results.LastTradeTime, "4:01PM");
            results = base.MapJsonToStockitems(SplitResults[2]);
            Assert.Equal(results.LastTradeTime, "4:02PM");
        }
    }
}