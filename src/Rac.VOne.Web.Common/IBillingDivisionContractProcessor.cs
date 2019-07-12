using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IBillingDivisionContractProcessor
    {
        Task<int> UpdateAsync(long BillingId, int UpdateBy, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(long BillingId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<BillingDivisionContract>> GetAsync(BillingDivisionContractSearch option, CancellationToken token = default(CancellationToken));
    }
}
