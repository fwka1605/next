using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IGridSettingQueryProcessor
    {
        Task<IEnumerable<GridSetting>> GetAsync(GridSettingSearch option, CancellationToken token = default(CancellationToken));
    }
}
