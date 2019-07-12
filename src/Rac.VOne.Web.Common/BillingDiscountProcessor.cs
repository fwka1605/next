using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class BillingDiscountProcessor : IBillingDiscountProcessor
    {
        private readonly IBillingDiscountQueryProcessor billingDiscountQueryProcessor;
        private readonly IDeleteBillingDiscountQueryProcessor deleteBillingDiscountQueryProcessor;
        private readonly IAddBillingDiscountQueryProcessor addBillingDiscountQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public BillingDiscountProcessor(
            IBillingDiscountQueryProcessor billingDiscountQueryProcessor,
            IDeleteBillingDiscountQueryProcessor deleteBillingDiscountQueryProcessor,
            IAddBillingDiscountQueryProcessor addBillingDiscountQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingDiscountQueryProcessor = billingDiscountQueryProcessor;
            this.deleteBillingDiscountQueryProcessor = deleteBillingDiscountQueryProcessor;
            this.addBillingDiscountQueryProcessor = addBillingDiscountQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<int> SaveAsync(BillingDiscount discount, CancellationToken token = default(CancellationToken))
        {
            var result = 0;
            using (var scope = transactionScopeBuilder.Create())
            {
                await deleteBillingDiscountQueryProcessor.DeleteAsync(discount.BillingId);
                if (discount.DiscountAmount1 != 0)
                {
                    discount.DiscountAmount = discount.DiscountAmount1;
                    discount.DiscountType = 1;
                    result += await addBillingDiscountQueryProcessor.SaveAsync(discount);
                }

                if (discount.DiscountAmount2 != 0)
                {
                    discount.DiscountAmount = discount.DiscountAmount2;
                    discount.DiscountType = 2;
                    result += await addBillingDiscountQueryProcessor.SaveAsync(discount);
                }

                if (discount.DiscountAmount3 != 0)
                {
                    discount.DiscountAmount = discount.DiscountAmount3;
                    discount.DiscountType = 3;
                    result += await addBillingDiscountQueryProcessor.SaveAsync(discount);
                }
                if (discount.DiscountAmount4 != 0)
                {
                    discount.DiscountAmount = discount.DiscountAmount4;
                    discount.DiscountType = 4;
                    result += await addBillingDiscountQueryProcessor.SaveAsync(discount);
                }

                if (discount.DiscountAmount5 != 0)
                {
                    discount.DiscountAmount = discount.DiscountAmount5;
                    discount.DiscountType = 5;
                    result += await addBillingDiscountQueryProcessor.SaveAsync(discount);
                }
                scope.Complete();
            }
            return result;
        }

        public async Task<IEnumerable<BillingDiscount>> GetAsync(long BillingId, CancellationToken token = default(CancellationToken))
            => await billingDiscountQueryProcessor.GetAsync(BillingId, token);

        public async Task<int> DeleteAsync(long BillingId, CancellationToken token = default(CancellationToken))
            => await deleteBillingDiscountQueryProcessor.DeleteAsync(BillingId, token);



    }
}
