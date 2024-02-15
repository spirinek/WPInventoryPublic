using System;
using System.Collections.Generic;
using System.Text;

namespace WPInventory.Worker.BackgroundService.PropCreators.SearchedPropModels
{
    public class SearchedHDD : SearchedItem
    {
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
    }
}
