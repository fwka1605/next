using System;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class TaxClassQueryProcessor : ITaxClassQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public TaxClassQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<TaxClass>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT * FROM TaxClass";
            return dbHelper.GetItemsAsync<TaxClass>(query, cancellatioToken: token);
        }

    }
}
