using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ICompanyProcessor
    {
        Task<IEnumerable<Company>> GetAsync(CompanySearch option, CancellationToken token = default(CancellationToken));
        Task<Company> SaveAsync(Company Company, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken));

    }
}
