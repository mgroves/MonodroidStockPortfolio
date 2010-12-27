using Android.Widget;

namespace MonoStockPortfolio.Activites
{
    public partial class AddPositionActivity
    {
        public static string ClassName { get { return "monostockportfolio.activites.AddPositionActivity"; } }
        public static string Extra_PortfolioID { get { return "monoStockPortfolio.AddPositionActivity.PortfolioID"; } }

        protected EditText TickerTextBox { get { return FindViewById<EditText>(Resource.id.addPositionTicker); } }
        protected EditText PriceTextBox { get { return FindViewById<EditText>(Resource.id.addPositionPrice); } }
        protected EditText SharesTextBox { get { return FindViewById<EditText>(Resource.id.addPositionShares); } }
        protected Button SaveButton { get { return FindViewById<Button>(Resource.id.addPositionSaveButton); } }
    }
}