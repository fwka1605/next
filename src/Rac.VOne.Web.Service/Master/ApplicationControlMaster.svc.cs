using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class ApplicationControlMaster : IApplicationControlMaster
    {
        private readonly ILogger logger;
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;

        public ApplicationControlMaster(
            IAuthorizationProcessor authorizationProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            logger = logManager.GetLogger(typeof(ApplicationControlMaster));
        }

        public async Task< ApplicationControlResult> GetAsync(string sessionKey, int companyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await applicationControlProcessor.GetAsync(companyId, token);
                return new ApplicationControlResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ApplicationControl = result,
                };
            }, logger);
        }

        public async Task<CountResult> SaveAsync(string sessionKey, ApplicationControl applicationControl)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await applicationControlProcessor.UpdateUseOperationLogAsync(applicationControl, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task< ApplicationControlResult> AddAsync(string sessionKey, ApplicationControl applicationControl)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await applicationControlProcessor.AddAsync(applicationControl, token);
                return new ApplicationControlResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ApplicationControl = result,
                };
            }, logger);
        }
    }
}
