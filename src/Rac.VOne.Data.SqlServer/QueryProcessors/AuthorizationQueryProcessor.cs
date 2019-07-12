using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Data.Entities;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class AuthorizationQueryProcessor : IAuthorizationQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public AuthorizationQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<SessionStorage> GetSessionAsync(string SessionKey)
        {
            var query = "SELECT TOP 1 * FROM SessionStorage WHERE SessionKey = @SessionKey";
            return dbHelper.ExecuteAsync<SessionStorage>(query, new { SessionKey });
        }
    }
}
