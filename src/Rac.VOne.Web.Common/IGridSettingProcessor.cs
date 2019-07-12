using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IGridSettingProcessor
    {
        Task<IEnumerable<GridSetting>> GetAsync(GridSettingSearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<GridSetting>> SaveAsync(IEnumerable<GridSetting> settings, CancellationToken token = default(CancellationToken));
    }
}
