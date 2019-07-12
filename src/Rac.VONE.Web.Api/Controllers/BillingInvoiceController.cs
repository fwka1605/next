using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Rac.VOne.Web.Api.Extensions;
using Rac.VOne.Web.Api.Hubs;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  請求書発行
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillingInvoiceController : ControllerBase
    {
        private readonly IBillingInvoiceProcessor billingInvoiceProcessor;
        private readonly IHubContext<ProgressHub> hubContext;

        /// <summary>
        /// constructor
        /// </summary>
        public BillingInvoiceController(
            IBillingInvoiceProcessor billingInvoiceProcessor,
            IHubContext<ProgressHub> hubContext
            )
        {
            this.billingInvoiceProcessor = billingInvoiceProcessor;
            this.hubContext = hubContext;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">非同期 進捗結果を受ける場合、事前に ConnectionId に値を設定</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BillingInvoice>>> GetR(BillingInvoiceSearch option)
        {
            return await hubContext.DoAsync(option.ConnectionId, async (notifier, token) => {
                var result = (await billingInvoiceProcessor.GetAsync(option, token)).ToArray();
                notifier?.UpdateState();
                return result;
            });
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">非同期 進捗結果を受ける場合、事前に ConnectionId に値を設定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BillingInvoice>>> Get(BillingInvoiceSearch option, CancellationToken token)
            => (await billingInvoiceProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<BillingInputResult>> PublishInvoicesR(BillingInvoicePublishSource source)
            => await hubContext.DoAsync(source.ConnectionId, async (notifier, token)
                => await billingInvoiceProcessor.PublishInvoicesAsync(source.Items, source.LoginUserId, token));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<BillingInputResult>> PublishInvoices(BillingInvoicePublishSource source, CancellationToken token)
            => await billingInvoiceProcessor.PublishInvoicesAsync(source.Items, source.LoginUserId, token);

        /// <summary>
        /// 取得 印刷用 配列
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BillingInvoiceDetailForPrint>>> GetDetailsForPrint(
            BillingInvoiceDetailSearch option, CancellationToken token)
            => (await billingInvoiceProcessor.GetDetailsForPrintAsync(option, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> GetCountR(BillingInvoiceSearch option)
        {
            return await hubContext.DoAsync(option.ConnectionId, async (notifier, token) =>
            {
                var result = (await billingInvoiceProcessor.GetAsync(option, token)).Count();
                notifier?.UpdateState();
                return result;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> GetCount(BillingInvoiceSearch option, CancellationToken token)
            => (await billingInvoiceProcessor.GetAsync(option, token)).Count();

        /// <summary>
        /// ワークデータの削除
        /// </summary>
        /// <param name="option">ClientKey のみ指定 </param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> DeleteWorkTable(BillingInvoiceSearch option, CancellationToken token)
            => await billingInvoiceProcessor.DeleteWorkTableAsync(option.ClientKey, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">ConnectionId, BillingIds のみ指定 </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> CancelPublishR(BillingInvoiceSearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token)
                => (await billingInvoiceProcessor.CancelPublishAsync(option.BillingIds, token)).Count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">ConnectionId, BillingIds のみ指定 </param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> CancelPublish(BillingInvoiceSearch option, CancellationToken token)
            => (await billingInvoiceProcessor.CancelPublishAsync(option.BillingIds, token)).Count;

        /// <summary>
        /// パラメータのモデル化
        /// </summary>
        /// <param name="option">CompanyId, ConnectionId, BillingInputIds のみ指定 </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BillingInvoiceDetailForExport>>> GetDetailsForExportR(BillingInvoiceSearch option)
        {
            return await hubContext.DoAsync(option.ConnectionId, async (notifier, token) =>
            {
                var result = (await billingInvoiceProcessor.GetDetailsForExportAsync(option.BillingInputIds, option.CompanyId, token)).ToList();
                notifier?.UpdateState();
                return result;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">CompanyId, ConnectionId, BillingInputIds のみ指定 </param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BillingInvoiceDetailForExport>>> GetDetailsForExport(BillingInvoiceSearch option, CancellationToken token)
            => (await billingInvoiceProcessor.GetDetailsForExportAsync(option.BillingInputIds, option.CompanyId, token)).ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">ConnectionId, BillingInputIds のみ指定</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> UpdatePublishAtR(BillingInvoiceSearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token)
                => (await billingInvoiceProcessor.UpdatePublishAtAsync(option.BillingInputIds, token)).Count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">ConnectionId, BillingInputIds のみ指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> UpdatePublishAt(BillingInvoiceSearch option, CancellationToken token)
            => (await billingInvoiceProcessor.UpdatePublishAtAsync(option.BillingInputIds, token)).Count;

    }
}
