using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateBillingDivisionContractQueryProcessor
    {
        Task<int> UpdateBillingAsync(BillingDivisionContract contract, CancellationToken token = default(CancellationToken));
        Task<int> UpdateWithBillingIdAsync(long billingId, int loginUserId, CancellationToken token = default(CancellationToken));
    }
}
