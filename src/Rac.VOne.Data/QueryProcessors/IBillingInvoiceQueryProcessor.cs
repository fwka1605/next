using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface IBillingInvoiceQueryProcessor
    {
        Task<IEnumerable<BillingInvoice>> GetAsync(BillingInvoiceSearch searchOption,
            InvoiceCommonSetting invoiceCommonSetting,
            CancellationToken token = default(CancellationToken));

        Task<IEnumerable<BillingInvoice>> UpdateBillingForPublishNewInputId(
            BillingInvoiceForPublish billingInvoiceThin,
            bool DoUpdateInvoiceCode,
            CancellationToken token = default(CancellationToken));

        Task<IEnumerable<BillingInvoiceDetailForPrint>> GetDetailsForPrintAsync(
            BillingInvoiceDetailSearch billingInvoiceDetailSearch,
            InvoiceCommonSetting invoiceCommonSetting, CancellationToken token = default(CancellationToken));

        Task<int> DeleteWorkTableAsync(byte[] ClientKey, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<BillingInvoiceDetailForExport>> GetDetailsForExportAsync(
            IEnumerable<long> BillingInputIds,
            InvoiceCommonSetting invoiceCommonSetting,
            CancellationToken token = default(CancellationToken));

        Task<DateTime?> GetMaxUpdateAtAsync(byte[] ClientKey,
            IEnumerable<long> temporaryBillingInputIds, CancellationToken token = default(CancellationToken));
    }
}
