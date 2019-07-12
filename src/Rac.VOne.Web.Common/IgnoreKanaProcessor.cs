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
    public class IgnoreKanaProcessor : IIgnoreKanaProcessor
    {
        private readonly IIgnoreKanasByCompanyIdQueryProcessor ignoreKanasByCompanyIdQueryProcessor;
        private readonly IIgnoreKanaByCodeQueryProcessor ignoreKanaByCodeQueryProcessor;
        private readonly IAddIgnoreKanaQueryProcessor addIgnoreKanaQueryProcessor;
        private readonly IDeleteIgnoreKanaQueryProcessor deleteIgnoreKanaQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public IgnoreKanaProcessor(
            IIgnoreKanasByCompanyIdQueryProcessor ignoreKanasByCompanyIdQueryProcessor,
            IIgnoreKanaByCodeQueryProcessor ignoreKanaByCodeQueryProcessor,
            IAddIgnoreKanaQueryProcessor addIgnoreKanaQueryProcessor,
            IDeleteIgnoreKanaQueryProcessor deleteIgnoreKanaQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.ignoreKanasByCompanyIdQueryProcessor = ignoreKanasByCompanyIdQueryProcessor;
            this.ignoreKanaByCodeQueryProcessor = ignoreKanaByCodeQueryProcessor;
            this.addIgnoreKanaQueryProcessor = addIgnoreKanaQueryProcessor;
            this.deleteIgnoreKanaQueryProcessor = deleteIgnoreKanaQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }


        public async Task<IgnoreKana> SaveAsync(IgnoreKana kana, CancellationToken token = default(CancellationToken))
            => await addIgnoreKanaQueryProcessor.SaveAsync(kana, token);

        public async Task<int> DeleteAsync(IgnoreKana kana, CancellationToken token = default(CancellationToken))
            => await deleteIgnoreKanaQueryProcessor.DeleteAsync(kana.CompanyId, kana.Kana, token);

        public async Task<bool> ExistCategoryAsync(int excludeCategoryId, CancellationToken token = default(CancellationToken))
            => await ignoreKanasByCompanyIdQueryProcessor.ExistCategoryAsync(excludeCategoryId);


        public async Task<IEnumerable<IgnoreKana>> GetAsync(IgnoreKana kana, CancellationToken token = default(CancellationToken))
            => await ignoreKanaByCodeQueryProcessor.GetAsync(kana, token);

        public async Task<ImportResult> ImportAsync(
            IEnumerable<IgnoreKana> insert,
            IEnumerable<IgnoreKana> update,
            IEnumerable<IgnoreKana> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var insertCount = 0;
                var updateCount = 0;
                var deleteCount = 0;
                foreach (var x in delete)
                {
                    await deleteIgnoreKanaQueryProcessor.DeleteAsync(x.CompanyId, x.Kana, token);
                    ++deleteCount;
                }

                foreach (var x in update)
                {
                    await addIgnoreKanaQueryProcessor.SaveAsync(x, token);
                    ++updateCount;
                }

                foreach (var x in insert)
                {
                    await addIgnoreKanaQueryProcessor.SaveAsync(x, token);
                    ++insertCount;
                }

                scope.Complete();

                return new ImportResult
                {
                    ProcessResult = new ProcessResult() { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                };
            }
        }
    }
}
