using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IBillingDueAtModifyProcessor
    {
        Task<IEnumerable<BillingDueAtModify>> GetAsync(BillingSearch option, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Billing>> UpdateAsync(IEnumerable<BillingDueAtModify> billings, CancellationToken token = default(CancellationToken));
    }
}
