using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ImportDataDetailQueryProcessor :
        IImportDataDetailQueryProcessor,
        IAddImportDataDetailQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ImportDataDetailQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<ImportDataDetail>> GetAsync(long importDataId, int? objectType = null, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        d.*
FROM        [dbo].[ImportDataDetail] d
WHERE       d.[ImportDataId]        = @importDataId";
            if (objectType.HasValue) query += @"
AND         d.[ObjectType]          = @objectType";

            query += @"
ORDER BY      d.[ImportDataId]      ASC
            , d.[ObjectType]        ASC
            , d.[Id]        ASC
";
            return dbHelper.GetItemsAsync<ImportDataDetail>(query, new { importDataId, objectType }, token);
        }

        public Task<int> SaveAsync(ImportDataDetail detail, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO     [dbo].[ImportDataDetail]
(               [ImportDataId]
,               [ObjectType]
,               [RecordItem]
)
VALUES
(               @ImportDataId
,               @ObjectType
,               @RecordItem
)
";
            return dbHelper.ExecuteAsync(query, detail, token);
        }
    }
}
