using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ISectionQueryProcessor
    {
        Task<IEnumerable<Section>> GetAsync(SectionSearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsBankAccountAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsSectionWithDepartmentAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsSectionWithLoginUserAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsReceiptAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsNettingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken));
    }
}
