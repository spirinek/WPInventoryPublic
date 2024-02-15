using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WPInventory.BL.Reporting
{
    public class XlsxAllComputersReportResult 
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public byte[] FileBytes { get; set; }
    }
}
