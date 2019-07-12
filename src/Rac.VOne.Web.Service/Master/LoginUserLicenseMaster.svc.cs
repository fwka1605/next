using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using NLog;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class LoginUserLicenseMaster : ILoginUserLicenseMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ILoginUserLicenseProcessor loginUserLicenseProcessor;
        private readonly ILogger logger;

        public LoginUserLicenseMaster(IAuthorizationProcessor authorizationProcessor,
            ILoginUserLicenseProcessor loginUserLicenseProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.loginUserLicenseProcessor = loginUserLicenseProcessor;
            logger = logManager.GetLogger(typeof(LoginUserLicense));
        }

        public async Task<LoginUserLicensesResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await loginUserLicenseProcessor.GetAsync(CompanyId, token)).ToList();
                return new LoginUserLicensesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    LoginUserLicenses = result,
                };
            }, logger);

            throw new NotImplementedException();
        }

        public async Task<LoginUserLicensesResult> SaveAsync(string SessionKey, int CompanyId, string[] LicenseKeys)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var licenses = LicenseKeys.Select(x => new LoginUserLicense { CompanyId = CompanyId, LicenseKey = x }).ToArray();
                var result = (await loginUserLicenseProcessor.SaveAsync(licenses, token)).ToList();


                return new LoginUserLicensesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    LoginUserLicenses = result
                };
            }, logger);
        }
    }
}
