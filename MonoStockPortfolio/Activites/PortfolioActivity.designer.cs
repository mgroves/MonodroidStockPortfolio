using Android.Widget;

namespace MonoStockPortfolio.Activites
{
    public partial class PortfolioActivity
    {
        public static string ClassName { get { return "monostockportfolio.activites.PortfolioActivity"; } }

        public static string Extra_PortfolioID { get { return "monoStockPortfolio.PortfolioActivity.PortfolioID"; } }
        public long ThisPortofolioId { get { return Intent.GetLongExtra(Extra_PortfolioID, -1); } }
    }
}