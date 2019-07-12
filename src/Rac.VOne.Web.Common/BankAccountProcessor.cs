using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class BankAccountProcessor : IBankAccountProcessor
    {
        private readonly IBankAccountQueryProcessor bankAccountQueryProcessor;
        private readonly IAddBankAccountQueryProcessor addBankAccountQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<BankAccount> identicalEntityGetByIdsQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<BankAccount> deleteIdenticalEntityQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public BankAccountProcessor(
            IBankAccountQueryProcessor bankAccountQueryProcessor,
            IAddBankAccountQueryProcessor addBankAccountQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<BankAccount> identicalEntityGetByIdsQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<BankAccount> deleteIdenticalEntityQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.bankAccountQueryProcessor = bankAccountQueryProcessor;
            this.addBankAccountQueryProcessor = addBankAccountQueryProcessor;
            this.identicalEntityGetByIdsQueryProcessor = identicalEntityGetByIdsQueryProcessor;
            this.deleteIdenticalEntityQueryProcessor = deleteIdenticalEntityQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }


        public async Task<IEnumerable<BankAccount>> GetAsync(BankAccountSearch option, CancellationToken token = default(CancellationToken))
            => await bankAccountQueryProcessor.GetAsync(option, token);

        public async Task<BankAccount> SaveAsync(BankAccount BankAccount, CancellationToken token = default(CancellationToken))
            => await addBankAccountQueryProcessor.SaveAsync(BankAccount, token);

        public async Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
            => await deleteIdenticalEntityQueryProcessor.DeleteAsync(Id, token);

        public async Task<bool> ExistCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken))
            => await bankAccountQueryProcessor.ExistCategoryAsnc(CategoryId, token);

        public async Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken))
            => await bankAccountQueryProcessor.ExistSectionAsync(SectionId, token);

        public async Task<ImportResult> ImportAsync(
            IEnumerable<BankAccount> insert,
            IEnumerable<BankAccount> update,
            IEnumerable<BankAccount> delete,
            CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;

                foreach (var x in delete)
                {
                    await DeleteAsync(x.Id, token);
                    deleteCount++;
                }

                foreach (var x in update)
                {
                    await SaveAsync(x, token);
                    updateCount++;
                }

                foreach (var x in insert)
                {
                    await SaveAsync(x, token);
                    insertCount++;
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

