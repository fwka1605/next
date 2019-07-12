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
    public class ReceiptMatchingJournalizingQueryProcessor :
        IUpdateReceiptMatchingJournalizingQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public ReceiptMatchingJournalizingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public async Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            if (option.OutputAt == null) option.OutputAt = new List<DateTime>();
            var query = @"
UPDATE r
   SET r.OutputAt               = NULL
     , r.UpdateAt               = @UpdateAt
     , r.UpdateBy               = @LoginUserId
  FROM Receipt r
 INNER JOIN Category ct
    ON ct.Id                    = r.ReceiptCategoryId
   AND r.CompanyId              = @CompanyId
   AND r.CurrencyId             = @CurrencyId
   AND ct.UseAdvanceReceived    = 1
   AND r.Apportioned            = 1
   AND r.Approved               = 1
   AND r.OutputAt               IN @OutputAt
";
            return await dbHelper.ExecuteAsync(query, option, token);
        }

        public Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE r
   SET r.OutputAt               = @UpdateAt
     , r.UpdateAt               = @UpdateAt
     , r.UpdateBy               = @LoginUserId
  FROM Receipt r
 INNER JOIN Category ct
    ON ct.Id                    = r.ReceiptCategoryId
   AND ct.UseAdvanceReceived    = 1
   AND r.CompanyId              = @CompanyId
   AND r.CurrencyId             = @CurrencyId
   AND r.Apportioned            = 1
   AND r.Approved               = 1
   AND r.OutputAt               IS NULL
   AND r.DeleteAt               IS NULL
";
            if (option.RecordedAtFrom.HasValue) query += @"
AND r.RecordedAt >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue) query += @"
AND r.RecordedAt <= @RecordedAtTo";
            return dbHelper.ExecuteAsync(query, option, token);
        }

        public Task<int> CancelDetailAsync(Receipt detail, CancellationToken token = default(CancellationToken))
        {
            // 前受振替時に退避用テーブルのカラムもクリア 前受計上の場合は、空なのので、処理されない
            var query = @"
UPDATE Receipt
   SET OutputAt = NULL
     , UpdateAt = @UpdateAt
     , UpdateBy = @UpdateBy
 WHERE Id       = @Id
;
UPDATE ar
   SET ar.TransferOutputAt = NULL
  FROM Receipt r
 INNER JOIN AdvanceReceivedBackup ar
    ON r.Id     = @Id
   AND r.OriginalReceiptId = ar.OriginalReceiptId
;
";
            return dbHelper.ExecuteAsync(query, detail, token);
        }
    }
}
