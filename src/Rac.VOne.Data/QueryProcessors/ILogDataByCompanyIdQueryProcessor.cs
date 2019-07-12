using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface ILogDataByCompanyIdQueryProcessor
    {
        Task<IEnumerable<LogData>> GetItemsAsync(LogDataSearch option, CancellationToken token = default(CancellationToken));
        Task<LogData> GetStatsAsync(int CompanyId, CancellationToken token = default(CancellationToken));
    }
}
