using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    [ServiceContract]
    public interface IBillingInvoiceService
    {
        [OperationContract]
        Task<BillingInvoiceResult> GetAsync(string SessionKey,
            BillingInvoiceSearch searchOption,
            string connectionId);

        [OperationContract]
        Task<BillingInputResult> PublishInvoicesAsync(string SessionKey,
            string connectionId,
            BillingInvoiceForPublish[] invoices,
            int LoginUserId);

        [OperationContract]
        Task<BillingInvoiceDetailResult> GetDetailsForPrintAsync(string SessionKey,
            BillingInvoiceDetailSearch billingInvoiceDetailSearch);

        [OperationContract]
        Task<CountResult> GetCountAsync(string SessionKey,
            BillingInvoiceSearch searchOption,
            string connectionId);

        [OperationContract]
        Task<CountResult> DeleteWorkTableAsync(string SessionKey, byte[] ClientKey);

        [OperationContract]
        Task<CountResult> CancelPublishAsync(string SessionKey,
            string connectionId,
            long[] BilinngInputIds);

        [OperationContract]
        Task<BillingInvoiceDetailForExportResult> GetDetailsForExportAsync(string SessionKey,
            long[] BillingInputIds,
            int CompanyId,
            string connectionId);

        [OperationContract]
        Task<CountResult> UpdatePublishAtAsync(string SessionKey,
            string connectionId,
            long[] BilinngInputIds);
    }
}
