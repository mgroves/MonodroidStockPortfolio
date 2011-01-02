namespace MonoStockPortfolio.Entities
{
    public enum StockDataItem
    {
        [StringValue("Change")]
        Change,
        [StringValue("Gain/Loss")]
        GainLoss,
        [StringValue("Ticker")]
        Ticker,
        [StringValue("Time")]
        Time,
        [StringValue("Volume")]
        Volume,
        [StringValue("Price")]
        LastTradePrice,
        [StringValue("Price-RT")]
        RealTimeLastTradeWithTime,
        [StringValue("Change-RT")]
        ChangeRealTime,
        [StringValue("Gain/Loss-RT")]
        GainLossRealTime
    }
}