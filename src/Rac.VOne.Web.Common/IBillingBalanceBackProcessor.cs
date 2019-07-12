using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IBillingBalanceBackProcessor
    {
        Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<BillingBalanceBack>> SaveAsync(int CompanyId, CancellationToken token = default(CancellationToken));
    }
}
