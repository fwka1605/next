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
    public class ReceiptExcludeQueryProcessor :
        IReceiptExcludeQueryProcessor,
        IAddReceiptExcludeQueryProcessor,
        IDeleteReceiptExcludeQueryProcessor,
        IUpdateReceiptExcludeJournalizingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReceiptExcludeQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public async Task<bool> ExistExcludeCategoryAsync(int ExcludeCategoryId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT TOP(1) 1 FROM ReceiptExclude 
                            WHERE  ExcludeCategoryId = @ExcludeCategoryId";
            return (await dbHelper.ExecuteAsync<int?>(query, new { ExcludeCategoryId }, token)).HasValue;
        }



        public Task<IEnumerable<ReceiptExclude>> GetByReceiptIdAsync(long ReceiptId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT * FROM ReceiptExclude WHERE ReceiptId = @ReceiptId ";
            return dbHelper.GetItemsAsync<ReceiptExclude>(query, new { ReceiptId });
        }

        //public ReceiptExclude SaveReceiptExclude(long ReceiptId, decimal ExcludeAmount, int? ExcludeCategoryId, int UserId, DateTime? OutputAt)
        //{
        //    var query = @"INSERT INTO ReceiptExclude(ReceiptId, ExcludeCategoryId, ExcludeAmount, OutputAt, CreateBy, CreateAt, UpdateBy, UpdateAt) OUTPUT inserted.*
        //                           VALUES (@ReceiptId, @ExcludeCategoryId, @ExcludeAmount, @OutputAt, @CreateBy, GETDATE(), @UpdateBy, GETDATE());";
        //    return dbHelper.Execute<ReceiptExclude>(query, new
        //        {
        //            ReceiptId , ExcludeCategoryId , ExcludeAmount, OutputAt, CreateBy = UserId, UpdateBy = UserId
        //        });
        //}

        public Task<int> DeleteAsync(long ReceiptId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE      ReceiptExclude 
WHERE       ReceiptId   = @ReceiptId ";
            return dbHelper.ExecuteAsync(query, new { ReceiptId }, token);
        }

//        public ReceiptExclude ExcludeWithReceiptApportion(ReceiptApportion receiptApportion)
//        {
//            var query = @"
//INSERT INTO ReceiptExclude
//    (ReceiptId
//    ,ExcludeAmount
//    ,ExcludeCategoryId
//    ,OutputAt
//    ,CreateBy
//    ,CreateAt
//    ,UpdateBy
//    ,UpdateAt
//    )
// OUTPUT inserted.*
// VALUES
//    (@Id
//    ,@ExcludeAmount
//    ,@ExcludeCategoryId
//    ,NULL
//    ,@UpdateBy
//    ,GETDATE()
//    ,@UpdateBy
//    ,GETDATE()
//    )";
//            return dbHelper.Execute<ReceiptExclude>(query, receiptApportion);
//        }



        public Task<int> DeleteByFileLogIdAsync(int Id, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE      re
FROM        ReceiptExclude re
INNER JOIN  Receipt r               ON r.Id     = re.ReceiptId
INNER JOIN  ReceiptHeader rh        ON rh.Id    = r.ReceiptHeaderId
WHERE       rh.ImportFileLogId      = @Id
";
            return dbHelper.ExecuteAsync(query, new { Id }, token);
        }

        public Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
UPDATE re
   SET re.OutputAt      = @UpdateAt
     , re.UpdateAt      = @UpdateAt
     , re.UpdateBy      = @LoginUserId
  FROM ReceiptExclude re
 INNER JOIN Receipt r
    ON r.Id             = re.ReceiptId
   AND r.CompanyId      = @CompanyId
   AND r.CurrencyId     = @CurrencyId
   AND r.Apportioned    = 1
   AND r.Approved       = 1
   AND re.OutputAt      IS NULL
";
            if (option.RecordedAtFrom.HasValue) query += @"
   AND r.RecordedAt >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue) query += @"
   AND r.RecordedAt <= @RecordedAtTo";
            return dbHelper.ExecuteAsync(query, option, token);
        }
        public async Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            if (!(option.OutputAt?.Any() ?? false)) return 0;
            var query = @"
UPDATE re
   SET re.OutputAt      = NULL
     , re.UpdateAt      = @UpdateAt
     , re.UpdateBy      = @LoginUserId
  FROM ReceiptExclude re
 INNER JOIN Receipt r
    ON r.Id             = re.ReceiptId
   AND r.CompanyId      = @CompanyId
   AND r.CurrencyId     = @CurrencyId
   AND r.Apportioned    = 1
   AND r.Approved       = 1
   AND re.OutputAt      IN @OutputAt
";
            return await dbHelper.ExecuteAsync(query, option, token);
        }
        public Task<int> CancelDetailAsync(ReceiptExclude detail, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE ReceiptExclude
   SET OutputAt = NULL
     , UpdateAt = @UpdateAt
     , UpdateBy = @UpdateBy
 WHERE Id       = @Id";
            return dbHelper.ExecuteAsync(query, detail, token);
        }

        public Task<ReceiptExclude> SaveAsync(ReceiptExclude exclude, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO ReceiptExclude
     ( ReceiptId
     , ExcludeAmount
     , ExcludeCategoryId
     , OutputAt
     , CreateBy
     , CreateAt
     , UpdateBy
     , UpdateAt
       )
OUTPUT inserted.*
VALUES
     ( @ReceiptId
     , @ExcludeAmount
     , @ExcludeCategoryId
     , @OutputAt
     , @UpdateBy
     , GETDATE()
     , @UpdateBy
     , GETDATE()
    )";
            return dbHelper.ExecuteAsync<ReceiptExclude>(query, exclude, token);
        }

    }
}
