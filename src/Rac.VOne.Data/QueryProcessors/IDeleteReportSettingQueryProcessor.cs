using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteReportSettingQueryProcessor
    {
        Task<int> DeleteAsync(int CompanyId, string ReportId, CancellationToken token = default(CancellationToken));
    }
}
