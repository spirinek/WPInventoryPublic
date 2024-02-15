using System;
using System.Collections.Generic;
using System.Text;

namespace WPInventory.Worker.BackgroundService.PropCreators.SearchedPropModels
{
    public class SearchedCPU : SearchedItem
    {
        public string Name { get; set; }
        public int NumberOfCores { get; set; }
        public string MaxClockSpeed { get; set; }
        public string ProcessorId { get; set; }
    }
}
