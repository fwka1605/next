using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class CustomerGroupProcessor : ICustomerGroupProcessor
    {
        private readonly ICustomerGroupByIdQueryProcessor customerGroupByIdQueryProcessor;
        private readonly IAddCustomerGroupQueryProcessor addCustomerGroupQueryProcessor;
        private readonly IDeleteCustomerGroupQueryProcessor deleteCustomerGroupQueryProcessor;
        private readonly ICustomerQueryProcessor customerQueryProcessor;
        private readonly IUpdateCustomerQueryProcessor updateCustomerQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public CustomerGroupProcessor(
            ICustomerGroupByIdQueryProcessor customerGroupByIdQueryProcessor,
            IAddCustomerGroupQueryProcessor addCustomerGroupQueryProcessor,
            IDeleteCustomerGroupQueryProcessor deleteCustomerGroupQueryProcessor,
            ICustomerQueryProcessor customerQueryProcessor,
            IUpdateCustomerQueryProcessor updateCustomerQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.customerGroupByIdQueryProcessor = customerGroupByIdQueryProcessor;
            this.addCustomerGroupQueryProcessor = addCustomerGroupQueryProcessor;
            this.deleteCustomerGroupQueryProcessor = deleteCustomerGroupQueryProcessor;
            this.customerQueryProcessor = customerQueryProcessor;
            this.updateCustomerQueryProcessor = updateCustomerQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }
        public async Task<IEnumerable<CustomerGroup>> GetAsync(CustomerGroupSearch option, CancellationToken token = default(CancellationToken))
            => await customerGroupByIdQueryProcessor.GetAsync(option, token);


        public async Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken))
            => await customerGroupByIdQueryProcessor.ExistCustomerAsync(CustomerId, token);

        public async Task<bool> HasChildAsync(int ParentCustomerId, CancellationToken token = default(CancellationToken))
            => await customerGroupByIdQueryProcessor.HasChildAsync(ParentCustomerId, token);

        public async Task<int> GetUniqueGroupCountAsync(IEnumerable<int> Ids, CancellationToken token = default(CancellationToken))
            => await customerGroupByIdQueryProcessor.GetUniqueGroupCountAsync(Ids, token);

        public async Task<IEnumerable<CustomerGroup>> SaveAsync(MasterImportData<CustomerGroup> items, CancellationToken token = default(CancellationToken))
        {
            var result = new List<CustomerGroup>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var x in items.DeleteItems)
                    await deleteCustomerGroupQueryProcessor.DeleteAsync(x, token);

                foreach (var x in items.InsertItems)
                    result.Add(await addCustomerGroupQueryProcessor.SaveAsync(x, token));

                scope.Complete();
            }
            return result;
        }

        public async Task<ImportResult> ImportAsync(
            IEnumerable<CustomerGroup> insert,
            IEnumerable<CustomerGroup> update,
            IEnumerable<CustomerGroup> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;
                var items = new List<CustomerGroup>();
                foreach (var x in delete)
                {
                    await deleteCustomerGroupQueryProcessor.DeleteAsync(x, token);
                    deleteCount++;
                }

                foreach (var x in update)
                {
                    items.Add(await addCustomerGroupQueryProcessor.SaveAsync(x, token));
                    updateCount++;
                }

                if (insert?.Any() ?? false)
                {
                    var userId = insert.Select(x => x.CreateBy).FirstOrDefault();
                    var parentIds = insert.Select(x => x.ParentCustomerId).Distinct();
                    var childIds  = insert.Select(x => x.ChildCustomerId ).Distinct();
                    await updateCustomerQueryProcessor.UpdateIsParentAsync(1, userId, parentIds, token);
                    await updateCustomerQueryProcessor.UpdateIsParentAsync(0, userId, childIds, token);
                    foreach (var x in insert)
                    {
                        items.Add(await addCustomerGroupQueryProcessor.SaveAsync(x, token));
                        insertCount++;
                    }
                }

                scope.Complete();

                return new ImportResultCustomerGroup
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                    CustomerGroup = items,
                };
            }
        }
    }
}
