using Rac.VOne.Data;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ICompanyLogoProcessor
    {
        Task<IEnumerable<CompanyLogo>> GetAsync(int companyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<CompanyLogo>> SaveAsync(IEnumerable<CompanyLogo> logs, CancellationToken token = default(CancellationToken));
        Task<int> DeleteByCompanyIdAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(IEnumerable<CompanyLogo> logos, CancellationToken token = default(CancellationToken));
    }
}
