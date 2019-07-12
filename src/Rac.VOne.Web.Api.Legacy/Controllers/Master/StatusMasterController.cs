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
    /// ステータスマスター 顛末管理用？
    /// </summary>
    public class StatusMasterController : ApiControllerAuthorized
    {
        private readonly IStatusProcessor statusProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public StatusMasterController(
            IStatusProcessor statusProcessor
            )
        {
            this.statusProcessor = statusProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件 会社ID、ステータスタイプを指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Status>> GetItems(StatusSearch option, CancellationToken token)
            => (await statusProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Status> Save(Status Status, CancellationToken token)
            => await statusProcessor.SaveAsync(Status, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="status">ID を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(Status status, CancellationToken token)
            => await statusProcessor.DeleteAsync(status.Id, token);

        /// <summary>
        /// 督促 データへの ステータスID の登録確認
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistReminder([FromBody] int statusId, CancellationToken token)
            => await statusProcessor.ExistReminderAsync(statusId, token);

        /// <summary>
        /// 督促履歴への ステータスIDの登録確認
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistReminderHistory([FromBody] int statusId, CancellationToken token)
            => await statusProcessor.ExistReminderHistoryAsync(statusId, token);

    }
}
