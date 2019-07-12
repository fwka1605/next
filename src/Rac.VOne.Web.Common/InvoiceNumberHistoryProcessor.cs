using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class InvoiceNumberHistoryProcessor : IInvoiceNumberHistoryProcessor
    {
        private readonly IByCompanyGetEntitiesQueryProcessor<InvoiceNumberHistory> getInvoiceNumberHistoryQueryProcessor;
        private readonly IAddInvoiceNumberHistoryQueryProcessor addInvoiceNumberHistoryQueryProcessor;
        private readonly IDeleteByCompanyQueryProcessor<InvoiceNumberHistory> deleteInvoiceNumberHistoryQueryProcessor;

        public InvoiceNumberHistoryProcessor(
            IByCompanyGetEntitiesQueryProcessor<InvoiceNumberHistory> getInvoiceNumberHistoryQueryProcessor,
            IAddInvoiceNumberHistoryQueryProcessor addInvoiceNumberHistoryQueryProcessor,
            IDeleteByCompanyQueryProcessor<InvoiceNumberHistory> deleteInvoiceNumberHistoryQueryProcessor
            )
        {
            this.getInvoiceNumberHistoryQueryProcessor = getInvoiceNumberHistoryQueryProcessor;
            this.addInvoiceNumberHistoryQueryProcessor = addInvoiceNumberHistoryQueryProcessor;
            this.deleteInvoiceNumberHistoryQueryProcessor = deleteInvoiceNumberHistoryQueryProcessor;
        }

        public async Task<IEnumerable<InvoiceNumberHistory>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await getInvoiceNumberHistoryQueryProcessor.GetItemsAsync(CompanyId, token);

        public async Task<InvoiceNumberHistory> SaveAsync(InvoiceNumberHistory InvoiceNumberHistory, CancellationToken token = default(CancellationToken))
            => await addInvoiceNumberHistoryQueryProcessor.SaveAsync(InvoiceNumberHistory, token);

        public async Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await deleteInvoiceNumberHistoryQueryProcessor.DeleteAsync(CompanyId, token);

        public async Task<InvoiceNumberHistory> GetAsync(InvoiceNumberHistory InvoiceNumberHistory, CancellationToken token = default(CancellationToken))
            => await addInvoiceNumberHistoryQueryProcessor.GetAsync(InvoiceNumberHistory, token);




    }
}
