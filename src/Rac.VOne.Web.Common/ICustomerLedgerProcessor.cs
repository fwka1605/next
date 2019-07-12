using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;

namespace Rac.VOne.Web.Common
{
    public interface ICustomerLedgerProcessor
    {
        Task<IEnumerable<CustomerLedger>> GetAsync(CustomerLedgerSearch searchOption, CancellationToken token, IProgressNotifier notifier);
    }
}
