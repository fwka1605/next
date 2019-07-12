using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteReminderLevelSettingQueryProcessor
    {
        Task<int> DeleteAsync(int CompanyId, int ReminderLevel, CancellationToken token = default(CancellationToken));
    }
}
