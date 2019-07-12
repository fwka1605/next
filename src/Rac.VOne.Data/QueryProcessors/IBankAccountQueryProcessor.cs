using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IBankAccountQueryProcessor
    {
        Task<IEnumerable<BankAccount>> GetAsync(BankAccountSearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCategoryAsnc(int ReceiptCategoryId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken));
    }
}
