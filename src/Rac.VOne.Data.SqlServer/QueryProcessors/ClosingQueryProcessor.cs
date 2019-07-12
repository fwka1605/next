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
    public class ClosingQueryProcessor : IClosingQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public ClosingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<ClosingHistory>> GetClosingHistoryAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
DECLARE @shimebi          int  = (SELECT ClosingDay   FROM Company WHERE Id        = @companyId)
DECLARE @lastClosingMonth date = (SELECT ClosingMonth FROM Closing WHERE CompanyId = @companyId)

SELECT 0 AS Selected
     , history.ClosingMonth
     , CASE WHEN history.ClosingMonth <= @lastClosingMonth
            THEN 1
            ELSE 0
       END AS IsClosed
     , SUM(history.BillingCount) AS BillingCount
     , SUM(history.ReceiptCount) AS ReceiptCount
FROM (

    SELECT CASE WHEN DAY(BilledAt) <=  @shimebi
                THEN DATEFROMPARTS(YEAR(BilledAt), MONTH(BilledAt), 1)
                ELSE DATEADD(month, 1, DATEFROMPARTS(YEAR(BilledAt), MONTH(BilledAt), 1))
           END AS ClosingMonth
         , COUNT(*) AS BillingCount
         , 0        AS ReceiptCount
      FROM Billing b
     WHERE b.CompanyId = @companyId
       AND ( b.DeleteAt IS NULL OR (b.DeleteAt IS NOT NULL AND b.AssignmentFlag <> 0))
     GROUP BY CASE WHEN DAY(BilledAt) <=  @shimebi
                   THEN DATEFROMPARTS(YEAR(BilledAt), MONTH(BilledAt), 1)
                   ELSE DATEADD(month, 1, DATEFROMPARTS(YEAR(BilledAt), MONTH(BilledAt), 1))
              END
    
     UNION ALL
    
    SELECT CASE WHEN DAY(RecordedAt) <=  @shimebi
                THEN DATEFROMPARTS(YEAR(RecordedAt), MONTH(RecordedAt), 1)
                ELSE DATEADD(month, 1, DATEFROMPARTS(YEAR(RecordedAt), MONTH(RecordedAt), 1))
           END AS ClosingMonth
         , 0        AS BillingCount
         , COUNT(*) AS ReceiptCount
      FROM Receipt r
     WHERE r.CompanyId = @companyId
       AND ( r.DeleteAt IS NULL OR (r.DeleteAt IS NOT NULL AND r.AssignmentFlag <> 0))
     GROUP BY CASE WHEN DAY(RecordedAt) <=  @shimebi
                   THEN DATEFROMPARTS(YEAR(RecordedAt), MONTH(RecordedAt), 1)
                   ELSE DATEADD(month, 1, DATEFROMPARTS(YEAR(RecordedAt), MONTH(RecordedAt), 1))
              END

) history
GROUP BY history.ClosingMonth
ORDER BY history.ClosingMonth DESC
";
            return dbHelper.GetItemsAsync<ClosingHistory>(query, new { companyId }, token);
        }


        public Task<Closing> SaveAsync(Closing closing, CancellationToken token = default(CancellationToken))
        {
            string query = @"
MERGE INTO Closing AS c
USING (
    SELECT
     @CompanyId AS CompanyId
) AS Target 
ON (
    c.CompanyId = @CompanyId
)
WHEN MATCHED THEN
    UPDATE SET
           ClosingMonth = @ClosingMonth
         , UpdateBy     = @UpdateBy
         , UpdateAt     = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, ClosingMonth, UpdateBy,UpdateAt)
    VALUES (
           @CompanyId
         , @ClosingMonth
         , @UpdateBy
         , GETDATE() )
OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<Closing>(query, closing, token);
        }

    }
}
