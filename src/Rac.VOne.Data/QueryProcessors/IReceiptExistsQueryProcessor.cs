using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReceiptExistsQueryProcessor
    {
        Task<bool> ExistReceiptCategoryAsync(int ReceiptCategoryId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCompanyAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistOriginalReceiptAsync(long ReceiptId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistNonApportionedAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken));
        Task<bool> ExistNonOutputedAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken));
        Task<bool> ExistNonAssignmentAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken));
    }
}
