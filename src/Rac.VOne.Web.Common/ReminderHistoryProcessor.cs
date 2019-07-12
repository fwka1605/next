using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class ReminderHistoryProcessor :
        IReminderHistoryProcessor
    {
        private readonly IReminderHistoryQueryProcessor reminderHistoryQueryProcessor;
        private readonly IUpdateReminderHistoryQueryProcessor updateReminderHistoryQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<ReminderHistory> deleteReminderHistoryQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<ReminderSummaryHistory> deleteReminderSummaryHistoryQueryProcessor;
        private readonly IUpdateReminderQueryProcessor updateReminderQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ReminderHistoryProcessor(
            IReminderHistoryQueryProcessor reminderHistoryQueryProcessor,
            IUpdateReminderHistoryQueryProcessor updateReminderHistoryQueryProcessor,
            IDeleteTransactionQueryProcessor<ReminderHistory> deleteReminderHistoryQueryProcessor,
            IDeleteTransactionQueryProcessor<ReminderSummaryHistory> deleteReminderSummaryHistoryQueryProcessor,
            IUpdateReminderQueryProcessor updateReminderQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.reminderHistoryQueryProcessor = reminderHistoryQueryProcessor;
            this.updateReminderHistoryQueryProcessor = updateReminderHistoryQueryProcessor;
            this.deleteReminderHistoryQueryProcessor = deleteReminderHistoryQueryProcessor;
            this.deleteReminderSummaryHistoryQueryProcessor = deleteReminderSummaryHistoryQueryProcessor;
            this.updateReminderQueryProcessor = updateReminderQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<ReminderHistory>> GetItemsByReminderIdAsync(int reminderId, CancellationToken token = default(CancellationToken))
            => await reminderHistoryQueryProcessor.GetItemsByReminderIdAsync(reminderId, token);

        public async Task<IEnumerable<ReminderSummaryHistory>> GetSummaryItemsByReminderSummaryIdAsync(int reminderSummaryId, CancellationToken token = default(CancellationToken))
            => await reminderHistoryQueryProcessor.GetSummaryItemsByReminderSummaryIdAsync(reminderSummaryId, token);

        public async Task<ReminderHistory> UpdateReminderHistoryAsync(ReminderHistory reminderHistory, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = await updateReminderHistoryQueryProcessor.UpdateReminderHistoryAsync(reminderHistory, token);

                if (reminderHistory.IsUpdateStatusMemo)
                {
                    var reminder = new Reminder()
                    {
                        Id = reminderHistory.ReminderId,
                        StatusId = reminderHistory.StatusId,
                        Memo = reminderHistory.Memo,
                    };
                    var reminderSummaryResult = await updateReminderQueryProcessor.UpdateStatusAsync(reminder);
                }
                scope.Complete();

                return result;
            }
        }

        public async Task<ReminderSummaryHistory> UpdateReminderSummaryHistoryAsync(ReminderSummaryHistory reminderSummaryHistory, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = await updateReminderHistoryQueryProcessor.UpdateReminderSummaryHistoryAsync(reminderSummaryHistory, token);

                if (reminderSummaryHistory.IsUpdateSummaryMemo)
                {
                    var rs = new ReminderSummary()
                    {
                        Id = reminderSummaryHistory.ReminderSummaryId,
                        Memo = reminderSummaryHistory.Memo,
                    };
                    var reminderSummaryResult = await updateReminderQueryProcessor.UpdateSummaryStatusAsync(rs, token);
                }
                scope.Complete();

                return result;
            }
        }

        public async Task<int> DeleteHistoryAsync(ReminderHistory reminderHistory, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = await deleteReminderHistoryQueryProcessor.DeleteAsync(reminderHistory.Id, token);

                if (reminderHistory.IsUpdateStatusMemo)
                {
                    var item = (await reminderHistoryQueryProcessor.GetItemsByReminderIdAsync(reminderHistory.ReminderId, token)).FirstOrDefault();

                    var reminder = new Reminder()
                    {
                        Id = reminderHistory.ReminderId,
                        StatusId = item?.StatusId ?? reminderHistory.StatusId,
                        Memo = item?.Memo ?? string.Empty,
                    };
                    await updateReminderQueryProcessor.UpdateStatusAsync(reminder, token);
                }
                scope.Complete();

                return result;
            }
        }

        public async Task<int> DeleteSummaryHistoryAsync(ReminderSummaryHistory reminderSummaryHistory, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = await deleteReminderSummaryHistoryQueryProcessor.DeleteAsync(reminderSummaryHistory.Id, token);

                if (reminderSummaryHistory.IsUpdateSummaryMemo)
                {
                    var rsh = (await reminderHistoryQueryProcessor.GetSummaryItemsByReminderSummaryIdAsync(reminderSummaryHistory.ReminderSummaryId, token)).FirstOrDefault();

                    var rs = new ReminderSummary()
                    {
                        Id = reminderSummaryHistory.ReminderSummaryId,
                        Memo = rsh?.Memo ?? string.Empty,
                    };
                    var reminderSummaryResult = await updateReminderQueryProcessor.UpdateSummaryStatusAsync(rs, token);
                }

                scope.Complete();

                return result;
            }
        }


    }
}
