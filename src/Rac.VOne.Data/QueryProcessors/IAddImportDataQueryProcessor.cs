using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddImportDataQueryProcessor
    {
        Task<ImportData> SaveAsync(ImportData data, CancellationToken token = default(CancellationToken));
    }
}
