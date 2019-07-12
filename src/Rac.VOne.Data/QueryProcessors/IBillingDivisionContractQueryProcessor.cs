using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IBillingDivisionContractQueryProcessor
    {
        Task<IEnumerable<BillingDivisionContract>> GetItemsAsync(BillingDivisionContractSearch option, CancellationToken token = default(CancellationToken));
        Task<long> GetNewContractNumberAsync(int cmpanyId, CancellationToken token = default(CancellationToken));
    }
}
