using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReceiptQueryProcessor :
        IReceiptQueryProcessor,
        IReceiptExistsQueryProcessor,
        IDeleteReceiptQueryProcessor,
        IUpdateReceiptQueryProcessor,
        IAddReceiptQueryProcessor,
        IReceiptApportionByIdQueryProcessor,
        IUpdateReceiptApportionQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReceiptQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public async Task<bool> ExistReceiptCategoryAsync(int ReceiptCategoryId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS (
       SELECT 1
         FROM Receipt
        WHERE ReceiptCategoryId = @ReceiptCategoryId
           OR ExcludeCategoryId = @ReceiptCategoryId)
";
            return (await dbHelper.ExecuteAsync<int?>(query, new { ReceiptCategoryId }, token)).HasValue;
        }

        public async Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS (
      SELECT 1
        FROM Receipt 
       WHERE CustomerId = @CustomerId)
";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CustomerId }, token)).HasValue;
        }

        public async Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS (
      SELECT 1
        FROM Receipt 
       WHERE SectionId = @SectionId)
";
            return (await dbHelper.ExecuteAsync<int?>(query, new { SectionId }, token)).HasValue;
        }


        public async Task<bool> ExistCompanyAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS (
       SELECT 1
         FROM Receipt 
        WHERE CompanyId = @CompanyId)
";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CompanyId }, token)).HasValue;
        }

        public async Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT 1
                    WHERE EXISTS( SELECT 1
                    FROM Receipt 
                    WHERE CurrencyId = @CurrencyId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CurrencyId }, token)).HasValue;
        }

        public async Task<bool> ExistOriginalReceiptAsync(long ReceiptId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT 1
                    WHERE EXISTS (SELECT 1
                          FROM Receipt
                          WHERE OriginalReceiptId = @OriginalReceiptId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { OriginalReceiptId = ReceiptId }, token)).HasValue;
        }

        public async Task<bool> ExistNonApportionedAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
WHERE EXISTS (
      SELECT 1
        FROM Receipt
       WHERE CompanyId   = @CompanyId
         AND Apportioned = 0
         AND RecordedAt  >= @ClosingFrom
         AND RecordedAt  <= @ClosingTo
)
";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CompanyId, ClosingFrom, ClosingTo }, token)).HasValue;
        }

        public async Task<bool> ExistNonOutputedAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
WHERE EXISTS (
    SELECT 1
      FROM Receipt
     WHERE  CompanyId = @CompanyId
       AND (DeleteAt IS NULL OR (DeleteAt IS NOT NULL AND AssignmentFlag <> 0))
       AND  OutputAt IS NULL
       AND  Apportioned <> 0
       AND  RecordedAt  >= @ClosingFrom
       AND  RecordedAt  <= @ClosingTo
)
";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CompanyId, ClosingFrom, ClosingTo }, token)).HasValue;
        }

        public async Task<bool> ExistNonAssignmentAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
WHERE EXISTS (
    SELECT 1
      FROM Receipt
     WHERE CompanyId = @CompanyId
       AND DeleteAt IS NULL
       AND Apportioned    <> 0
       AND AssignmentFlag <> 2
       AND RecordedAt     >= @ClosingFrom
       AND RecordedAt     <= @ClosingTo
)
";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CompanyId, ClosingFrom, ClosingTo }, token)).HasValue;
        }


        private string GetQueryDeleteReceiptWhereAnyAssigned() => @"DELETE FROM Receipt WHERE Id = @Id AND AssignmentFlag = 0 AND DeleteAt IS NULL AND OutputAt IS NULL";

        public Task<int> CancelAdvanceReceivedAsync(long Id, CancellationToken token = default(CancellationToken))
        {
            var query = GetQueryDeleteReceiptWhereAnyAssigned();
            return dbHelper.ExecuteAsync(query, new { Id }, token);
        }

        public Task<int> DeleteByFileLogIdAsync(int ImportFileLogId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE      r
FROM        Receipt r
INNER JOIN  ReceiptHeader rh        ON rh.Id        = r.ReceiptHeaderId
WHERE       rh.ImportFileLogId      = @ImportFileLogId";
            return dbHelper.ExecuteAsync(query, new { ImportFileLogId }, token);
        }

        private string GetQueryUpdateOriginalReceiptRemain() => @"
