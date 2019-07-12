using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteTransactionDataQueryProcessor<TEntity>
        where TEntity : ITransactionData
    {
        Task<int> DeleteAsync(TEntity entity, CancellationToken token = default(CancellationToken));
    }
}
