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
    /// 入金ヘッダー
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReceiptHeaderController : ControllerBase
    {
        private readonly IReceiptHeaderProcessor receiptHeaderProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public ReceiptHeaderController(
            IReceiptHeaderProcessor receiptHeaderProcessor
            )
        {
            this.receiptHeaderProcessor = receiptHeaderProcessor;
        }

        /// <summary>
        /// 取得 配列 振替対象となるヘッダーの一覧を取得
        /// </summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReceiptHeader>>> Get([FromBody] int companyId, CancellationToken token)
            => (await receiptHeaderProcessor.GetItemsAsync(companyId, token)).ToArray();

        /// <summary>
        /// 消込済のヘッダを更新
        /// </summary>
        /// <param name="option">更新条件
        /// 会社IDを指定 → 入金データが 一部消込以上の関連する ヘッダーを 割り当て済へ
        /// 入金IDを指定 → 該当の入金ID が属する ヘッダーを割り当て済へ
        /// ヘッダーIDを指定 → 該当のヘッダーを割り当て済へ
        /// </param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Update(ReceiptHeaderUpdateOption option, CancellationToken token)
            => await receiptHeaderProcessor.UpdateAsync(option, token);

    }
}
