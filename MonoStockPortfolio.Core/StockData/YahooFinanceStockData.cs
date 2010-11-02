using System;
using FileHelpers;

namespace MonoStockPortfolio.Core.StockData
{
    [DelimitedRecord(",")]
    public class YahooFinanceStockData
    {
        [FieldQuoted(QuoteMode.OptionalForBoth)] 
        public string Ticker;
        
        public decimal LastTradePrice;
        
        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public string Name;
        
        public string Volume;

        public decimal Change;

        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public string LastTradeTime;

        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public string RealTimeLastTradeWithTime;

        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public string ChangeRealTime;
    }
}