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
    /// MF明細連携 明細コントローラー
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MfAggrTransactionController : ControllerBase
    {
        private readonly IMfAggrTransactionProcessor mfAggrTransactionProcessor;

        /// <summary>constructor</summary>
        public MfAggrTransactionController(
            IMfAggrTransactionProcessor mfAggrTransactionProcessor
            )
        {
            this.mfAggrTransactionProcessor = mfAggrTransactionProcessor;
        }

        /// <summary>
        /// MF明細連携取得
        /// </summary>
        /// <param name="option">検索条件 CompanyId の指定は必須</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MfAggrTransaction[]> Get(MfAggrTransactionSearch option, CancellationToken token)
            => (await mfAggrTransactionProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// MF明細連携 登録済の transaction.id を取得
        /// </summary>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long[]> GetIds(CancellationToken token)
            => (await mfAggrTransactionProcessor.GetIdsAsync(token)).ToArray();

        /// <summary>
        /// MF明細連携 登録 （入金は自動的に作成）
        /// </summary>
        /// <param name="transactions">登録する transaction 配列 入金区分ID, 通貨ID, 入金部門ID などの指定が必要</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Save([FromBody] MfAggrTransaction[] transactions, CancellationToken token)
            => await mfAggrTransactionProcessor.SaveAsync(transactions, token);


        /// <summary>
        /// MF明細連携 最終登録レコードを取得 未登録の場合は null
        /// </summary>
        /// <param name="option">検索条件 CompanyId を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MfAggrTransaction> GetLastOne(MfAggrTransactionSearch option, CancellationToken token)
            => (await mfAggrTransactionProcessor.GetLastOneAsync(option, token)).FirstOrDefault();

        /// <summary>
        /// MF明細連携 データ削除
        /// </summary>
        /// <param name="ids">削除する MF明細連携ID の配列 <see cref="MfAggrTransaction.Id"/></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete([FromBody] long[] ids, CancellationToken token)
            => await mfAggrTransactionProcessor.DeleteAsync(ids, token);

    }
}