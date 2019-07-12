using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface ISectionProcessor
    {
        Task<IEnumerable<Section>> GetAsync(SectionSearch option, CancellationToken token = default(CancellationToken));

        Task<Section> SaveAsync(Section Section, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsBankAccountAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsSectionWithDepartmentAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsSectionWithLoginUserAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsReceiptAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MasterData>> GetImportItemsNettingAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<Section> insert,
            IEnumerable<Section> update,
            IEnumerable<Section> delete, CancellationToken token = default(CancellationToken));
    }
}
