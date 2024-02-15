using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WPInventory.BL.Computers;
using WPInventory.BL.Renderers;
using WPInventory.Data;
using WPInventory.Data.Models.Entities;

namespace WPInventory.BL.Reporting
{
    public class GetXlsxReport : IRequestHandler<ComputerListXlsxReportRequest,
        XlsxAllComputersReportResult>
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly ComputerInfoContext _dbContext;
        private readonly ILogger<GetComputersSimpleModel> _logger;

        public GetXlsxReport(ComputerInfoContext dbContext, ILogger<GetComputersSimpleModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<XlsxAllComputersReportResult> Handle(ComputerListXlsxReportRequest request, CancellationToken cancellationToken)
        {
           var computers = await _dbContext.Computers.Where(x => !x.IsArchived && request.Computers.Contains(x.Guid))
                .Include(x => x.RAMs)
                .Include(x => x.MotherBoard)
                .Include(x => x.CPUs)
                .Include(x => x.NWAdapters)
                .Include(x => x.ScanDates)
                .Include(x => x.Monitors)
                .Include(x => x.PhisicalDisks)
                .Include(x => x.ScanDates)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            var renderModel = new XlsxRenderer("Computers");
            foreach (var computer in computers)
            {
                renderModel["ComputerName"].Add(computer.Name ?? string.Empty);
                renderModel["User"].Add(computer.Description ?? string.Empty);
                renderModel["CPU"].Add(computer.CPUs.FirstOrDefault()?.Name.Trim() ?? string.Empty);
                renderModel["MotherBoard"].Add(computer.MotherBoard.Manufacturer + computer.MotherBoard.Product);
                renderModel["RAM"].Add(computer.RAMs.ToList().Sum(y => y.Capacity).ToString());
                renderModel["Monitors"].Add(string.Join(", ", computer.Monitors.ToList().Select(y => y.Name)));
                renderModel["MAC"].Add(computer.NWAdapters.FirstOrDefault(y => !string.IsNullOrEmpty(y.MAC))?.MAC ?? string.Empty);
            }

            var result = new XlsxAllComputersReportResult
            {
                FileBytes = renderModel.Render(),
                Format = XlsxContentType,
                Name = "VmmComputers.xlsx"
            };

            return result;
        }
    }
}
