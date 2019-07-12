using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ICategoryProcessor
    {
        Task<Category> SaveAsync(Category Category, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Category>> SaveItemsAsync(IEnumerable<Category> categories, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));
        Task<bool> ExistAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistPaymentAgencyAsync(int PaymentAgencyId, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Category>> GetAsync(CategorySearch option, CancellationToken token = default(CancellationToken));
    }
}
