using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IReportSettingProcessor
    {
        Task<IEnumerable<ReportSetting>> GetAsync(int CompanyId, string ReportId, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<ReportSetting>> SaveAsync(IEnumerable<ReportSetting> settings, CancellationToken token = default(CancellationToken));
     }
}
