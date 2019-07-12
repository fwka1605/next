using Rac.VOne.Data;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Web.Common
{
    public interface IGeneralSettingProcessor
    {
        Task<IEnumerable<GeneralSetting>> GetAsync(GeneralSetting setting, CancellationToken token = default(CancellationToken));
        Task<GeneralSetting> SaveAsync(GeneralSetting setting, CancellationToken token = default(CancellationToken));

    }
}
