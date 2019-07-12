using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data;
using Entities = Rac.VOne.Data.Entities;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class BillingInputProcessor : IBillingInputProcessor
    {
        private readonly IAddBillingInputQueryProcessor addBillingInputQueryProcessor;

        public BillingInputProcessor(
            IAddBillingInputQueryProcessor addBillingInputQueryProcessor
            )
        {
            this.addBillingInputQueryProcessor = addBillingInputQueryProcessor;
        }

        public Task<BillingInput> SaveAsync(CancellationToken token = default(CancellationToken))
            => addBillingInputQueryProcessor.AddAsync(token);

        public Task<BillingInput> UpdateForPublishAsync(BillingInputSource billingInputsource, CancellationToken token = default(CancellationToken))
            => addBillingInputQueryProcessor.UpdateForPublishAsync(billingInputsource, token);

        public Task<int> UpdateForCancelPublishAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken))
            => addBillingInputQueryProcessor.UpdateForCancelPublishAsync(Ids, token);

        public Task<int> DeleteForCancelPublishAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken))
            => addBillingInputQueryProcessor.DeleteForCancelPublishAsync(Ids, token);


    }
}
