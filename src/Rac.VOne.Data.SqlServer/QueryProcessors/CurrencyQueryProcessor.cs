using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CurrencyQueryProcessor :
        ICurrencyQueryProcessor,
        IAddCurrencyQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CurrencyQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<Currency>> GetAsync(CurrencySearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        Currency c
WHERE       c.Id            = c.Id";
            if (option.CompanyId.HasValue) query += @"
AND         c.CompanyId     = @CompanyId";
            if (!string.IsNullOrWhiteSpace(option.Name))
            {
                option.Name = Sql.GetWrappedValue(option.Name);
                query += @"
AND         c.Name          LIKE @Name";
            }
            if (option.Ids?.Any() ?? false) query += @"
AND         c.Id            IN (SELECT Id   FROM @Ids)";
            if (option.Codes?.Any() ?? false) query += @"
AND         c.Code          IN (SELECT Code FROM @Codes)";
            query += @"
ORDER BY    c.DisplayOrder      ASC
          , c.CompanyId         ASC
          , c.Code              ASC";
            return dbHelper.GetItemsAsync<Currency>(query, new {
                            option.CompanyId,
                            option.Name,
                Ids     =   option.Ids.GetTableParameter(),
                Codes   =   option.Codes.GetTableParameter(),
            }, token);
        }

        public Task<Currency> SaveAsync(Currency Currency, CancellationToken token)
        {
            #region merge query
            var query = @"
MERGE INTO Currency AS Org 
USING (SELECT
    @CompanyId As CompanyId,
    @Code As Code
   ) As Target
ON ( 
    Org.CompanyId = Target.CompanyId
    AND Org.Code = Target.Code
) 
WHEN MATCHED THEN 
    UPDATE SET 
     Name = @Name
    ,Symbol = @Symbol
    ,Precision = @Precision
    ,DisplayOrder = @DisplayOrder
    ,Note = @Note
    ,Tolerance = @Tolerance
    ,UpdateBy = @UpdateBy
    ,UpdateAt = GETDATE()
WHEN NOT MATCHED THEN 
 INSERT (CompanyId, Code, Name, Symbol, Precision, DisplayOrder, Note, Tolerance, CreateBy, CreateAt, UpdateBy, UpdateAt)
 VALUES (@CompanyId, @Code, @Name, @Symbol, @Precision, @DisplayOrder, @Note, @Tolerance, @CreateBy, GETDATE(),@UpdateBy, GETDATE())
 OUTPUT inserted.*; ";
            #endregion
            return dbHelper.ExecuteAsync<Currency>(query, Currency, token);
        }

        #region インポートのその他Checkingのため
        public Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code,
            CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM Currency cur
WHERE cur.CompanyId = @CompanyId
  AND EXISTS (
      SELECT *
        FROM Billing b
       WHERE b.CompanyId = @CompanyId
         AND b.CurrencyId = cur.Id )";
            if (Code != null && Code.Any())
            {
                query += @" AND cur.Code NOT IN (SELECT Code FROM @Code) ";
            }
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsReceiptAsync(int CompanyId, IEnumerable<string> Code,
            CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM Currency cur
WHERE cur.CompanyId = @CompanyId
  AND EXISTS (
      SELECT *
        FROM Receipt r
       WHERE r.CompanyId = @CompanyId
         AND r.CurrencyId = cur.Id )";
            if (Code != null && Code.Any())
            {
                query += @" AND cur.Code NOT IN (SELECT Code FROM @Code) ";
            }
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsNettingAsync(
            int CompanyId, IEnumerable<string> Code,
            CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM Currency cur
WHERE cur.CompanyId = @CompanyId
  AND EXISTS (
      SELECT *
        FROM Netting n
       WHERE n.CompanyId = @CompanyId
         AND n.CurrencyId = cur.Id )";
            if (Code != null && Code.Any())
            {
                query += @" AND cur.Code NOT IN (SELECT Code FROM @Code) ";
            }
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }
        #endregion
    }
}
