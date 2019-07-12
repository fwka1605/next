using System;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BillingDiscountQueryProcessor :
        IUpdateBillingDiscountQueryProcessor,
        IBillingDiscountQueryProcessor,
        IDeleteBillingDiscountQueryProcessor,
        IAddBillingDiscountQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingDiscountQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<BillingDiscount>> GetAsync(long BillingId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM  BillingDiscount
 WHERE BillingId = @BillingId
 ORDER BY
       BillingId ASC
     , DiscountType ASC";
            return dbHelper.GetItemsAsync<BillingDiscount>(query, new{ BillingId }, token);
        }

        public Task<int> UpdateAssignmentFlagAsync(long BillingId, int AssignmentFlag, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE BillingDiscount
   SET AssignmentFlag   = @AssignmentFlag
 WHERE BillingId        = @BillingId";
            return dbHelper.ExecuteAsync(query, new
            {
                BillingId,
                AssignmentFlag
            }, token);
        }

        public Task<int> DeleteAsync(long BillingId, CancellationToken token = default(CancellationToken))
        {
            var query = @"DELETE FROM  BillingDiscount WHERE BillingId = @BillingId";
            return dbHelper.ExecuteAsync(query, new { BillingId }, token);
        }

        public Task<int> SaveAsync(BillingDiscount BillingDiscount, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO BillingDiscount
(BillingId
,DiscountType
,DiscountAmount
,AssignmentFlag
)
VALUES
(@BillingId
,@DiscountType
,@DiscountAmount
,@AssignmentFlag);
";
            return dbHelper.ExecuteAsync(query, BillingDiscount, token);
        }
    }
}
