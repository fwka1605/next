using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IImportDataDetailQueryProcessor
    {
        Task<IEnumerable<ImportDataDetail>> GetAsync(long importDataId, int? objectType = null, CancellationToken token = default(CancellationToken));
    }
}
