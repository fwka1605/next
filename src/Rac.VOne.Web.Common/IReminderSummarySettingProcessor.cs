using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IReminderSummarySettingProcessor
    {
        Task<IEnumerable<ReminderSummarySetting>> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderSummarySetting>> SaveAsync(IEnumerable<ReminderSummarySetting> settings, CancellationToken token = default(CancellationToken));
    }
}
