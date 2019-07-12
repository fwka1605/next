using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// 督促
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReminderController : ControllerBase
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
        public async Task<ActionResult<bool>> Exist([FromBody] int companyId, CancellationToken token)
            => await reminderProcessor.ExistAsync(companyId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetItems(ReminderSearch option, CancellationToken token)
            => (await reminderProcessor.GetItemsAsync(option, option.Setting, option.SummarySettings, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< ReminderHistory >>> GetHistoryItemsByReminderId([FromBody] int reminderId, CancellationToken token)
            => (await reminderHistoryProcessor.GetItemsByReminderIdAsync(reminderId, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderSummaryId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< ReminderSummaryHistory >>> GetSummaryHistoryItemsByReminderSummaryId([FromBody] int reminderSummaryId, CancellationToken token)
            => (await reminderHistoryProcessor.GetSummaryItemsByReminderSummaryIdAsync(reminderSummaryId, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReminderSummary>>> GetSummaryItems(ReminderSearch option, CancellationToken token)
            => (await reminderProcessor.GetSummaryItemsAsync(option, option.Setting)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReminderSummary>>> GetSummaryItemsByDestination(ReminderSearch option, CancellationToken token)
            => (await reminderProcessor.GetSummaryItemsByDestinationAsync(option, option.Setting)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Reminder>>> Create(ReminderSource source, CancellationToken token)
            => (await reminderProcessor.CreateAsync(source.CompanyId, source.LoginUserId, source.UseForeignCurrency, source.Items, source.Setting, source.SummarySettings, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> UpdateStatus(ReminderSource source, CancellationToken token)
            => await reminderProcessor.UpdateStatusAsync(source.LoginUserId, source.Items, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> UpdateSummaryStatus(ReminderSource source, CancellationToken token)
            => await reminderProcessor.UpdateSummaryStatusAsync(source.LoginUserId, source.Summaries, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReminderBilling>>> GetReminderBillingForPrint(ReminderSource source, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForPrintAsync(source.CompanyId, source.ReminderIds, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminders"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReminderBilling>>> GetReminderBillingForPrintByDestinationCode(IEnumerable<Reminder> reminders, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForPrintByDestinationCodeAsync(reminders, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReminderBilling>>> GetReminderBillingForSummaryPrint(ReminderSource source, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForSummaryPrintAsync(source.CompanyId, source.CustomerIds, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderSummaries"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReminderBilling>>> GetReminderBillingForSummaryPrintByDestinationCode(IEnumerable<ReminderSummary> reminderSummaries, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForSummaryPrintByDestinationCodeAsync(reminderSummaries, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReminderBilling>>> GetReminderBillingForReprint(ReminderSource source, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForReprintAsync(source.CompanyId, source.ReminderOutputed, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReminderBilling>>> GetReminderBillingForReprintByDestination(ReminderSource source, CancellationToken token)
            => (await reminderProcessor.GetReminderBillingItemsForReprintByDestinationAsync(source.CompanyId, source.ReminderOutputed, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> UpdateReminderOutputed(ReminderSource source, CancellationToken token)
            => await reminderProcessor.UpdateReminderOutputedAsync(source.LoginUserId, source.ReminderOutputeds, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> UpdateReminderSummaryOutputed(ReminderSource source, CancellationToken token)
            => await reminderProcessor.UpdateReminderSummaryOutputedAsync(source.LoginUserId, source.ReminderOutputeds, source.Summaries, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< ReminderOutputed >>> GetOutputedItems(ReminderOutputedSearch option, CancellationToken token)
            => (await reminderProcessor.GetOutputedItemsAsync(option, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> GetMaxOutputNo([FromBody] int companyId, CancellationToken token)
            => await reminderProcessor.GetMaxOutputNoAsync(companyId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReminderHistory>>> GetReminderList(ReminderSearch option, CancellationToken token)
            => (await reminderProcessor.GetReminderListAsync(option, option.Setting)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderHistory"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< ReminderHistory >> UpdateReminderHistory(ReminderHistory reminderHistory, CancellationToken token)
            => await reminderHistoryProcessor.UpdateReminderHistoryAsync(reminderHistory, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderSummaryHistory"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< ReminderSummaryHistory >> UpdateReminderSummaryHistory(ReminderSummaryHistory reminderSummaryHistory, CancellationToken token)
            => await reminderHistoryProcessor.UpdateReminderSummaryHistoryAsync(reminderSummaryHistory, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderHistory"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> DeleteHistory(ReminderHistory reminderHistory, CancellationToken token)
            => await reminderHistoryProcessor.DeleteHistoryAsync(reminderHistory, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminderSummaryHistory"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> DeleteSummaryHistory(ReminderSummaryHistory reminderSummaryHistory, CancellationToken token)
            => await reminderHistoryProcessor.DeleteSummaryHistoryAsync(reminderSummaryHistory, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetCancelDecisionItems(ReminderSearch option, CancellationToken token)
            => (await reminderProcessor.GetCancelDecisionItemsAsync(option, option.Setting, option.SummarySettings, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminders"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> CancelReminders(IEnumerable<Reminder> reminders, CancellationToken token)
            => await reminderProcessor.CancelAsync(reminders, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DestinationId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistDestination([FromBody] int DestinationId, CancellationToken token)
            => await reminderProcessor.ExistDestinationAsync(DestinationId, token);

    }
}
