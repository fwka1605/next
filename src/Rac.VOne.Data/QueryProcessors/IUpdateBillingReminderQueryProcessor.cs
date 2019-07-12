using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateBillingReminderQueryProcessor
    {
        Task<IEnumerable<Billing>> UpdateAsync(Reminder reminder, ReminderCommonSetting setting, IEnumerable<ReminderSummarySetting> summary, CancellationToken token = default(CancellationToken));

        Task<int> CancelAsync(int reminderId, CancellationToken token = default(CancellationToken));
    }
}
