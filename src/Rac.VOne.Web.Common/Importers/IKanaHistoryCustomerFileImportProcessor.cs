using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Importers
{
    public interface IKanaHistoryCustomerFileImportProcessor
    {
        Task<ImportResult> ImportAsync(MasterImportSource source, CancellationToken token = default(CancellationToken));
    }
}
