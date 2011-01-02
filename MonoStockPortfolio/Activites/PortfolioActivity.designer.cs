using Android.Widget;

namespace MonoStockPortfolio.Activites
{
    public partial class PortfolioActivity
    {
        public static string ClassName { get { return "monostockportfolio.activites.PortfolioActivity"; } }

        public static string Extra_PortfolioID { get { return "monoStockPortfolio.PortfolioActivity.PortfolioID"; } }
        public long ThisPortofolioId { get { return Intent.GetLongExtra(Extra_PortfolioID, -1); } }

        protected ListView QuoteListview { get { return FindViewById<ListView>(Resource.id.quoteListview); } }
        protected Button AddPositionButton { get { return FindViewById<Button>(Resource.id.btnAddPosition); } }
        protected LinearLayout QuoteListviewHeader { get { return FindViewById<LinearLayout>(Resource.id.quoteHeaderLayout); } }
    }
}