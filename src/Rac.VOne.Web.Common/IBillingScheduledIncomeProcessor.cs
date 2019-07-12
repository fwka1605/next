using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Web.Common
{
    // TODO: remove
    public interface IBillingScheduledIncomeProcessor
    {
        Task<BillingScheduledIncome> SaveAsync(BillingScheduledIncome billingScheduledIncome, CancellationToken token = default(CancellationToken));
    }
}
