using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class TaskScheduleProcessor  : ITaskScheduleProcessor
    {
        private readonly ITaskScheduleQueryProcessor taskScheduleQueryProcessor;

        public TaskScheduleProcessor(
            ITaskScheduleQueryProcessor taskScheduleQueryProcessor)
        {
            this.taskScheduleQueryProcessor = taskScheduleQueryProcessor;
        }


        public async Task<IEnumerable<TaskSchedule>> GetAsync(TaskScheduleSearch option, CancellationToken token = default(CancellationToken))
            => await taskScheduleQueryProcessor.GetAsync(option, token);

        public async Task<bool> ExistsAsync(TaskScheduleSearch option, CancellationToken token = default(CancellationToken))
            => await taskScheduleQueryProcessor.ExistsAsync(option, token);

        public async Task<TaskSchedule> SaveAsync(TaskSchedule TaskSchedule, CancellationToken token = default(CancellationToken))
            => await taskScheduleQueryProcessor.SaveAsync(TaskSchedule, token);

        public async Task<int> DeleteAsync(int CompanyId, int ImportType, int ImportSubType, CancellationToken token = default(CancellationToken))
            => await taskScheduleQueryProcessor.DeleteAsync(CompanyId, ImportType, ImportSubType, token);

    }
}
