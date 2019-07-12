using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddBillingDiscountQueryProcessor
    {
        Task<int> SaveAsync(BillingDiscount discount, CancellationToken token = default(CancellationToken));
    }
}
