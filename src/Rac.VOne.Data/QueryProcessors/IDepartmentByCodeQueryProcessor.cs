using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDepartmentByCodeQueryProcessor
    {
        Task<IEnumerable<Department>> GetAsync(DepartmentSearch option, CancellationToken token = default(CancellationToken));
        //For Importing
        Task<IEnumerable<MasterData>> GetImportItemsStaffAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsSectionWithDepartmentAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
    }
}
