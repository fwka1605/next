using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IInvoiceTemplateSettingProcessor
    {
        Task<bool> ExistCollectCategoryAsync(int CollectCategoryId, CancellationToken token = default(CancellationToken));
        Task<InvoiceTemplateSetting> GetByCodeAsync(int CompanyId, string Code, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<InvoiceTemplateSetting>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<InvoiceTemplateSetting> SaveAsync(InvoiceTemplateSetting InvoiceTemplateSetting, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));
    }
}
