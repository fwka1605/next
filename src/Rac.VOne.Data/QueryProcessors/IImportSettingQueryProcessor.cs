using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IImportSettingQueryProcessor
    {
        Task<IEnumerable<ImportSetting>> GetAsync(ImportSettingSearch option, CancellationToken token = default(CancellationToken));
    }
}