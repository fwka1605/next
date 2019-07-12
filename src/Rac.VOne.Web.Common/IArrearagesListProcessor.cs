using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IArrearagesListProcessor
    {
        Task<IEnumerable<ArrearagesList>> GetAsync(ArrearagesListSearch option, CancellationToken token = default(CancellationToken));

    }
}
