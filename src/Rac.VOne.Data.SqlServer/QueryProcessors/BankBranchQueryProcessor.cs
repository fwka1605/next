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
    public class BankBranchQueryProcessor :
        IBankBranchQueryProcessor,
        IAddBankBranchQueryProcessor,
        IDeleteBankBranchQueryProcessor

    {
        private readonly IDbHelper dbHelper;

        public BankBranchQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<BankBranch>> GetAsync(BankBranchSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        BankBranch bb
WHERE       bb.CompanyId    = @CompanyId";
            if (option.BankCodes?.Any() ?? false) query += @"
AND         bb.BankCode     IN (SELECT Code FROM @BankCodes)";
            if (option.BranchCodes?.Any() ?? false) query += @"
AND         bb.BranchCode   IN (SELECT Code FROM @BranchCodes)";

            if (!string.IsNullOrWhiteSpace(option.BankName))
            {
                option.BankName = Sql.GetWrappedValue(option.BankName);
                query += @"
AND        (bb.BankCode LIKE @BankName
         OR bb.BankName LIKE @BankName
         OR bb.BankKana LIKE @BankName)";
            }

            if (!string.IsNullOrWhiteSpace(option.BranchName))
            {
                option.BranchName = Sql.GetWrappedValue(option.BranchName);
                query += @"
AND        (bb.BranchCode LIKE @BranchName
         OR bb.BranchName LIKE @BranchName
         OR bb.BranchKana LIKE @BranchName)";
            }

            query += @"
ORDER BY    bb.CompanyId        ASC
          , bb.BankCode         ASC
          , bb.BranchCode       ASC
";

            return dbHelper.GetItemsAsync<BankBranch>(query, new {
                                option.CompanyId,
                                option.BankName,
                                option.BranchName,
                BankCodes   =   option.BankCodes.GetTableParameter(),
                BranchCodes =   option.BranchCodes.GetTableParameter(),
            }, token);
        }

        public Task<BankBranch> SaveAsync(BankBranch BankBranch, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO BankBranch target 
USING ( 
       SELECT   @CompanyId  CompanyId
              , @BankCode   BankCode
              , @BranchCode BranchCode
      ) AS source 
ON ( 
        target.CompanyId    = source.CompanyId
    AND target.BankCode     = source.BankCode
    AND target.BranchCode   = source.BranchCode
)
WHEN MATCHED THEN
    UPDATE SET
                BankName    = @BankName
              , BankKana    = @BankKana
              , BranchName  = @BranchName
              , BranchKana  = @BranchKana
              , UpdateBy    = @UpdateBy
              , UpdateAt    = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (
                CompanyId,
                BankCode,
                BranchCode, 
                BankName,
                BankKana,
                BranchName,
                BranchKana,
                CreateBy, 
                CreateAt, 
                UpdateBy, 
                UpdateAt
            ) 
    VALUES (
                @CompanyId,
                @BankCode,
                @BranchCode, 
                @BankName,
                @BankKana,
                @BranchName,
                @BranchKana,
                @CreateBy, 
                GETDATE(), 
                @UpdateBy, 
                GETDATE()
            ) 
OUTPUT inserted.*; ";
            #endregion
            return dbHelper.ExecuteAsync<BankBranch>(query, BankBranch, token);
        }

        public Task<int> DeleteAsync(int CompanyId, string BankCode, string BranchCode, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE FROM BankBranch
 WHERE CompanyId    = @CompanyId 
   AND BankCode     = @BankCode
   AND BranchCode   = @BranchCode ";
            return dbHelper.ExecuteAsync(query, new { CompanyId, BankCode, BranchCode }, token);
        }

    }
}
