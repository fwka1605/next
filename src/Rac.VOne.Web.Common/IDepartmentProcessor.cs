using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IDepartmentProcessor
    {
        Task<IEnumerable<Department>> GetAsync(DepartmentSearch option, CancellationToken token = default(CancellationToken));


        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));


        Task<IEnumerable<Department>> SaveAsync(IEnumerable<Department> departments, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(IEnumerable<Department> insert, IEnumerable<Department> update, IEnumerable<Department> delete, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MasterData>> GetImportItemsStaffAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsSectionWithDepartmentAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));

    }
}
