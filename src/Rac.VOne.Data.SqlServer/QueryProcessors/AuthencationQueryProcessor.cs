using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class AuthencationQueryProcessor : IAuthenticationQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        private readonly IConnectionFactory _factory;

        public AuthencationQueryProcessor(IDbHelper dbHelper,
            IConnectionFactory factory)
        {
            this.dbHelper = dbHelper;
            _factory = factory;
        }

        public Task<string> CreateSessionKeyAsync(string connectionString, CancellationToken token = default(CancellationToken))
        {
            return dbHelper.DoAsync(helper => {
                var query = @"
DECLARE @InsertedRows AS TABLE (GUID uniqueidentifier);
INSERT INTO SessionStorage
(SessionKey, ConnectionInfo, CreatedAt)
OUTPUT INSERTED.SessionKey INTO @InsertedRows
VALUES (NEWID(), @connectionString, GETDATE());
SELECT CONVERT(varchar(100), GUID) FROM @InsertedRows";
                return helper.ExecuteAsync<string>(query, new { connectionString }, token);
            }, _factory);
        }

        public Task<bool> ExistsAuthencticationKeyAsync(string AuthenticationKey, CancellationToken token = default(CancellationToken))
        {
            return dbHelper.DoAsync(async helper =>
            {
                var query = @"
                            SELECT 1
                            WHERE EXISTS (
                                     SELECT 1
                                     FROM AuthenticationStorage
                                     WHERE AuthenticationKey = @AuthenticationKey
                                    )";
                var result = await helper.ExecuteAsync<int?>(query, new { AuthenticationKey  }, token);
                return result.HasValue;
            }, _factory);
        }

        public Task<bool> IsConnectableAsync(IConnectionFactory factory, CancellationToken token = default(CancellationToken))
        {
            return dbHelper.DoAsync(async helper =>
            {
                var query = @"SELECT getdate()";
                var result = await helper.ExecuteAsync<DateTime?>(query);
                return result.HasValue;
            }, factory);
        }

        public Task<string> GetConnectionStringAsync(string tenantCode, CancellationToken token = default(CancellationToken))
        {
            return dbHelper.DoAsync(helper => {
                var query = @"SELECT ConnectionString FROM ConnectionInfo WHERE CompanyCode = @CompanyCode";
                return helper.ExecuteAsync<string>(query,
                    new { CompanyCode = GetDbString(value: tenantCode, isAnsi: true, length: 100) });
            }, _factory);
        }

        private Dapper.DbString GetDbString(string value,
            bool isAnsi = true,
            bool isFixedLength = false,
            int length = -1)
        {
            var dbString = new Dapper.DbString
            {
                Value = value,
                IsAnsi = isAnsi,
                IsFixedLength = isFixedLength,
                Length = length,
            };
            return dbString;
        }


    }
}
