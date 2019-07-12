using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class SynchronizationQueryProcessor:ISynchronizationQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public SynchronizationQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<TDestination>> CheckAsync<TEntity, TDestination>(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            var query = $@"SELECT Id,UpdateAt 
                              FROM [{typeof(TEntity).Name}]
                              WHERE UpdateAt >= @UpdateAt
                              ORDER BY Id ASC";
            return dbHelper.GetItemsAsync<TDestination>(query, new { UpdateAt }, token);
        }
    }
}
