using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICustomerExistsQueryProcessor
    {
        Task<bool> ExistCompanyAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCategoryAsync(int Collectcategoryid, CancellationToken token = default(CancellationToken));
        Task<bool> ExistStaffAsync(int StaffId, CancellationToken token = default(CancellationToken));

    }
}
