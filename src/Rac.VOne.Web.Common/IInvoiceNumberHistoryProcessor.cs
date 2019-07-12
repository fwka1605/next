using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IInvoiceNumberHistoryProcessor
    {
        Task<IEnumerable<InvoiceNumberHistory>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<InvoiceNumberHistory> SaveAsync(InvoiceNumberHistory InvoiceNumberHistory, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<InvoiceNumberHistory> GetAsync( InvoiceNumberHistory InvoiceNumberHistory, CancellationToken token = default(CancellationToken));
    }
}
