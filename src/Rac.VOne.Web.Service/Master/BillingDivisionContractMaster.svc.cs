using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class BillingDivisionContractMaster : IBillingDivisionContractMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IBillingDivisionContractProcessor billingDivisionContractProcessor;
        private readonly ILogger logger;
        public BillingDivisionContractMaster(
            IAuthorizationProcessor authorizationProcessor,
            IBillingDivisionContractProcessor billingDivisionContractProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.billingDivisionContractProcessor = billingDivisionContractProcessor;
            logger = logManager.GetLogger(typeof(BillingDivisionContractMaster));
        }

        public async Task<BillingDivisionContractsResult> GetByBillingIdsAsync(string sessionKey, long[] billingIds)
            => await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token => {
                var contracts = (await billingDivisionContractProcessor.GetAsync(new BillingDivisionContractSearch {
                    BillingIds      = billingIds,
                }, token)).ToList();
                return new BillingDivisionContractsResult {
                    BillingDivisionContracts = contracts,
                    ProcessResult = new ProcessResult { Result = true },
                };
            }, logger);

        public async Task<BillingDivisionContractsResult> GetByContractNumberAsync(string SessionKey, int CompanyId, int CustomerId, string ContractNumber)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = (await  billingDivisionContractProcessor.GetAsync(new BillingDivisionContractSearch {
                    CompanyId       = CompanyId,
                    CustomerId      = CustomerId,
                    ContractNumber  = ContractNumber,
                }, token)).ToList();
                return new BillingDivisionContractsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingDivisionContracts = result,
                };
            }, logger);

        public async Task<BillingDivisionContractsResult> GetByCustomerIdsAsync(string SessionKey, int CompanyId, int[] CustomerIds)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingDivisionContractProcessor.GetAsync(new BillingDivisionContractSearch {
                    CompanyId       = CompanyId,
                    CustomerIds     = CustomerIds,
                }, token)).ToList();
                return new BillingDivisionContractsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingDivisionContracts = result,
                };
            }, logger);
        }

        public async Task<BillingDivisionContractsResult> GetItemsAsync(string SessionKey, int CompanyId, int CustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingDivisionContractProcessor.GetAsync(new BillingDivisionContractSearch {
                    CompanyId       = CompanyId,
                    CustomerId      = CustomerId,
                }, token)).ToList();
                return new BillingDivisionContractsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingDivisionContracts = result,
                };
            }, logger);
        }

    }
}
