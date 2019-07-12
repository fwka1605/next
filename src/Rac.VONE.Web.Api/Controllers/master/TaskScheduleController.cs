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
    /// タスクスケジュールマスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskScheduleController : ControllerBase
    {
        private readonly ITaskScheduleProcessor taskScheduleProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public TaskScheduleController(
            ITaskScheduleProcessor taskScheduleProcessor
            )
        {
            this.taskScheduleProcessor = taskScheduleProcessor;
        }


        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件 CompanyId 必須</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< TaskSchedule >>> GetItems(TaskScheduleSearch option, CancellationToken token)
            => (await taskScheduleProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="TaskSchedule"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TaskSchedule>> Save(TaskSchedule TaskSchedule, CancellationToken token)
            => await taskScheduleProcessor.SaveAsync(TaskSchedule, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(TaskSchedule schedule, CancellationToken token)
            => await taskScheduleProcessor.DeleteAsync(schedule.CompanyId, schedule.ImportType, schedule.ImportSubType, token);

        /// <summary>
        /// 存在確認
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> Exists(TaskScheduleSearch option, CancellationToken token)
            => await taskScheduleProcessor.ExistsAsync(option, token);

    }
}
