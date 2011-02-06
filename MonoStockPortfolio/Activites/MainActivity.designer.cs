using Android.Widget;

namespace MonoStockPortfolio.Activites
{
    public partial class MainActivity
    {
        public static string ClassName { get { return "monostockportfolio.activites.MainActivity"; } }

        protected Button AddPortfolioButton { get { return FindViewById<Button>(Resource.Id.btnAddPortfolio); } }
        protected ListView PortfolioListView { get { return FindViewById<ListView>(Resource.Id.portfolioList); } }
    }
}