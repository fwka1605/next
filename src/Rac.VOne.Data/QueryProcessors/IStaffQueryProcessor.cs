using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;
namespace Rac.VOne.Data.QueryProcessors
{
    public interface IStaffQueryProcessor
    {
        Task<IEnumerable<Staff>> GetAsync(StaffSearch option, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MasterData>> GetImportItemsLoginUserAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsCustomerAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken));
    }
}