UPDATE Receipt
   SET AssignmentFlag   = 2
     , RemainAmount     = 0
     , UpdateBy         = @UpdateBy
     , UpdateAt         = @UpdateAt
OUTPUT inserted.*
 WHERE Id               = @Id
";

        public Task<Receipt> UpdateOriginalRemainAsync(long Id, int UpdateBy, DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return dbHelper.ExecuteAsync<Receipt>(GetQueryUpdateOriginalReceiptRemain(), new { Id, UpdateBy, UpdateAt }, token);
        }

        public Task<Receipt> UpdateCancelAdvancedReceivedAsync(long receiptId, long originalReceiptId, int updateBy, DateTime updateAt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE r
   SET r.RemainAmount    = r.RemainAmount + ar.ReceiptAmount
     , r.AssignmentFlag  = CASE WHEN r.RemainAmount + ar.ReceiptAmount = r.ReceiptAmount THEN 0 ELSE 1 END
     , r.UpdateBy        = @updateBy
     , r.UpdateAt        = GETDATE()
OUTPUT inserted.*
  FROM Receipt r
 INNER JOIN Receipt ar
    ON r.Id         = ar.OriginalReceiptId
   AND r.Id         = @originalReceiptId
   AND ar.Id        = @receiptId
   AND ar.UpdateAt  = @updateAt;";
            return dbHelper.ExecuteAsync<Receipt>(query, new { receiptId, originalReceiptId, updateBy, updateAt }, token);
        }


        private string GetQueryInsertAdvanceReceived() => @"
INSERT INTO Receipt
     (  CompanyId
     ,  CurrencyId
     ,  ReceiptHeaderId
     ,  ReceiptCategoryId
     ,  CustomerId
     ,  SectionId
     ,  InputType
     ,  Apportioned
     ,  Approved 
     ,  Workday
     ,  RecordedAt
     ,  ReceiptAmount
     ,  AssignmentAmount
     ,  RemainAmount
     ,  AssignmentFlag
     ,  PayerCode
     ,  PayerName
     ,  PayerNameRaw
     ,  SourceBankName
     ,  SourceBranchName
     ,  DueAt
     ,  MailedAt
     ,  OriginalReceiptId
     ,  ExcludeFlag
     ,  ExcludeAmount
     ,  ReferenceNumber
     ,  RecordNumber
     ,  DensaiRegisterAt
     ,  Note1
     ,  Note2
     ,  Note3
     ,  Note4
     ,  BillNumber
     ,  BillBankCode
     ,  BillBranchCode
     ,  BillDrawAt
     ,  BillDrawer
     ,  CollationKey
     ,  BankCode
     ,  BankName
     ,  BranchCode
     ,  BranchName
     ,  AccountTypeId
     ,  AccountNumber
     ,  AccountName
     ,  CreateBy
     ,  CreateAt
     ,  UpdateBy
     ,  UpdateAt )
