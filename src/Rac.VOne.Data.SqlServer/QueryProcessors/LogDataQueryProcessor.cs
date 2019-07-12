using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class LogDataQueryProcessor :
        ILogDataByCompanyIdQueryProcessor,
        IAddLogDataQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public LogDataQueryProcessor(IDbHelper helper)
        {
            dbHelper = helper;
        }

        public Task<IEnumerable<LogData>> GetItemsAsync(LogDataSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        LogData
WHERE       CompanyId               = @CompanyId ";
            if (option.LoggedAtFrom.HasValue) query += @"
AND         LogData.LoggedAt        >= @LoggedAtFrom";

            if (option.LoggedAtTo.HasValue) query += @"
AND         LogData.LoggedAt        <= @LoggedAtTo";

            if (!string.IsNullOrEmpty(option.LoginUserCode)) query += @"
AND         LogData.LoginUserCode   = @LoginUserCode ";

            query += @"
ORDER BY    LoggedAt DESC";
            return dbHelper.GetItemsAsync<LogData>(query, option, token);
        }

        public Task<int> SaveAsync(LogData LogData, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO LogData 
(CompanyId, ClientName, LoggedAt, LoginUserCode, LoginUserName
, MenuId, MenuName, OperationName) 
VALUES
(@CompanyId, @ClientName, GetDate(), @LoginUserCode, @LoginUserName
, @MenuId, @MenuName, @OperationName) ";
            return dbHelper.ExecuteAsync(query, LogData, token);
        }

        public Task<LogData> GetStatsAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT MIN(LoggedAt) as FirstLoggedAt, COUNT(1) as LogCount
  FROM LogData 
 WHERE CompanyId = @CompanyId";
            return dbHelper.ExecuteAsync<LogData>(query, new { CompanyId }, token);
        }


    }
}