using Rac.VOne.Data;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;

namespace Rac.VOne.Web.Common
{
    public interface IMatchingProcessor
    {
        Task<IEnumerable<Receipt>> SearchReceiptDataAsync(MatchingReceiptSearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Billing>> SearchBillingDataAsync(MatchingBillingSearch option, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MatchingHeader>> SearchMatchedDataAsync(CollationSearch CollationSearch, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Receipt>> SearchReceiptByIdAsync(IEnumerable<long> ReceiptId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Matching>> GetAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MatchingHeader>> GetHeaderItemsAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken));


        #region cancellation
        Task<IEnumerable<Billing>> GetMatchedBillingsAsync(MatchingBillingSearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Receipt>> GetMatchedReceiptsAsync(MatchingReceiptSearch option, CancellationToken token = default(CancellationToken));

        #endregion

        Task<int> SaveWorkSectionTargetAsync(byte[] ClientKey, int CompanyId, IEnumerable<int> SectionIds, CancellationToken token = default(CancellationToken));
        Task<int> SaveWorkDepartmentTargetAsync(byte[] ClientKey, int CompanyId, IEnumerable<int> DepartmentIds, CancellationToken token = default(CancellationToken));

    }
}
