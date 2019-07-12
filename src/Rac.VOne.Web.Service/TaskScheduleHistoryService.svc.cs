using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class TaskScheduleHistoryService : ITaskScheduleHistoryService
    {
        private readonly ILogger logger;
        private readonly IAuthorizationProcessor authorizationProcess;
        private readonly ITaskScheduleHistoryProcessor taskScheduleHistoryProcessor;
        public TaskScheduleHistoryService(IAuthorizationProcessor authorization,
        ITaskScheduleHistoryProcessor taskScheduleHistory,
        ILogManager logManager)
        {
            authorizationProcess = authorization;
            taskScheduleHistoryProcessor = taskScheduleHistory;
            logger = logManager.GetLogger(typeof(TaskScheduleHistory));
        }

        public async Task<TaskScheduleHistoryResult> GetItemsAsync(string SessionKey, TaskScheduleHistorySearch searchConditions)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await taskScheduleHistoryProcessor.GetAsync(searchConditions, token)).ToList();

                return new TaskScheduleHistoryResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    TaskScheduleHistoryList = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await taskScheduleHistoryProcessor.DeleteAsync(CompanyId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<TaskScheduleHistoryResult> SaveAsync(string SessionKey, TaskScheduleHistory history)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await taskScheduleHistoryProcessor.SaveAsync(history, token);
                return new TaskScheduleHistoryResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    TaskScheduleHistoryList = new List<TaskScheduleHistory> { result },
                };
            }, logger);
        }
    }
}
