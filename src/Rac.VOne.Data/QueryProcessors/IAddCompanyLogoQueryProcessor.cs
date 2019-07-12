using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddCompanyLogoQueryProcessor
    {
        Task<CompanyLogo> SaveAsync(CompanyLogo CompanyLogo, CancellationToken token = default(CancellationToken));
    }
}
