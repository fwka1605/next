using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReportSettingQueryProcessor
    {
        Task<IEnumerable<ReportSetting>> GetAsync(int CompanyId, string ReportId, CancellationToken token = default(CancellationToken));
    }
}
