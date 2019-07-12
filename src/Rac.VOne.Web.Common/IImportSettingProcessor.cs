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
   public interface IImportSettingProcessor
    {
        Task<IEnumerable<ImportSetting>> GetAsync(ImportSettingSearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ImportSetting>> SaveAsync(IEnumerable<ImportSetting> settings, CancellationToken token = default(CancellationToken));
    }
}
