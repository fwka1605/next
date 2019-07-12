using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IExportFieldSettingProcessor
    {
        Task<IEnumerable<ExportFieldSetting>> GetAsync(ExportFieldSetting setting, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ExportFieldSetting>> SaveAsync(IEnumerable<ExportFieldSetting> settings, CancellationToken token = default(CancellationToken));
    }
}
