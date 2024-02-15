using System.Collections.Generic;
using WPInventory.BL.Searching.SearchedPropModels;

namespace WPInventory.BL.Searching.Searchers
{
    public abstract class BaseSearcher<TItem> where TItem: SearchedItem
    {
        protected bool _searched;
        protected readonly List<TItem> _items = new List<TItem>();
        
        public List<TItem> Items
        {
            get
            {
                if (!_searched)
                {
                    Search();
                }
                return _items;
            }
        }
        protected virtual void Search() { }
    }
}
