using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IAccountTitleProcessor
    {
        Task<IEnumerable<AccountTitle>> GetAsync(AccountTitleSearch option, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(int id, CancellationToken token = default(CancellationToken));
        Task<AccountTitle> SaveAsync(AccountTitle accountTitle, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<AccountTitle> insert,
            IEnumerable<AccountTitle> update,
            IEnumerable<AccountTitle> delete,
            CancellationToken token = default(CancellationToken));


        Task<IEnumerable<MasterData>> GetImportItemsCategoryAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsCustomerDiscountAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsDebitBillingAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsCreditBillingAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken));
    }
}
