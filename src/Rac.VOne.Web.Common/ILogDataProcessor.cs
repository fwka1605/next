using System.Collections.Generic;
using Rac.VOne.Data;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ILogDataProcessor
    {
        Task<IEnumerable<LogData>> GetItemsAsync(LogDataSearch option, CancellationToken token = default(CancellationToken));
        Task<int> LogAsync(LogData LogData, CancellationToken token = default(CancellationToken));
        Task<LogData> GetStatsAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAllAsync(int CompanyId, CancellationToken token = default(CancellationToken));
    }
}
