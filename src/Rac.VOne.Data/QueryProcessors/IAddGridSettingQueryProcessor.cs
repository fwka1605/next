using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddGridSettingQueryProcessor
    {
        Task<GridSetting> SaveAsync(GridSetting gridSetting, bool updateAllUsers, CancellationToken token = default(CancellationToken));
    }
}
