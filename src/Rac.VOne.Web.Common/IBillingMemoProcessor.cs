using Rac.VOne.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IBillingMemoProcessor
    {
        Task<int> SaveMemoAsync(long BillingId, string BillingMemo, CancellationToken token = default(CancellationToken));
        Task<string> GetMemoAsync(long billingId, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(long BillingId, CancellationToken token = default(CancellationToken));
    }
}
