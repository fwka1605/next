using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Web.Common
{
    public class CollationSettingProcessor : ICollationSettingProcessor
    {
        private readonly ICollationSettingByCompanyIdQueryProcessor collactionSettingQueryProcessor;
        private readonly IAddCollationSettingQueryProcessor addCollationSettingQueryProcessor;

        private readonly IByCompanyGetEntityQueryProcessor<CollationSetting> collationSettingGetByCompanyQueryProcessor;
        private readonly IByCompanyGetEntitiesQueryProcessor<CollationOrder> collationOrdersGetByCompanyQueryProcessor;

        private readonly IDeleteByCompanyQueryProcessor<CollationSetting> deleteCollationSettingByCompanyQueryProcessor;
        private readonly IDeleteByCompanyQueryProcessor<CollationOrder> deleteCollationOrderByCompanyQueryProcessor;
        private readonly IDeleteByCompanyQueryProcessor<MatchingOrder> deleteMatchingOrderByCompanyQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public CollationSettingProcessor(
            ICollationSettingByCompanyIdQueryProcessor collactionSettingQueryProcessor,
            IAddCollationSettingQueryProcessor addCollationSettingQueryProcessor,
            IByCompanyGetEntityQueryProcessor<CollationSetting> collationSettingGetByCompanyQueryProcessor,
            IByCompanyGetEntitiesQueryProcessor<CollationOrder> collationOrdersGetByCompanyQueryProcessor,
            IDeleteByCompanyQueryProcessor<CollationSetting> deleteCollationSettingByCompanyQueryProcessor,
            IDeleteByCompanyQueryProcessor<CollationOrder> deleteCollationOrderByCompanyQueryProcessor,
            IDeleteByCompanyQueryProcessor<MatchingOrder> deleteMatchingOrderByCompanyQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.collactionSettingQueryProcessor = collactionSettingQueryProcessor;
            this.addCollationSettingQueryProcessor = addCollationSettingQueryProcessor;

            this.collationSettingGetByCompanyQueryProcessor = collationSettingGetByCompanyQueryProcessor;
            this.collationOrdersGetByCompanyQueryProcessor = collationOrdersGetByCompanyQueryProcessor;

            this.deleteCollationSettingByCompanyQueryProcessor = deleteCollationSettingByCompanyQueryProcessor;
            this.deleteCollationOrderByCompanyQueryProcessor = deleteCollationOrderByCompanyQueryProcessor;
            this.deleteMatchingOrderByCompanyQueryProcessor = deleteMatchingOrderByCompanyQueryProcessor;

            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<CollationSetting> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var setting = await collationSettingGetByCompanyQueryProcessor.GetAsync(CompanyId, token);
            setting.CollationOrders         = (await GetCollationOrderAsync(CompanyId, token)).ToArray();
            setting.BillingMatchingOrders   = (await GetMatchingBillingOrderAsync(CompanyId, token)).ToArray();
            setting.ReceiptMatchingOrders   = (await GetMatchingReceiptOrderAsync(CompanyId, token)).ToArray();
            return setting;
        }


        public async Task<IEnumerable<MatchingOrder>> GetMatchingBillingOrderAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await collactionSettingQueryProcessor.GetMatchingBillingOrderAsync(CompanyId, token);

        public async Task<IEnumerable<MatchingOrder>> GetMatchingReceiptOrderAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await collactionSettingQueryProcessor.GetMatchingReceiptOrderAsync(CompanyId, token);

        public async Task<IEnumerable<CollationOrder>> GetCollationOrderAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => (await collationOrdersGetByCompanyQueryProcessor.GetItemsAsync(CompanyId, token)).OrderBy(x => x.ExecutionOrder);

        private async Task DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            await deleteCollationOrderByCompanyQueryProcessor.DeleteAsync(CompanyId, token);
            await deleteMatchingOrderByCompanyQueryProcessor.DeleteAsync(CompanyId, token);
            await deleteCollationSettingByCompanyQueryProcessor.DeleteAsync(CompanyId, token);
        }

        public async Task<CollationSetting> SaveAsync(CollationSetting setting, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var companyId = setting.CompanyId;

                await DeleteAsync(companyId, token);

                var result = await addCollationSettingQueryProcessor.SaveAsync(setting, token);
                var collationOrders = new List<CollationOrder>();
                foreach (var order in setting.CollationOrders)
                    collationOrders.Add(await addCollationSettingQueryProcessor.SaveCollationOrderAsync(order, token));
                var matchingOrders = new List<MatchingOrder>();
                foreach (var order in setting.BillingMatchingOrders.Concat(setting.ReceiptMatchingOrders))
                    matchingOrders.Add(await addCollationSettingQueryProcessor.SaveMatchingOrderAsync(order, token));

                scope.Complete();

                result.CollationOrders = collationOrders.OrderBy(x => x.ExecutionOrder).ToArray();
                result.BillingMatchingOrders = matchingOrders.Where(x => x.TransactionCategory == 1).OrderBy(x => x.ExecutionOrder).ToArray();
                result.ReceiptMatchingOrders = matchingOrders.Where(x => x.TransactionCategory == 2).OrderBy(x => x.ExecutionOrder).ToArray();
                return result;

            }
        }

    }
}
