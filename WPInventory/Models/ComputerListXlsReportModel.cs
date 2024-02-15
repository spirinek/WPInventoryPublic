using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPInventory.Models
{
    public class ComputerListXlsReportModel
    {
        public ICollection<Guid> Computers { get; set; }
    }
}
