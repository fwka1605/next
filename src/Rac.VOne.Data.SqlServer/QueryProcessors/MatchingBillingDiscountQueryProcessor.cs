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
    public class MatchingBillingDiscountQueryProcessor :
        IAddMatchingBillingDiscountQueryProcessor,
        IDeleteMatchingBillingDiscountQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public MatchingBillingDiscountQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<int> DeleteByMatchingIdAsync(long MatchingId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
IF EXISTS (
    SELECT 1
      FROM MatchingBillingDiscount mbd
     WHERE mbd.MatchingId   = @MatchingId )
BEGIN
    UPDATE bd
       SET bd.AssignmentFlag = 0
      FROM (
           SELECT DISTINCT m.BillingId
             FROM Matching m
            WHERE m.Id          = @MatchingId
           ) m
     INNER JOIN BillingDiscount bd
        ON m.BillingId      = bd.BillingId;

    DELETE mbd
      FROM MatchingBillingDiscount mbd
     WHERE mbd.MatchingId   = @MatchingId;
END
";
            return dbHelper.ExecuteAsync(query, new { MatchingId }, token);
        }

        public Task<int> SaveAsync(MatchingBillingDiscount MatchingBillingDiscount, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO MatchingBillingDiscount 
(MatchingId
,DiscountType
,DiscountAmount
)
OUTPUT inserted.*
VALUES
(@MatchingId
,@DiscountType
,@DiscountAmount)
";
            return dbHelper.ExecuteAsync(query, MatchingBillingDiscount, token);
        }
    }
}
