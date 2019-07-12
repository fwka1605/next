using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddImportDataDetailQueryProcessor
    {
        Task<int> SaveAsync(ImportDataDetail detail, CancellationToken token = default(CancellationToken));
    }
}
