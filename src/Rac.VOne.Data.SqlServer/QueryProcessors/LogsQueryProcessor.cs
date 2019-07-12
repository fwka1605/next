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
    public class LogsQueryProcessor : ILogsQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        private readonly IConnectionFactory factory;

        public LogsQueryProcessor(IDbHelper dbHelper, IConnectionFactory factory)
        {
            this.dbHelper = dbHelper;
            this.factory = factory;
        }

        public Task<int> SaveAsync(Logs log, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO [dbo].[Logs]
([LoggedAt],[Level],[Logger]
,[SessionKey],[CompanyCode]
,[Message],[Exception]
,[DatabaseName],[Query],[Parameters])
SELECT
 p.LoggedAt,p.[Level],p.Logger
,p.SessionKey,ci.CompanyCode
,p.[Message],p.Exception
,coalesce(@DatabaseName, ci.DatabaseName) [DatabaseName]
,p.Query,p.[Parameters]
  FROM (
SELECT
 GETDATE() [LoggedAt]
,@Level [Level]
,@Logger [Logger]
,@SessionKey [SessionKey]
,@Message [Message]
,@Exception [Exception]
,@DatabaseName [DatabaseName]
,@Query [Query]
,@Parameters [Parameters]
) p
left join (
    SELECT top 1 ss.ConnectionInfo
         , ss.SessionKey
         , ci.CompanyCode
         , ci.DatabaseName
      FROM SessionStorage ss
     INNER JOIN ConnectionInfo ci
        ON ss.ConnectionInfo = ci.ConnectionString
       AND ss.SessionKey = @SessionKey
) ci
ON p.SessionKey = ci.SessionKey
";
            return dbHelper.DoAsync(helper =>
                {
                    return helper.ExecuteAsync(query, log, token);
                }, factory);
        }

    }
}

