using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddReminderSummarySettingQueryProcessor
    {

        Task<ReminderSummarySetting> SaveAsync(ReminderSummarySetting setting, CancellationToken token = default(CancellationToken));
    }
}
