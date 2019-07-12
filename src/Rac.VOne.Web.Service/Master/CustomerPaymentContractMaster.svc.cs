using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class CustomerPaymentContractMaster : ICustomerPaymentContractMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICustomerPaymentContractProcessor customerPaymentContractProcessor;
        private readonly ILogger logger;

        public CustomerPaymentContractMaster(
            IAuthorizationProcessor authorizationProcessor,
            ICustomerPaymentContractProcessor customerPaymentContractProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.customerPaymentContractProcessor = customerPaymentContractProcessor;
            logger = logManager.GetLogger(typeof(CustomerPaymentContractMaster));
        }

        public async Task<ExistResult> ExistAsync(int Id, string SessionKey)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await customerPaymentContractProcessor.ExistCategoryAsync(Id, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }
    }
}
