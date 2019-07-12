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
    /// �^�X�N�X�P�W���[���}�X�^�[
    /// </summary>
    public class TaskScheduleController : ApiControllerAuthorized
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
        /// �擾 �z��
        /// </summary>
        /// <param name="option">�������� CompanyId �K�{</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable< TaskSchedule >> GetItems(TaskScheduleSearch option, CancellationToken token)
            => (await taskScheduleProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// �o�^
        /// </summary>
        /// <param name="TaskSchedule"></param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TaskSchedule> Save(TaskSchedule TaskSchedule, CancellationToken token)
            => await taskScheduleProcessor.SaveAsync(TaskSchedule, token);

        /// <summary>
        /// �폜
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(TaskSchedule schedule, CancellationToken token)
            => await taskScheduleProcessor.DeleteAsync(schedule.CompanyId, schedule.ImportType, schedule.ImportSubType, token);

        /// <summary>
        /// ���݊m�F
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Exists(TaskScheduleSearch option, CancellationToken token)
            => await taskScheduleProcessor.ExistsAsync(option, token);

    }
}
