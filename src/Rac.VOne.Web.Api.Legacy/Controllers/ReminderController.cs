using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// 督促
    /// </summary>
    public class ReminderController : ApiControllerAuthorized
    {

        private readonly IReminderProcessor reminderProcessor;
        private readonly IReminderHistoryProcessor reminderHistoryProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public ReminderController(
            IReminderProcessor reminderProcessor,
            IReminderHistoryProcessor reminderHistoryProcessor
            )
        {
            this.reminderProcessor = reminderProcessor;
            this.reminderHistoryProcessor = reminderHistoryProcessor;
        }


        /// <summary>
        /// 会社ID のデータ登録があるか確認
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Exist([FromBody] int companyId, CancellationToken token)
            => await reminderProcessor.ExistAsync(companyId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Reminder>> GetItems(ReminderSearch option, CancellationToken token)
            => (await reminderProcessor.GetItemsAsync(option, option.Setting, option.SummarySettings, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderHistory>> GetHistoryItemsByReminderId([FromBody] int reminderId, CancellationToken token)
            => (await reminderHistoryProcessor.GetItemsByReminderIdAsync(reminderId, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderSummaryId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderSummaryHistory>> GetSummaryHistoryItemsByReminderSummaryId([FromBody] int reminderSummaryId, CancellationToken token)
            => (await reminderHistoryProcessor.GetSummaryItemsByReminderSummaryIdAsync(reminderSummaryId, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderSummary>> GetSummaryItems(ReminderSearch option, CancellationToken token)
            => (await reminderProcessor.GetSummaryItemsAsync(option, option.Setting)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderSummary>> GetSummaryItemsByDestination(ReminderSearch option, CancellationToken token)
            => (await reminderProcessor.GetSummaryItemsByDestinationAsync(option, option.Setting)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Reminder>> Create(ReminderSource source, CancellationToken token)
            => (await reminderProcessor.CreateAsync(source.CompanyId, source.LoginUserId, source.UseForeignCurrency, source.Items, source.Setting, source.SummarySettings, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> UpdateStatus(ReminderSource source, CancellationToken token)
            => await reminderProcessor.UpdateStatusAsync(source.LoginUserId, source.Items, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> UpdateSummaryStatus(ReminderSource source, CancellationToken token)
            => await reminderProcessor.UpdateSummaryStatusAsync(source.LoginUserId, source.Summaries, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingForPrint(ReminderSource source, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForPrintAsync(source.CompanyId, source.ReminderIds, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminders"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingForPrintByDestinationCode(IEnumerable<Reminder> reminders, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForPrintByDestinationCodeAsync(reminders, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingForSummaryPrint(ReminderSource source, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForSummaryPrintAsync(source.CompanyId, source.CustomerIds, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderSummaries"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingForSummaryPrintByDestinationCode(IEnumerable<ReminderSummary> reminderSummaries, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForSummaryPrintByDestinationCodeAsync(reminderSummaries, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingForReprint(ReminderSource source, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForReprintAsync(source.CompanyId, source.ReminderOutputed, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderBilling>> GetReminderBillingForReprintByDestination(ReminderSource source, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForReprintByDestinationAsync(source.CompanyId, source.ReminderOutputed, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> UpdateReminderOutputed(ReminderSource source, CancellationToken token)
            => await reminderProcessor.UpdateReminderOutputedAsync(source.LoginUserId, source.ReminderOutputeds, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> UpdateReminderSummaryOutputed(ReminderSource source, CancellationToken token)
            => await reminderProcessor.UpdateReminderSummaryOutputedAsync(source.LoginUserId, source.ReminderOutputeds, source.Summaries, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderOutputed>> GetOutputedItems(ReminderOutputedSearch option, CancellationToken token)
            => (await reminderProcessor.GetOutputedItemsAsync(option, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> GetMaxOutputNo([FromBody] int companyId, CancellationToken token)
            => await reminderProcessor.GetMaxOutputNoAsync(companyId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderHistory>> GetReminderList(ReminderSearch option, CancellationToken token)
            => (await reminderProcessor.GetReminderListAsync(option, option.Setting)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderHistory"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReminderHistory> UpdateReminderHistory(ReminderHistory reminderHistory, CancellationToken token)
            => await reminderHistoryProcessor.UpdateReminderHistoryAsync(reminderHistory, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderSummaryHistory"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReminderSummaryHistory> UpdateReminderSummaryHistory(ReminderSummaryHistory reminderSummaryHistory, CancellationToken token)
            => await reminderHistoryProcessor.UpdateReminderSummaryHistoryAsync(reminderSummaryHistory, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderHistory"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> DeleteHistory(ReminderHistory reminderHistory, CancellationToken token)
            => await reminderHistoryProcessor.DeleteHistoryAsync(reminderHistory, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderSummaryHistory"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> DeleteSummaryHistory(ReminderSummaryHistory reminderSummaryHistory, CancellationToken token)
            => await reminderHistoryProcessor.DeleteSummaryHistoryAsync(reminderSummaryHistory, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Reminder>> GetCancelDecisionItems(ReminderSearch option, CancellationToken token)
            => (await reminderProcessor.GetCancelDecisionItemsAsync(option, option.Setting, option.SummarySettings, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminders"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> CancelReminders(IEnumerable<Reminder> reminders, CancellationToken token)
            => await reminderProcessor.CancelAsync(reminders, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DestinationId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistDestination([FromBody] int DestinationId, CancellationToken token)
            => await reminderProcessor.ExistDestinationAsync(DestinationId, token);

    }
}
