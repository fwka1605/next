using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IDestinationProcessor
    {

        Task<IEnumerable<Destination>> GetAsync(DestinationSearch option, CancellationToken token = default(CancellationToken));

        Task<Destination> SaveAsync(Destination Destination, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));
    }
}
