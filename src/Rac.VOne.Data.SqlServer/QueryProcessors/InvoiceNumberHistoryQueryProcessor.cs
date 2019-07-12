using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class InvoiceNumberHistoryQueryProcessor : IAddInvoiceNumberHistoryQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public InvoiceNumberHistoryQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        public Task<InvoiceNumberHistory> SaveAsync(InvoiceNumberHistory InvoiceNumberHistory, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO InvoiceNumberHistory target
USING (
    SELECT @CompanyId         [CompanyId]
         , @NumberingYear     [NumberingYear]
         , @NumberingMonth    [NumberingMonth]
         , @FixedString       [FixedString]
) source
ON    (
        target.CompanyId      = source.CompanyId
    AND target.NumberingYear  = source.NumberingYear
    AND target.NumberingMonth = source.NumberingMonth
    AND target.FixedString    = source.FixedString
)
WHEN MATCHED THEN
    UPDATE SET 
         LastNumber    = @LastNumber
        ,UpdateBy      = @UpdateBy
        ,UpdateAt      = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, NumberingYear, NumberingMonth, FixedString, LastNumber, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @NumberingYear, @NumberingMonth, @FixedString, @LastNumber, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<InvoiceNumberHistory>(query, InvoiceNumberHistory, token);
        }

        public Task<InvoiceNumberHistory> GetAsync(InvoiceNumberHistory InvoiceNumberHistory, CancellationToken token = default(CancellationToken))
        {
            #region query
            var query = @"
SELECT [CompanyId]
     , [NumberingYear]
     , [NumberingMonth]
     , [FixedString]
     , [LastNumber]
     , [CreateBy]
     , [CreateAt]
     , [UpdateBy]
     , [UpdateAt]
 FROM InvoiceNumberHistory
WHERE CompanyId      = @CompanyId
  AND NumberingYear  = @NumberingYear
  AND NumberingMonth = @NumberingMonth
  AND FixedString    = @FixedString
";
            #endregion

            return dbHelper.ExecuteAsync<InvoiceNumberHistory>(query, InvoiceNumberHistory, token);
        }

    }
}
