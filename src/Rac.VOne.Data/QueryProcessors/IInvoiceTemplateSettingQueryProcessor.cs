using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IInvoiceTemplateSettingQueryProcessor
    {
        Task<bool> ExistCollectCategoryAsync(int CollectCategoryId, CancellationToken token = default(CancellationToken));
    }
}
