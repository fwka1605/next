using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IBankAccountProcessor
    {
        Task<IEnumerable<BankAccount>> GetAsync(BankAccountSearch option, CancellationToken token = default(CancellationToken));

        Task<BankAccount> SaveAsync(BankAccount BankAccount, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(IEnumerable<BankAccount> insert, IEnumerable<BankAccount> update, IEnumerable<BankAccount> delete, CancellationToken token = default(CancellationToken));
    }
}
