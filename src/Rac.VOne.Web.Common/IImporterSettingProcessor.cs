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
    public interface IImporterSettingProcessor
    {
        Task<IEnumerable<ImporterSetting>> GetAsync(ImporterSetting setting, CancellationToken token = default(CancellationToken));

        Task<ImporterSetting> SaveAsync(ImporterSetting setting, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int ImporterSettingId, CancellationToken token = default(CancellationToken));
    }
}
