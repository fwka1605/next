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
    public interface IImportFileLogProcessor
    {
        Task<IEnumerable<ImportFileLog>> GetHistoryAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ImportFileLog>> SaveAsync(IEnumerable<ImportFileLog> logs, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(IEnumerable<int> fileLogIds, CancellationToken token = default(CancellationToken));
    }
}
