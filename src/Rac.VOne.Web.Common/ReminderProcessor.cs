using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class ReminderProcessor :
        IReminderProcessor
    {
        private readonly IReminderQueryProcessor reminderQueryProcessor;
        private readonly IByCompanyGetEntitiesQueryProcessor<ReminderCommonSetting> getReminderCommonSettingQueryProcessor;
        private readonly IReminderSummarySettingQueryProcessor reminderSummarySettingQueryProcessor;
        private readonly IUpdateBillingReminderQueryProcessor updateBillingReminderQueryProcessor;
        private readonly IStatusQueryProcessor statusQueryProcessor;
        private readonly IAddReminderQueryProcessor addReminderQueryProcessor;
        private readonly IAddReminderHistoryQueryProcessor addReminderHistoryQueryProcessor;
        private readonly IReminderOutputedQueryProcessor reminderOutputedQueryProcessor;
        private readonly IUpdateReminderQueryProcessor updateReminderQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<Reminder> deleteReminderQueryProcessor;
        private readonly IDeleteReminderHistoryQueryProcessor deleteReminderHistoryQueryProcessor;
        private readonly IDeleteReminderSummaryQueryProcessor deleteReminderSummaryQueryProcessor;
        private readonly IReminderOutputedExistsQueryProcessor reminderOutputedExistsQueryProcessor;
        private readonly IByCompanyExistQueryProcessor<Reminder> reminderByCompanyExistQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ReminderProcessor(
            IReminderQueryProcessor reminderQueryProcessor,
            IAddReminderQueryProcessor addReminderQueryProcessor,
            IUpdateReminderQueryProcessor updateReminderQueryProcessor,
            IByCompanyGetEntitiesQueryProcessor<ReminderCommonSetting> getReminderCommonSettingQueryProcessor,
            IUpdateBillingReminderQueryProcessor updateBillingReminderQueryProcessor,
            IReminderSummarySettingQueryProcessor reminderSummarySettingQueryProcessor,
            IStatusQueryProcessor statusQueryProcessor,
            IAddReminderHistoryQueryProcessor addReminderHistoryQueryProcessor,
            IReminderOutputedQueryProcessor reminderOutputedQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<Reminder> deleteReminderQueryProcessor,
            IDeleteReminderHistoryQueryProcessor deleteReminderHistoryQueryProcessor,
            IDeleteReminderSummaryQueryProcessor deleteReminderSummaryQueryProcessor,
            IReminderOutputedExistsQueryProcessor reminderOutputedExistsQueryProcessor,
            IByCompanyExistQueryProcessor<Reminder> reminderByCompanyExistQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.reminderQueryProcessor = reminderQueryProcessor;
            this.addReminderQueryProcessor = addReminderQueryProcessor;
            this.updateReminderQueryProcessor = updateReminderQueryProcessor;
            this.getReminderCommonSettingQueryProcessor = getReminderCommonSettingQueryProcessor;
            this.updateBillingReminderQueryProcessor = updateBillingReminderQueryProcessor;
            this.reminderSummarySettingQueryProcessor = reminderSummarySettingQueryProcessor;
            this.statusQueryProcessor = statusQueryProcessor;
            this.addReminderHistoryQueryProcessor = addReminderHistoryQueryProcessor;
            this.reminderOutputedQueryProcessor = reminderOutputedQueryProcessor;
            this.deleteReminderQueryProcessor = deleteReminderQueryProcessor;
            this.deleteReminderHistoryQueryProcessor = deleteReminderHistoryQueryProcessor;
            this.deleteReminderSummaryQueryProcessor = deleteReminderSummaryQueryProcessor;
            this.reminderOutputedExistsQueryProcessor = reminderOutputedExistsQueryProcessor;
            this.reminderByCompanyExistQueryProcessor = reminderByCompanyExistQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<bool> ExistAsync(int companyId, CancellationToken token = default(CancellationToken))
            => await reminderByCompanyExistQueryProcessor.ExistAsync(companyId, token);

        public async Task<IEnumerable<Reminder>> GetItemsAsync(ReminderSearch search, ReminderCommonSetting setting, ReminderSummarySetting[] summary, CancellationToken token = default(CancellationToken))
            => await reminderQueryProcessor.GetItemsAsync(search, setting, summary, token);

        public async Task<IEnumerable<ReminderSummary>> GetSummaryItemsAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken))
            => await reminderQueryProcessor.GetSummaryItemsAsync(search, setting, token);

        public async Task<IEnumerable<ReminderSummary>> GetSummaryItemsByDestinationAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken))
            => await reminderQueryProcessor.GetSummaryItemsByDestinationAsync(search, setting, token);

        public async Task<IEnumerable<Reminder>> CreateAsync(int companyId, int loginUserId, int useForeignCurrency, Reminder[] Reminder, ReminderCommonSetting setting, ReminderSummarySetting[] summary, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                //var commonSetting = getReminderCommonSettingQueryProcessor.GetItems(companyId);
                //if (commonSetting == null || !commonSetting.First().Equals(setting))
                //    return null;

                //var summarySetting = reminderSummarySettingQueryProcessor.GetItems(companyId, useForeignCurrency);
                //if (summarySetting == null || !summarySetting.ToArray().Equals(summary))
                //    return null;

                var defaultStatusId = (await statusQueryProcessor.GetAsync(new StatusSearch { CompanyId = companyId, StatusType = 1, Codes = new[] { "00" } }, token)).First().Id;

                var result = new List<Reminder>();
                foreach(var r in Reminder)
                {
                    r.StatusId = defaultStatusId;
                    var reminderEntity = await addReminderQueryProcessor.AddAsync(r, token);
                    r.Id = reminderEntity.Id;

                    var reminderSummary = new ReminderSummary()
                    {
                        CustomerId = r.CustomerId,
                        CurrencyId = r.CurrencyId,
                        Memo = "",
                    };
                    await addReminderQueryProcessor.AddSummaryAsync(reminderSummary, token);

                    var billingUpdateResult = (await updateBillingReminderQueryProcessor.UpdateAsync(r, setting, summary)).ToArray();

                    if (r.DetailCount != billingUpdateResult.Length || r.RemainAmount != billingUpdateResult.Sum(x => x.RemainAmount))
                        return null;

                    result.Add(reminderEntity);
                }
                scope.Complete();

                return result.ToArray();

            }
        }

        public async Task<int> CancelAsync(IEnumerable<Reminder> Reminder, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var count = 0;

                foreach (var reminder in Reminder)
                {
                    var billingResult = await updateBillingReminderQueryProcessor.CancelAsync(reminder.Id, token);
                    if (billingResult == 0) return -1;

                    var historyResult = await deleteReminderHistoryQueryProcessor.DeleteByReminderIdAsync(reminder.Id, token);

                    var summaryResult = await deleteReminderSummaryQueryProcessor.DeleteReminderSummaryAsync(reminder, token);
                    var summaryHistoryResult = await deleteReminderHistoryQueryProcessor.DeleteReminderSummaryHistoryAsync(reminder, token);

                    var reminderResult = await deleteReminderQueryProcessor.DeleteAsync(reminder.Id, token);
                    if (reminderResult == 0) return -1;

                    count = reminderResult;
                }
                scope.Complete();

                return count;
            }
        }

        public async Task<int> UpdateStatusAsync(int loginUserId, Reminder[] Reminder, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var count = 0;
                foreach(var r in Reminder)
                {
                    var result = await updateReminderQueryProcessor.UpdateStatusAsync(r, token);
                    var history = new ReminderHistory()
                    {
                        ReminderId = result.Id,
                        StatusId = result.StatusId,
                        Memo = result.Memo,
                        OutputFlag = result.OutputAt != null ? 1 : 0,
                        InputType = (int)ReminderHistory.ReminderHistoryInputType.StatusChange,
                        ReminderAmount = r.ReminderAmount,
                        CreateBy = loginUserId,
                    };

                    var historyResult = await addReminderHistoryQueryProcessor.AddAsync(history, token);

                    if (historyResult == null)
                    {
                        return -1;
                    }
                    else
                    {
                        count++;
                    }
                }
                scope.Complete();
                return count;
            }
        }

        public async Task<int> UpdateSummaryStatusAsync(int loginUserId, ReminderSummary[] ReminderSummary, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var count = 0;
                foreach (var r in ReminderSummary)
                {
                    var result = await updateReminderQueryProcessor.UpdateSummaryStatusAsync(r, token);
                    var history = new ReminderSummaryHistory()
                    {
                        ReminderSummaryId = r.Id,
                        Memo = result.Memo,
                        InputType = (int)ReminderSummaryHistory.ReminderSummaryHistoryInputType.StatusChange,
                        ReminderAmount = r.ReminderAmount,
                        CreateBy = loginUserId,
                    };

                    var historyResult = await addReminderHistoryQueryProcessor.AddSummaryAsync(history, token);

                    if (historyResult == null)
                    {
                        return -1;
                    }
                    else
                    {
                        count++;
                    }
                }
                scope.Complete();

                return count;
            }
        }

        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForPrintAsync(int companyId, int[] reminderIds, CancellationToken token = default(CancellationToken))
            => await reminderQueryProcessor.GetReminderBillingItemsForPrintAsync(companyId, reminderIds, token);

        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForPrintByDestinationCodeAsync(IEnumerable<Reminder> reminders, CancellationToken token = default(CancellationToken))
        {
            var reminderBillings = new List<ReminderBilling>();
            foreach (var reminder in reminders)
            {
                reminderBillings.AddRange(await reminderQueryProcessor.GetReminderBillingItemsForPrintByDestinationCodeAsync(reminder, token));
            }
            return reminderBillings;
        }

        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForSummaryPrintAsync(int companyId, int[] customerIds, CancellationToken token = default(CancellationToken))
            => await reminderQueryProcessor.GetReminderBillingItemsForSummaryPrintAsync(companyId, customerIds, token);

        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForSummaryPrintByDestinationCodeAsync(IEnumerable<ReminderSummary> reminderSummaries, CancellationToken token = default(CancellationToken))
        {
            var reminderBillings = new List<ReminderBilling>();
            foreach (var summary in reminderSummaries)
            {
                reminderBillings.AddRange(await reminderQueryProcessor.GetReminderBillingItemsForSummaryPrintByDestinationCodeAsync(summary, token));
            }
            return reminderBillings;
        }

        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForReprintAsync(int companyId, ReminderOutputed reminderOutputed, CancellationToken token = default(CancellationToken))
            => await reminderQueryProcessor.GetReminderBillingItemsForReprintAsync(companyId, reminderOutputed, token);

        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForReprintByDestinationAsync(int companyId, ReminderOutputed reminderOutputed, CancellationToken token = default(CancellationToken))
            => await reminderQueryProcessor.GetReminderBillingItemsForReprintByDestinationAsync(companyId, reminderOutputed, token);

        public async Task<int> UpdateReminderOutputedAsync(int loginUserId, ReminderOutputed[] ReminderOutputed, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var count = await AddReminderOutputedAsync(ReminderOutputed, token);

                if (count <= 0)
                    return count;

                foreach (var ro in ReminderOutputed.GroupBy(x => x.ReminderId).Select(x => new ReminderOutputed
                {
                    ReminderId = x.Key,
                    OutputAt = x.Min(r => r.OutputAt),
                    RemainAmount = x.Sum(r => r.RemainAmount)
                }))
                {
                    await reminderOutputedQueryProcessor.UpdateReminderAsync(ro.ReminderId, ro.OutputAt, token);
                    await reminderOutputedQueryProcessor.AddReminderHistoryAsync(loginUserId, ro.ReminderId, ro.OutputAt, ro.RemainAmount, token);
                }

                scope.Complete();

                return count;
            }
        }

        private async Task<int> AddReminderOutputedAsync(IEnumerable<ReminderOutputed> reminderOutputed, CancellationToken token)
        {
            var count = 0;
            foreach (var ro in reminderOutputed)
            {
                var addResult = await reminderOutputedQueryProcessor.AddAsync(ro, token);
                if (addResult > 0)
                    count++;
                else
                    return -1;
            }
            return count;
        }

        public async Task<int> UpdateReminderSummaryOutputedAsync(int loginUserId, ReminderOutputed[] reminderOutputed, ReminderSummary[] reminderSummary, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var count = await AddReminderOutputedAsync(reminderOutputed, token);

                if (count <= 0)
                    return count;

                foreach (var rh in reminderSummary)
                {
                    var history = new ReminderSummaryHistory()
                    {
                        ReminderSummaryId = rh.Id,
                        Memo = "",
                        InputType = (int)ReminderSummaryHistory.ReminderSummaryHistoryInputType.Output,
                        ReminderAmount = rh.ReminderAmount,
                        CreateBy = loginUserId,
                    };

                    var result = await addReminderHistoryQueryProcessor.AddSummaryAsync(history, token);
                    if (result == null)
                        return -1;
                    else
                        count++;
                }
                scope.Complete();

                return count;
            }
        }

        public async Task<IEnumerable<ReminderOutputed>> GetOutputedItemsAsync(ReminderOutputedSearch search, CancellationToken token = default(CancellationToken))
            => await reminderOutputedQueryProcessor.GetItemsAsync(search, token);

        public async Task<int> GetMaxOutputNoAsync(int companyId, CancellationToken token = default(CancellationToken))
            => await reminderOutputedQueryProcessor.GetMaxOutputNoAsync(companyId, token);

        public async Task<IEnumerable<ReminderHistory>> GetReminderListAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken))
            => await reminderQueryProcessor.GetReminderListAsync(search, setting, token);

        public async Task<IEnumerable<Reminder>> GetCancelDecisionItemsAsync(ReminderSearch search, ReminderCommonSetting setting, IEnumerable<ReminderSummarySetting> summary, CancellationToken token = default(CancellationToken))
            => await reminderQueryProcessor.GetCancelDecisionItemsAsync(search, setting, summary, token);

        public async Task<bool> ExistDestinationAsync(int DestinationId, CancellationToken token = default(CancellationToken))
            => await reminderOutputedExistsQueryProcessor.ExistDestinationAsync(DestinationId, token);

    }
}
