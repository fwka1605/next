using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public  class BankBranchProcessor: IBankBranchProcessor
    {
        private readonly IBankBranchQueryProcessor bankBranchQueryProcessor;
        private readonly IAddBankBranchQueryProcessor addBankBranchQueryProcessor;
        private readonly IDeleteBankBranchQueryProcessor deleteBankBranchQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public  BankBranchProcessor(
            IBankBranchQueryProcessor bankBranchQueryProcessor,
            IAddBankBranchQueryProcessor addBankBranchQueryProcessor,
            IDeleteBankBranchQueryProcessor deleteBankBranchQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.bankBranchQueryProcessor = bankBranchQueryProcessor;
            this.addBankBranchQueryProcessor = addBankBranchQueryProcessor;
            this.deleteBankBranchQueryProcessor = deleteBankBranchQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<BankBranch>> GetAsync(BankBranchSearch option, CancellationToken token = default(CancellationToken))
            => await bankBranchQueryProcessor.GetAsync(option, token);

        public async Task<BankBranch> SaveAsync(BankBranch bankBranch, CancellationToken token = default(CancellationToken))
            => await addBankBranchQueryProcessor.SaveAsync(bankBranch, token);

        public async Task<int> DeleteAsync(int CompanyId, string BankCode, string BranchCode, CancellationToken token = default(CancellationToken))
            => await deleteBankBranchQueryProcessor.DeleteAsync(CompanyId, BankCode, BranchCode, token);


        public async Task<ImportResult> ImportAsync(IEnumerable<BankBranch> insert, IEnumerable<BankBranch> update, IEnumerable<BankBranch> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;

                foreach (var x in delete)
                {
                    await DeleteAsync(x.CompanyId, x.BankCode, x.BranchCode);
                    deleteCount++;
                }

                foreach (var x in update)
                {
                    await SaveAsync(x);
                    updateCount++;
                }

                foreach (var x in insert)
                {
                    await SaveAsync(x);
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
