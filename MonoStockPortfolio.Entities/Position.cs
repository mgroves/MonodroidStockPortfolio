namespace MonoStockPortfolio.Entities
{
    public class Position
    {
        public Position() { }
        public Position(long id) { ID = id; }

        public long ID { get; private set; }
        public string Ticker { get; set; }
        public decimal Shares { get; set; }
        public decimal PricePerShare { get; set; }
        public long ContainingPortfolioID { get; set; }
    }
}