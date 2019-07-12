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

namespace Rac.VOne.Web.Service.Master
{
    public class EBFormatMaster : IEBFormatMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IEBFormatProcessor ebFormatProcessor;
        private readonly ILogger logger;

        public EBFormatMaster(
            IAuthorizationProcessor authorizationProcessor,
            IEBFormatProcessor ebFormatProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.ebFormatProcessor = ebFormatProcessor;
            logger = logManager.GetLogger(typeof(EBFormatMaster));
        }

        public async Task<EBFormatsResult> GetItemsAsync(string sessionKey)
            => await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token => {
                var result = (await ebFormatProcessor.GetAsync(token)).ToList();
                return new EBFormatsResult {
                    ProcessResult = new ProcessResult { Result = true },
                    EBFileFormats = result,
                };
            }, logger);

    }
}
