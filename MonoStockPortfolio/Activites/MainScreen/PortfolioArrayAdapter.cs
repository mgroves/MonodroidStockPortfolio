using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Activites.MainScreen
{
    public class PortfolioArrayAdapter : ArrayAdapter<string>
    {
        private readonly IList<Portfolio> _results;

        public PortfolioArrayAdapter(Context context, int resource, IList<Portfolio> results, IList<string> labels) 
            : base(context, resource, labels)
        {
            _results = results;
        }

        public override long GetItemId(int position)
        {
            return _results[position].ID ?? -1;
        }
    }
}