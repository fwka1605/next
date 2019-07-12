using System;
using System.Collections.Generic;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IScheduledPaymentListProcessor
    {
        Task<IEnumerable<ScheduledPaymentList>> GetAsync(ScheduledPaymentListSearch option, CancellationToken token = default(CancellationToken));
    }
}
