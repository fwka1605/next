using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class BillingDueAtModifyProcessor : IBillingDueAtModifyProcessor
    {
        private readonly IBillingDueAtModifySearchQueryProcessor billingDueAtModifySearchQueryProcessor;
        private readonly IUpdateBillingDueAtModifyQueryProcessor updateBillingDueAtModifyQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public BillingDueAtModifyProcessor(
            IBillingDueAtModifySearchQueryProcessor billingDueAtModifySearchQueryProcessor,
            IUpdateBillingDueAtModifyQueryProcessor updateBillingDueAtModifyQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingDueAtModifySearchQueryProcessor = billingDueAtModifySearchQueryProcessor;
            this.updateBillingDueAtModifyQueryProcessor = updateBillingDueAtModifyQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<BillingDueAtModify>> GetAsync(BillingSearch option, CancellationToken token = default(CancellationToken))
            => await billingDueAtModifySearchQueryProcessor.GetAsync(option, token);


        public async Task<IEnumerable<Billing>> UpdateAsync(IEnumerable<BillingDueAtModify> billings, CancellationToken token = default(CancellationToken))
        {
            var result = new Dictionary<long, Billing>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var billing in billings)
                {
                    if (billing.NewDueAt.HasValue)
                    {
                        var dic = (await updateBillingDueAtModifyQueryProcessor.UpdateDueAtAsync(billing, token)).ToDictionary(x => x.Id);
                        foreach (var key in dic.Keys)
                        {
                            if (result.ContainsKey(key))
                                result[key] = dic[key];
                            else
                                result.Add(key, dic[key]);
                        }
                    }
                    if (billing.NewCollectCategoryId.HasValue)
                    {
                        var dic = (await updateBillingDueAtModifyQueryProcessor.UpdateCollectCategoryIdAsync(billing, token)).ToDictionary(x => x.Id);
                        foreach (var key in dic.Keys)
                        {
                            if (result.ContainsKey(key))
                                result[key] = dic[key];
                            else
                                result.Add(key, dic[key]);
                        }
                    }
                }
                scope.Complete();
            }
            return result.Values;
        }

    }
}
