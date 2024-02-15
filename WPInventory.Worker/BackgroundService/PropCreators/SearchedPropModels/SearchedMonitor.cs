using System;
using System.Collections.Generic;
using System.Text;

namespace WPInventory.Worker.BackgroundService.PropCreators.SearchedPropModels
{
    public class SearchedMonitor : SearchedItem
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string YearOfManufacture { get; set; }
    }
}
