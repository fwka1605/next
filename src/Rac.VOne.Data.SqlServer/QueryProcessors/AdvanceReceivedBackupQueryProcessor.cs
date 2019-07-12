using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class AdvanceReceivedBackupQueryProcessor :
        IAdvanceReceivedBackupQueryProcessor,
        IUpdateAdvanceReceivedBackupJournalizingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public AdvanceReceivedBackupQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<AdvanceReceivedBackup> GetByOriginalReceiptIdAsync(long OriginalReceiptId, CancellationToken token = default(CancellationToken))
        {
            // 必ず0～1件。分割前のデータをバックアップする前提なので複数は無い。
            var query = @"SELECT * FROM AdvanceReceivedBackup WHERE OriginalReceiptId = @OriginalReceiptId";
            return dbHelper.ExecuteAsync<AdvanceReceivedBackup>(query, new { OriginalReceiptId }, token);
        }

        /// <summary>
        /// 全てのカラム値を指定通りにセットしたレコードを登録する。
        /// IdやCreateAtも指定する必要があるので注意
        /// </summary>
        public Task<AdvanceReceivedBackup> SaveAsync(AdvanceReceivedBackup AdvanceReceivedBackup, CancellationToken token = default(CancellationToken))
        {
            #region insert query
            var query = @"
INSERT INTO AdvanceReceivedBackup
( Id
, CompanyId
, CurrencyId
, ReceiptHeaderId
, ReceiptCategoryId
, CustomerId
, SectionId
, InputType
, Apportioned
, Approved 
, Workday
, RecordedAt
, OriginalRecordedAt
, ReceiptAmount
, AssignmentAmount
, RemainAmount
, AssignmentFlag
, PayerCode
, PayerName
, PayerNameRaw
, SourceBankName
, SourceBranchName
, OutputAt
, DueAt
, MailedAt
, OriginalReceiptId
, ExcludeFlag
, ExcludeCategoryId
, ExcludeAmount
, ReferenceNumber
, RecordNumber
, DensaiRegisterAt
, Note1
, Note2
, Note3
, Note4
, BillNumber
, BillBankCode
, BillBranchCode
, BillDrawAt
, BillDrawer
, DeleteAt
, CreateBy
, CreateAt
, Memo
, TransferOutputAt
, StaffId
, CollationKey
, BankCode
, BankName
, BranchCode
, BranchName
, AccountTypeId
, AccountNumber
, AccountName
) OUTPUT inserted.* VALUES
(@Id
,@CompanyId
,@CurrencyId
,@ReceiptHeaderId
,@ReceiptCategoryId
,@CustomerId
,@SectionId
,@InputType
,@Apportioned
,@Approved 
,@Workday
,@RecordedAt
,@OriginalRecordedAt
,@ReceiptAmount
,@AssignmentAmount
,@RemainAmount
,@AssignmentFlag
,@PayerCode
,@PayerName
,@PayerNameRaw
,@SourceBankName
,@SourceBranchName
,@OutputAt
,@DueAt
,@MailedAt
,@OriginalReceiptId
,@ExcludeFlag
,@ExcludeCategoryId
,@ExcludeAmount
,@ReferenceNumber
,@RecordNumber
,@DensaiRegisterAt
,@Note1
,@Note2
,@Note3
,@Note4
,@BillNumber
,@BillBankCode
,@BillBranchCode
,@BillDrawAt
,@BillDrawer
,@DeleteAt
,@CreateBy
,@CreateAt
,@Memo
,@TransferOutputAt
,@StaffId
,@CollationKey
,@BankCode
,@BankName
,@BranchCode
,@BranchName
,@AccountTypeId
,@AccountNumber
,@AccountName
);
";
            #endregion
            return dbHelper.ExecuteAsync<AdvanceReceivedBackup>(query, AdvanceReceivedBackup, token);
        }

        public Task<int> DeleteAsync(long OriginalReceiptId, CancellationToken token = default(CancellationToken))
        {
            var query = @"DELETE FROM AdvanceReceivedBackup WHERE OriginalReceiptId = @OriginalReceiptId";
            return dbHelper.ExecuteAsync(query, new { OriginalReceiptId }, token);
        }
        public Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE ar
   SET ar.TransferOutputAt  = @UpdateAt
  FROM AdvanceReceivedBackup ar
 WHERE ar.CompanyId         = @CompanyId
   AND ar.CurrencyId        = @CurrencyId
   AND ar.TransferOutputAt  IS NULL
   AND EXISTS (
       SELECT 1
         FROM Receipt r
        WHERE r.OriginalReceiptId   = ar.OriginalReceiptId
          AND r.CompanyId           = @CompanyId
          AND r.CurrencyId          = @CurrencyId
          AND r.OutputAt            = @UpdateAt";
            if (option.RecordedAtFrom.HasValue) query += @"
          AND r.RecordedAt         >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue) query += @"
          AND r.RecordedAt         <= @RecordedAtTo";
            query += @" )
";
            return dbHelper.ExecuteAsync(query, option, token);
        }

        public async Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            if (option.OutputAt == null) option.OutputAt = new List<DateTime>();
            var query = @"
UPDATE ar
   SET ar.TransferOutputAt = NULL
  FROM AdvanceReceivedBackup ar
 WHERE ar.CompanyId         = @CompanyId
   AND ar.CurrencyId        = @CurrencyId
   AND ar.TransferOutputAt IN @OutputAt
   AND EXISTS (
       SELECT 1
         FROM Receipt r
        WHERE CompanyId             = @CompanyId
          AND r.CurrencyId          = @CurrencyId
          AND r.OriginalReceiptId   = ar.OriginalReceiptId
          AND r.OutputAt            IS NULL )
";
            return await dbHelper.ExecuteAsync(query, option, token);
        }
    }
}
