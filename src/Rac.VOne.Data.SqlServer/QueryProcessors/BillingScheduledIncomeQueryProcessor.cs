using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BillingScheduledIncomeQueryProcessor : IAddBillingScheduledIncomeQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingScheduledIncomeQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<BillingScheduledIncome> SaveAsync(BillingScheduledIncome BillingScheduledIncome, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO BillingScheduledIncome(
BillingId,
MatchingId,
CreateBy,
CreateAt
)
OUTPUT inserted.* 
VALUES (
@BillingId,
@MatchingId,
@CreateBy,
@CreateAt
)";
            return dbHelper.ExecuteAsync<BillingScheduledIncome>(query, BillingScheduledIncome, token);
        }
    }
}
