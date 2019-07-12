using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface ITaskScheduleHistoryProcessor
    {
        Task<IEnumerable<TaskScheduleHistory>> GetAsync(TaskScheduleHistorySearch option, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken));

        Task<TaskScheduleHistory> SaveAsync(TaskScheduleHistory history, CancellationToken token = default(CancellationToken));
    }
}
