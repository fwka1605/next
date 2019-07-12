using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class CurrencyProcessor : ICurrencyProcessor
    {
        private readonly ICurrencyQueryProcessor currencyQueryProcessor;
        private readonly IAddCurrencyQueryProcessor addCurrencyQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<Currency> deleteIdenticalEntityQueryProcessor;
        private readonly IMasterGetItemsQueryProcessor<Currency> masterGetItemsQueryProcessor;
        private readonly IMasterGetByCodesQueryProcessor<Currency> masterGetByCodesQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<Currency> identicalEntityGetByIdsQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public CurrencyProcessor(
            ICurrencyQueryProcessor currencyQueryProcessor,
            IAddCurrencyQueryProcessor addCurrencyQueryProcessor,
            IMasterGetItemsQueryProcessor<Currency> masterGetItemsQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<Currency> deleteIdenticalEntityQueryProcessor,
            IMasterGetByCodesQueryProcessor<Currency> masterGetByCodesQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<Currency> identicalEntityGetByIdsQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.currencyQueryProcessor = currencyQueryProcessor;
            this.addCurrencyQueryProcessor = addCurrencyQueryProcessor;
            this.masterGetItemsQueryProcessor = masterGetItemsQueryProcessor;
            this.deleteIdenticalEntityQueryProcessor = deleteIdenticalEntityQueryProcessor;
            this.masterGetByCodesQueryProcessor = masterGetByCodesQueryProcessor;
            this.identicalEntityGetByIdsQueryProcessor = identicalEntityGetByIdsQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Currency>> GetAsync(CurrencySearch option, CancellationToken token = default(CancellationToken))
            => await currencyQueryProcessor.GetAsync(option, token);

        public async Task<Currency> SaveAsync(Currency Currency, CancellationToken token = default(CancellationToken))
            => await addCurrencyQueryProcessor.SaveAsync(Currency, token);

        public async Task<int> DeleteAsync(int id, CancellationToken token = default(CancellationToken))
            => await deleteIdenticalEntityQueryProcessor.DeleteAsync(id, token);
        
        public async Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await currencyQueryProcessor.GetImportItemsBillingAsync(CompanyId, Code, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsReceiptAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await currencyQueryProcessor.GetImportItemsReceiptAsync(CompanyId, Code, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsNettingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
            => await currencyQueryProcessor.GetImportItemsNettingAsync(CompanyId, Code, token);

        public async Task<ImportResult> ImportAsync(IEnumerable<Currency> insert, IEnumerable<Currency> update, IEnumerable<Currency> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;
                foreach (var x in delete)
                {
                    await DeleteAsync(x.Id, token);
                    ++deleteCount;
                }

                foreach (var x in insert)
                {
                    await SaveAsync(x, token);
                    ++insertCount;
                }

                foreach (var x in update)
                {
                    await SaveAsync(x, token);
                    ++updateCount;
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

    }
}
