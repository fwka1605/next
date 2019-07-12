using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class CollationProcessor : ICollationProcessor
    {
        private readonly ICollationQueryProcessor collationQueryProcessor;
        private readonly IByCompanyGetEntitiesQueryProcessor<CollationOrder> getCollationOrderQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<CollationSetting> getCollationSettingQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public CollationProcessor(
            ICollationQueryProcessor collationQueryProcessor,
            IByCompanyGetEntitiesQueryProcessor<CollationOrder> getCollationOrderQueryProcessor,
            IByCompanyGetEntityQueryProcessor<CollationSetting> getCollationSettingQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.collationQueryProcessor = collationQueryProcessor;
            this.getCollationOrderQueryProcessor = getCollationOrderQueryProcessor;
            this.getCollationSettingQueryProcessor = getCollationSettingQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Collation>> CollateAsync(CollationSearch collationSearch,
            CancellationToken token = default(CancellationToken),
            IProgressNotifier notifier = null)
        {
            var orders = (await getCollationOrderQueryProcessor.GetItemsAsync(collationSearch.CompanyId, token))
                .Where(x => x.Available == 1)
                .OrderBy(x => x.ExecutionOrder).ToArray();

            using (var scope = transactionScopeBuilder.Create())
            {
                var collationSetting = (await getCollationSettingQueryProcessor.GetAsync(collationSearch.CompanyId, token));
                collationSearch.SortOrderDirection = collationSetting.SortOrderDirection;

                await collationQueryProcessor.InitializeAsync(collationSearch, token);
                notifier?.UpdateState();

                foreach (var order in orders)
                {
                    switch (order.CollationTypeId)
                    {
                        case 0: await collationQueryProcessor.CollatePayerCodeAsync (collationSearch, token); break;
                        case 1: await collationQueryProcessor.CollateCustomerIdAsync(collationSearch, token); break;
                        case 2: await collationQueryProcessor.CollateHistoryAsync   (collationSearch, token); break;
                        case 3: await collationQueryProcessor.CollatePayerNameAsync (collationSearch, token); break;
                        case 4: await collationQueryProcessor.CollateKeyAsync       (collationSearch, token); break;
                    }
                    notifier?.UpdateState();
                }
                await collationQueryProcessor.CollateMissingAsync(collationSearch, token);

                notifier?.UpdateState();
                var items = await collationQueryProcessor.GetItemsAsync(collationSearch, token);
                notifier?.UpdateState();

                scope.Complete();
                return items;

            }
        }
    }
}
