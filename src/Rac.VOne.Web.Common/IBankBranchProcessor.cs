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
    public interface IBankBranchProcessor
    {
        Task<IEnumerable<BankBranch>> GetAsync(BankBranchSearch option, CancellationToken token = default(CancellationToken));

        Task<BankBranch> SaveAsync(BankBranch bankBranch, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(int CompanyId, string BankCode, string BranchCode, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(IEnumerable<BankBranch> insert, IEnumerable<BankBranch> update, IEnumerable<BankBranch> delete, CancellationToken token = default(CancellationToken));
    }
}
