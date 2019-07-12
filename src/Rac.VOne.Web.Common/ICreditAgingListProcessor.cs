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
    public interface ICreditAgingListProcessor
    {

        Task<IEnumerable<CreditAgingList>> GetAsync(CreditAgingListSearch searchOption,
            IProgressNotifier notifier = null,
            CancellationToken token = default(CancellationToken));

    }
}
