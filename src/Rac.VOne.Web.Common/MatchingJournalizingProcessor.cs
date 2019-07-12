using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class MatchingJournalizingProcessor : IMatchingJournalizingProcessor
    {
        private readonly IDbSystemDateTimeQueryProcessor dbSystemDateTimeQueryProcessor;
        private readonly IMatchingJournalizingQueryProcessor matchingJournalizingQueryProcessor;
        private readonly IMatchingJournalizingSummaryQueryProcessor matchingJournalizingSummaryQueryProcessor;
        private readonly IMatchingGeneralJournalizingQueryProcessor matchingGeneralJournalizingQueryProcessor;
        private readonly IMatchedReceiptQueryProcessor matchedReceiptQueryProcessor;
        private readonly IUpdateReceiptMatchingJournalizingQueryProcessor updateReceiptMatchingJournalizingQueryProcessor;
        private readonly IUpdateReceiptExcludeJournalizingQueryProcessor updateReceiptExcludeJournalizingQueryProcessor;
        private readonly IUpdateAdvanceReceivedBackupJournalizingQueryProcessor updateAdvanceReceivedBackupJournaliginzQueryProcessor;
        private readonly IUpdateMatchingJournalizingQueryProcessor updateMatchingJournalizingQueryProcessor;
        private readonly IMatchingJournalizingDetailQueryProcessor matchingJournalizingDetailQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MatchingJournalizingProcessor(
            IDbSystemDateTimeQueryProcessor dbSystemDateTimeQueryProcessor,
            IMatchingJournalizingQueryProcessor matchingJournalizingQueryProcessor,
            IMatchingJournalizingSummaryQueryProcessor matchingJournalizingSummaryQueryProcessor,
            IMatchingGeneralJournalizingQueryProcessor matchingGeneralJournalizingQueryProcessor,
            IMatchedReceiptQueryProcessor matchedReceiptQueryProcessor,
            IUpdateMatchingJournalizingQueryProcessor updateMatchingJournalizingQueryProcessor,
            IUpdateReceiptMatchingJournalizingQueryProcessor updateReceiptMatchingJournalizingQueryProcessor,
            IUpdateAdvanceReceivedBackupJournalizingQueryProcessor updateAdvanceReceivedBackupJournaliginzQueryProcessor,
            IUpdateReceiptExcludeJournalizingQueryProcessor updateReceiptExcludeJournalizingQueryProcessor,
            IMatchingJournalizingDetailQueryProcessor matchingJournalizingDetailQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.dbSystemDateTimeQueryProcessor = dbSystemDateTimeQueryProcessor;
            this.matchingJournalizingQueryProcessor = matchingJournalizingQueryProcessor;
            this.matchingJournalizingSummaryQueryProcessor = matchingJournalizingSummaryQueryProcessor;
            this.matchingGeneralJournalizingQueryProcessor = matchingGeneralJournalizingQueryProcessor;
            this.matchedReceiptQueryProcessor = matchedReceiptQueryProcessor;
            this.updateMatchingJournalizingQueryProcessor = updateMatchingJournalizingQueryProcessor;
            this.updateReceiptMatchingJournalizingQueryProcessor = updateReceiptMatchingJournalizingQueryProcessor;
            this.updateAdvanceReceivedBackupJournaliginzQueryProcessor = updateAdvanceReceivedBackupJournaliginzQueryProcessor;
            this.updateReceiptExcludeJournalizingQueryProcessor = updateReceiptExcludeJournalizingQueryProcessor;
            this.matchingJournalizingDetailQueryProcessor = matchingJournalizingDetailQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
            => await matchingJournalizingSummaryQueryProcessor.GetSummaryAsync(option, token);

        public async Task<IEnumerable<MatchingJournalizing>> ExtractAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
            => await matchingJournalizingQueryProcessor.ExtractAsync(option, token);

        public async Task<IEnumerable<GeneralJournalizing>> ExtractGeneralJournalizingAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
            => await matchingGeneralJournalizingQueryProcessor.GetGeneralJournalizingAsync(option, token);

        public async Task<IEnumerable<MatchedReceipt>> GetMatchedReceiptAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
            => await matchedReceiptQueryProcessor.GetMatchedReceiptsAsync(option, token);

        public async Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var count = 0;
            using (var scope = transactionScopeBuilder.Create())
            {
                option.UpdateAt = await dbSystemDateTimeQueryProcessor.GetAsync(token);
                count += await updateMatchingJournalizingQueryProcessor.CancelAsync(option, token);
                count += await updateReceiptMatchingJournalizingQueryProcessor.CancelAsync(option, token);
                count += await updateAdvanceReceivedBackupJournaliginzQueryProcessor.CancelAsync(option, token);
                count += await updateReceiptExcludeJournalizingQueryProcessor.CancelAsync(option, token);

                scope.Complete();
            }
            return count;
        }

        public async Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var count = 0;
            using (var scope = transactionScopeBuilder.Create())
            {
                option.UpdateAt = await dbSystemDateTimeQueryProcessor.GetAsync(token);
                count += await updateMatchingJournalizingQueryProcessor.UpdateAsync(option, token);
                count += await updateReceiptMatchingJournalizingQueryProcessor.UpdateAsync(option, token);
                count += await updateAdvanceReceivedBackupJournaliginzQueryProcessor.UpdateAsync(option, token);
                count += await updateReceiptExcludeJournalizingQueryProcessor.UpdateAsync(option, token);

                scope.Complete();
            }
            return count;
        }

        public async Task<IEnumerable<MatchingJournalizingDetail>> GetDetailsAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
            => await matchingJournalizingDetailQueryProcessor.GetDetailsAsync(option, token);

        public async Task<int> CancelDetailsAsync(IEnumerable<MatchingJournalizingDetail> details,
            CancellationToken token = default(CancellationToken))
        {
            var count = 0;

            using (var scope = transactionScopeBuilder.Create())
            {
                var updateAt = await dbSystemDateTimeQueryProcessor.GetAsync(token);
                foreach (var detail in details)
                {
                    if (detail.JournalizingType == JournalizingType.Matching)
                    {
                        var matching = new Matching
                        {
                            Id = detail.Id,
                            UpdateAt = updateAt,
                            UpdateBy = detail.UpdateBy,
                        };
                        count += await updateMatchingJournalizingQueryProcessor.CancelDetailAsync(matching, token);
                    }
                    if (detail.JournalizingType == JournalizingType.ReceiptExclude)
                    {
                        var exclude = new ReceiptExclude
                        {
                            Id = detail.Id,
                            UpdateAt = updateAt,
                            UpdateBy = detail.UpdateBy,
                        };
                        count += await updateReceiptExcludeJournalizingQueryProcessor.CancelDetailAsync(exclude, token);
                    }
                    if (detail.JournalizingType == JournalizingType.AdvanceReceivedOccured
                     || detail.JournalizingType == JournalizingType.AdvanceReceivedTransfer)
                    {
                        var receipt = new Receipt
                        {
                            Id = detail.Id,
                            UpdateAt = updateAt,
                            UpdateBy = detail.UpdateBy,
                        };
                        count += await updateReceiptMatchingJournalizingQueryProcessor.CancelDetailAsync(receipt, token);
                    }
                }
                scope.Complete();
            }

            return count;
        }

        public async Task<IEnumerable<MatchingJournalizing>> MFExtractAsync(JournalizingOption option,
            CancellationToken token = default(CancellationToken))
            => await matchingJournalizingQueryProcessor.MFExtractAsync(option, token);

    }
}
