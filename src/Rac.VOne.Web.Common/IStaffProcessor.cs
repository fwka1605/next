using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Web.Common
{
    public interface IStaffProcessor
    {
        Task<IEnumerable<Staff>> GetAsync(StaffSearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MasterData>> GetImportItemsLoginUserAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsCustomerAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<Staff> SaveAsync(Staff Staff, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<Staff> insert,
            IEnumerable<Staff> update,
            IEnumerable<Staff> delete, CancellationToken token = default(CancellationToken));
    }
}
