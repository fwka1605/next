using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReminderOutputedExistsQueryProcessor
    {
        Task<bool> ExistDestinationAsync(int DestinationId, CancellationToken token = default(CancellationToken));
    }
}
