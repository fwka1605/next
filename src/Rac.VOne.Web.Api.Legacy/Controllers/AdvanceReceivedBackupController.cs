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
    ///  前受分割処理
    /// </summary>
    public class AdvanceReceivedBackupController : ApiControllerAuthorized
    {
        private readonly IAdvanceReceivedBackupProcessor advanceReceivedBackupProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public AdvanceReceivedBackupController(
            IAdvanceReceivedBackupProcessor advanceReceivedBackupProcessor
            )
        {
            this.advanceReceivedBackupProcessor = advanceReceivedBackupProcessor;
        }

        /// <summary>
        /// 取得 単一
        /// </summary>
        /// <param name="OriginalReceiptId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task< AdvanceReceivedBackup > GetByOriginalReceiptId([FromBody] long OriginalReceiptId, CancellationToken token)
            => await advanceReceivedBackupProcessor.GetByOriginalReceiptIdAsync(OriginalReceiptId, token);


        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="ids">前受ID 配列</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable< AdvanceReceivedBackup >> GetByIds([FromBody] IEnumerable<long> ids, CancellationToken token)
            => (await advanceReceivedBackupProcessor.GetByIdAsync(ids, token)).ToArray();
    }
}
