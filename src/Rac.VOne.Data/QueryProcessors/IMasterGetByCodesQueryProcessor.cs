using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IMasterGetByCodesQueryProcessor<TEntity> where TEntity : IMaster
    {
        Task<IEnumerable<TEntity>> GetByCodesAsync(int CompanyId, IEnumerable<string> Codes, CancellationToken token = default(CancellationToken));
    }
}
