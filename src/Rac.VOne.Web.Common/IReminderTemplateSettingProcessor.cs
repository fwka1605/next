using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IReminderTemplateSettingProcessor
    {
        Task<IEnumerable<ReminderTemplateSetting>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<ReminderTemplateSetting> GetByCodeAsync(int CompanyId, string Code, CancellationToken token = default(CancellationToken));
        Task<ReminderTemplateSetting> SaveAsync(ReminderTemplateSetting ReminderTemplateSetting, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistAsync(int Id, CancellationToken token = default(CancellationToken));
    }
}
