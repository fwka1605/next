using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;

namespace Rac.VOne.Web.Common
{
    public interface IBillingInvoiceProcessor
    {
        Task<IEnumerable<BillingInvoice>> GetAsync(BillingInvoiceSearch option, CancellationToken token = default(CancellationToken));

        Task<BillingInputResult> PublishInvoicesAsync(BillingInvoiceForPublish[] invoices,
            int LoginUserId,
            CancellationToken token = default(CancellationToken));

        Task<IEnumerable<BillingInvoiceDetailForPrint>> GetDetailsForPrintAsync(
            BillingInvoiceDetailSearch option, CancellationToken token = default(CancellationToken));

        Task<int> DeleteWorkTableAsync(byte[] clientKey, CancellationToken token = default(CancellationToken));

        Task<CountResult> CancelPublishAsync(long[] BillingInputIds,
            CancellationToken token = default(CancellationToken));

        Task<IEnumerable<BillingInvoiceDetailForExport>> GetDetailsForExportAsync(
            IEnumerable<long> BillingInputIds,
            int CompanyId,
            CancellationToken token = default(CancellationToken));

        Task<CountResult> UpdatePublishAtAsync(long[] BillingInputIds,
            CancellationToken token = default(CancellationToken));
    }
}
