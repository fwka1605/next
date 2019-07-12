using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Common
{
    public class BillingAccountTransferProcessor : IBillingAccountTransferProcessor
    {
        private readonly IBillingQueryProcessor billingQueryProcessor;
        private readonly IMasterGetIdByCodeQueryProcessor<Currency> currencyGetIdByCodeQueryProcessor;
        private readonly IUpdateCustomerQueryProcessor updateCustomerQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public BillingAccountTransferProcessor(
            IBillingQueryProcessor billingQueryProcessor,
            IMasterGetIdByCodeQueryProcessor<Currency> currencyGetIdByCodeQueryProcessor,
            IUpdateCustomerQueryProcessor updateCustomerQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingQueryProcessor = billingQueryProcessor;
            this.currencyGetIdByCodeQueryProcessor = currencyGetIdByCodeQueryProcessor;
            this.updateCustomerQueryProcessor = updateCustomerQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }
        public async Task<IEnumerable<Billing>> GetAsync(int companyId, int paymentAgencyId, DateTime transferDate, CancellationToken token = default(CancellationToken))
        {
            var currencyId = await currencyGetIdByCodeQueryProcessor.GetIdByCodeAsync(companyId, DefaultCurrencyCode, token);
            return await billingQueryProcessor.GetAccountTransferMatchingTargetListAsync(paymentAgencyId, transferDate, currencyId, token);
        }

        public async Task<IEnumerable<Billing>> ImportAsync(IEnumerable<AccountTransferImportData> datas, CancellationToken token = default(CancellationToken))
        {
            var result = new List<Billing>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var data in datas)
                {
                    foreach (var billingId in data.BillingIdList)
                    {
                        result.Add(await billingQueryProcessor.UpdateForAccountTransferImportAsync(billingId, data, token));
                    }

                    if (data.ResultCode == 0)
                    {
                        foreach (var id in data.CustomerIds)
                            await updateCustomerQueryProcessor.UpdateTransferNewCodeAsync(id, data.UpdateBy, "0");
                    }
                }
                scope.Complete();
                return result;
            }
        }


    }
}
