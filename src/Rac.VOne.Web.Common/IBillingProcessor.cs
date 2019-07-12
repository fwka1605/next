using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IBillingProcessor
    {
        Task<bool> ExistDebitAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCreditAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistBillingCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistStaffAsync(int StaffId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistDestinationAsync(int DestinationId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Billing>> GetByIdsAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Billing>> GetByBillingInputIdAsync(long BillingInputId, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(BillingDeleteSource source, CancellationToken token = default(CancellationToken));


        Task<CountResult> OmitAsync(OmitSource source, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<int>> BillingImportDuplicationCheckAsync(int CompanyId, IEnumerable<BillingImportDuplicationWithCode> BillingImportDuplication,
            IEnumerable<ImporterSettingDetail> ImporterSettingDetail, CancellationToken token = default(CancellationToken));

        Task<Billing> UpdateForResetInvoiceCodeAsync(IEnumerable<long> BillingInputIds, CancellationToken token = default(CancellationToken));

        Task<Billing> UpdateForResetInputIdAsync(IEnumerable<long> BillingInputIds, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Billing>> UpdateBillingForPublishAsync(BillingInvoiceForPublish billingInvoiceForPublish, bool doUpdateInvoiceCode, CancellationToken token = default(CancellationToken));


    }
}
