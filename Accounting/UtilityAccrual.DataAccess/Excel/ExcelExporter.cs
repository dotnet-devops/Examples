using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityAccrual.Shared.Models.Display;

namespace UtilityAccrual.DataAccess.Excel
{
    public class ExcelExporter
    {

        private int row = 2;

        public XLWorkbook ExportAdjustments(IEnumerable<AdjustmentDisplayModel> adjustments)
        {
            if (!adjustments.Any())
                return null;

            string month = adjustments.Select(adj => adj.Adjustment.Month).FirstOrDefault().ToString();
            string year = DateTime.Now.Year.ToString();
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Adjustments");
            SetColumns(ws);
            SetHeader(ws);
            SetRows(ws, adjustments);
            ws.Columns(1, 4).AdjustToContents();
            return wb;
        }

        private void SetHeader(IXLWorksheet ws)
        {
            ws.SheetView.FreezeRows(1);
            ws.FirstRow().Cells(1, 13).SetDataType(XLDataType.Text);
            ws.FirstRow().Cells(1, 13).Style.Border.SetBottomBorder(XLBorderStyleValues.Double);
            ws.FirstRow().Cells(1, 13).Style.Font.FontSize = 14;
            ws.FirstRow().Cell(1).Value = "Redacted";
        }

        private void SetColumns(IXLWorksheet ws)
        {
            ws.Column(1).DataType = XLDataType.Text;
            ws.Column(2).DataType = XLDataType.Number;
            ws.Column(3).DataType = XLDataType.Number;
            SetColumnToAccounting(ws.Column(4));
        }

        private void SetColumnToAccounting(IXLColumn column)
        {
            column.DataType = XLDataType.Number;
            column.Style.NumberFormat.Format = "_ * # ##0.00_ ;_ * -# ##0.00_ ;_ * \"-\"??_ ;_ @_ ";
            column.Style.NumberFormat.SetNumberFormatId(43);
        }

        private void SetRows(IXLWorksheet ws, IEnumerable<AdjustmentDisplayModel> adjustments)
        {
            row = 2;
            foreach (var a in adjustments)
            {
                ws.Row(row).Cell(1).Value = a.Utility.Redacted;
                ws.Row(row).Cell(2).Value = a.Utility.Redacted;
                ws.Row(row).Cell(3).Value = a.Utility.Redacted;
                ws.Row(row).Cell(4).Value = a.Utility.Redacted;
                row += 1;
            }
        }
    }
}
