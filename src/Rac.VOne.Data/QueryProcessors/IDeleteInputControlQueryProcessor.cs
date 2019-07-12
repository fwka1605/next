using System.Threading;
using System.Threading.Tasks;
namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteInputControlQueryProcessor
    {
        Task<int> DeleteAsync(int CompanyId, int LoginUserId, int InputGridTypeId, CancellationToken token = default(CancellationToken));
    }
}
