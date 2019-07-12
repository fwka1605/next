using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IMatchingOutputedProcessor
    {

        Task<int> SaveOutputAtAsync(IEnumerable<long> MatchingHeaderIds, CancellationToken token = default(CancellationToken));
    }
}
