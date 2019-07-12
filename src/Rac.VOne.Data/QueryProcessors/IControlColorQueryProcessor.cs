using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.Entities;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IControlColorQueryProcessor
    {
        Task<ControlColor> GetAsync(int CompanyId, int LoginUserId, CancellationToken token = default(CancellationToken));
    }
}
