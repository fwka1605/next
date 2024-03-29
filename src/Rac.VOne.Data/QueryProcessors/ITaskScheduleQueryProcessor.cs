﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ITaskScheduleQueryProcessor
    {
        Task<IEnumerable<TaskSchedule>> GetAsync(TaskScheduleSearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExistsAsync(TaskScheduleSearch option, CancellationToken token = default(CancellationToken));
        Task<TaskSchedule> SaveAsync(TaskSchedule TaskSchedule, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int CompanyId, int ImportType, int ImportSubType, CancellationToken token = default(CancellationToken));
    }
}
