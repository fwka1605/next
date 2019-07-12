using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddReminderLevelSettingQueryProcessor
    {
        Task<ReminderLevelSetting> SaveAsync(ReminderLevelSetting ReminderLevelSetting, CancellationToken token = default(CancellationToken));
    }
}
