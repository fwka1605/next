using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class MasterQueryProcessor<TEntity> :
        IMasterGetByCodeQueryProcessor<TEntity>,
        IMasterGetByCodesQueryProcessor<TEntity>,
        IMasterGetIdByCodeQueryProcessor<TEntity>,
        IMasterGetItemsQueryProcessor<TEntity>
        where TEntity : IMaster
    {

        private readonly IDbHelper dbHelper;

        public MasterQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        private string GetQueryForGetByCodes(bool any)
        {
            var query = $@"SELECT * FROM [{typeof(TEntity).Name}] WHERE CompanyId = @CompanyId";
            if (any) query += @" AND Code IN (SELECT Code FROM @Codes)";
            query += @" ORDER BY CompanyId ASC, Code ASC";
            return query;
        }

        public Task<TEntity> GetByCodeAsync(int CompanyId, string Code, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync<TEntity>(GetQueryForGetByCodes(any: true), new {
                CompanyId,
                Codes = (new[] { Code }).GetTableParameter(),
            }, token);


        public Task<IEnumerable<TEntity>> GetByCodesAsync(int CompanyId, IEnumerable<string> Codes, CancellationToken token = default(CancellationToken))
        {
            var query = GetQueryForGetByCodes(Codes?.Any() ?? false);
            return dbHelper.GetItemsAsync<TEntity>(query, new { CompanyId, Codes = Codes.GetTableParameter(), }, token);
        }

        public Task<int> GetIdByCodeAsync(int CompanyId, string Code, CancellationToken token = default(CancellationToken))
        {
            var query = $@"SELECT Id FROM [{typeof(TEntity).Name}] WHERE CompanyId = @CompanyId AND Code = @Code";
            return dbHelper.ExecuteAsync<int>(query, new { CompanyId, Code }, token);
        }

        private string GetQueryByCompanyId()
            => $@"SELECT * FROM [{typeof(TEntity).Name}] WHERE CompanyId = @CompanyId ORDER BY CompanyId ASC";


        public Task<IEnumerable<TEntity>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => dbHelper.GetItemsAsync<TEntity>(GetQueryByCompanyId(), new { CompanyId }, token);

    }
}
