using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IReminderLevelSettingProcessor
    {
        Task<IEnumerable<ReminderLevelSetting>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistReminderTemplateAsync(int ReminderTemplateId, CancellationToken token = default(CancellationToken));
        Task<int> GetMaxLevelAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<ReminderLevelSetting> GetItemByLevelAsync(int CompanyId, int ReminderLevel, CancellationToken token = default(CancellationToken));
        Task<ReminderLevelSetting> SaveAsync(ReminderLevelSetting ReminderLevelSetting, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int CompanyId, int ReminderLevel, CancellationToken token = default(CancellationToken));
    }
}
