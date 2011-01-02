using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using System.Linq;

namespace MonoStockPortfolio.Framework
{
    public abstract class GenericArrayAdapter<T> : BaseAdapter<T>
    {
        private readonly IList<T> _items;
        protected readonly Context Context;

        protected GenericArrayAdapter(Context context, IEnumerable<T> results)
        {
            _items = results.ToList();
            Context = context;
        }

        public override int Count
        {
            get { return _items.Count(); }
        }

        public override T this[int position]
        {
            get { return _items[position]; }
        }
    }
}