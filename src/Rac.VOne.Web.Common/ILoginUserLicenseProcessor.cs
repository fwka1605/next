using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ILoginUserLicenseProcessor
    {
        Task<IEnumerable<LoginUserLicense>> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<LoginUserLicense>> SaveAsync(IEnumerable<LoginUserLicense> licenses, CancellationToken token = default(CancellationToken));
    }
}
