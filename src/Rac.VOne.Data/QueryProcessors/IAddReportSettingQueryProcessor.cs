using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddReportSettingQueryProcessor
    {
        Task<ReportSetting> SaveAsync(ReportSetting setting, CancellationToken token = default(CancellationToken));
    }
}
