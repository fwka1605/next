using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class KanaHistoryPaymentAgencyProcessor : IKanaHistoryPaymentAgencyProcessor
    {
        private readonly IKanaHistoryPaymentAgencyQueryProcessor kanaHistoryPaymentAgencyQueryProcessor;
        private readonly IAddKanaHistoryPaymentAgencyQueryProcessor addKanaHistoryPaymentAgencyQueryProcessor;
        private readonly IDeleteKanaHistoryPaymentAgencyQueryProcessor deleteKanaHistoryPaymentAgencyQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public KanaHistoryPaymentAgencyProcessor(
            IKanaHistoryPaymentAgencyQueryProcessor kanaHistoryPaymentAgencyQueryProcessor,
            IAddKanaHistoryPaymentAgencyQueryProcessor addKanaHistoryPaymentAgencyQueryProcessor,
            IDeleteKanaHistoryPaymentAgencyQueryProcessor deleteKanaHistoryPaymentAgencyQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
           )
        {
            this.kanaHistoryPaymentAgencyQueryProcessor = kanaHistoryPaymentAgencyQueryProcessor;
            this.addKanaHistoryPaymentAgencyQueryProcessor = addKanaHistoryPaymentAgencyQueryProcessor;
            this.deleteKanaHistoryPaymentAgencyQueryProcessor = deleteKanaHistoryPaymentAgencyQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<KanaHistoryPaymentAgency>> GetAsync(KanaHistorySearch option, CancellationToken token = default(CancellationToken))
            => await kanaHistoryPaymentAgencyQueryProcessor.GetAsync(option, token);


        public async Task<int> DeleteAsync(KanaHistoryPaymentAgency history, CancellationToken token = default(CancellationToken))
            => await deleteKanaHistoryPaymentAgencyQueryProcessor.DeleteAsync(history, token);


        public async Task<ImportResult> ImportAsync(IEnumerable<KanaHistoryPaymentAgency> insert, IEnumerable<KanaHistoryPaymentAgency> update, IEnumerable<KanaHistoryPaymentAgency> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;
                foreach (var x in delete)
                {
                    await deleteKanaHistoryPaymentAgencyQueryProcessor.DeleteAsync(x, token);
                    ++deleteCount;
                }
                foreach (var x in update)
                {
                    await addKanaHistoryPaymentAgencyQueryProcessor.SaveAsync(x, token);
                    ++updateCount;
                }
                foreach (var x in insert)
                {
                    await addKanaHistoryPaymentAgencyQueryProcessor.SaveAsync(x, token);
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
