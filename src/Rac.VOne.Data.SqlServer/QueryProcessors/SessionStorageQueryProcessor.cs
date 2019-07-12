using Rac.VOne.Data.Entities;
using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class SessionStorageQueryProcessor : ISessionStorageQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public SessionStorageQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<SessionStorage> GetAsync(IConnectionFactory factory, string SessionKey, CancellationToken token = default(CancellationToken))
        {
            return dbHelper.DoAsync(helper =>
            {
                var query = "SELECT TOP 1 * FROM SessionStorage WHERE SessionKey = @SessionKey";
                return helper.ExecuteAsync<SessionStorage>(query, new { SessionKey }, token);
            }, factory);
        }

    }
}
