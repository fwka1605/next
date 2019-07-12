using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class DataMaintenanceService : IDataMaintenanceService
    {
        private IAuthorizationProcessor authorizationProcessor;
        private IDataMaintenanceProcessor dataMaintenanceProcessor;
        private readonly ILogger logger;

        public DataMaintenanceService(
            IAuthorizationProcessor authorizationProcessor,
            IDataMaintenanceProcessor dataMaintenanceProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.dataMaintenanceProcessor = dataMaintenanceProcessor;
            logger = logManager.GetLogger(typeof(SessionService));
        }

        public async Task<CountResult> DeleteDataAsync(string SessionKey, DateTime deleteDate)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await dataMaintenanceProcessor.DeleteDataAsync(deleteDate, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }
    }
}
