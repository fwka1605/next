using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class LoginUserPasswordMaster : ILoginUserPasswordMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ILoginUserPasswordProcessor loginUserPasswordProcessor;
        private readonly ILogger logger;

        public LoginUserPasswordMaster(
            IAuthorizationProcessor authorizationProcessor,
            ILoginUserPasswordProcessor loginUserPasswordProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.loginUserPasswordProcessor = loginUserPasswordProcessor;
            logger = logManager.GetLogger(typeof(LoginUserPasswordMaster));
        }

        public async Task<LoginPasswordChangeResult> ChangeAsync(
            string SessionKey, int CompanyId, int LoginUserId, string OldPassword, string NewPassword)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await loginUserPasswordProcessor.ChangeAsync(CompanyId, LoginUserId, OldPassword, NewPassword, token);
                return new LoginPasswordChangeResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Result = result,
                };
            }, logger);
        }

        public async Task<LoginProcessResult> LoginAsync(string SessionKey, int CompanyId, int LoginUserId, string Password)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await loginUserPasswordProcessor.LoginAsync(CompanyId, LoginUserId, Password, token);
                return new LoginProcessResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Result = result,
                };
            }, logger);
        }

        public async Task<LoginPasswordChangeResult> SaveAsync(string SessionKey, int CompanyId, int LoginUserId, string Password)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await loginUserPasswordProcessor.SaveAsync(CompanyId, LoginUserId, Password, token);

                return new LoginPasswordChangeResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Result = result,
                };
            }, logger);
        }
    }
}
