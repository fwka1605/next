using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class BillingDivisionContractProcessor : IBillingDivisionContractProcessor
    {
        private readonly IBillingDivisionContractQueryProcessor billingDivisionContractQueryProcessor;
        private readonly IUpdateBillingDivisionContractQueryProcessor updateBillingDivisionContractQueryProcessor;
        private readonly IDeleteBillingDivisionContractQueryProcessor deleteBillingDivisionContractQueryProcessor;

        public BillingDivisionContractProcessor(
            IBillingDivisionContractQueryProcessor billingDivisionContractQueryProcessor,
            IUpdateBillingDivisionContractQueryProcessor updateBillingDivisionContractQueryProcessor,
            IDeleteBillingDivisionContractQueryProcessor deleteBillingDivisionContractQueryProcessor
            )
        {
            this.billingDivisionContractQueryProcessor = billingDivisionContractQueryProcessor;
            this.updateBillingDivisionContractQueryProcessor = updateBillingDivisionContractQueryProcessor;
            this.deleteBillingDivisionContractQueryProcessor = deleteBillingDivisionContractQueryProcessor;
        }

        public async Task<int> UpdateAsync(long BillingId, int UpdateBy, CancellationToken token = default(CancellationToken))
            => await updateBillingDivisionContractQueryProcessor.UpdateWithBillingIdAsync(BillingId, UpdateBy);

        public async Task<int> DeleteAsync(long BillingId, CancellationToken token = default(CancellationToken))
            => await deleteBillingDivisionContractQueryProcessor.DeleteWithBillingIdAsync(BillingId);

        public async Task<IEnumerable<BillingDivisionContract>> GetAsync(BillingDivisionContractSearch option, CancellationToken token = default(CancellationToken))
            => await billingDivisionContractQueryProcessor.GetItemsAsync(option, token);
    }
}
