using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IMfAggrTagProcessor
    {
        Task<int> SaveAsync(IEnumerable<MfAggrTag> tags, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MfAggrTag>> GetAsync(CancellationToken token = default(CancellationToken));
    }
}
