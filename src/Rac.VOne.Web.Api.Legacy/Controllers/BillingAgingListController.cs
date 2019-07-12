using Microsoft.AspNet.SignalR;
using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Common;
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
    ///  請求残高年齢表
    /// </summary>
    public class BillingAgingListController : ApiControllerAuthorized
    {
        private readonly IBillingAgingListProcessor billingAgingListProcessor;
        private readonly IBillingAgingListSpreadsheetProcessor billingAgingListSpreadsheetProcessor;
        private readonly IHubContext hubContext;
        /// <summary>
        /// constructor
        /// </summary>
        public BillingAgingListController(
            IBillingAgingListProcessor billingAgingListProcessor,
            IBillingAgingListSpreadsheetProcessor billingAgingListSpreadsheetProcessor
            )
        {
            this.billingAgingListProcessor = billingAgingListProcessor;
            this.billingAgingListSpreadsheetProcessor = billingAgingListSpreadsheetProcessor;
            this.hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
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
        public async Task<IEnumerable<BillingAgingList>> GetR(BillingAgingListSearch option)
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
        public async Task<IEnumerable<BillingAgingList>> Get(BillingAgingListSearch option, CancellationToken token)
            => (await billingAgingListProcessor.GetAsync(option, null, token)).ToArray();

        /// <summary>
        /// データ取得 spreadsheet
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetSpreadsheet(BillingAgingListSearch option, CancellationToken token)
        {
            var content = await billingAgingListSpreadsheetProcessor.GetAsync(option, null, token);
            return Request.GetSpreadsheetResponseMessage(content, "billing-aging-list.xlsx");
        }

        /// <summary>
        /// 明細取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<BillingAgingListDetail>> GetDetailsAsync(BillingAgingListSearch option, CancellationToken token)
            => (await billingAgingListProcessor.GetDetailsAsync(option, token)).ToArray();

    }
}
