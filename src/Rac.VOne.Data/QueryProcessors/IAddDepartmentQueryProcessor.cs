using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddDepartmentQueryProcessor
    {
        Task<Department> SaveAsync(Department department, CancellationToken token = default(CancellationToken));

    }
}
