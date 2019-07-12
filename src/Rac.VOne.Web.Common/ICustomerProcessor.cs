using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface ICustomerProcessor
    {
        Task<IEnumerable<Customer>> GetAsync(CustomerSearch option, CancellationToken token = default(CancellationToken));

        Task<bool> ExistCompanyAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCategoryAsync(int CollectCategoryId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistStaffAsync(int StaffId, CancellationToken token = default(CancellationToken));

        Task<Customer> SaveAsync(Customer Customer, bool requireIsParentUpdate = false, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Customer>> SaveItemsAsync(IEnumerable<Customer> customers, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(int CustomerId, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MasterData>> GetImportForCustomerGroupParentAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportForCustomerGroupChildAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportForKanaHistoryAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportForBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportForReceiptAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportForNettingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        //Customer UpdateForBillingImport(Customer UpdateCustomer);

        Task<IEnumerable<CustomerMin>> GetMinItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<Customer> insert,
            IEnumerable<Customer> update,
            IEnumerable<Customer> delete, CancellationToken token = default(CancellationToken));
    }
}
