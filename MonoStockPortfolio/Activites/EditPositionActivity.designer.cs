using Android.Widget;

namespace MonoStockPortfolio.Activites
{
    public partial class EditPositionActivity
    {
        public static string ClassName { get { return "monostockportfolio.activites.EditPositionActivity"; } }
        public static string Extra_PortfolioID { get { return "monoStockPortfolio.EditPositionActivity.PortfolioID"; } }
        public long ThisPortfolioId { get { return Intent.GetLongExtra(Extra_PortfolioID, -1); } }
        public static string Extra_PositionID { get { return "monoStockPortfolio.EditPositionActivity.PositionID"; } }
        public long ThisPositionId { get { return Intent.GetLongExtra(Extra_PositionID, -1); } }

        protected EditText TickerTextBox { get { return FindViewById<EditText>(Resource.id.addPositionTicker); } }
        protected EditText PriceTextBox { get { return FindViewById<EditText>(Resource.id.addPositionPrice); } }
        protected EditText SharesTextBox { get { return FindViewById<EditText>(Resource.id.addPositionShares); } }
        protected Button SaveButton { get { return FindViewById<Button>(Resource.id.addPositionSaveButton); } }
    }
}