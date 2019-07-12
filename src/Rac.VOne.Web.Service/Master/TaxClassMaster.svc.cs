using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class TaxClassMaster : ITaxClassMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ITaxClassProcessor taxClassProcessor;
        private readonly ILogger logger;

        public TaxClassMaster(
            IAuthorizationProcessor authorizationProcessor,
            ITaxClassProcessor taxClassProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.taxClassProcessor = taxClassProcessor;
            logger = logManager.GetLogger(typeof(TaxClassMaster));
        }

        public async Task<TaxClassResult> GetItemsAsync(string sessionKey)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = (await taxClassProcessor.GetAsync(token)).ToList();
                return new TaxClassResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    TaxClass = result,
                };
            }, logger);
        }
    }
}
