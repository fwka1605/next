using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class TransactionQueryProcessor<TEntity> :
        ITransactionalGetByIdQueryProcessor<TEntity>,
        ITransactionalGetByIdsQueryProcessor<TEntity>,
        IDeleteTransactionQueryProcessor<TEntity>
        where TEntity : ITransactional
    {

        private readonly IDbHelper dbHelper;

        public TransactionQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        #region IDeleteTransactionQueryProcessor

        private string GetQueryDeleteById() => $@"DELETE [{typeof(TEntity).Name}] WHERE Id = @Id";


        public Task<int> DeleteAsync(long Id, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync(GetQueryDeleteById(), new { Id }, token);

        #endregion

        #region ITransactionalGetByIdQueryProcessor

        private string GetQuerySelectById() => $@"SELECT * FROM [{typeof(TEntity).Name}] WHERE Id = @Id";


        public Task<TEntity> GetByIdAsync(long Id, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync<TEntity>(GetQuerySelectById(), new { Id }, token);

        #endregion

        #region ITransactionalGetByIdsQueryProcessor

        private string GetQuerySelectByIds() => $@"SELECT * FROM [{typeof(TEntity).Name}] WHERE Id IN (SELECT Id FROM @Ids) ORDER BY Id ASC";


        public Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken))
            => dbHelper.GetItemsAsync<TEntity>(GetQuerySelectByIds(), new { Ids = Ids.GetTableParameter() }, token);

        #endregion

    }
}
