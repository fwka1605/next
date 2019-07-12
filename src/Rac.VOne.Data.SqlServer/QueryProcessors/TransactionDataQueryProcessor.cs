using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class TransactionDataQueryProcessor<TEntity>
        : IDeleteTransactionDataQueryProcessor<TEntity>
        where TEntity : ITransactionData
    {
        private readonly IDbHelper dbHelper;
        public TransactionDataQueryProcessor(IDbHelper dbHelper){
            this.dbHelper = dbHelper;
        }

        private string GetQueryForDelete() => $@"
DELETE  [{typeof(TEntity).Name}]
WHERE   Id          = @Id
AND     UpdateAt    = @UpdateAt
";

        public Task<int> DeleteAsync(TEntity entity, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync(GetQueryForDelete(), entity, token);

    }
}
