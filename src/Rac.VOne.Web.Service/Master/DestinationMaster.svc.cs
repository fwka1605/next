using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class DestinationMaster : IDestinationMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IDestinationProcessor destinationProcessor;
        private readonly ILogger logger;

        public DestinationMaster(
            IAuthorizationProcessor authorizationProcessor,
            IDestinationProcessor destinationProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.destinationProcessor = destinationProcessor;
            logger = logManager.GetLogger(typeof(DestinationMaster));
        }

        public async Task<DestinationsResult> GetItemsAsync(string SessionKey, DestinationSearch option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = (await destinationProcessor.GetAsync(option, token)).ToList();
                return new DestinationsResult {
                    Destinations = result,
                    ProcessResult = new ProcessResult { Result = true },
                };
            }, logger);

        public async Task<DestinationResult> SaveAsync(string SessionKey, Destination Destination)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await destinationProcessor.SaveAsync(Destination, token);
                return new DestinationResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Destination = result,
                };
            }, logger);

        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await destinationProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);

    }
}
