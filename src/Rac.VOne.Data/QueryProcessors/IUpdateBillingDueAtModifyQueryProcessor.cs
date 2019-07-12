using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateBillingDueAtModifyQueryProcessor
    {
        Task<IEnumerable<Billing>> UpdateDueAtAsync(BillingDueAtModify billing, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Billing>> UpdateCollectCategoryIdAsync(BillingDueAtModify billing, CancellationToken token = default(CancellationToken));
    }
}
