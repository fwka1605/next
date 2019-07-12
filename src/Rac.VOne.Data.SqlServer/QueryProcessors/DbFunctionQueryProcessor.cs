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
    public class DbFunctionQueryProcessor :
        ICreateClientKeyQueryProcessor,
        IDbSystemDateTimeQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public DbFunctionQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<byte[]> CreateAsync(ClientKeySearch option, CancellationToken token = default(CancellationToken))
        {
            var query = "SELECT dbo.GetClientKey(@ProgramId, @ClientName, @CompanyCode, @LoginUserCode)";
            return dbHelper.ExecuteAsync<byte[]>(query, option, token);
        }

        private string GetQueryGetDate() => "SELECT GETDATE()";
        public Task<DateTime> GetAsync(CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync<DateTime>(GetQueryGetDate(), null, token);

    }
}
