using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReceiptHeaderQueryProcessor :
        IUpdateReceiptHeaderQueryProcessor,
        IReceiptHeaderQueryProcessor,
        IDeleteReceiptHeaderQueryProcessor,
        IAddReceiptHeaderQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReceiptHeaderQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<int> UpdateAsync(ReceiptHeaderUpdateOption option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE      rh
SET           rh.AssignmentFlag     = 1
            , rh.UpdateBy           = @UpdateBy
            , rh.UpdateAt           = GETDATE()
FROM        ReceiptHeader rh
WHERE       rh.Id                   = rh.Id
AND         rh.AssignmentFlag       = 0";
            if (option.ReceiptHeaderId.HasValue) query += @"
AND         rh.Id                   = @ReceiptHeaderId";
            if (option.ReceiptId.HasValue) query += @"
AND         rh.Id                   IN (
            SELECT          DISTINCT r.ReceiptHeaderId
            FROM            Receipt r
            WHERE           r.Id            = @ReceiptId
            AND             r.InputType     = 1
            )";
            if (option.CompanyId.HasValue) query += @"
AND         rh.CompanyId            = @CompanyId
AND         rh.Id                   IN (
            SELECT          DISTINCT r.ReceiptHeaderId
            FROM            Receipt r
            WHERE           r.CompanyId         = @CompanyId
            AND             r.AssignmentFlag    IN (1, 2)
            )";

            return dbHelper.ExecuteAsync(query, option, token);
        }


        public Task<int> DeleteByFileLogIdAsync(int ImportFileLogId, CancellationToken token = default(CancellationToken))
        {
            var query = @"DELETE FROM ReceiptHeader WHERE ImportFileLogId = @ImportFileLogId";
            return dbHelper.ExecuteAsync(query, new { ImportFileLogId }, token);
        }


        public Task<ReceiptHeader> SaveAsync(ReceiptHeader header, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO ReceiptHeader
      ( CompanyId
      , FileType
      , CurrencyId
      , ImportFileLogId
      , AssignmentFlag
      , ImportCount
      , ImportAmount
      , CreateBy
      , CreateAt
      , UpdateBy
      , UpdateAt
        )
 OUTPUT inserted.*
 VALUES
      ( @CompanyId
      , @FileType
      , @CurrencyId
      , @ImportFileLogId
      , @AssignmentFlag
      , @ImportCount
      , @ImportAmount
      , @CreateBy
      , GETDATE()
      , @UpdateBy
      , GETDATE()
    )";
            return dbHelper.ExecuteAsync<ReceiptHeader>(query, header, token);
        }

        public Task<IEnumerable<ReceiptHeader>> GetItemsAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT rh.Id
     , rh.CompanyId
     , rh.FileType
     , rh.CurrencyId
     , rh.CreateAt
     , rh.UpdateAt
     , bat.Name [AccountTypeName]
     , cur.Code [CurrencyCode]
     , r.MaxApportioned [ExistApportioned]
     , r.MinApportioned [IsAllApportioned]
     , r.BankCode
     , r.BranchCode
     , r.AccountTypeId
     , r.AccountNumber
     , r.AccountName
     , COALESCE(ba.BankName     , r.BankName)     [BankName]
     , COALESCE(ba.BranchName   , r.BranchName)   [BranchName]
  FROM ReceiptHeader rh
 INNER JOIN (
       SELECT r.ReceiptHeaderId
            , MAX( r.Apportioned )      [MaxApportioned]
            , MIN( r.Apportioned )      [MinApportioned]
            , MAX( r.BankCode )         [BankCode]
            , MAX( r.BankName )         [BankName]
            , MAX( r.BranchCode )       [BranchCode]
            , MAX( r.BranchName )       [BranchName]
            , MAX( r.AccountTypeId )    [AccountTypeId]
            , MAX( r.AccountNumber )    [AccountNumber]
            , MAX( r.AccountName )      [AccountName]
         FROM Receipt r
        WHERE r.CompanyId       = @CompanyId 
        GROUP BY ReceiptHeaderId 
       ) r
    ON r.ReceiptHeaderId    = rh.Id 
 INNER JOIN BankAccountType bat
    ON bat.Id               = r.AccountTypeId 
 INNER JOIN Currency cur
    ON cur.Id               = rh.CurrencyId 
  LEFT JOIN BankAccount ba 
    ON ba.CompanyId         = rh.CompanyId
   AND ba.BankCode          =  r.BankCode
   AND ba.BranchCode        =  r.BranchCode
   AND ba.AccountTypeId     =  r.AccountTypeId
   AND ba.AccountNumber     =  r.AccountNumber
WHERE rh.CompanyId         = @CompanyId 
   AND (rh.AssignmentFlag    = 0 OR r.MinApportioned = 0) 
 ORDER BY rh.Id DESC";
            return dbHelper.GetItemsAsync<ReceiptHeader>(query, new { CompanyId = companyId }, token);
        }
    }
}
