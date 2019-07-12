using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICurrencyQueryProcessor
    {
        Task<IEnumerable<Currency>> GetAsync(CurrencySearch option, CancellationToken token = default(CancellationToken));

        //インポートのその他Checkingのため
        Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsReceiptAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsNettingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
    }
}
