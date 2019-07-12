using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Web.Common
{
    public class ScheduledPaymentListProcessor : IScheduledPaymentListProcessor
    {
        private readonly IScheduledPaymentListQueryProcessor scheduledPaymentListQueryProcessor;

        public ScheduledPaymentListProcessor(IScheduledPaymentListQueryProcessor scheduledPaymentListQueryProcessor)
        {
            this.scheduledPaymentListQueryProcessor = scheduledPaymentListQueryProcessor;
        }

        public async Task<IEnumerable<ScheduledPaymentList>> GetAsync(ScheduledPaymentListSearch option, CancellationToken token = default(CancellationToken))
            => await scheduledPaymentListQueryProcessor.GetAsync(option, token);
    }
}
