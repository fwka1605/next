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
    public class BillingJournalizingProcessor : IBillingJournalizingProcessor
    {
        private readonly IBillingJournalizingQueryProcessor billingJournalizingQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public BillingJournalizingProcessor(
            IBillingJournalizingQueryProcessor billingJournalizingQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingJournalizingQueryProcessor = billingJournalizingQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken))
            => await billingJournalizingQueryProcessor.GetAsync(option, token);

        public async Task<IEnumerable<BillingJournalizing>> ExtractAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken))
            => await billingJournalizingQueryProcessor.ExtractAsync(option, token);

        public async Task<IEnumerable<Billing>> UpdateAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            IEnumerable<Billing> result = null;
            using (var scope = transactionScopeBuilder.Create())
            {
                result = (await billingJournalizingQueryProcessor.UpdateAsync(option, token)).ToArray();
                scope.Complete();
            }
            return result;
        }

        public async Task<IEnumerable<Billing>> CancelAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            IEnumerable<Billing> result = null;
            using (var scope = transactionScopeBuilder.Create())
            {
                result = (await billingJournalizingQueryProcessor.CancelAsync(option, token)).ToArray();
                scope.Complete();
            }
            return result;
        }

    }
}
