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
    public class BillingDueAtModifyQueryProcessor :
        IUpdateBillingDueAtModifyQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingDueAtModifyQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<Billing>> UpdateDueAtAsync(BillingDueAtModify billing, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Billing
   SET
       DueAt                        = @NewDueAt
     , OriginalDueAt                = COALESCE(OriginalDueAt, DueAt)
     , UpdateBy                     = @UpdateBy
     , UpdateAt                     = GETDATE()
OUTPUT inserted.*";
            if (billing.BillingInputId.HasValue) query += @"
 WHERE BillingInputId               = @BillingInputId";
            else query += @"
 WHERE Id                           = @Id";
            return dbHelper.GetItemsAsync<Billing>(query, billing, token);
        }

        public Task<IEnumerable<Billing>> UpdateCollectCategoryIdAsync(BillingDueAtModify billing, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Billing
   SET
       CollectCategoryId            = @NewCollectCategoryId
     , OriginalCollectCategoryId    = COALESCE(OriginalCollectCategoryId, CollectCategoryId)
     , UpdateBy                     = @UpdateBy
     , UpdateAt                     = GETDATE()
OUTPUT inserted.*";
            if (billing.BillingInputId.HasValue) query += @"
 WHERE BillingInputId               = @BillingInputId";
            else query += @"
 WHERE Id                           = @Id";
            return dbHelper.GetItemsAsync<Billing>(query, billing, token);
        }


    }
}
