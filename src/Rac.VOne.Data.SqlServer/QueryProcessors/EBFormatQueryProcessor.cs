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
    public class EBFormatQueryProcessor : IEBFormatQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public EBFormatQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<EBFormat>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM EBFormat";
            return dbHelper.GetItemsAsync<EBFormat>(query, token);
        }

    }
}
