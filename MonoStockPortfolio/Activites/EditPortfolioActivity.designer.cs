using Android.Widget;

namespace MonoStockPortfolio.Activites
{
    public partial class EditPortfolioActivity
    {
        public static string ClassName { get { return "monostockportfolio.activites.EditPortfolioActivity"; } }
        public static string Extra_PortfolioID { get { return "monoStockPortfolio.EditPortfolioActivity.PortfolioID"; } }

        protected Button SaveButton { get { return FindViewById<Button>(Resource.id.btnSave); } }
        protected EditText PortfolioName { get { return FindViewById<EditText>(Resource.id.portfolioName); } }
    }
}