using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IReceiptProcessor
    {
        Task<bool> ExistReceiptCategoryAsync(int ReceiptCategoryId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistOriginalReceiptAsync(long ReceiptId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCompanyAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistNonApportionedAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken));
        Task<bool> ExistNonOutputedAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken));
        Task<bool> ExistNonAssignmentAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken));


        Task<IEnumerable<Receipt>> GetByIdsAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken));

        Task<Receipt> GetReceiptAsync(long id, CancellationToken token = default(CancellationToken));


        Task<CountResult> OmitAsync(OmitSource source, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<int>> ReceiptImportDuplicationCheckAsync(int CompanyId, IEnumerable<ReceiptImportDuplication> ReceiptImportDuplication, IEnumerable<ImporterSettingDetail> ImporterSettingDetail, CancellationToken token = default(CancellationToken));

        Task<ReceiptInputsResult> SaveAsync(ReceiptSaveItem item, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(long id, CancellationToken token = default(CancellationToken));
    }
}
