using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IEBExcludeAccountSettingQueryProcessor
    {
        Task<EBExcludeAccountSetting> SaveAsync(EBExcludeAccountSetting setting, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(EBExcludeAccountSetting setting, CancellationToken token = default(CancellationToken));
    }
}
