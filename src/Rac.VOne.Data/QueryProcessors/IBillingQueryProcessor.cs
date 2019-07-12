using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IBillingQueryProcessor
    {
        Task<IEnumerable<Billing>> GetDataForScheduledPaymentAsync(ScheduledPaymentImport scheduledPayment, IEnumerable<ImporterSettingDetail> details, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Billing>> GetByBillingInputIdAsync(long BillingInputId, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<int>> BillingImportDuplicationCheckAsync(int CompanyId, IEnumerable<BillingImportDuplicationWithCode>  BillingImportDuplication,
            IEnumerable<ImporterSettingDetail> ImporterSettingDetail, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Billing>> GetAccountTransferMatchingTargetListAsync(int PaymentAgencyId, DateTime TransferDate, int currencyId, CancellationToken token = default(CancellationToken));
        Task<int> OmitAsync(int doDelete, int loginUserId, Transaction item, CancellationToken token = default(CancellationToken));

        Task<Billing> UpdateForAccountTransferImportAsync(long Id, AccountTransferImportData importData, CancellationToken token = default(CancellationToken));

        Task<Billing> UpdateForResetInvoiceCodeAsync(IEnumerable<long> BillingInputIds, CancellationToken token = default(CancellationToken));
        Task<Billing> UpdateForResetInputIdAsync(IEnumerable<long> BillingInputIds, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Billing>> UpdateForPublishAsync(BillingInvoiceForPublish billingInvoiceForPublish,bool doUpdateInvoiceCode, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Billing>> GetItemsForScheduledPaymentImportAsync(ScheduledPaymentImport schedulePayment,
            IEnumerable<ImporterSettingDetail> importerSettingDetails, CancellationToken token = default(CancellationToken));

    }
}
