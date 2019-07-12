using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IDbFunctionProcessor
    {
        Task<byte[]> CreateClientKeyAsync(ClientKeySearch option, CancellationToken token = default(CancellationToken));
        Task<DateTime> GetDbDateTimeAsync(CancellationToken token = default(CancellationToken));
    }
}
