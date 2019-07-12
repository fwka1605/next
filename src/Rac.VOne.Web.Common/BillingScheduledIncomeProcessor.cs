using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    // TODO: remove
    public class BillingScheduledIncomeProcessor : IBillingScheduledIncomeProcessor
    {
        private readonly IAddBillingScheduledIncomeQueryProcessor billingScheduledIncomeQueryProcessor;

        public BillingScheduledIncomeProcessor(
            IAddBillingScheduledIncomeQueryProcessor billingScheduledIncome
            )
        {
            billingScheduledIncomeQueryProcessor = billingScheduledIncome;
        }

        public async Task<BillingScheduledIncome> SaveAsync(BillingScheduledIncome billingScheduledIncome, CancellationToken token = default(CancellationToken))
            => await billingScheduledIncomeQueryProcessor.SaveAsync(billingScheduledIncome, token);

    }
}
