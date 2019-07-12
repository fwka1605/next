using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IBillingDueAtModifySearchQueryProcessor
    {
        /// <summary>入金予定日変更用 <see cref="Billing.BillingInputId"/>が同一のものは集約する</summary>
        /// <param name="option"></param>
        Task<IEnumerable<BillingDueAtModify>> GetAsync(BillingSearch option, CancellationToken token = default(CancellationToken));
    }
}
