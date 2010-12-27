using Android.Widget;

namespace MonoStockPortfolio
{
    public partial class MainActivity
    {
        public static string ClassName { get { return "monostockportfolio.MainActivity"; } }

        protected Button AddPortfolioButton { get { return FindViewById<Button>(Resource.id.btnAddPortfolio); } }
        protected ListView PortfolioListView { get { return FindViewById<ListView>(Resource.id.portfolioList); } }
    }
}