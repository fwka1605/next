using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class MenuAuthorityMaster : IMenuAuthorityMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IMenuAuthorityProcessor menuAuthorityProcessor;
        private readonly ILogger logger;

        public MenuAuthorityMaster(
            IAuthorizationProcessor authorizationProcessor,
            IMenuAuthorityProcessor menuAuthorityProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.menuAuthorityProcessor = menuAuthorityProcessor;
            logger = logManager.GetLogger(typeof(MenuAuthorityMaster));
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await menuAuthorityProcessor.DeleteAsync(CompanyId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
            throw new NotImplementedException();
        }

        public async  Task<MenuAuthoritiesResult> GetItemsAsync(string SessionKey, int? CompanyId, int? LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await menuAuthorityProcessor.GetAsync(new MenuAuthoritySearch {
                    CompanyId   = CompanyId,
                    LoginUserId = LoginUserId,
                }, token)).ToList();

                return new MenuAuthoritiesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MenuAuthorities = result,
                };
            }, logger);
        }

        public async Task<MenuAuthoritiesResult> SaveAsync(string SessionKey, MenuAuthority[] menuAuthorities)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await menuAuthorityProcessor.SaveAsync(menuAuthorities, token)).ToList();
                return new MenuAuthoritiesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MenuAuthorities = result,
                };
            }, logger);
        }
    }
}
