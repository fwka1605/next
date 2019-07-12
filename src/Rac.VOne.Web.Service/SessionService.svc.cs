using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Data;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class SessionService : ISessionService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ISessionStorageProcessor sessionStorageProcessor;
        private readonly ILogger logger;

        public SessionService(
            IAuthorizationProcessor authorizationProcessor,
            ISessionStorageProcessor sessionStorageProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.sessionStorageProcessor = sessionStorageProcessor;
            logger = logManager.GetLogger(typeof(SessionService));
        }

        public async Task<ConnectionInfoResult> GetConnectionInfoAsync(string SessionKey)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await sessionStorageProcessor.GetAsync(SessionKey, token);

                return new ConnectionInfoResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ConnectionInfo = result.ConnectionInfo,
                };
            }, logger);
        }
    }
}
