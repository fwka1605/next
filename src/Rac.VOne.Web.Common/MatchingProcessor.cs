using Rac.VOne.Common;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class MatchingProcessor : IMatchingProcessor
    {
        private readonly IMatchingQueryProcessor matchingQueryProcessor;
        private readonly ITransactionalGetByIdsQueryProcessor<Matching> byIdsMatchingQueryProcessor;
        private readonly ITransactionalGetByIdsQueryProcessor<MatchingHeader> byIdsMatchingHeaderQueryProcessor;
        private readonly IWorkSectionTargetQueryProcessor workSectionTargetQueryProcessor;
        private readonly IWorkDepartmentTargetQueryProcessor workDepartmentTargetQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<CollationSetting> getCollationSettingQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MatchingProcessor(
            IMatchingQueryProcessor matchingQueryProcessor,
            ITransactionalGetByIdsQueryProcessor<Matching> byIdsMatchingQueryProcessor,
            ITransactionalGetByIdsQueryProcessor<MatchingHeader> byIdsMatchingHeaderQueryProcessor,
            IWorkSectionTargetQueryProcessor workSectionTargetQueryProcessor,
            IWorkDepartmentTargetQueryProcessor workDepartmentTargetQueryProcessor,
            IByCompanyGetEntityQueryProcessor<CollationSetting> getCollationSettingQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
          )
        {
            this.matchingQueryProcessor = matchingQueryProcessor;
            this.byIdsMatchingQueryProcessor = byIdsMatchingQueryProcessor;
            this.byIdsMatchingHeaderQueryProcessor = byIdsMatchingHeaderQueryProcessor;
            this.workSectionTargetQueryProcessor = workSectionTargetQueryProcessor;
            this.workDepartmentTargetQueryProcessor = workDepartmentTargetQueryProcessor;
            this.getCollationSettingQueryProcessor = getCollationSettingQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Receipt>> SearchReceiptDataAsync(MatchingReceiptSearch option, CancellationToken token = default(CancellationToken))
            => await matchingQueryProcessor.GetReceiptsForMatchingAsync(option, token);

        public async Task<IEnumerable<Receipt>> GetMatchedReceiptsAsync(MatchingReceiptSearch option, CancellationToken token = default(CancellationToken))
            => await matchingQueryProcessor.GetMatchgedReceiptsAsync(option, token);

        public async Task<IEnumerable<Billing>> SearchBillingDataAsync(MatchingBillingSearch option, CancellationToken token = default(CancellationToken))
            => await matchingQueryProcessor.GetBillingsForMatchingAsync(option, token);


        public async Task<IEnumerable<Billing>> GetMatchedBillingsAsync(MatchingBillingSearch option, CancellationToken token = default(CancellationToken))
            => await matchingQueryProcessor.GetMatchedBillingsAsync(option, token);

        public async Task<IEnumerable<MatchingHeader>> SearchMatchedDataAsync(CollationSearch CollationSearch, CancellationToken token = default(CancellationToken))
        {
            var collationSetting = (await getCollationSettingQueryProcessor.GetAsync(CollationSearch.CompanyId, token));
            CollationSearch.SortOrderDirection = collationSetting.SortOrderDirection;

            var items = await matchingQueryProcessor.SearchMatchedDataAsync(CollationSearch, token);
            return items;
        }

        public async Task<IEnumerable<Receipt>> SearchReceiptByIdAsync(IEnumerable<long> ReceiptId, CancellationToken token = default(CancellationToken))
            => await matchingQueryProcessor.SearchReceiptByIdAsync(ReceiptId, token);

        public async Task<IEnumerable<Matching>> GetAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken))
            => await byIdsMatchingQueryProcessor.GetByIdsAsync(Ids, token);

        public async Task<IEnumerable<MatchingHeader>> GetHeaderItemsAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken))
            => await byIdsMatchingHeaderQueryProcessor.GetByIdsAsync(Ids, token);


        public async Task<int> SaveWorkSectionTargetAsync(byte[] ClientKey, int CompanyId, IEnumerable<int> SectionIds, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                await workSectionTargetQueryProcessor.DeleteAsync(ClientKey, token);
                var count = 0;
                if (SectionIds != null)
                {
                    foreach (var id in SectionIds)
                        count += await workSectionTargetQueryProcessor.SaveAsync(ClientKey, CompanyId, id, token);
                }
                scope.Complete();
                return count;
            }
        }

        public async Task<int> SaveWorkDepartmentTargetAsync(byte[] ClientKey, int CompanyId, IEnumerable<int> DepartmentIds, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                await workDepartmentTargetQueryProcessor.DeleteAsync(ClientKey, token);
                var count = 0;
                if (DepartmentIds != null)
                {
                    foreach (var id in DepartmentIds)
                        count += await workDepartmentTargetQueryProcessor.SaveAsync(ClientKey, CompanyId, id, token);
                }
                scope.Complete();
                return count;
            }
        }



    }
}
