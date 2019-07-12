using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface  IAddStaffQueryProcessor
    {
        Task<Staff> SaveAsync(Staff Staff, CancellationToken token = default(CancellationToken));
    }
}
