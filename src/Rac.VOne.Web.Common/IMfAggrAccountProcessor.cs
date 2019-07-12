using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IMfAggrAccountProcessor
    {
        Task<int> SaveAsync(IEnumerable<MfAggrAccount> accounts, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MfAggrAccount>> GetAsync(CancellationToken token = default(CancellationToken));
    }
}
