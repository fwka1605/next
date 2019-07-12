using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.Entities;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddControlColorQueryProcessor
    {
        Task<ControlColor> SaveAsync(ControlColor ControlColor, CancellationToken token = default(CancellationToken));
    }
}
