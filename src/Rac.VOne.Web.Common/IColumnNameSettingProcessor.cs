using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IColumnNameSettingProcessor
    {
        Task<IEnumerable<ColumnNameSetting>> GetAsync(ColumnNameSetting setting, CancellationToken token = default(CancellationToken));
        Task<ColumnNameSetting> SaveAsync(ColumnNameSetting ColumnName, CancellationToken token = default(CancellationToken));
    }
}
