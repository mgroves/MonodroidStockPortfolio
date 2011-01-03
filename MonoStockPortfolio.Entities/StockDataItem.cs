namespace MonoStockPortfolio.Entities
{
    public enum StockDataItem
    {
        [StringValue("Change")]
        Change = 0,
        [StringValue("Gain/Loss")]
        GainLoss = 1,
        [StringValue("Ticker")]
        Ticker = 2,
        [StringValue("Time")]
        Time = 3,
        [StringValue("Volume")]
        Volume = 4,
        [StringValue("Price")]
        LastTradePrice = 5,
        [StringValue("Price-RT")]
        RealTimeLastTradeWithTime = 6,
        [StringValue("Change-RT")]
        ChangeRealTime = 7,
        [StringValue("Gain/Loss-RT")]
        GainLossRealTime = 8
    }
}