OUTPUT inserted.*
SELECT r.CompanyId
     , r.CurrencyId
     , r.ReceiptHeaderId
     , rc.Id           [ReceiptCategoryId]
     , @customerId     [CustomerId]
     , r.SectionId
     , r.InputType
     , 1               [Apportioned]
     , 1               [Approved]
     , GETDATE()       [Workday]
     , r.RecordedAt
     , r.RemainAmount  [ReceiptAmount]
     , 0               [AssignmentAmount]
     , r.RemainAmount
     , 0               [AssignmentFlag]
     , r.PayerCode
     , r.PayerName
     , r.PayerNameRaw
     , r.SourceBankName
     , r.SourceBranchName
     , r.DueAt
     , r.MailedAt
     , r.Id            [OriginalReceiptId]
     , 0               [ExcludeFlag]
     , 0               [ExcludeAmount]
     , r.ReferenceNumber
     , r.RecordNumber
     , r.DensaiRegisterAt
     , r.Note1
     , r.Note2
     , r.Note3
     , r.Note4
     , r.BillNumber
     , r.BillBankCode
     , r.BillBranchCode
     , r.BillDrawAt
     , r.BillDrawer
     , r.CollationKey
     , r.BankCode
     , r.BankName
     , r.BranchCode
     , r.BranchName
     , r.AccountTypeId
     , r.AccountNumber
     , r.AccountName
     , @updateBy        [CreateBy]
     , @newUpdateAt     [CreateAt]
     , @updateBy        [UpdateBy]
     , @newUpdateAt     [UpdateAt]
  FROM Receipt r
 INNER JOIN Category rc        ON rc.CompanyId = r.CompanyId AND rc.CategoryType = 2 AND rc.Code = N'99'
 WHERE r.Id             = @id
   AND r.UpdateAt       = @updateAt
