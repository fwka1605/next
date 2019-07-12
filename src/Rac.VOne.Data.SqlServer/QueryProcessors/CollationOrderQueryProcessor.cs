using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CollationOrderQueryProcessor : ICollationOrderQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CollationOrderQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<int> InitializeAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO [dbo].[CollationOrder]
     ( [CompanyId], [CollationTypeId], [ExecutionOrder], [Available], [CreateBy], [CreateAt], [UpdateBy], [UpdateAt] )
SELECT @CompanyId   [CompanyId]
     , u.[CollationTypeId]
     , u.[CollationTypeId] + 1 [ExecutionOrder]
     , 1 [Available]
     , @LoginUserId [CreateBy]
     , getdate()    [CreateAt]
     , @LoginUserId [UpdateBy]
     , getdate()    [UpdateAt]
  FROM (
             select 0 [CollationTypeId] /* uspColletionPayerCode */
 union all   select 1 [CollationTypeId] /* uspCollationCutomerId */
 union all   select 2 [CollationTypeId] /* uspCollationHistory   */
 union all   select 3 [CollationTypeId] /* uspCollationPayerName */
 union all   select 4 [CollationTypeId] /* uspCollationKey       */
       ) u
";
            return dbHelper.ExecuteAsync(query, new { CompanyId = companyId, LoginUserId = loginUserId }, token);
        }
    }
}
