using System.Linq;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class StatusMaster : IStatusMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IStatusProcessor statusProcessor;
        private readonly ILogger logger;

        public StatusMaster(
            IAuthorizationProcessor authorizationProcessor,
            IStatusProcessor statusProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.statusProcessor = statusProcessor;
            logger = logManager.GetLogger(typeof(StatusMaster));
        }

        public async Task<StatusResult> GetStatusByCodeAsync(string SessionKey, int CompanyId, int StatusType, string Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await statusProcessor.GetAsync(new StatusSearch {
                    CompanyId   = CompanyId,
                    StatusType  = StatusType,
                    Codes       = new[] { Code },
                }, token)).FirstOrDefault();
                return new StatusResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Status = result,
                };
            }, logger);
        }


        public async Task<StatusesResult> GetStatusesByStatusTypeAsync(string SessionKey, int CompanyId, int StatusType)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await statusProcessor.GetAsync(new StatusSearch { CompanyId = CompanyId, StatusType = StatusType, }, token)).ToList();
                return new StatusesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Statuses = result,
                };
            }, logger);
        }

        public async Task<StatusResult> SaveAsync(string SessionKey, Status Status)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await statusProcessor.SaveAsync(Status, token);
                return new StatusResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Status = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await statusProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistReminderAsync(string SessionKey, int StatusId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await statusProcessor.ExistReminderAsync(StatusId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistReminderHistoryAsync(string SessionKey, int StatusId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await statusProcessor.ExistReminderHistoryAsync(StatusId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

    }
}
