using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddPasswordPolicyQueryProcessor
    {
        Task<PasswordPolicy> SaveAsync(PasswordPolicy PasswordPolicy, CancellationToken token = default(CancellationToken));
    }
}
