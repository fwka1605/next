using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IBillingBalanceQueryProcessor
    {
        Task<DateTime?> GetLastCarryOverAtAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<BillingBalance>> GetBillingBalancesAsync(int CompanyId, DateTime? LastCarryOverAt, DateTime CarryOverAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<BillingBalance>> GetLastBillingBalanceAsync(int CompanyId, int CurrencyId, int CustomerId, int StaffId, int DepartmentId, CancellationToken token = default(CancellationToken));

        Task<decimal> GetBillingAmountAsync(int CompanyId, int CurrencyId, int CustomerId, int StaffId, int DepartmentId, DateTime CarryOverAt, DateTime? LastCarryOverAt, CancellationToken token = default(CancellationToken));
        Task<decimal> GetReceiptAmountAsync(int CompanyId, int CurrencyId, int CustomerId, int StaffId, int DepartmentId, DateTime CarryOverAt, DateTime? LastCarryOverAt, CancellationToken token = default(CancellationToken));
    }
}
