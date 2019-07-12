using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteCompanyLogoQueryProcessor
    {
        Task<int> DeleteAsync(int CompanyId, int LogoType, CancellationToken token = default(CancellationToken));
    }
}
