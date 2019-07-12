using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using System.Collections.Generic;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReceiptQueryProcessor
    {


        Task<Receipt> GetReceiptAsync(long Id, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Receipt>> GetAdvanceReceiptsAsync(long OriginalReceiptId, CancellationToken token = default(CancellationToken));


        Task<IEnumerable<int>> ReceiptImportDuplicationCheckAsync(int CompanyId, IEnumerable<ReceiptImportDuplication> ReceiptImportDuplication, IEnumerable<ImporterSettingDetail> ImporterSettingDetail, CancellationToken token = default(CancellationToken));
    }
}
