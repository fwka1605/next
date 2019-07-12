using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteMatchingBillingDiscountQueryProcessor
    {
        Task<int> DeleteByMatchingIdAsync(long MatchingId, CancellationToken token = default(CancellationToken));

    }
}
