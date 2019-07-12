using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IReminderCommonSettingProcessor
    {
        Task<ReminderCommonSetting> GetItemAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<ReminderCommonSetting> SaveAsync(ReminderCommonSetting ReminderCommonSetting, CancellationToken token = default(CancellationToken));
    }
}
