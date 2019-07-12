using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReminderSummarySettingQueryProcessor
    {
        Task<IEnumerable<ReminderSummarySetting>> GetAsync(int CompanyId, int UseForeignCurrency, CancellationToken token = default(CancellationToken));
    }
}
