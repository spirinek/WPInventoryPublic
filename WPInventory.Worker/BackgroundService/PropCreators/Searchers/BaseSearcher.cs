using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using WPInventory.Worker.BackgroundService.PropCreators.SearchedPropModels;

namespace WPInventory.Worker.BackgroundService.PropCreators.Searchers
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
