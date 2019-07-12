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
    public interface INettingProcessor
    {
        Task<bool> ExistReceiptCategoryAsync(int ReceiptCatgoryid, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Netting>> SaveAsync(IEnumerable<Netting> Netting, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(IEnumerable<long> Id, CancellationToken token = default(CancellationToken));
    }
}
