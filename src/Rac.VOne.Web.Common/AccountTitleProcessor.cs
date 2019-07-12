using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class AccountTitleProcessor : IAccountTitleProcessor
    {
        private readonly IDeleteIdenticalEntityQueryProcessor<AccountTitle> deleteIdenticalEntityQueryProcessor;
        private readonly IAccountTitleQueryProcessor accountTitleQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<AccountTitle> identicalEntityGetByIdsQueryProcessor;
        private readonly IAddAccountTitleQueryProcessor addAccountTitleQueryProcessor;
        private readonly IAccountTitleForImportQueryProcessor accountTitleForImportQueryProcessor;
        private readonly IMasterGetByCodesQueryProcessor<AccountTitle> masterGetByCodesQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public AccountTitleProcessor(
            IDeleteIdenticalEntityQueryProcessor<AccountTitle> deleteIdenticalEntityQueryProcessor,
            IAccountTitleQueryProcessor accountTitleQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<AccountTitle> identicalEntityGetByIdsQueryProcessor,
            IAddAccountTitleQueryProcessor addAccountTitleQueryProcessor,
            IAccountTitleForImportQueryProcessor accountTitleForImportQueryProcessor,
            IMasterGetByCodesQueryProcessor<AccountTitle> masterGetByCodesQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.deleteIdenticalEntityQueryProcessor = deleteIdenticalEntityQueryProcessor;
            this.accountTitleQueryProcessor = accountTitleQueryProcessor;
            this.identicalEntityGetByIdsQueryProcessor = identicalEntityGetByIdsQueryProcessor;
            this.addAccountTitleQueryProcessor = addAccountTitleQueryProcessor;
            this.accountTitleForImportQueryProcessor = accountTitleForImportQueryProcessor;
            this.masterGetByCodesQueryProcessor = masterGetByCodesQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public Task<IEnumerable<AccountTitle>> GetAsync(AccountTitleSearch option, CancellationToken token = default(CancellationToken))
            => accountTitleQueryProcessor.GetAsync(option, token);


        public Task<int> DeleteAsync(int id, CancellationToken token = default(CancellationToken))
            => deleteIdenticalEntityQueryProcessor.DeleteAsync(id, token);

        public Task<AccountTitle> SaveAsync(AccountTitle AccountTitle, CancellationToken token = default(CancellationToken))
            => addAccountTitleQueryProcessor.AddAsync(AccountTitle, token);

        //forImporting
        public Task<IEnumerable<MasterData>> GetImportItemsCategoryAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken))
            => accountTitleForImportQueryProcessor.GetImportItemsCategoryAsync(companyId, codes, token);

        public Task<IEnumerable<MasterData>> GetImportItemsCustomerDiscountAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken))
            => accountTitleForImportQueryProcessor.GetImportItemsCustomerDiscountAsync(companyId, codes, token);

        public Task<IEnumerable<MasterData>> GetImportItemsDebitBillingAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken))
            => accountTitleForImportQueryProcessor.GetImportItemsDebitBillingAsync(companyId, codes, token);

        public Task<IEnumerable<MasterData>> GetImportItemsCreditBillingAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken))
            => accountTitleForImportQueryProcessor.GetImportItemsCreditBillingAsync(companyId, codes, token);

        public async Task<ImportResult> ImportAsync(IEnumerable<AccountTitle> insert, IEnumerable<AccountTitle> update, IEnumerable<AccountTitle> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var insertCount = 0;
                var updateCount = 0;
                var deleteCount = 0;
                var items = new List<AccountTitle>();
                foreach (var x in delete)
                {
                    await DeleteAsync(x.Id, token);
                    ++deleteCount;
                }
                foreach (var x in update)
                {
                    var item = await SaveAsync(x, token);
                    items.Add(item);
                    ++updateCount;
                }
                foreach (var x in insert)
                {
                    var item = await SaveAsync(x, token);
                    items.Add(item);
                    ++insertCount;
                }

                scope.Complete();

                return new ImportResultAccountTitle
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                    AccountTitles = items
                };

            }
        }
    }
}
