using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace WPInventory.BL.Renderers
{
    public class XlsxRenderer
    {
        public string WorkSheetName { get; set; }
        public List<(string Header, List<string> Values)> CellValues { get; set; }

        public List<string> this[string header]
        {
            get
            {
                var collection = CellValues.FirstOrDefault(x => x.Header == header).Values;
                if (collection == null)
                {
                    CellValues.Add((header,new List<string>()));
                }
                return CellValues.FirstOrDefault(x => x.Header == header).Values;
            }
        }
        public XlsxRenderer(string workSheetName)
        {
            WorkSheetName = workSheetName;
            CellValues = new List<(string Header, List<string> Values)>();
        }

        public byte[] Render()
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(WorkSheetName);

            for (int i = 0; i < CellValues.Count; i++)
            {
                var cells = worksheet.Cells[1, i + 1];
                cells.Value = CellValues[i].Header;
                cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cells.Style.Fill.BackgroundColor.SetColor(1,155, 194,230);

                for (int j = 0; j < CellValues[i].Values.Count; j++)
                {
                    var cell = worksheet.Cells[j + 2, i + 1];
                    cell.Value = CellValues[i].Values[j];
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
                cells.Style.Border.BorderAround(ExcelBorderStyle.Medium);
            }

            worksheet.Cells[1, 1, 1, CellValues.Count].AutoFilter = true;

            worksheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }
    }
}
