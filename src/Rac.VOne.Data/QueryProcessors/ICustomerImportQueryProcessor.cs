using System;
using System.Collections.Generic;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICustomerImportQueryProcessor
    {
        Task<IEnumerable<MasterData>> GetImportItemsParentAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsChildAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsKanaHistoryAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsNettingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsReceiptAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));

    }
}
