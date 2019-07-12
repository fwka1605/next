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
    public class KanaHistoryCustomerQueryProcessor :
        IKanaHistoryCustomerQueryProcessor,
        IAddKanaHistoryCustomerQueryProcessor,
        IUpdateKanaHistoryCustomerQueryProcessor,
        IDeleteKanaHistoryCustomerQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public KanaHistoryCustomerQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<KanaHistoryCustomer>> GetAsync(KanaHistorySearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      kh.*
          , cs.Code CustomerCode
          , cs.Name CustomerName
FROM        KanaHistoryCustomer kh
INNER JOIN  Customer cs             ON cs.Id    = kh.CustomerId
WHERE       kh.CompanyId            = @CompanyId";
            if (!string.IsNullOrWhiteSpace(option.PayerName))
            {
                option.PayerName = Sql.GetWrappedValue(option.PayerName);
                query += @"
AND         kh.PayerName            LIKE @PayerName";
            }
            if (!string.IsNullOrWhiteSpace(option.CodeFrom)) query += @"
AND         cs.Code                 >= @CodeFrom";
            if (!string.IsNullOrWhiteSpace(option.CodeTo)) query += @"
AND         cs.Code                 <= @CodeTo";
            query += @"
ORDER BY    cs.Code                 ASC
          , kh.PayerName            ASC";
            return dbHelper.GetItemsAsync<KanaHistoryCustomer>(query, option, token);
        }

        public async  Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 
         FROM KanaHistoryCustomer
        WHERE CustomerId = @CustomerId )";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CustomerId }, token)).HasValue;
        }


        public async Task<bool> ExistAsync(KanaHistoryCustomer KanaHistoryCustomer, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1
         FROM KanaHistoryCustomer 
        WHERE CompanyId         = @CompanyId
          AND PayerName         = @PayerName
          AND CustomerId        = @CustomerId
          AND SourceBankName    = @SourceBankName
          AND SourceBranchName  = @SourceBranchName)";
            return (await dbHelper.ExecuteAsync<int?>(query, KanaHistoryCustomer, token)).HasValue;
        }



        public Task<KanaHistoryCustomer> SaveAsync(KanaHistoryCustomer kanaHistoryCustomer, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO KanaHistoryCustomer AS target
USING ( 
    SELECT 
     @CompanyId         CompanyId
    ,@PayerName         PayerName
    ,@SourceBankName    SourceBankName
    ,@SourceBranchName  SourceBranchName
    ,@CustomerId        CustomerId
) AS source
ON (
        target.CompanyId        = source.CompanyId
    AND target.PayerName        = source.PayerName
    AND target.SourceBankName   = source.SourceBankName
    AND target.SourceBranchName = source.SourceBranchName
    AND target.CustomerId       = source.CustomerId
)
WHEN MATCHED THEN
    UPDATE SET
           HitCount = @HitCount
         , UpdateBy = @UpdateBy
         , UpdateAt = GETDATE()
WHEN NOT MATCHED THEN
    INSERT ( CompanyId,  PayerName,  SourceBankName,  SourceBranchName,  CustomerId,  HitCount, CreateBy, CreateAt, UpdateBy, UpdateAt)
    VALUES (@CompanyId, @PayerName, @SourceBankName, @SourceBranchName, @CustomerId, @HitCount, @UpdateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<KanaHistoryCustomer>(query, kanaHistoryCustomer, token);
        }

        /// <summary>
        /// 消込時に利用 消込回数 を加算する
        /// 登録/更新日時を統一するため、指定したパラメータを利用
        /// </summary>
        /// <param name="KanaHistoryCustomer"></param>
        /// <returns></returns>
        public Task<int> UpdateAsync(KanaHistoryCustomer KanaHistoryCustomer, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO KanaHistoryCustomer AS target 
USING ( 
    SELECT 
     @CompanyId         CompanyId
    ,@PayerName         PayerName
    ,@SourceBankName    SourceBankName
    ,@SourceBranchName  SourceBranchName
    ,@CustomerId        CustomerId
) AS source
ON (
        target.CompanyId        = source.CompanyId
    AND target.PayerName        = source.PayerName
    AND target.SourceBankName   = source.SourceBankName
    AND target.SourceBranchName = source.SourceBranchName
    AND target.CustomerId       = source.CustomerId
)
WHEN MATCHED THEN
    UPDATE SET
           HitCount = HitCount + @HitCount
         , UpdateBy = @UpdateBy
         , UpdateAt = @UpdateAt
WHEN NOT MATCHED THEN
    INSERT ( CompanyId,  PayerName,  SourceBankName,  SourceBranchName,  CustomerId,  HitCount,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt)
    VALUES (@CompanyId, @PayerName, @SourceBankName, @SourceBranchName, @CustomerId, @HitCount, @CreateBy, @CreateAt, @UpdateBy, @UpdateAt)
;
";
            return dbHelper.ExecuteAsync(query, KanaHistoryCustomer, token);
        }

        public Task<int> DeleteAsync(KanaHistoryCustomer kana, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE KanaHistoryCustomer 
 WHERE CompanyId            = @CompanyId
   AND PayerName            = @PayerName
   AND SourceBankName       = @SourceBankName
   AND SourceBranchName     = @SourceBranchName
   AND CustomerId           = @CustomerId";
            return dbHelper.ExecuteAsync(query, kana, token);
        }
    }
}
