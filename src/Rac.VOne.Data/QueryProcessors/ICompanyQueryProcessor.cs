using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICompanyQueryProcessor
    {
        Task<IEnumerable<Company>> GetAsync(CompanySearch option, CancellationToken token = default(CancellationToken));
    }
}
