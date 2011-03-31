namespace MonoStockPortfolio.Entities
{
    public class Portfolio
    {
        public Portfolio() { }
        public Portfolio(long id) { ID = id; }

        public long? ID { get; private set; }
        public string Name { get; set; }
    }
}