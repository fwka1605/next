using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class IdenticalEntityQueryProcessor<TEntity> :
        IDeleteIdenticalEntityQueryProcessor<TEntity>,
        IIdenticalEntityExistByIdQueryprocessor<TEntity>,
        IIdenticalEntityGetByIdsQueryProcessor<TEntity>
        where TEntity : IIdentical
    {

        private readonly IDbHelper dbHelper;

        public IdenticalEntityQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        private string DeleteQuery => $@"DELETE [{typeof(TEntity).Name}] WHERE Id = @Id";

        public Task<int> DeleteAsync(int id, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync(DeleteQuery, new { Id = id }, token);

        private string GetExistsQuery() => $@"
SELECT 1
    WHERE EXISTS (
          SELECT 1
            FROM [{typeof(TEntity).Name}]
           WHERE Id      = @Id )";
        public async Task<bool> ExistAsync(int Id, CancellationToken token = default(CancellationToken))
            => (await dbHelper.ExecuteAsync<int?>(GetExistsQuery(), new { Id }, token)).HasValue;


        #region get by ids
        private string GetQuerySelectByIds() => $@"SELECT * FROM [{typeof(TEntity).Name}] WHERE Id IN (SELECT Id FROM @Ids)";
        public Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<int> Ids, CancellationToken token = default(CancellationToken))
        {
            if (Ids == null) Ids = new int[] { };
            return dbHelper.GetItemsAsync<TEntity>(GetQuerySelectByIds(), new { Ids = Ids.GetTableParameter() }, token);
        }


        #endregion
    }
}
