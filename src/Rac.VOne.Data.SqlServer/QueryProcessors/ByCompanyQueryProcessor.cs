using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ByCompanyQueryProcessor<TEntity> :
        IByCompanyGetEntityQueryProcessor<TEntity>,
        IByCompanyGetEntitiesQueryProcessor<TEntity>,
        IByCompanyExistQueryProcessor<TEntity>,
        IDeleteByCompanyQueryProcessor<TEntity>
        where TEntity : IByCompany
    {
        private readonly IDbHelper dbHelper;

        public ByCompanyQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        private string GetQuerySelectData()
            => $@"SELECT * FROM [{typeof(TEntity).Name}] WHERE CompanyId = @CompanyId";

        #region get a TEntity


        public Task<TEntity> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync<TEntity>(GetQuerySelectData(), new { CompanyId }, token);

        #endregion

        #region get multiple TEntity


        public Task<IEnumerable<TEntity>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => dbHelper.GetItemsAsync<TEntity>(GetQuerySelectData(), new { CompanyId }, token);

        #endregion

        private string GetExistsQuery() => $@"
SELECT 1
 WHERE EXISTS (
       SELECT
        1
       FROM [{typeof(TEntity).Name}]
       WHERE
           CompanyId   = @CompanyId
        )";

        public async Task<bool> ExistAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => (await dbHelper.ExecuteAsync<int?>(GetExistsQuery(), new { CompanyId }, token)).HasValue;

        private string GetQueryDelete() => $@"DELETE FROM [{typeof(TEntity).Name}] WHERE CompanyId = @CompanyId";

        public Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync(GetQueryDelete(), new { CompanyId }, token);

    }
}
