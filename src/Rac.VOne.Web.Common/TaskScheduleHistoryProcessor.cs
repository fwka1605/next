using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{

    public class TaskScheduleHistoryProcessor : ITaskScheduleHistoryProcessor
    {
        private readonly IAddTaskScheduleHistoryQueryProcessor addTaskScheduleHistoryQueryProcessor;
        private readonly ITaskScheduleHistoryQueryProcessor taskScheduleHistoryQueryProcessor;
        private readonly IDeleteByCompanyQueryProcessor<TaskScheduleHistory> deleteTaskScheduleHistoryQueryProcessor;

        public TaskScheduleHistoryProcessor(
            IAddTaskScheduleHistoryQueryProcessor addTaskScheduleHistoryQueryProcessor,
            ITaskScheduleHistoryQueryProcessor taskScheduleHistoryQueryProcessor,
            IDeleteByCompanyQueryProcessor<TaskScheduleHistory> deleteTaskScheduleHistoryQueryProcessor
            )
        {
            this.addTaskScheduleHistoryQueryProcessor = addTaskScheduleHistoryQueryProcessor;
            this.taskScheduleHistoryQueryProcessor = taskScheduleHistoryQueryProcessor;
            this.deleteTaskScheduleHistoryQueryProcessor = deleteTaskScheduleHistoryQueryProcessor;
        }

        public async Task<IEnumerable<TaskScheduleHistory>> GetAsync(TaskScheduleHistorySearch option, CancellationToken token = default(CancellationToken))
            => await taskScheduleHistoryQueryProcessor.GetAsync(option, token);

        public async Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await deleteTaskScheduleHistoryQueryProcessor.DeleteAsync(CompanyId, token);

        public async Task<TaskScheduleHistory> SaveAsync(TaskScheduleHistory history, CancellationToken token = default(CancellationToken))
            => await addTaskScheduleHistoryQueryProcessor.AddAsync(history, token);



    }
}
