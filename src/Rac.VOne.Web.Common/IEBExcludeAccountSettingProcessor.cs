using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IEBExcludeAccountSettingProcessor
    {
        Task<IEnumerable<EBExcludeAccountSetting>> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken));

        Task<EBExcludeAccountSetting> SaveAsync(EBExcludeAccountSetting setting, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(EBExcludeAccountSetting setting, CancellationToken token = default(CancellationToken));
    }
}
