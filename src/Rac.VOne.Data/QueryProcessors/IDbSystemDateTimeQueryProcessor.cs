using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDbSystemDateTimeQueryProcessor
    {
        Task<DateTime> GetAsync(CancellationToken token = default(CancellationToken));
    }
}
