using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddAccountTitleQueryProcessor
    {
        Task<AccountTitle> AddAsync(AccountTitle AccountTitle, CancellationToken token = default(CancellationToken));
    }
}
