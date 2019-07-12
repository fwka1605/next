using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class CustomerDiscountProcessor : ICustomerDiscountProcessor
    {
        private readonly ICustomerDiscountQueryProcessor customerDiscountQueryProcessor;
        private readonly IAddCustomerDiscountQueryProcessor addCustomerDiscountQueryProcessor;
        private readonly IDeleteCustomerDiscountQueryProcessor deleteCustomerDiscountQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public CustomerDiscountProcessor(
            ICustomerDiscountQueryProcessor customerDiscountQueryProcessor,
            IAddCustomerDiscountQueryProcessor addCustomerDiscountQueryProcessor,
            IDeleteCustomerDiscountQueryProcessor deleteCustomerDiscountQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.customerDiscountQueryProcessor = customerDiscountQueryProcessor;
            this.addCustomerDiscountQueryProcessor = addCustomerDiscountQueryProcessor;
            this.deleteCustomerDiscountQueryProcessor = deleteCustomerDiscountQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<bool> ExistAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken))
            => await customerDiscountQueryProcessor.ExistAccountTitleAsync(AccountTitleId, token);

        public async Task<CustomerDiscount> SaveAsync(CustomerDiscount CustomerDiscount, CancellationToken token = default(CancellationToken))
            => await addCustomerDiscountQueryProcessor.SaveAsync(CustomerDiscount, token);

        public async Task<int> DeleteAsync(CustomerDiscount discount, CancellationToken token = default(CancellationToken))
            => await deleteCustomerDiscountQueryProcessor.DeleteAsync(discount, token);

        public async Task<IEnumerable<CustomerDiscount>> GetAsync(int customerId, CancellationToken token = default(CancellationToken))
            => await customerDiscountQueryProcessor.GetAsync(customerId, token);

        public async Task<ImportResult> ImportAsync(
            IEnumerable<CustomerDiscount> insert,
            IEnumerable<CustomerDiscount> update,
            IEnumerable<CustomerDiscount> delete,
            CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;

                foreach (var x in delete)
                {
                    x.Sequence = 0;
                    await deleteCustomerDiscountQueryProcessor.DeleteAsync(x, token);
                    ++deleteCount;
                }

                foreach (var x in update)
                {
                    // 削除
                    await deleteCustomerDiscountQueryProcessor.DeleteAsync(new CustomerDiscount { CustomerId = x.CustomerId }, token);
                    foreach (var item in x.ToUpdateItems())
                    {
                        await addCustomerDiscountQueryProcessor.SaveAsync(item, token);
                    }
                    ++updateCount;
                }

                foreach (var x in insert)
                {
                    foreach (var item in x.ToUpdateItems())
                    {
                        await addCustomerDiscountQueryProcessor.SaveAsync(item, token);
                    }
                    ++insertCount;
                }

                scope.Complete();

                return new ImportResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                };

            }
        }

        public async Task<IEnumerable<CustomerDiscount>> GetItemsAsync(CustomerSearch option, CancellationToken token = default(CancellationToken))
            => await customerDiscountQueryProcessor.GetItemsAsync(option, token);

    }
}
