using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ImportDataQueryProcessor : IAddImportDataQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public ImportDataQueryProcessor(
            IDbHelper dbHelper
            )
        {
            this.dbHelper = dbHelper;
        }

        public Task<ImportData> SaveAsync(ImportData data, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO [dbo].[ImportData]
(       [CompanyId]
,       [FileName]
,       [FileSize]
,       [CreateBy]
,       [CreateAt]
)
OUTPUT inserted.*
VALUES
(       @CompanyId
,       @FileName
,       @FileSize
,       @CreateBy
,       GETDATE() /*@CreateAt*/
)";
            return dbHelper.ExecuteAsync<ImportData>(query, data, token);
        }
    }
}
