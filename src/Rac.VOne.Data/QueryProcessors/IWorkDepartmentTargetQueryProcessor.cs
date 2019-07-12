using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IWorkDepartmentTargetQueryProcessor
    {
        Task<int> DeleteAsync(byte[] ClientKey, CancellationToken token = default(CancellationToken));
        Task<int> SaveAsync(byte[] ClientKey, int CompanyId, int DepartmentId, CancellationToken token = default(CancellationToken));

    }
}
