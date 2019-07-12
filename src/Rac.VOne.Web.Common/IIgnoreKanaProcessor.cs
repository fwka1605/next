using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IIgnoreKanaProcessor
    {
        Task<IEnumerable<IgnoreKana>> GetAsync(IgnoreKana kana, CancellationToken token = default(CancellationToken));

        Task<IgnoreKana> SaveAsync(IgnoreKana kana, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(IgnoreKana kana, CancellationToken token = default(CancellationToken));



        Task<bool> ExistCategoryAsync(int excludeCategoryId, CancellationToken token = default(CancellationToken));


        Task<ImportResult> ImportAsync(
            IEnumerable<IgnoreKana> insert,
            IEnumerable<IgnoreKana> update,
            IEnumerable<IgnoreKana> delete, CancellationToken token = default(CancellationToken));
    }
}
