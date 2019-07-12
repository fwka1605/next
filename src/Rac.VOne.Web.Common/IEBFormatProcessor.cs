using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IEBFormatProcessor
    {
        Task<IEnumerable<EBFormat>> GetAsync(CancellationToken token = default(CancellationToken));
    }
}
