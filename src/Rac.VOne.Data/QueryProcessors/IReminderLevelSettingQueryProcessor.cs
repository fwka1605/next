using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReminderLevelSettingQueryProcessor
    {
        Task<bool> ExistReminderTemplateAsync(int ReminderTemplateId, CancellationToken token = default(CancellationToken));
        Task<int> GetMaxLevelAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<ReminderLevelSetting> GetItemByLevelAsync(int CompanyId, int ReminderLevel, CancellationToken token = default(CancellationToken));
    }
}
