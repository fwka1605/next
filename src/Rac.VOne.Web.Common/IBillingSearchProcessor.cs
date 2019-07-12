using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IBillingSearchProcessor
    {
        Task<IEnumerable<Billing>> GetAsync(BillingSearch option, CancellationToken token = default(CancellationToken));
    }
}
