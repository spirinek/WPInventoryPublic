using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WPInventory.BL.Reporting;
using WPInventory.Models;

namespace WPInventory.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingController : MediatorController
    {
        [HttpPost("ComputerListXlsxReport")]
        public async Task<IActionResult> ComputerListXlsxReport([FromBody] ComputerListXlsReportModel model)
        {
            var request = new ComputerListXlsxReportRequest()
            {
                Computers = model.Computers
            };

            var result = await Mediator.Send(request);
            if (result.FileBytes.Length == 0)
            {
                return NoContent();
            }

            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = result.Name,
                Inline = false,
                DispositionType = "attachment"
            };

            Response.Headers.Add("Content-Disposition", cd.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return File(result.FileBytes, result.Format, result.Name);
        }
    }
}
