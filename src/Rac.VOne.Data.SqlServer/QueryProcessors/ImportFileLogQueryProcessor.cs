using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ImportFileLogQueryProcessor :
        IImportFileLogQueryProcessor,
        IAddImportFileLogQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ImportFileLogQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<ImportFileLog>> GetHistoryAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        l.*
            , r.ApportionedAny [Apportioned]
FROM        ImportFileLog l
INNER JOIN  (
            SELECT        rh.ImportFileLogId
                        , MAX(r.Apportioned) [ApportionedAny]
            FROM        Receipt r
            INNER JOIN  ReceiptHeader rh    ON rh.Id                = r.ReceiptHeaderId
                                           AND r.CompanyId          = @CompanyId
                                           AND rh.AssignmentFlag    = 0
            GROUP BY      rh.ImportFileLogId
            ) r
    ON l.Id         = r.ImportFileLogId
   AND l.CompanyId  = @CompanyId
 ORDER BY l.Id DESC
";
            return dbHelper.GetItemsAsync<ImportFileLog>(query, new { CompanyId }, token);
        }


        public Task<ImportFileLog> SaveAsync(ImportFileLog ImportFileLog, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO ImportFileLog
     ( CompanyId 
     , FileName 
     , FileSize 
     , ReadCount 
     , ImportCount 
     , ImportAmount 
     , CreateBy
     , CreateAt 
       )
OUTPUT inserted.*
VALUES
     ( @CompanyId
     , @FileName 
     , @FileSize 
     , @ReadCount 
     , @ImportCount 
     , @ImportAmount 
     , @CreateBy
     , GETDATE()
    )";
            return dbHelper.ExecuteAsync<ImportFileLog>(query, ImportFileLog, token);
        }

    }
}