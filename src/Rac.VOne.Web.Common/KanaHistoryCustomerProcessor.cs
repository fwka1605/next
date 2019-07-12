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
    public class KanaHistoryCustomerProcessor : IKanaHistoryCustomerProcessor
    {
        private readonly IKanaHistoryCustomerQueryProcessor kanaHistoryCustomerQueryProcessor;
        private readonly IAddKanaHistoryCustomerQueryProcessor addKanaHistoryCustomerQueryProcessor;
        private readonly IDeleteKanaHistoryCustomerQueryProcessor deleteKanaHistoryCustomerQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public KanaHistoryCustomerProcessor(
            IKanaHistoryCustomerQueryProcessor kanaHistoryCustomerQueryProcessor,
            IAddKanaHistoryCustomerQueryProcessor addKanaHistoryCustomerQueryProcessor,
            IDeleteKanaHistoryCustomerQueryProcessor deleteKanaHistoryCustomerQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.kanaHistoryCustomerQueryProcessor = kanaHistoryCustomerQueryProcessor;
            this.addKanaHistoryCustomerQueryProcessor = addKanaHistoryCustomerQueryProcessor;
            this.deleteKanaHistoryCustomerQueryProcessor = deleteKanaHistoryCustomerQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken))
            => await kanaHistoryCustomerQueryProcessor.ExistCustomerAsync(CustomerId, token);

        public async Task<IEnumerable<KanaHistoryCustomer>> GetAsync(KanaHistorySearch option, CancellationToken token = default(CancellationToken))
            => await kanaHistoryCustomerQueryProcessor.GetAsync(option, token);

        public async Task<bool> ExistAsync(KanaHistoryCustomer KanaHistoryCustomer, CancellationToken token = default(CancellationToken))
            => await kanaHistoryCustomerQueryProcessor.ExistAsync(KanaHistoryCustomer, token);

        public async Task<KanaHistoryCustomer> SaveAsync(KanaHistoryCustomer item, CancellationToken token = default(CancellationToken))
            => await addKanaHistoryCustomerQueryProcessor.SaveAsync(item, token);


        public async Task<int> DeleteAsync(KanaHistoryCustomer item, CancellationToken token = default(CancellationToken))
            => await deleteKanaHistoryCustomerQueryProcessor.DeleteAsync(item, token);

        public async Task<ImportResult> ImportAsync(
            IEnumerable<KanaHistoryCustomer> insert,
            IEnumerable<KanaHistoryCustomer> update,
            IEnumerable<KanaHistoryCustomer> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;
                foreach (var x in delete)
                {
                    await deleteKanaHistoryCustomerQueryProcessor.DeleteAsync(x, token);
                    ++deleteCount;
                }
                foreach (var x in update)
                {
                    await addKanaHistoryCustomerQueryProcessor.SaveAsync(x, token);
                    ++updateCount;
                }
                foreach (var x in insert)
                {
                    await addKanaHistoryCustomerQueryProcessor.SaveAsync(x, token);
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

    }
}
