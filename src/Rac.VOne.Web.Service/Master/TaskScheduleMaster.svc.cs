using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class TaskScheduleMaster : ITaskScheduleMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ITaskScheduleProcessor taskScheduleProcessor;
        private readonly ILogger logger;

        public TaskScheduleMaster(
            IAuthorizationProcessor authorizationProcessor,
            ITaskScheduleProcessor taskScheduleProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.taskScheduleProcessor = taskScheduleProcessor;
            logger = logManager.GetLogger(typeof(TaskScheduleMaster));
        }

        public Task<TaskScheduleResult> GetAsync(string SessionKey, int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<TaskSchedulesResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await taskScheduleProcessor.GetAsync(new TaskScheduleSearch { CompanyId = CompanyId, }, token)).ToList();

                return new TaskSchedulesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    TaskSchedules = result,
                };
            }, logger);
        }

        public async Task<TaskScheduleResult> SaveAsync(string SessionKey, TaskSchedule TaskSchedule)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await taskScheduleProcessor.SaveAsync(TaskSchedule);

                return new TaskScheduleResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    TaskSchedule = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, int ImportType, int ImportSubType)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await taskScheduleProcessor.DeleteAsync(CompanyId, ImportType, ImportSubType);

                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistsAsync(string SessionKey, int CompanyId, int ImportType, int ImportSubType)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await taskScheduleProcessor.ExistsAsync(new TaskScheduleSearch {
                    CompanyId       = CompanyId,
                    ImportType      = ImportType,
                    ImportSubType   = ImportSubType,
                }, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }
    }
}
