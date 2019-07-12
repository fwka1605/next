using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IImportFileLogQueryProcessor
    {
        Task<IEnumerable<ImportFileLog>> GetHistoryAsync(int CompanyId, CancellationToken token = default(CancellationToken));
    }
}