using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IBillingInputProcessor
    {
        Task<BillingInput> SaveAsync(CancellationToken token = default(CancellationToken));

        Task<BillingInput> UpdateForPublishAsync(BillingInputSource billingInputsource, CancellationToken token = default(CancellationToken));

        Task<int> UpdateForCancelPublishAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken));

        Task<int> DeleteForCancelPublishAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken));
    }
}
