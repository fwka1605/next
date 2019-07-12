using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  口座振替
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountTransferController : ControllerBase
    {
        private readonly IAccountTransferProcessor accounttransferProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="accounttransferProcessor"></param>
        public AccountTransferController(
            IAccountTransferProcessor accounttransferProcessor
            )
        {
            this.accounttransferProcessor = accounttransferProcessor;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Cancel(IEnumerable<AccountTransferLog> logs, CancellationToken token)
            => await accounttransferProcessor.CancelAsync(logs, token);

        /// <summary>
        /// 抽出
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<AccountTransferExtractResult>> Extract(AccountTransferSearch option, CancellationToken token)
        {
            var result = new AccountTransferExtractResult();
            result.Details = (await accounttransferProcessor.ExtractAsync(option, token)).ToArray();
            result.Logs = result.Details.SelectMany(x => x.GetInvalidLogs()).ToArray();
            return result;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AccountTransferLog>>> Get([FromBody] int CompanyId, CancellationToken token)
            => (await accounttransferProcessor.GetAsync(CompanyId, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="details"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AccountTransferDetail>>> Save(IEnumerable<AccountTransferDetail> details, CancellationToken token)
            => (await accounttransferProcessor.SaveAsync(details, token)).ToList();
    }
}
