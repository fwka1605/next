using Rac.VOne.Data;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ISectionWithDepartmentProcessor
    {
        Task<IEnumerable<SectionWithDepartment>> GetAsync(SectionWithDepartmentSearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken));
        Task<SectionWithDepartment> SaveAsync(SectionWithDepartment SectionWithDepartment, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<SectionWithDepartment>> SaveAsync(IEnumerable<SectionWithDepartment> upsert, IEnumerable<SectionWithDepartment> delete, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int SectionId, int DepartmentId, CancellationToken token = default(CancellationToken));
        Task<ImportResult> ImportAsync(
            IEnumerable<SectionWithDepartment> insert,
            IEnumerable<SectionWithDepartment> update,
            IEnumerable<SectionWithDepartment> delete, CancellationToken token = default(CancellationToken));
    }
}
