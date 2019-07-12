using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class InputControlMaster : IInputControlMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IInputControlProcessor inputControlProcessor;
        private readonly ILogger logger;

        public InputControlMaster(
            IAuthorizationProcessor authorizationProcessor,
            IInputControlProcessor inputControlProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.inputControlProcessor = inputControlProcessor;
            logger = logManager.GetLogger(typeof(InputControlMaster));
        }

        public async Task<InputControlsResult> GetAsync(string SessionKey, int CompanyId, int LoginUserId, int InputGridTypeId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await inputControlProcessor.GetAsync(new InputControl {
                    CompanyId       = CompanyId,
                    LoginUserId     = LoginUserId,
                    InputGridTypeId = InputGridTypeId,
                }, token)).ToList();

                return new InputControlsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InputControls = result,
                };
            }, logger);
        }

        public async Task<InputControlsResult> SaveAsync(string SessionKey, int CompanyId, int LoginUserId, int InputGridTypeId,
            InputControl[] InputControl)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await inputControlProcessor.SaveAsync(InputControl, token)).ToList();
                return new InputControlsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InputControls = result,
                };
            }, logger);
        }

    }
}
