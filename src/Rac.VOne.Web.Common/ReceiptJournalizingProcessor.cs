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
    public class ReceiptJournalizingProcessor :
        IReceiptJournalizingProcessor
    {
        private readonly IReceiptJournalizingQueryProcessor receiptJournalizingQueryProcessor;
        private readonly IReceiptGeneralJournalizingQueryProcessor receiptGeneralJournalizingQueryProcessor;
        private readonly IUpdateReceiptJournalizingQueryProcessor updateReceiptJournalizingQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ReceiptJournalizingProcessor(
            IReceiptJournalizingQueryProcessor receiptJournalizingQueryProcessor,
            IReceiptGeneralJournalizingQueryProcessor receiptGeneralJournalizingQueryProcessor,
            IUpdateReceiptJournalizingQueryProcessor updateReceiptJournalizingQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.receiptJournalizingQueryProcessor = receiptJournalizingQueryProcessor;
            this.receiptGeneralJournalizingQueryProcessor = receiptGeneralJournalizingQueryProcessor;
            this.updateReceiptJournalizingQueryProcessor = updateReceiptJournalizingQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
            => await receiptJournalizingQueryProcessor.GetSummaryAsync(option, token);

        public async Task<IEnumerable<ReceiptJournalizing>> ExtractAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
            => await receiptJournalizingQueryProcessor.ExtractAsync(option, token);

        public async Task<IEnumerable<GeneralJournalizing>> ExtractGeneralAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
            => await receiptGeneralJournalizingQueryProcessor.ExtractGeneralJournalizingAsync(option, token);

        public async Task<int> UpdateOutputAtAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var count = await updateReceiptJournalizingQueryProcessor.UpdateAsync(option, token);
                if (count > 0)
                    scope.Complete();
                return count;
            }

        }

        public async Task<int> CancelJournalizingAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var count = await updateReceiptJournalizingQueryProcessor.CancelAsync(option, token);
                if (count > 0)
                    scope.Complete();
                return count;
            }
        }

    }
}
