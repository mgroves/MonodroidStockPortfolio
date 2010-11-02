namespace MonoStockPortfolio.Entities
{
    public class StockQuote
    {
        public string Ticker { get; set; }
        public decimal LastTradePrice { get; set; }
        public string Name { get; set; }
        public string Volume { get; set; }
        public decimal Change { get; set; }
        public string LastTradeTime { get; set; }
        public decimal RealTimeLastTradePrice { get; set; }
        public string ChangeRealTime { get; set; }
    }
}