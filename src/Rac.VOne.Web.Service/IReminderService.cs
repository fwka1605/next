using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IReminderService" を変更できます。
    [ServiceContract]
    public interface IReminderService
    {
        [OperationContract]
        Task<ExistResult> ExistAsync(string SessionKey, int companyId);
        [OperationContract]
        Task<ReminderResult> GetItemsAsync(string SessionKey, ReminderSearch condition, ReminderCommonSetting setting, ReminderSummarySetting[] summary);
        [OperationContract]
        Task<ReminderSummaryResult> GetSummaryItemsAsync(string SessionKey, ReminderSearch condition, ReminderCommonSetting setting);
        [OperationContract]
        Task<ReminderSummaryResult> GetSummaryItemsByDestinationAsync(string SessionKey, ReminderSearch condition, ReminderCommonSetting setting);
        [OperationContract]
        Task<ReminderBillingResult> GetReminderBillingForPrintAsync(string SessionKey, int companyId, int[] reminderIds);
        [OperationContract]
        Task<ReminderBillingResult> GetReminderBillingForPrintByDestinationCodeAsync(string SessionKey, IEnumerable<Reminder> reminders);
        [OperationContract]
        Task<ReminderBillingResult> GetReminderBillingForSummaryPrintAsync(string SessionKey, int companyId, int[] customerIds);
        [OperationContract]
        Task<ReminderBillingResult> GetReminderBillingForSummaryPrintByDestinationCodeAsync(string SessionKey, IEnumerable<ReminderSummary> reminderSummaries);
        [OperationContract]
        Task<ReminderBillingResult> GetReminderBillingForReprintAsync(string SessionKey, int companyId, ReminderOutputed reminderOutputed);
        [OperationContract]
        Task<ReminderBillingResult> GetReminderBillingForReprintByDestinationAsync(string SessionKey, int companyId, ReminderOutputed reminderOutputed);
        [OperationContract]
        Task<ReminderResult> CreateAsync(string SessionKey, int companyId, int loginUserId, int useForeignCurrency, Reminder[] Reminder, ReminderCommonSetting setting, ReminderSummarySetting[] summary);
        [OperationContract]
        Task<ReminderHistoriesResult> GetHistoryItemsByReminderIdAsync(string SessionKey, int reminderId);
        [OperationContract]
        Task<ReminderSummaryHistoriesResult> GetSummaryHistoryItemsByReminderSummaryIdAsync(string SessionKey, int reminderSummaryId);
        [OperationContract]
        Task<CountResult> UpdateStatusAsync(string SessionKey, int loginUserId, Reminder[] Reminder);
        [OperationContract]
        Task<CountResult> UpdateSummaryStatusAsync(string SessionKey, int loginUserId, ReminderSummary[] ReminderSummary);
        [OperationContract]
        Task<CountResult> UpdateReminderOutputedAsync(string SessionKey, int loginUserId, ReminderOutputed[] ReminderOutputed);
        [OperationContract]
        Task<CountResult> UpdateReminderSummaryOutputedAsync(string SessionKey, int loginUserId, ReminderOutputed[] reminderOutputed, ReminderSummary[] ReminderSummary);
        [OperationContract]
        Task<ReminderOutputedResult> GetOutputedItemsAsync(string SessionKey, ReminderOutputedSearch search);
        [OperationContract]
        Task<CountResult> GetMaxOutputNoAsync(string SessionKey, int companyId);
        [OperationContract]
        Task<ReminderHistoriesResult> GetReminderListAsync(string SessionKey, ReminderSearch search, ReminderCommonSetting setting);
        [OperationContract]
        Task<ReminderHistoryResult> UpdateReminderHistoryAsync(string SessionKey, ReminderHistory reminderHistory);
        [OperationContract]
        Task<ReminderSummaryHistoryResult> UpdateReminderSummaryHistoryAsync(string SessionKey, ReminderSummaryHistory reminderSummaryHistory);
        [OperationContract]
        Task<CountResult> DeleteHistoryAsync(string SessionKey, ReminderHistory reminderHistory);
        [OperationContract]
        Task<CountResult> DeleteSummaryHistoryAsync(string SessionKey, ReminderSummaryHistory reminderSummaryHistory);
        [OperationContract]
        Task<ReminderResult> GetCancelDecisionItemsAsync(string SessionKey, ReminderSearch condition, ReminderCommonSetting setting, IEnumerable<ReminderSummarySetting> summary);
        [OperationContract]
        Task<CountResult> CancelRemindersAsync(string SessionKey, IEnumerable<Reminder> reminders);
        [OperationContract]
        Task<ExistResult> ExistDestinationAsync(string SessionKey, int DestinationId);
    }
}
