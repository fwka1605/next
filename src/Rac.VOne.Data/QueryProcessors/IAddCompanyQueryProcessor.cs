using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddCompanyQueryProcessor
    {
        Task<Company> SaveAsync(Company Company, CancellationToken token = default(CancellationToken));
    }
}
