using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface INettingQueryProcessor
    {
        Task<Netting> GetByIdAsync(long Id, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Netting>> GetByMatchingHeaderIdAsync(long headerId, CancellationToken token = default(CancellationToken));

        Task<bool> ExistReceiptCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken));
    }
}
