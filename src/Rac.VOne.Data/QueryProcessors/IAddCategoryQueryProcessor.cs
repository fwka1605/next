using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddCategoryQueryProcessor
    {
        Task<Category> SaveAsync(Category category, CancellationToken token = default(CancellationToken));
    }
}