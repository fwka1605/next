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
    /// タスクスケジュール実行履歴
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskScheduleHistoryController : ControllerBase
    {
        private readonly ITaskScheduleHistoryProcessor taskScheduleHistoryProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="taskScheduleHistoryProcessor"></param>
        public TaskScheduleHistoryController(
            ITaskScheduleHistoryProcessor taskScheduleHistoryProcessor
            )
        {
            this.taskScheduleHistoryProcessor = taskScheduleHistoryProcessor;
        }

        /// <summary>
        /// 取得配列
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TaskScheduleHistory>>> GetItems(TaskScheduleHistorySearch option, CancellationToken token)
            => (await taskScheduleHistoryProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(int CompanyId, CancellationToken token)
            => await taskScheduleHistoryProcessor.DeleteAsync(CompanyId, token);

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="history"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TaskScheduleHistory>> Save(TaskScheduleHistory history, CancellationToken token)
            => await taskScheduleHistoryProcessor.SaveAsync(history, token);

    }
}
