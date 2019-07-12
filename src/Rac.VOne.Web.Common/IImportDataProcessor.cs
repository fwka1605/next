using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IImportDataProcessor
    {
        Task<ImportData> SaveAsync(ImportData data, CancellationToken token = default(CancellationToken));
        Task<ImportData> GetAsync(long id, int? objectType = null, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(long id, CancellationToken token = default(CancellationToken));
    }
}
