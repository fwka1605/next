using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Importers
{
    public interface IEbFileImportProcessor
    {
        Task<IEnumerable<EbFileInformation>> ImportAsync(IEnumerable<EbFileInformation> files, CancellationToken token = default(CancellationToken));
    }
}
