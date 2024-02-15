using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WPInventory.BL.Computers;

namespace WPInventory.Models
{
    public class ComputerFilter
    {
        public bool IncludeArchived { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public FilterDateType? DateType { get; set; }
    }
    
}
