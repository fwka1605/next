using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Spreadsheets;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Reports.Settings;
using static Rac.VOne.Web.Common.Constants;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// 帳票 帳票ごとに、Controller 自体を分ける
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportSettingProcessor reportSettingProcessor;
        private readonly IArrearagesListProcessor arrearagesListProcessor;
        private readonly IScheduledPaymentListProcessor scheduledPaymentListProcessor;

        private readonly IArrearagesListSpreadsheetProcessor arrearagesListSpreadsheetProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public ReportController(
            IReportSettingProcessor reportSettingProcessor,
            IArrearagesListProcessor arrearagesListProcessor,
            IScheduledPaymentListProcessor scheduledPaymentListProcessor,
            IArrearagesListSpreadsheetProcessor arrearagesListSpreadsheetProcessor
            )
        {
            this.reportSettingProcessor = reportSettingProcessor;
            this.arrearagesListProcessor = arrearagesListProcessor;
            this.scheduledPaymentListProcessor = scheduledPaymentListProcessor;
            this.arrearagesListSpreadsheetProcessor = arrearagesListSpreadsheetProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< ArrearagesList >>> ArrearagesList(ArrearagesListSearch option, CancellationToken token)
        {
            option.ReportSettings = (await reportSettingProcessor.GetAsync(option.CompanyId, nameof(PF0401), token)).ToList();
            return (await arrearagesListProcessor.GetAsync(option, token)).ToArray();
        }




        /// <summary>
        /// 帳票 Spreadsheet の取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetArrearagesSpreadsheet(ArrearagesListSearch option, CancellationToken token)
        {
            option.ReportSettings = (await reportSettingProcessor.GetAsync(option.CompanyId, nameof(PF0401), token)).ToList();
            var content = await arrearagesListSpreadsheetProcessor.GetAsync(option, token);
            return File(content, SpreadsheetContentType, "arrearages-spreadsheet.xlsx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< ScheduledPaymentList >>> ScheduledPaymentList(ScheduledPaymentListSearch option, CancellationToken token)
        {
            option.ReportSettings = (await reportSettingProcessor.GetAsync(option.CompanyId, nameof(PF0301), token)).ToList();
            return (await scheduledPaymentListProcessor.GetAsync(option, token)).ToArray();
        }

    }
}
