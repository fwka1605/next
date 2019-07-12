using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// 入金メモ
    /// </summary>
    public class ReceiptMemoController : ApiControllerAuthorized
    {
        private readonly IReceiptMemoProcessor receiptMemoProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="receiptMemoProcessor"></param>
        public ReceiptMemoController(
            IReceiptMemoProcessor receiptMemoProcessor
            )
        {
            this.receiptMemoProcessor = receiptMemoProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="receiptIds"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReceiptMemo>> GetItems([FromBody] IEnumerable<long> receiptIds, CancellationToken token)
            => (await receiptMemoProcessor.GetItemsAsync(receiptIds, token)).ToArray();

    }
}
