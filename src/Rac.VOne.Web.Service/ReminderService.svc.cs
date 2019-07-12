using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "ReminderService" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで ReminderService.svc または ReminderService.svc.cs を選択し、デバッグを開始してください。
    public class ReminderService : IReminderService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IReminderProcessor reminderProcessor;
        private readonly IReminderHistoryProcessor reminderHistoryProcessor;
        
        private readonly ILogger logger;

        public ReminderService(
            IAuthorizationProcessor authorizationProcessor,
            IReminderProcessor reminderProcessor,
            IReminderHistoryProcessor reminderHistoryProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.reminderProcessor = reminderProcessor;
            this.reminderHistoryProcessor = reminderHistoryProcessor;
            logger = logManager.GetLogger(typeof(ReminderService));
        }

        public async Task<ExistResult> ExistAsync(string SessionKey, int companyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderProcessor.ExistAsync(companyId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ReminderResult> GetItemsAsync(string SessionKey, ReminderSearch condition, ReminderCommonSetting setting, ReminderSummarySetting[] summary)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetItemsAsync(condition, setting, summary, token)).ToList();

                return new ReminderResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Reminder = result,
                };
            }, logger);
        }

        public async Task<ReminderHistoriesResult> GetHistoryItemsByReminderIdAsync(string SessionKey, int reminderId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderHistoryProcessor.GetItemsByReminderIdAsync(reminderId, token)).ToList();

                return new ReminderHistoriesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderHistories = result,
                };
            }, logger);
        }

        public async Task<ReminderSummaryHistoriesResult> GetSummaryHistoryItemsByReminderSummaryIdAsync(string SessionKey, int reminderSummaryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderHistoryProcessor.GetSummaryItemsByReminderSummaryIdAsync(reminderSummaryId, token)).ToList();

                return new ReminderSummaryHistoriesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderSummaryHistories = result,
                };
            }, logger);
        }

        public async Task<ReminderSummaryResult> GetSummaryItemsAsync(string SessionKey, ReminderSearch condition, ReminderCommonSetting setting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetSummaryItemsAsync(condition, setting, token)).ToList();

                return new ReminderSummaryResult()
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderSummary = result,
                };
            }, logger);
        }

        public async Task<ReminderSummaryResult> GetSummaryItemsByDestinationAsync(string SessionKey, ReminderSearch condition, ReminderCommonSetting setting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetSummaryItemsByDestinationAsync(condition, setting, token)).ToList();

                return new ReminderSummaryResult()
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderSummary = result,
                };
            }, logger);
        }

        public async Task<ReminderResult> CreateAsync(string SessionKey, int companyId, int loginUserId, int useForeignCurrency, Reminder[] Reminder, ReminderCommonSetting setting, ReminderSummarySetting[] summary)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var processResult = new ProcessResult();
                var result = (await reminderProcessor.CreateAsync(companyId, loginUserId, useForeignCurrency, Reminder, setting, summary, token)).ToList();

                if (result != null)
                {
                    processResult.Result = true;
                }

                return new ReminderResult
                {
                    ProcessResult = processResult,
                    Reminder = result,
                };
            }, logger);
        }

        public async Task<CountResult> UpdateStatusAsync(string SessionKey, int loginUserId, Reminder[] Reminder)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var processResult = new ProcessResult();
                var result = await reminderProcessor.UpdateStatusAsync(loginUserId, Reminder, token);
                if (result > 0)
                {
                    processResult.Result = true;
                }

                return new CountResult
                {
                    ProcessResult = processResult,
                    Count = result,
                };
            }, logger);
        }

        public async Task<CountResult> UpdateSummaryStatusAsync(string SessionKey, int loginUserId, ReminderSummary[] ReminderSummary)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var processResult = new ProcessResult();
                var result = await reminderProcessor.UpdateSummaryStatusAsync(loginUserId, ReminderSummary, token);

                if (result > 0)
                {
                    processResult.Result = true;
                }

                return new CountResult
                {
                    ProcessResult = processResult,
                    Count = result,
                };
            }, logger);
        }

        public async Task<ReminderBillingResult> GetReminderBillingForPrintAsync(string SessionKey, int companyId, int[] reminderIds)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetReminderBillingItemsForPrintAsync(companyId, reminderIds, token)).ToList();

                return new ReminderBillingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderBilling = result,
                };
            }, logger);
        }

        public async Task<ReminderBillingResult> GetReminderBillingForPrintByDestinationCodeAsync(string SessionKey, IEnumerable<Reminder> reminders)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetReminderBillingItemsForPrintByDestinationCodeAsync(reminders, token)).ToList();

                return new ReminderBillingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderBilling = result,
                };
            }, logger);
        }

        public async Task<ReminderBillingResult> GetReminderBillingForSummaryPrintAsync(string SessionKey, int companyId, int[] customerIds)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetReminderBillingItemsForSummaryPrintAsync(companyId, customerIds, token)).ToList();

                return new ReminderBillingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderBilling = result,
                };
            }, logger);
        }

        public async Task<ReminderBillingResult> GetReminderBillingForSummaryPrintByDestinationCodeAsync(string SessionKey, IEnumerable<ReminderSummary> reminderSummaries)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetReminderBillingItemsForSummaryPrintByDestinationCodeAsync(reminderSummaries, token)).ToList();

                return new ReminderBillingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderBilling = result,
                };
            }, logger);
        }

        public async Task<ReminderBillingResult> GetReminderBillingForReprintAsync(string SessionKey, int companyId, ReminderOutputed reminderOutputed)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetReminderBillingItemsForReprintAsync(companyId, reminderOutputed, token)).ToList();

                return new ReminderBillingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderBilling = result,
                };
            }, logger);
        }

        public async Task<ReminderBillingResult> GetReminderBillingForReprintByDestinationAsync(string SessionKey, int companyId, ReminderOutputed reminderOutputed)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetReminderBillingItemsForReprintByDestinationAsync(companyId, reminderOutputed, token)).ToList();

                return new ReminderBillingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderBilling = result,
                };
            }, logger);
        }

        public async Task<CountResult> UpdateReminderOutputedAsync(string SessionKey, int loginUserId, ReminderOutputed[] ReminderOutputed)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var processResult = new ProcessResult();
                var result = await reminderProcessor.UpdateReminderOutputedAsync(loginUserId, ReminderOutputed, token);
                if (result > 0)
                {
                    processResult.Result = true;
                }

                return new CountResult
                {
                    ProcessResult = processResult,
                    Count = result,
                };
            }, logger);
        }

        public async Task<CountResult> UpdateReminderSummaryOutputedAsync(string SessionKey, int loginUserId, ReminderOutputed[] reminderOutputed, ReminderSummary[] ReminderSummary)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var processResult = new ProcessResult();
                var result = await reminderProcessor.UpdateReminderSummaryOutputedAsync(loginUserId, reminderOutputed, ReminderSummary, token);
                if (result > 0)
                {
                    processResult.Result = true;
                }

                return new CountResult
                {
                    ProcessResult = processResult,
                    Count = result,
                };
            }, logger);
        }

        public async Task<ReminderOutputedResult> GetOutputedItemsAsync(string SessionKey, ReminderOutputedSearch search)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var processResult = new ProcessResult();
                var result = (await reminderProcessor.GetOutputedItemsAsync(search, token)).ToList();
                if (result != null)
                {
                    processResult.Result = true;
                }

                return new ReminderOutputedResult
                {
                    ProcessResult = processResult,
                    ReminderOutputed = result,
                };
            }, logger);
        }

        public async Task<CountResult> GetMaxOutputNoAsync(string SessionKey, int companyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderProcessor.GetMaxOutputNoAsync(companyId, token);

                return new CountResult
                {
                    ProcessResult = new ProcessResult() { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ReminderHistoriesResult> GetReminderListAsync(string SessionKey, ReminderSearch search, ReminderCommonSetting setting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetReminderListAsync(search, setting, token)).ToList();

                return new ReminderHistoriesResult
                {
                    ProcessResult = new ProcessResult() { Result = true },
                    ReminderHistories = result,
                };
            }, logger);
        }

        public async Task<ReminderHistoryResult> UpdateReminderHistoryAsync(string SessionKey, ReminderHistory reminderHistory)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderHistoryProcessor.UpdateReminderHistoryAsync(reminderHistory, token);
                return new ReminderHistoryResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderHistory = result,
                };
            }, logger);
        }

        public async Task<ReminderSummaryHistoryResult> UpdateReminderSummaryHistoryAsync(string SessionKey, ReminderSummaryHistory reminderSummaryHistory)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderHistoryProcessor.UpdateReminderSummaryHistoryAsync(reminderSummaryHistory, token);
                return new ReminderSummaryHistoryResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderSummaryHistory = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteHistoryAsync(string SessionKey, ReminderHistory reminderHistory)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderHistoryProcessor.DeleteHistoryAsync(reminderHistory, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteSummaryHistoryAsync(string SessionKey, ReminderSummaryHistory reminderSummaryHistory)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderHistoryProcessor.DeleteSummaryHistoryAsync(reminderSummaryHistory, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ReminderResult> GetCancelDecisionItemsAsync(string SessionKey, ReminderSearch condition, ReminderCommonSetting setting, IEnumerable<ReminderSummarySetting> summary)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderProcessor.GetCancelDecisionItemsAsync(condition, setting, summary, token)).ToList();

                return new ReminderResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Reminder = result,
                };
            }, logger);
        }

        public async Task<CountResult> CancelRemindersAsync(string SessionKey, IEnumerable<Reminder> reminders)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderProcessor.CancelAsync(reminders, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistDestinationAsync(string SessionKey, int DestinationId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderProcessor.ExistDestinationAsync(DestinationId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

    }
}
