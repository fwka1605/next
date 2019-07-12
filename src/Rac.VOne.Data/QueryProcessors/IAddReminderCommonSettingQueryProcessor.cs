using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddReminderCommonSettingQueryProcessor
    {
        Task<ReminderCommonSetting> SaveAsync(ReminderCommonSetting ReminderCommonSetting, CancellationToken token = default(CancellationToken));
    }
}
