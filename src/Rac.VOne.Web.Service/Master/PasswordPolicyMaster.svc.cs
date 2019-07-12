using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class PasswordPolicyMaster : IPasswordPolicyMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IPasswordPolicyProcessor passwordPolicyProcessor;
        private readonly ILogger logger;

        public PasswordPolicyMaster(
            IAuthorizationProcessor authorizationProcessor,
            IPasswordPolicyProcessor passwordPolicyProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.passwordPolicyProcessor = passwordPolicyProcessor;
            logger = logManager.GetLogger(typeof(PasswordPolicyMaster));
        }

        public async Task<PasswordPolicyResult> GetAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await passwordPolicyProcessor.GetAsync(CompanyId, token);
                return new PasswordPolicyResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PasswordPolicy = result,
                };
            }, logger);
        }

        public async Task<PasswordPolicyResult> SaveAsync(string SessionKey, PasswordPolicy PasswordPolicy)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await passwordPolicyProcessor.SaveAsync(PasswordPolicy, token);
                return new PasswordPolicyResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PasswordPolicy = result,
                };
            }, logger);
        }
    }
}
