using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// ステータスマスター 顛末管理用？
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatusMasterController : ControllerBase
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
        public async Task<ActionResult<IEnumerable<Status>>> GetItems(StatusSearch option, CancellationToken token)
            => (await statusProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Status>> Save(Status Status, CancellationToken token)
            => await statusProcessor.SaveAsync(Status, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="status">ID を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(Status status, CancellationToken token)
            => await statusProcessor.DeleteAsync(status.Id, token);

        /// <summary>
        /// 督促 データへの ステータスID の登録確認
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistReminder([FromBody] int statusId, CancellationToken token)
            => await statusProcessor.ExistReminderAsync(statusId, token);

        /// <summary>
        /// 督促履歴への ステータスIDの登録確認
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistReminderHistory([FromBody] int statusId, CancellationToken token)
            => await statusProcessor.ExistReminderHistoryAsync(statusId, token);

    }
}
