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
    public class MfAggrTagRelQueryProcessor :
        IMfAggrTagRelQueryProcessor,
        IAddMfAggrTagRelQueryProcessor,
        IDeleteMfAggrTagRelQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public MfAggrTagRelQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<int> AddAsync(long subAccountId, long tagId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO [dbo].[MfAggrTagRel]
(   [SubAccountId]
,   [TagId]
) VALUES 
(   @subAccountId
,   @tagId
)";
            return dbHelper.ExecuteAsync(query, new { subAccountId, tagId }, token);
        }

        public Task<int> DeleteAsync(long subAccountId, long? tagId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE FROM [dbo].[MfAggrTagRel]
WHERE       [SubAccountId]  = @subAccountId";
            if (tagId.HasValue) query += @"
AND         [TagId]         = @tagId
";
            return dbHelper.ExecuteAsync(query, new { subAccountId, tagId }, token);
        }

        public Task<IEnumerable<MfAggrTagRel>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        [dbo].[MfAggrTagRel]
ORDER BY    [SubAccountId]  ASC
,           [TagId]         ASC
";
            return dbHelper.GetItemsAsync<MfAggrTagRel>(query, cancellatioToken: token);
        }
    }
}
