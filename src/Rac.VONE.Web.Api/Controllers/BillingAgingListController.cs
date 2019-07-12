using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Spreadsheets;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Api.Extensions;
using Rac.VOne.Web.Api.Hubs;
using static Rac.VOne.Web.Common.Constants;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  請求残高年齢表
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillingAgingListController : ControllerBase
    {
        private readonly IBillingAgingListProcessor billingAgingListProcessor;
        private readonly IBillingAgingListSpreadsheetProcessor billingAgingListSpreadsheetProcessor;
        private readonly IHubContext<ProgressHub> hubContext;

        /// <summary>
        /// constructor
        /// </summary>
        public BillingAgingListController(
            IBillingAgingListProcessor billingAgingListProcessor,
            IBillingAgingListSpreadsheetProcessor billingAgingListSpreadsheetProcessor,
            IHubContext<ProgressHub> hubContext
            )
        {
            this.billingAgingListProcessor = billingAgingListProcessor;
            this.billingAgingListSpreadsheetProcessor = billingAgingListSpreadsheetProcessor;
            this.hubContext = hubContext;

        }

        /// <summary>
        /// データ取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        /// <remarks>signalr の 進捗状況連携あり
        /// web api 呼出し前に、connectionId を取得する
        /// 取得後は、<see cref="BillingAgingListSearch.ConnectionId"/>へ値をセットし、連携する
        /// client 側に 更新が来るので、正常に受取 進捗状況の表現を行う
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BillingAgingList>>> GetR(BillingAgingListSearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token)
                => (await billingAgingListProcessor.GetAsync(option, notifier, token)).ToArray());

        /// <summary>
        /// データ取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        /// <remarks>signalr の 進捗状況連携なし
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BillingAgingList>>> Get(BillingAgingListSearch option, CancellationToken token)
            => (await billingAgingListProcessor.GetAsync(option, null, token)).ToArray();

        /// <summary>
        /// データ取得 spreadsheet
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetSpreadsheet(BillingAgingListSearch option, CancellationToken token)
        {
            var content = await billingAgingListSpreadsheetProcessor.GetAsync(option, null, token);
            return File(content, SpreadsheetContentType, "billing-aging-list.xlsx");
        }

        /// <summary>
        /// 明細取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BillingAgingListDetail>>> GetDetailsAsync(BillingAgingListSearch option, CancellationToken token)
            => (await billingAgingListProcessor.GetDetailsAsync(option, token)).ToArray();



    }
}
