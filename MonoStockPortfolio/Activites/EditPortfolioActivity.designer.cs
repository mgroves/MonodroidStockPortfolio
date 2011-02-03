using Android.Widget;

namespace MonoStockPortfolio.Activites
{
    public partial class EditPortfolioActivity
    {
        public static string ClassName { get { return "monostockportfolio.activites.EditPortfolioActivity"; } }
        public static string Extra_PortfolioID { get { return "monoStockPortfolio.EditPortfolioActivity.PortfolioID"; } }
        public long ThisPortfolioId { get { return Intent.GetLongExtra(Extra_PortfolioID, -1); } }

        protected Button SaveButton { get { return FindViewById<Button>(Resource.Id.btnSave); } }
        protected EditText PortfolioName { get { return FindViewById<EditText>(Resource.Id.portfolioName); } }
    }
}