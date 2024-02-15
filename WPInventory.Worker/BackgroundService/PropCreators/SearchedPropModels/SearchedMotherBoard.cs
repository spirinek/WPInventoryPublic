using System;
using System.Collections.Generic;
using System.Text;

namespace WPInventory.Worker.BackgroundService.PropCreators.SearchedPropModels
{
    public class SearchedMotherBoard : SearchedItem
    {
        public string Manufacturer { get; set; }
        public string Product { get; set; }
    }
}
