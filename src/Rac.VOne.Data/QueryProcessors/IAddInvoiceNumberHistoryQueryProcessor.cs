using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddInvoiceNumberHistoryQueryProcessor
    {
        Task<InvoiceNumberHistory> SaveAsync(InvoiceNumberHistory InvoiceNumberHistory, CancellationToken token = default(CancellationToken));
        Task<InvoiceNumberHistory> GetAsync(InvoiceNumberHistory InvoiceNumberHistory, CancellationToken token = default(CancellationToken));
    }
}
