using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface ISynchronizationProcessor
    {
        Task<IEnumerable<Entity>> CheckCustomerAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Entity>> CheckCompanyAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Entity>> CheckDepartmentAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Entity>> CheckStaffAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Entity>> CheckLoginUserAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Entity>> CheckAccountTitleAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Entity>> CheckBankAccountAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Transaction>> CheckBillingAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Transaction>> CheckReceiptAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Transaction>> CheckMatchingHeaderAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Transaction>> CheckMatchingAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken));
    }
}
