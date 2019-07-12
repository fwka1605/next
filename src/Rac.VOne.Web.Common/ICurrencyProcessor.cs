using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ICurrencyProcessor
    {
        Task<IEnumerable<Currency>> GetAsync(CurrencySearch option, CancellationToken token = default(CancellationToken));

        Task<Currency> SaveAsync(Currency Currency, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<Currency> insert,
            IEnumerable<Currency> update,
            IEnumerable<Currency> delete,
            CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsReceiptAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsNettingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
    }
}
