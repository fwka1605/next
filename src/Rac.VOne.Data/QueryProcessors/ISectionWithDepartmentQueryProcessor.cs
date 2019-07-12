using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ISectionWithDepartmentQueryProcessor
    {
        Task<IEnumerable<SectionWithDepartment>> GetAsync(SectionWithDepartmentSearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken));

    }
}
