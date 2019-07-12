using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface ICollationProcessor
    {
        Task<IEnumerable<Collation>> CollateAsync(CollationSearch collationSearch,
            CancellationToken token = default(CancellationToken),
            IProgressNotifier notifier = null);

    }
}
