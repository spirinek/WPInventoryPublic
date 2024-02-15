using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace WPInventory.BL.Reporting
{
    public class ComputerListXlsxReportRequest : IRequest<XlsxAllComputersReportResult>
    {
        public ICollection<Guid> Computers { get; set; }
    }
}
