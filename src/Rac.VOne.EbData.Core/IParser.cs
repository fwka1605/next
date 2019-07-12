using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.EbData
{
    internal interface IParser
    {
        FileInformation FileInformation { get; set; }
        Helper Helper { get; set; }
        Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> lines, CancellationToken token = default(CancellationToken));
    }
}
