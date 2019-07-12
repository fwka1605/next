using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Import
{
    public interface IImporter
    {
        Task<ImportResult> ImportAsync(string csvPath,
            ImportMethod method,
            CancellationToken? cancel,
            IProgress<int> progress);
    }
}
