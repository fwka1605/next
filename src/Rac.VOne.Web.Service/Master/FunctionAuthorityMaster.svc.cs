using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class FunctionAuthorityMaster : IFunctionAuthorityMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IFunctionAuthorityProcessor functionAuthorityProcessor;
        private readonly ILogger logger;

        private readonly ILoginUserProcessor loginUserProcessor;

        public FunctionAuthorityMaster (
            IAuthorizationProcessor authorizationProcessor,
            IFunctionAuthorityProcessor functionAuthorityProcessor,

            ILoginUserProcessor loginUserProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.functionAuthorityProcessor = functionAuthorityProcessor;

            this.loginUserProcessor = loginUserProcessor;

            logger = logManager.GetLogger(typeof(FunctionAuthorityMaster));
        }

        public Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, int FunctionType, int AuthorityLevel)
        {
            throw new NotImplementedException();
        }

        public Task<FunctionAuthorityResult> GetAsync(string SessionKey, int CompanyId, int FunctionType, int AuthorityLevel)
        {
            throw new NotImplementedException();
        }

        public Task<FunctionAuthoritiesResult> GetByFunctionTypeAsync(string SessionKey, int CompanyId, int FunctionType)
        {
            throw new NotImplementedException();
        }

        public async Task<FunctionAuthoritiesResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await functionAuthorityProcessor.GetAsync(new FunctionAuthoritySearch { CompanyId = CompanyId }, token)).ToList();
                return new FunctionAuthoritiesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    FunctionAuthorities = result,
                };
            }, logger);
        }

        public async Task<FunctionAuthoritiesResult> GetByLoginUserAsync(string SessionKey, int CompanyId, string LoginUserCode, int[] FunctionType)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var user = (await loginUserProcessor.GetAsync(new LoginUserSearch {
                    CompanyId       = CompanyId,
                    Codes           = new[] { LoginUserCode },
                }, token)).First();
                var result = (await functionAuthorityProcessor.GetAsync(new FunctionAuthoritySearch {
                    CompanyId       = CompanyId,
                    LoginUserId     = user.Id,
                    FunctionTypes   = FunctionType,
                }, token)).ToList();
                return new FunctionAuthoritiesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    FunctionAuthorities = result.ToList(),
                };
            }, logger);
        }

        public async Task<FunctionAuthoritiesResult> SaveAsync(string SessionKey, FunctionAuthority[] functionAuthorities)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var list = (await functionAuthorityProcessor.SaveAsync(functionAuthorities, token)).ToList();
                return new FunctionAuthoritiesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    FunctionAuthorities = list,
                };
            }, logger);
        }
    }
}
