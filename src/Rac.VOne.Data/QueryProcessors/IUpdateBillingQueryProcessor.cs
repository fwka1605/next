using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateBillingQueryProcessor
    {
        Task<Billing> UpdateScheduledPaymentAsync(Billing billing, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Billing>> UpdateScheduledPaymentForDueAtAsync(Billing billing, CancellationToken token = default(CancellationToken));
        Task<int> UpdateForScheduledPaymentAsync(Billing billing, CancellationToken token = default(CancellationToken));
        Task<int> UpdateForScheduledPaymentSameCustomersAsync(int companyId, int updateBy, int updateAll, IEnumerable<long> targetBillingIds, IEnumerable<int> targetBillingCustomerIds, CancellationToken token = default(CancellationToken));
        Task<Billing> UpdateAsync(Billing Billing, CancellationToken token = default(CancellationToken));
        Task<int> UpdateDeleteAtAsync(IEnumerable<long> ids, int loginUserId, CancellationToken token = default(CancellationToken));

    }
}
