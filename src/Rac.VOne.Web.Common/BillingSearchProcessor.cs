using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class BillingSearchProcessor : IBillingSearchProcessor
    {
        private readonly IBillingSearchQueryProcessor billingSearchQueryProcessor;

        public BillingSearchProcessor(
            IBillingSearchQueryProcessor billingSearchQueryProcessor
            )
        {
            this.billingSearchQueryProcessor = billingSearchQueryProcessor;
        }

        public async Task<IEnumerable<Billing>> GetAsync(BillingSearch option, CancellationToken token = default(CancellationToken))
            => await billingSearchQueryProcessor.GetAsync(option, token);

    }
}