;
";


        public Task<Receipt> AddAdvanceReceivedAsync(long id, int? customerId, int updateBy, DateTime updateAt, DateTime newUpdateAt, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync<Receipt>(GetQueryInsertAdvanceReceived(),
                new { id, updateAt, customerId, updateBy, newUpdateAt }, token);

        public Task<int> UpdateExcludeAmountAsync(Receipt receipt, CancellationToken token = default(CancellationToken))
        {
            var query = "";

            if (receipt.ExcludeFlag == 1)
            {
                query = @"
UPDATE r
   SET r.ExcludeCategoryId  = @ExcludeCategoryId
     , r.ExcludeFlag        = 1
     , r.RemainAmount       = r.RemainAmount + r.ExcludeAmount - @ExcludeAmount
     , r.ExcludeAmount      = @ExcludeAmount
     , r.AssignmentFlag     = CASE WHEN r.RemainAmount + r.ExcludeAmount = @ExcludeAmount THEN 2 ELSE 1 END
     , r.UpdateBy           = @UpdateBy
     , r.UpdateAt           = GETDATE()
  FROM Receipt r
 WHERE r.Id                 = @Id
   AND r.UpdateAt           = @UpdateAt
";
            }
            else
            {
                query = @"
UPDATE r
   SET r.ExcludeCategoryId  = NULL
     , r.ExcludeFlag        = 0
     , r.RemainAmount       = r.RemainAmount + r.ExcludeAmount
     , r.ExcludeAmount      = 0
     , r.AssignmentFlag     = CASE WHEN r.ReceiptAmount = r.RemainAmount + r.ExcludeAmount THEN 0 ELSE 1 END
     , r.UpdateBy           = @UpdateBy
     , r.UpdateAt           = GETDATE()
  FROM Receipt r
 WHERE r.Id                 = @Id
   AND r.UpdateAt           = @UpdateAt
";
            }
            return dbHelper.ExecuteAsync(query, receipt, token);
        }

        public Task<IEnumerable<ReceiptApportion>> GetApportionItemsAsync(IEnumerable<long> headerIds, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT r.Id
     , r.ReceiptHeaderId
     , r.CompanyId
     , r.CurrencyId
     , r.ExcludeFlag
     , r.ExcludeCategoryId
     , r.ExcludeAmount
     , r.SectionId
     , r.CustomerId
     , r.PayerName
     , r.PayerNameRaw
     , r.ReceiptAmount
     , r.RecordedAt
     , r.Workday
     , r.SourceBankName
     , r.SourceBranchName
     , r.Apportioned
     , r.UpdateAt
     , CASE WHEN LEN(r.PayerCode) > 7 THEN LEFT (r.PayerCode, 3) ELSE '' END [ExcludeVirtualBranchCode]
     , CASE WHEN LEN(r.PayerCode) > 7 THEN RIGHT(r.PayerCode, 7) ELSE '' END [ExcludeAccountNumber]
     , cur.Code        [CurrencyCode]
     , sec.Code        [SectionCode]
     , sec.Name        [SectionName]
     , cus.Code        [CustomerCode]
     , cus.Name        [CustomerName]
     , COALESCE(pay_cus.Id  , his1_cus.Id  , his2_cus.Id  , kana_cus.Id  )    [RefCustomerId]
     , COALESCE(pay_cus.Code, his1_cus.Code, his2_cus.Code, kana_cus.Code)    [RefCustomerCode]
     , COALESCE(pay_cus.Name, his1_cus.Name, his2_cus.Name, kana_cus.Name)    [RefCustomerName]
  FROM Receipt r
 INNER JOIN Currency cur        ON cur.Id          = r.CurrencyId
  LEFT JOIN ReceiptHeader rh    ON r.ReceiptHeaderId = rh.Id
  LEFT JOIN Section sec         ON sec.Id          = r.SectionId
  LEFT JOIN Customer cus        ON cus.Id          = r.CustomerId
  LEFT JOIN Customer pay_cus    ON pay_cus.id      = (
        SELECT TOP 1 pay_cus_buf.Id
        FROM Customer pay_cus_buf
        WHERE pay_cus_buf.CompanyId                 = r.CompanyId
          AND pay_cus_buf.ExclusiveBankCode         = r.BankCode
          AND pay_cus_buf.ExclusiveBranchCode       = r.BranchCode
          AND pay_cus_buf.ExclusiveAccountNumber    = r.PayerCode
          ORDER BY
                pay_cus_buf.Code
    )
  LEFT JOIN Customer his1_cus   ON his1_cus.Id     = (
        SELECT TOP 1 his1.CustomerId
          FROM KanaHistoryCustomer his1
         WHERE his1.CompanyId           = r.CompanyId
           AND his1.PayerName           = r.PayerName
           AND his1.SourceBankName      = r.SourceBankName
           AND his1.SourceBranchName    = r.SourceBranchName
         ORDER BY
               his1.HitCount DESC
             , his1.UpdateAt DESC )
  LEFT JOIN Customer his2_cus   ON his2_cus.Id      = (
        SELECT TOP 1 his2.CustomerId
          FROM KanaHistoryCustomer his2
         WHERE his2.CompanyId           = r.CompanyId
           AND his2.PayerName           = r.PayerName
           AND his2.SourceBankName      = N''
           AND his2.SourceBranchName    = N''
        ORDER BY
               his2.HitCount DESC
             , his2.UpdateAt DESC )
  LEFT JOIN Customer kana_cus   ON kana_cus.Id      = (
       SELECT TOP 1 kana_cus_buf.Id
         FROM Customer kana_cus_buf
        WHERE kana_cus_buf.CompanyId    = r.CompanyId
          AND kana_cus_buf.Kana         = r.PayerName
       ORDER BY
              kana_cus_buf.Code ASC    )
 WHERE r.ReceiptHeaderId IN (SELECT Id FROM @ReceiptHeaderId )
 ORDER BY r.Id  ASC ";
            return dbHelper.GetItemsAsync<ReceiptApportion>(query, new { ReceiptHeaderId = headerIds.GetTableParameter() }, token);
        }

        public Task<int> OmitByApportionAsync(ReceiptApportion receiptApportion, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE      Receipt SET
              Apportioned   = @Apportioned
            , CustomerId    = @CustomerId
            , SectionId     = @SectionId
            , DeleteAt      = GETDATE()
            , UpdateBy      = @UpdateBy
            , UpdateAt      = GETDATE()
WHERE       Id              = @Id
AND         UpdateAt        = @UpdateAt";
            return dbHelper.ExecuteAsync(query, receiptApportion, token);
        }

        public Task<ReceiptApportion> UpdateApportionAsync(ReceiptApportion apportion, CancellationToken token = default(CancellationToken))
        {
            var query = @"
                        UPDATE Receipt
                           SET Apportioned       = @Apportioned
                             , CustomerId        = @CustomerId
                             , SectionId         = @SectionId 
                             , ExcludeFlag       = @ExcludeFlag
                             , ExcludeCategoryId = @ExcludeCategoryId
                             , ExcludeAmount     = @ExcludeAmount
                             , RemainAmount      = RemainAmount - @ExcludeAmount
                             , AssignmentFlag    = CASE WHEN @ExcludeAmount = ReceiptAmount THEN 2 
                                                        WHEN @ExcludeAmount <> 0            THEN 1
                                                        ELSE 0 END
                             ,UpdateBy          = @UpdateBy
                             ,UpdateAt          = GETDATE()
                        OUTPUT inserted.*
                        WHERE Id = @Id AND UpdateAt = @UpdateAt ";
            return dbHelper.ExecuteAsync<ReceiptApportion>(query, apportion, token);
        }


        // 前受金振替・分割処理画面に必要な項目のみ取得
        private string GetSelectQueryForAdvanceReceived() => @"
SELECT
       r.*
     , rc.Code  CategoryCode
     , rc.Name  CategoryName
     , ec.Code  ExcludeCategoryCode
     , ec.Name  ExcludeCategoryName
     , cs.Code  CustomerCode
     , cs.Name  CustomerName
     , cr.Code  CurrencyCode
     , sc.Code  SectionCode
     , sc.Name  SectionName
     , rm.Memo  ReceiptMemo
  FROM Receipt r
  LEFT JOIN Category rc       ON rc.Id =  r.ReceiptCategoryId
  LEFT JOIN Category ec       ON ec.Id =  r.ExcludeCategoryId
  LEFT JOIN Currency cr       ON cr.Id =  r.CurrencyId
  LEFT JOIN Customer cs       ON cs.Id =  r.CustomerId
  LEFT JOIN Section sc        ON sc.Id =  r.SectionId
  LEFT JOIN ReceiptMemo rm    ON  r.Id = rm.ReceiptId";

        public Task<Receipt> GetReceiptAsync(long Id, CancellationToken token = default(CancellationToken))
        {
            var query = GetSelectQueryForAdvanceReceived() + @"
WHERE
    r.Id = @Id
";
            return dbHelper.ExecuteAsync<Receipt>(query, new { Id }, token);
        }

        public Task<IEnumerable<Receipt>> GetAdvanceReceiptsAsync(long OriginalReceiptId, CancellationToken token = default(CancellationToken))
        {
            var query = GetSelectQueryForAdvanceReceived() + @"
WHERE
    r.OriginalReceiptId = @OriginalReceiptId
";
            return dbHelper.GetItemsAsync<Receipt>(query, new { OriginalReceiptId }, token);
        }

        public async Task<IEnumerable<ReceiptInput>> SaveReceiptInputAsync(IEnumerable<ReceiptInput> ReceiptInput, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO Receipt(
CompanyId
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
, UpdateBy
, UpdateAt
, CollationKey
, BankCode
, BankName
, BranchCode
, BranchName
, AccountTypeId
, AccountNumber
, AccountName
 )
OUTPUT inserted.*
VALUES(
@CompanyId
, @CurrencyId
, @ReceiptHeaderId
, @ReceiptCategoryId
, @CustomerId
, @SectionId
, @InputType
, @Apportioned
, @Approved
, GETDATE()
, @RecordedAt
, @OriginalRecordedAt
, @ReceiptAmount
, @AssignmentAmount
, @RemainAmount
, @AssignmentFlag
, @PayerCode
, @PayerName
, @PayerNameRaw
, @SourceBankName
, @SourceBranchName
, @OutputAt
, @DueAt
, @MailedAt
, @OriginalReceiptId
, @ExcludeFlag
, @ExcludeCategoryId
, @ExcludeAmount
, @ReferenceNumber
, @RecordNumber
, @DensaiRegisterAt
, @Note1
, @Note2
, @Note3
, @Note4
, @BillNumber
, @BillBankCode
, @BillBranchCode
, @BillDrawAt
, @BillDrawer
, @DeleteAt
, @CreateBy
, GETDATE()
, @UpdateBy
, GETDATE()
, @CollationKey
, @BankCode
, @BankName
, @BranchCode
, @BranchName
, @AccountTypeId
, @AccountNumber
, @AccountName
 )";
            var result = new List<ReceiptInput>();
            foreach (var receipt in ReceiptInput)
                result.Add(await dbHelper.ExecuteAsync<ReceiptInput>(query, receipt, token));
            return result;

        }

        public Task<ReceiptInput> UpdateReceiptInputAsync(ReceiptInput receipt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE      Receipt
SET         CurrencyId          = @CurrencyId
          , ReceiptCategoryId   = @ReceiptCategoryId
          , CustomerId          = @CustomerId
          , SectionId           = @SectionId
          , RecordedAt          = @RecordedAt
          , ReceiptAmount       = @ReceiptAmount
          , RemainAmount        = @RemainAmount
          , PayerName           = @PayerName
          , PayerNameRaw        = @PayerNameRaw
          , DueAt               = @DueAt
          , Note1               = @Note1
          , Note2               = @Note2
          , Note3               = @Note3
          , Note4               = @Note4
          , BillNumber          = @BillNumber
          , BillBankCode        = @BillBankCode
          , BillBranchCode      = @BillBranchCode
          , BillDrawAt          = @BillDrawAt
          , BillDrawer          = @BillDrawer
          , UpdateBy            = @UpdateBy
          , UpdateAt            = GETDATE()
OUTPUT      inserted.*
WHERE       Id                  = @Id
AND         UpdateAt            = @UpdateAt";
            return dbHelper.ExecuteAsync<ReceiptInput>(query, receipt, token);
        }


        public Task<ReceiptInput> UpdateCustomerIdAsync(ReceiptInput receipt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE      Receipt
SET         CustomerId          = @CustomerId
          , UpdateBy            = @UpdateBy
          , UpdateAt            = GETDATE()
OUTPUT      inserted.*
WHERE       Id = @Id
AND         UpdateAt            = @UpdateAt";
            return dbHelper.ExecuteAsync<ReceiptInput>(query, receipt, token);
        }



        public Task<int> OmitAsync(int doDelete, int loginUserId, Transaction item, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE          Receipt
SET             DeleteAt    = CASE @doDelete WHEN 1 THEN GETDATE() ELSE NULL END
,               UpdateAt    = GETDATE()
,               UpdateBy    = @loginUserId
WHERE           Id          = @Id
AND             UpdateAt    = @UpdateAt";
            return dbHelper.ExecuteAsync(query, new { doDelete, loginUserId, item.Id, item.UpdateAt }, token);
        }


        public Task<Receipt> UpdateReceiptSectionAsync(int DestinationSectionId, int LoginUserId, long Id, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Receipt
   SET SectionId    = @DestinationSectionId
     , UpdateAt     = GETDATE()
     , UpdateBy     = @LoginUserId
OUTPUT inserted.*
  FROM Receipt
 WHERE Id = @Id
";
            return dbHelper.ExecuteAsync<Receipt>(query, new { DestinationSectionId, LoginUserId, Id }, token);
        }

        public Task<Receipt> UpdateReceiptAmountAsync(decimal DestinationAmount, int LoginUserId, long Id, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Receipt
SET RemainAmount += @DestinationAmount
     , AssignmentFlag = CASE WHEN RemainAmount + @DestinationAmount = ReceiptAmount THEN 0 ELSE 1 END
     , UpdateAt = GETDATE()
     , UpdateBy = @LoginUserId
    OUTPUT inserted.*
    FROM Receipt
WHERE Id =@Id
";
            return dbHelper.ExecuteAsync<Receipt>(query, new { DestinationAmount, LoginUserId, Id }, token);
        }

        public Task<IEnumerable<int>> ReceiptImportDuplicationCheckAsync(int CompanyId, IEnumerable<ReceiptImportDuplication> ReceiptImportDuplication, IEnumerable<ImporterSettingDetail> ImporterSettingDetail,
            CancellationToken token = default(CancellationToken))
        {
            var columns = new List<string>();
            foreach (var import in ImporterSettingDetail.Where(x => x.IsUnique == 1))
            {
                var columnName = import.TargetColumn;
                switch (columnName)
                {
                    case "CustomerCode":        columns.Add("CustomerId");          break;
                    case "CurrencyCode":        columns.Add("CurrencyId");          break;
                    case "SectionCode":         columns.Add("SectionId");           break;
                    case "ReceiptCategoryCode": columns.Add("ReceiptCategoryId");   break;
                    default: columns.Add(columnName); break;
                }
            }
            var query = $@"
SELECT d.RowNumber
  FROM @ReceiptImportDuplication d
 INNER JOIN(
       SELECT DISTINCT {(string.Join(", ", columns.Select(x => $"r.{x}")))}
         FROM Receipt r
        WHERE r.CompanyId   = @CompanyId
          AND r.DeleteAt    IS NULL
       ) r
    ON {(string.Join(" AND ", columns.Select(x => $"d.{x} = r.{x}")))}";

            return dbHelper.GetItemsAsync<int>(query, new {
                                            CompanyId,
                ReceiptImportDuplication =  ReceiptImportDuplication.GetTableParameter(),
            }, token);
        }

        public Task<Receipt> SaveAsync(Receipt Receipt, bool specifyCreateAt = false, CancellationToken token = default(CancellationToken))
        {
            #region query
            var query = $@"
INSERT INTO Receipt
     ( CompanyId
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
     , UpdateBy
     , UpdateAt
     , ProcessingAt
     , StaffId
     , CollationKey
     , BankCode
     , BankName
     , BranchCode
     , BranchName
     , AccountTypeId
     , AccountNumber
     , AccountName
    )
OUTPUT inserted.*
VALUES
     ( @CompanyId
     , @CurrencyId
     , @ReceiptHeaderId
     , @ReceiptCategoryId
     , @CustomerId
     , @SectionId
     , @InputType
     , @Apportioned
     , @Approved
     , @Workday
     , @RecordedAt
     , @OriginalRecordedAt
     , @ReceiptAmount
     , @AssignmentAmount
     , @RemainAmount
     , @AssignmentFlag
     , @PayerCode
     , @PayerName
     , @PayerNameRaw
     , @SourceBankName
     , @SourceBranchName
     , @OutputAt
     , @DueAt
     , @MailedAt
     , @OriginalReceiptId
     , @ExcludeFlag
     , @ExcludeCategoryId
     , @ExcludeAmount
     , @ReferenceNumber
     , @RecordNumber
     , @DensaiRegisterAt
     , @Note1
     , @Note2
     , @Note3
     , @Note4
     , @BillNumber
     , @BillBankCode
     , @BillBranchCode
     , @BillDrawAt
     , @BillDrawer
     , @DeleteAt
     , @CreateBy
     , {(specifyCreateAt ? "@CreateAt" : "GETDATE()")}
     , @UpdateBy
     , GETDATE()
     , @ProcessingAt
     , @StaffId
     , @CollationKey
     , @BankCode
     , @BankName
     , @BranchCode
     , @BranchName
     , @AccountTypeId
     , @AccountNumber
     , @AccountName
    )";
            #endregion
            return dbHelper.ExecuteAsync<Receipt>(query, Receipt, token);
        }

    }
}
