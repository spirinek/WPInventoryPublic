using System;
using System.Collections.Generic;
using System.Text;

namespace WPInventory.Worker.BackgroundService.PropCreators.SearchedPropModels
{
    public class SearchedNWAdapter : SearchedItem
    {
        public int NetConnectionStatus { get; set; }
        public string ProductName { get; set; }
        public string MACAddress { get; set; }
        public string ServiceName { get; set; }
    }
}
