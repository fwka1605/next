using Rac.VOne.Common.Logging;
using System;
using System.Threading.Tasks;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class ControlColorMaster : IControlColorMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IControlColorProcessor controlcolorProcessor;
        private readonly ILogger logger;

        public ControlColorMaster(
            IAuthorizationProcessor authorizationProcessor,
            IControlColorProcessor controlcolorProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.controlcolorProcessor = controlcolorProcessor;
            logger = logManager.GetLogger(typeof(ControlColorMaster));
        }

        public async Task<ControlColorResult> GetAsync(string SessionKey, int CompanyId, int LoginuserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await controlcolorProcessor.GetAsync(CompanyId, LoginuserId, token);
                return new ControlColorResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Color = (result == null) ? null : new ControlColor[] { result },
                };
            }, logger);
        }

        public Task<ControlColorsResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            throw new NotImplementedException();
        }

        public async Task<ControlColorResult> SaveAsync(string SessionKey, ControlColor ControlColor)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await controlcolorProcessor.SaveAsync(ControlColor, token);
                return new ControlColorResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Color = (result == null) ? null : new ControlColor[] { result },
                };
            }, logger);
        }

        public Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, int LoginUserId)
        {
            throw new NotImplementedException();
        }
    }
}
