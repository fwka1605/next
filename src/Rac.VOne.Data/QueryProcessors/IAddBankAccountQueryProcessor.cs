using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddBankAccountQueryProcessor
    {
        Task<BankAccount> SaveAsync(BankAccount BankAccount, CancellationToken token = default(CancellationToken));
    }
}
