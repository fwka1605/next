using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class MFBillingQueryProcessor : IMFBillingQueryProcessor, IAddMFBillingQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public MFBillingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<MFBilling> SaveAsync(MFBilling billing, CancellationToken token = default(CancellationToken))
        {
            var query = @"INSERT INTO MFBilling(
  BillingId
, CompanyId
, Id
) OUTPUT inserted.*
VALUES(
  @BillingId
, @CompanyId
, @Id
) ";
            return dbHelper.ExecuteAsync<MFBilling>(query, billing, token);
        }

        public Task<IEnumerable<MFBilling>> GetItems(MFBillingSource source, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
SELECT      mfb.*
FROM        MFBilling mfb
INNER JOIN  Billing b
ON          b.Id        = mfb.BillingId";
            if (source.CompanyId.HasValue) query += @"
AND         b.CompanyId = @CompanyId";
            if (source.Ids?.Any() ?? false) query += @"
AND         b.Id        IN (SELECT Id FROM @Ids)";
            if (source.IsMatched.HasValue) query += $@"
AND         b.AssignmentFlag {(source.IsMatched.Value ? "=" : "<>")} 2";
            return dbHelper.GetItemsAsync<MFBilling>(query, source, token);
        }
    }
}
