using System;
using System.Linq;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class MatchingHeaderQueryProcessor :
        IUpdateMatchingHeaderQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public MatchingHeaderQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        public Task<MatchingHeader> ApproveAsync(MatchingHeader header, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE  MatchingHeader
SET     Approved    = 1
,       UpdateBy    = @UpdateBy
,       UpdateAt    = GETDATE()
OUTPUT  inserted.*
WHERE   Id          = @Id
AND     UpdateAt    = @UpdateAt";
            return dbHelper.ExecuteAsync<MatchingHeader>(query, header, token);
        }

        public Task<MatchingHeader> CancelApprovalAsync(MatchingHeader header, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE  MatchingHeader
SET     Approved    = 0
,       UpdateBy    = @UpdateBy
,       UpdateAt    = GETDATE()
OUTPUT  inserted.*
WHERE   Id          = @Id
AND     UpdateAt    = @UpdateAt";
            return dbHelper.ExecuteAsync<MatchingHeader>(query, header, token);
        }

    }
}
