using System;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using Rac.VOne.Common.TypeMapping;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class BillingMemoProcessor : IBillingMemoProcessor
    {
        private readonly IUpdateBillingMemoQueryProcessor updateBillingMemoQueryProcessor;
        private readonly IBillingMemoQueryProcessor billingMemoQueryProcessor;
        private readonly IDeleteBillingMemoQueryProcessor deleteBillingMemoQueryProcessor;

        public BillingMemoProcessor(
            IUpdateBillingMemoQueryProcessor updateBillingMemoQueryProcessor,
            IBillingMemoQueryProcessor billingMemoQueryProcessor,
            IDeleteBillingMemoQueryProcessor deleteBillingMemoQueryProcessor)
        {
            this.updateBillingMemoQueryProcessor = updateBillingMemoQueryProcessor;
            this.billingMemoQueryProcessor = billingMemoQueryProcessor;
            this.deleteBillingMemoQueryProcessor = deleteBillingMemoQueryProcessor;
        }

        public async Task<int> SaveMemoAsync(long BillingId, string BillingMemo, CancellationToken token = default(CancellationToken))
            => await updateBillingMemoQueryProcessor.SaveMemoAsync(BillingId, BillingMemo, token);

        public async Task<string> GetMemoAsync(long billingId, CancellationToken token = default(CancellationToken))
            => await billingMemoQueryProcessor.GetMemoAsync(billingId, token);

        public async Task<int> DeleteAsync(long BillingId, CancellationToken token = default(CancellationToken))
            => await deleteBillingMemoQueryProcessor.DeleteAsync(BillingId, token);

    }
}
