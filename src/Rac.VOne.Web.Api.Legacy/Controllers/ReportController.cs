using Rac.VOne.Client.Reports.Settings;
using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Common.Spreadsheets;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// 帳票 帳票ごとに、Controller 自体を分ける
    /// </summary>
    public class ReportController : ApiControllerAuthorized
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
        public async Task<IEnumerable< ArrearagesList >> ArrearagesList(ArrearagesListSearch option, CancellationToken token)
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
        public async Task<HttpResponseMessage> GetArrearagesSpreadsheet(ArrearagesListSearch option, CancellationToken token)
        {
            option.ReportSettings = (await reportSettingProcessor.GetAsync(option.CompanyId, nameof(PF0401), token)).ToList();
            var content = await arrearagesListSpreadsheetProcessor.GetAsync(option, token);
            return Request.GetSpreadsheetResponseMessage(content, "arrearages-spreadsheet.xlsx");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable< ScheduledPaymentList >> ScheduledPaymentList(ScheduledPaymentListSearch option, CancellationToken token)
        {
            option.ReportSettings = (await reportSettingProcessor.GetAsync(option.CompanyId, nameof(PF0301), token)).ToList();
            return (await scheduledPaymentListProcessor.GetAsync(option, token)).ToArray();
        }

    }
}
