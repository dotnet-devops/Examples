using ClosedXML.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UtilityAccrual.DataAccess.Excel;


namespace Accounting.Web.Server.Controllers.UtilityAccrualControllers
{
    [Authorize(Roles="UtilityAccrualUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly AdjustmentController _ac;
        private readonly ExcelExporter _xl;

        public ExportController(AdjustmentController ac, ExcelExporter xl)
        {
            _ac = ac;
            _xl = xl;
        }

        [HttpGet("{month}/{year}")]
        public async Task<FileContentResult> Get(int month, int year)
        {
            var adjustments = await _ac.GetLatestAdjustments(month, year);
            var wb = _xl.ExportAdjustments(adjustments);
            var workbookBytes = new byte[0];
            using (var ms = new MemoryStream())
            {
                wb.SaveAs(ms);
                workbookBytes = ms.ToArray();
            };
            return File(workbookBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Adjustments-{month:00}-{year}.xlsx"
            );
        }        
    }
}
