using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Common.Importer.PaymentSchedule;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BillingQueryProcessor :
        IBillingQueryProcessor,
        IAddBillingQueryProcessor,
        IUpdateBillingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        public Task<Billing> UpdateScheduledPaymentAsync(Billing billing, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE b
   SET b.DueAt                  = @DueAt
     , b.OriginalDueAt          = CASE WHEN b.OriginalDueAt IS NULL AND b.DueAt <> @DueAt THEN b.DueAt ELSE b.OriginalDueAt END
     , b.OffsetAmount           = @OffsetAmount
     , b.ScheduledPaymentKey    = @ScheduledPaymentKey
     , b.UpdateBy               = @UpdateBy
     , b.UpdateAt               = GETDATE()
OUTPUT inserted.*
  FROM Billing b
 WHERE b.Id                     = @Id";
            return dbHelper.ExecuteAsync<Billing>(query, billing, token);
        }

        public Task<IEnumerable<Billing>> UpdateScheduledPaymentForDueAtAsync(Billing billing, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE b
   SET b.DueAt                  = @DueAt
     , b.OriginalDueAt          = CASE WHEN b.OriginalDueAt IS NULL AND b.DueAt <> @DueAt THEN b.DueAt ELSE b.OriginalDueAt END
     , b.UpdateBy               = @UpdateBy
     , b.UpdateAt               = GETDATE()
OUTPUT inserted.*
  FROM Billing b
 WHERE b.BillingInputId         = @BillingInputId";
            return dbHelper.GetItemsAsync<Billing>(query, billing, token);
        }

        public Task<IEnumerable<Billing>> GetDataForScheduledPaymentAsync(ScheduledPaymentImport scheduledPayment,
            IEnumerable<ImporterSettingDetail> details, CancellationToken token = default(CancellationToken))
        {
            var query = new StringBuilder(@"
SELECT b.*, cg.ParentCustomerId
  FROM Billing AS b
  LEFT JOIN CustomerGroup AS cg             ON cg.ChildCustomerId = b.CustomerId
 WHERE b.CompanyId              = @CompanyId
   AND b.DeleteAt               IS NULL");
            CreateSchedulePaymentCondition(scheduledPayment, details, query);
            var order = CreateSchedulePaymentOrder(details);
            if (order != "")
            {
                query.Append($@"
 ORDER BY {order}");
            }
            return dbHelper.GetItemsAsync<Billing>(query.ToString(), scheduledPayment, token);
        }

        private void CreateSchedulePaymentCondition(ScheduledPaymentImport billing,
            IEnumerable<ImporterSettingDetail> details, StringBuilder builder)
        {
            foreach (var data in details.Where(d => d.ImportDivision == 1))
            {
                if (data.Sequence == (int)Fields.ParentCustomerCode) builder.Append(@"
   AND COALESCE(cg.ParentCustomerId, b.CustomerId) = @ParentCustomerId");
                else if (data.Sequence == (int)Fields.CustomerCode) builder.Append(@"
   AND b.CustomerId             = @CustomerId");
                else if (data.Sequence == (int)Fields.BilledAt) builder.Append(@"
   AND b.BilledAt               = @BilledAt");
                else if (data.Sequence == (int)Fields.BillingAmount) builder.Append(@"
   AND b.BillingAmount          = @BillingAmount");
                else if (data.Sequence == (int)Fields.DueAt) builder.Append(@"
   AND b.DueAt                  = @DueAt");
                else if (data.Sequence == (int)Fields.DepartmentCode) builder.Append(@"
   AND b.DepartmentId           = @DepartmentId");
                else if (data.Sequence == (int)Fields.DebitAccountTitleCode) builder.Append(@"
   AND b.DebitAccountTitleId    = @DebitAccountTitleId");
                else if (data.Sequence == (int)Fields.SalesAt) builder.Append(@"
   AND b.SalesAt                = @SalesAt");
                else if (data.Sequence == (int)Fields.InvoiceCode) builder.Append(@"
   AND b.InvoiceCode            = @InvoiceCode");
                else if (data.Sequence == (int)Fields.ClosingAt) builder.Append(@"
   AND b.ClosingAt              = @ClosingAt");
                else if (data.Sequence == (int)Fields.Note1) builder.Append(@"
   AND b.Note1                  = @Note1");
                else if (data.Sequence == (int)Fields.BillingCategoryCode) builder.Append(@"
   AND b.BillingCategoryId      = @BillingCategoryId");
                else if (data.Sequence == (int)Fields.Note2) builder.Append(@"
   AND b.Note2                  = @Note2");
                else if (data.Sequence == (int)Fields.Note3) builder.Append(@"
   AND b.Note3                  = @Note3");
                else if (data.Sequence == (int)Fields.Note4) builder.Append(@"
   AND b.Note4                  = @Note4");
                else if (data.Sequence == (int)Fields.Note5) builder.Append(@"
   AND b.Note5                  = @Note5");
                else if (data.Sequence == (int)Fields.Note6) builder.Append(@"
   AND b.Note6                  = @Note6");
                else if (data.Sequence == (int)Fields.Note7) builder.Append(@"
   AND b.Note7                  = @Note7");
                else if (data.Sequence == (int)Fields.Note8) builder.Append(@"
   AND b.Note8                  = @Note8");
                else if (data.Sequence == (int)Fields.CurrencyCode) builder.Append(@"
   AND b.CurrencyId             = @CurrencyId");
            }
            if (billing.AssignmentFlag == 0)
                builder.Append(@"
   AND b.AssignmentFlag = 0");
            else
                builder.Append(@"
   AND (b.AssignmentFlag = 0 OR b.AssignmentFlag = 1)");
        }

        private string CreateSchedulePaymentOrder(IEnumerable<ImporterSettingDetail> details)
            => string.Join($"{Environment.NewLine}       , ",
                details.Where(x => x.ImportDivision == 0 && x.FieldIndex != 0 && (int)Fields.CompanyCode <= x.Sequence && x.Sequence <= (int)Fields.CurrencyCode)
                .OrderBy(x => x.FieldIndex).ThenBy(x => x.Sequence)
                .Select(x => $"[{ConvertImporterSettingDetailToColumnName(x)}] {(x.ItemPriority == 0 ? "ASC" : "DESC")}"));

        /// <summary>
        /// 取込設定 明細から 実カラムへの変換
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private string ConvertImporterSettingDetailToColumnName(ImporterSettingDetail detail)
            => detail.Sequence == (int)Fields.CompanyCode           ? "CompanyId"
             : detail.Sequence == (int)Fields.ParentCustomerCode    ? "ParentCustomerId"
             : detail.Sequence == (int)Fields.CustomerCode          ? "CustomerId"
             : detail.Sequence == (int)Fields.BilledAt              ? "BilledAt"
             : detail.Sequence == (int)Fields.BillingAmount         ? "BillingAmount"
             : detail.Sequence == (int)Fields.TaxAmount             ? "TaxAmount"
             : detail.Sequence == (int)Fields.DueAt                 ? "DueAt"
             : detail.Sequence == (int)Fields.DepartmentCode        ? "DepartmentId"
             : detail.Sequence == (int)Fields.DebitAccountTitleCode ? "DebitAccountTitleId"
             : detail.Sequence == (int)Fields.SalesAt               ? "SalesAt"
             : detail.Sequence == (int)Fields.InvoiceCode           ? "InvoiceCode"
             : detail.Sequence == (int)Fields.ClosingAt             ? "ClosingAt"
             : detail.Sequence == (int)Fields.StaffCode             ? "StaffId"
             : detail.Sequence == (int)Fields.Note1                 ? "Note1"
             : detail.Sequence == (int)Fields.BillingCategoryCode   ? "BillingCategoryId"
             : detail.Sequence == (int)Fields.Note2                 ? "Note2"
             : detail.Sequence == (int)Fields.Note3                 ? "Note3"
             : detail.Sequence == (int)Fields.Note4                 ? "Note4"
             : detail.Sequence == (int)Fields.Note5                 ? "Note5"
             : detail.Sequence == (int)Fields.Note6                 ? "Note6"
             : detail.Sequence == (int)Fields.Note7                 ? "Note7"
             : detail.Sequence == (int)Fields.Note8                 ? "Note8"
             : detail.Sequence == (int)Fields.CurrencyCode          ? "CurrencyId"
             : "";

        public Task<int> UpdateForScheduledPaymentAsync(Billing Billing, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Billing
   SET OffsetAmount         = @OffsetAmount
     , ScheduledPaymentKey  = @ScheduledPaymentKey
     , UpdateBy             = @UpdateBy
     , UpdateAt             = GETDATE()
WHERE
       Id                   = @Id
   AND CompanyId            = @CompanyId";
            return dbHelper.ExecuteAsync(query, Billing, token);
        }

        public Task<int> UpdateForScheduledPaymentSameCustomersAsync(int CompanyId, int UpdateBy, int updateAll,
            IEnumerable<long> targetBillingIds, IEnumerable<int> targetBillingCustomerIds, CancellationToken token = default(CancellationToken))
        {

            var query = $@"
UPDATE b
   SET b.OffsetAmount   = b.RemainAmount
     , UpdateBy         = @UpdateBy
     , UpdateAt         = GETDATE()
  FROM Billing b
  LEFT JOIN CustomerGroup csg
    ON csg.ChildCustomerId = b.CustomerId
 WHERE b.CompanyId = @CompanyId
   AND b.Id NOT IN (SELECT Id FROM @targetBillingIds)
   AND COALESCE(csg.ParentCustomerId, b.CustomerId) IN (SELECT Id FROM @targetBillingCustomerIds)
   AND b.AssignmentFlag IN ({(updateAll == 0 ? "0" : "0, 1")})";
            return dbHelper.ExecuteAsync(query, new {
                CompanyId,
                UpdateBy,
                targetBillingIds = targetBillingIds.GetTableParameter(),
                targetBillingCustomerIds = targetBillingCustomerIds.GetTableParameter(),
            }, token);
        }




        public Task<int> OmitAsync(int doDelete, int loginUserId, Transaction item, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE      Billing
SET         DeleteAt    = CASE @doDelete WHEN 1 THEN GETDATE() ELSE NULL END
,           UpdateAt    = GETDATE()
,           UpdateBy    = @loginUserId
WHERE       Id          = @Id
AND         UpdateAt    = @UpdateAt";
            return dbHelper.ExecuteAsync(query, new { doDelete, loginUserId, item.Id, item.UpdateAt }, token);
        }

        public Task<IEnumerable<Billing>> GetItemsForScheduledPaymentImportAsync(ScheduledPaymentImport schedulePayment,
            IEnumerable<ImporterSettingDetail> importerSettingDetails, CancellationToken token = default(CancellationToken))
        {
            var query = new StringBuilder(@"
SELECT b.*, cg.ParentCustomerId
  FROM Billing AS b
  LEFT JOIN CustomerGroup AS cg         ON cg.ChildCustomerId = b.CustomerId
 WHERE b.CompanyId              = @CompanyId");
            CreateSchedulePaymentCondition(schedulePayment, importerSettingDetails, query);
            return dbHelper.GetItemsAsync<Billing>(query.ToString(), schedulePayment, token);
        }


        public Task<int> UpdateDeleteAtAsync(IEnumerable<long> ids, int loginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Billing
   SET DeleteAt = GETDATE()
     , UpdateBy = @loginUserId
     , UpdateAt = GETDATE()
 WHERE Id IN (SELECT Id FROM @Ids)";
            return dbHelper.ExecuteAsync(query, new { loginUserId, Ids = ids.GetTableParameter() }, token);
        }


        public Task<IEnumerable<int>> BillingImportDuplicationCheckAsync(int CompanyId, IEnumerable<BillingImportDuplicationWithCode> BillingImportDuplication,
            IEnumerable<ImporterSettingDetail> ImporterSettingDetail, CancellationToken token = default(CancellationToken))
        {
            var columns = ImporterSettingDetail.Select(import => {
                var column = import.TargetColumn;
                switch (column)
                {
                    case "CustomerCode"         : column = "CustomerId";             break;
                    case "DepartmentCode"       : column = "DepartmentId";           break;
                    case "DebitAccountTitleCode": column = "DebitAccountTitleId";    break;
                    case "StaffCode"            : column = "StaffId";                break;
                    case "BillingCategoryCode"  : column = "BillingCategoryId";      break;
                    case "CollectCategoryCode"  : column = "CollectCategoryId";      break;
                    case "CurrencyCode"         : column = "CurrencyId";             break;
                }
                return column;
            }).ToList();

            var query = $@"
SELECT d.RowNumber
  FROM @BillingImportDuplication d
 INNER JOIN (
       SELECT DISTINCT
              {(string.Join(",", columns))}
         FROM Billing
        WHERE CompanyId     = @CompanyId
          AND DeleteAt      IS NULL
       ) b
    ON {(string.Join((Environment.NewLine + "   AND "), columns.Select(x => $"d.{x} = b.{x}")))}";
            return dbHelper.GetItemsAsync<int>(query,new { CompanyId, BillingImportDuplication = BillingImportDuplication.GetTableParameter() }, token);
        }

        /// <summary>
        /// 口座振替結果データ取込：取込データの照合対象(候補)を取得する
        /// </summary>
        /// <param name="PaymentAgencyId"></param>
        /// <param name="TransferDate"></param>
        /// <returns></returns>
        public Task<IEnumerable<Billing>> GetAccountTransferMatchingTargetListAsync(int PaymentAgencyId, DateTime TransferDate, int CurrencyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        b.*
            , cs.Code   [CustomerCode]
            , cs.Name   [CustomerName]
            , dp.Code   [DepartmentCode]
            , dp.Name   [DepartmentName]
FROM        Billing b
LEFT JOIN   Category cc         ON cc.Id    = b.CollectCategoryId
LEFT JOIN   Currency cy         ON cy.Id    = b.CurrencyId
LEFT JOIN   Customer cs         ON cs.Id    = b.CustomerId
LEFT JOIN   Department dp       ON dp.Id    = b.DepartmentId
WHERE       cc.PaymentAgencyId  = @PaymentAgencyId
AND         b.DueAt             = @TransferDate
AND         b.CurrencyId        = @CurrencyId
AND         b.AssignmentFlag    = 0
AND         b.DeleteAt          IS NULL
AND         (b.ResultCode IS NULL OR b.ResultCode <> 0)
";
            return dbHelper.GetItemsAsync<Billing>(query, new {
                PaymentAgencyId,
                TransferDate,
                CurrencyId,
            });
        }

        /// <summary>
        /// 口座振替結果データ取込：照合の済んだ取込データを登録する
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ResultCode"></param>
        /// <param name="DueDateOffset">
        /// resultCodeが0の場合に更新する。
        /// resultCodeが0以外の場合はnullを指定してもよいが、0の場合にnullだと例外スロー。
        /// </param>
        /// <param name="DueDateOffset">
        /// resultCodeが0以外、且つこの値がnull以外の場合に更新する。
        /// </param>
        /// <returns></returns>
        public Task<Billing> UpdateForAccountTransferImportAsync(long Id,
            AccountTransferImportData data, CancellationToken token = default(CancellationToken))
        {
            var builder = new StringBuilder();
            builder.AppendLine(@"
UPDATE
    Billing
SET
    ResultCode = @ResultCode");

            // 振替済の場合
            if (data.ResultCode == 0)
            {
                if (data.DueDateOffset == null)
                {
                    throw new ArgumentNullException(nameof(data.DueDateOffset));
                }
                else
                {
                    builder.AppendLine("  , DueAt = DATEADD(DAY, @DueDateOffset, DueAt)");
                }
            }
            // 振替不能の場合
            else
            {
                if (data.CollectCategoryId.HasValue)
                {
                    builder.AppendLine("  , CollectCategoryId = @CollectCategoryId");
                }
               
                if (data.DoUpdateAccountTransferLogId)
                {
                    //依頼データの抽出対象とするためNULLに戻す
                    builder.AppendLine("  , AccountTransferLogId = NULL");
                }
                else
                {
                    //ゆうちょ形式専用
                    if (!data.CollectCategoryId.HasValue)
                    {
                        if (!data.DueDate.HasValue)
                            throw new ArgumentNullException(nameof(data.DueDate));

                        //入金予定日を再振替日に更新する
                        builder.AppendLine("  , DueAt = @DueDate");
                    }
                }
            }

            builder.AppendLine(@"
  , UpdateBy    = @UpdateBy
  , UpdateAt    = GETDATE()
OUTPUT
    INSERTED.*
WHERE
    Id = @Id
");

            return dbHelper.ExecuteAsync<Billing>(builder.ToString(), new {
                Id,
                data.ResultCode,
                data.DueDateOffset,
                data.CollectCategoryId,
                data.DueDate,
                data.UpdateBy,
            }, token);
        }


        public Task<Billing> UpdateForResetInvoiceCodeAsync(IEnumerable<long> BillingInputIds, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE      b
SET         b.InvoiceCode       = N''
OUTPUT      INSERTED.*
FROM        Billing b
INNER JOIN  BillingInput bi
ON          bi.Id               = b.BillingInputId
WHERE       bi.Id               IN (SELECT Id FROM @BillingInputIds)
AND         bi.UseInvoiceCodeNumbering = 1
";
            return dbHelper.ExecuteAsync<Billing>(query, new{ BillingInputIds = BillingInputIds.GetTableParameter() }, token);
        }

        public Task<Billing> UpdateForResetInputIdAsync(IEnumerable<long> BillingInputIds, CancellationToken token = default(CancellationToken))
        {
            var query = @"
Update Billing
SET BillingInputId = NULL
OUTPUT INSERTED.*
FROM Billing b
WHERE b.BillingInputId IN (SELECT Id FROM @BillingInputIds)
  AND b.InputType = 1
";
            return dbHelper.ExecuteAsync<Billing>(query, new { BillingInputIds = BillingInputIds.GetTableParameter() }, token);
        }
        public Task<IEnumerable<Billing>> UpdateForPublishAsync(BillingInvoiceForPublish billingInvoiceForPublish,
            bool doUpdateInvoiceCode, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
Update Billing
SET DestinationId = @DestinationId
{(doUpdateInvoiceCode ? "  , InvoiceCode   = @InvoiceCode" : string.Empty)}
OUTPUT INSERTED.*
FROM Billing b
WHERE b.BillingInputId = @BillingInputId
";
            return  dbHelper.GetItemsAsync<Billing>(query, billingInvoiceForPublish, token);
        }

        public Task<IEnumerable<Billing>> GetByBillingInputIdAsync(long BillingInputId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM Billing
 WHERE BillingInputId = @BillingInputId";
            return dbHelper.GetItemsAsync<Billing>(query, new { BillingInputId }, token);
        }

        public Task<Billing> UpdateAsync(Billing Billing, CancellationToken token = default(CancellationToken))
        {
            #region update query
            var query = @"
UPDATE Billing
SET
  CurrencyId                = @CurrencyId
, CustomerId                = @CustomerId
, DepartmentId              = @DepartmentId
, StaffId                   = @StaffId
, BillingCategoryId         = @BillingCategoryId
, BilledAt                  = @BilledAt
, ClosingAt                 = @ClosingAt
, SalesAt                   = @SalesAt
, DueAt                     = @DueAt
, BillingAmount             = @BillingAmount
, TaxAmount                 = @TaxAmount
, AssignmentAmount          = 0
, RemainAmount              = @BillingAmount
, OffsetAmount              = 0
, CollectCategoryId         = @CollectCategoryId
, OriginalCollectCategoryId = @OriginalCollectCategoryId
, DebitAccountTitleId       = @DebitAccountTitleId
, CreditAccountTitleId      = @CreditAccountTitleId
, OriginalDueAt             = @OriginalDueAt
, InvoiceCode               = @InvoiceCode
, TaxClassId                = @TaxClassId
, Note1                     = @Note1
, Note2                     = @Note2
, Note3                     = @Note3
, Note4                     = @Note4
, Note5                     = @Note5
, Note6                     = @Note6
, Note7                     = @Note7
, Note8                     = @Note8
, Quantity                  = @Quantity
, UnitPrice                 = @UnitPrice
, UnitSymbol                = @UnitSymbol
, Price                     = @Price
, DestinationId             = @DestinationId
, UpdateBy                  = @UpdateBy
, UpdateAt                  = GETDATE()
OUTPUT inserted.*
WHERE Id = @Id";
            #endregion
            return dbHelper.ExecuteAsync<Billing>(query, Billing, token);
        }

        public Task<Billing> AddAsync(Billing Billing, CancellationToken token = default(CancellationToken))
        {
            #region insert query
            var query = @"
INSERT INTO Billing(
  CompanyId
, CurrencyId
, CustomerId
, DepartmentId
, StaffId
, BillingCategoryId
, InputType
, BillingInputId
, BilledAt
, ClosingAt
, SalesAt
, DueAt
, BillingAmount
, TaxAmount
, AssignmentAmount
, RemainAmount
, OffsetAmount
, AssignmentFlag
, Approved 
, CollectCategoryId
, OriginalCollectCategoryId
, DebitAccountTitleId
, CreditAccountTitleId
, OriginalDueAt
, OutputAt
, PublishAt
, InvoiceCode
, TaxClassId
, Note1
, Note2
, Note3
, Note4
, Note5
, Note6
, Note7
, Note8
, DeleteAt
, RequestDate
, ResultCode
, TransferOriginalDueAt
, ScheduledPaymentKey
, Quantity
, UnitPrice
, UnitSymbol
, Price
, DestinationId
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
) OUTPUT inserted.*
VALUES (
  @CompanyId
, @CurrencyId
, @CustomerId
, @DepartmentId
, @StaffId
, @BillingCategoryId
, @InputType
, @BillingInputId
, @BilledAt
, @ClosingAt
, @SalesAt
, @DueAt
, @BillingAmount
, @TaxAmount
, 0
, @BillingAmount
, 0
, 0
, 1
, @CollectCategoryId
, NULL
, @DebitAccountTitleId
, @CreditAccountTitleId
, NULL
, NULL
, NULL
, @InvoiceCode
, @TaxClassId
, @Note1
, @Note2
, @Note3
, @Note4
, @Note5
, @Note6
, @Note7
, @Note8
, NULL
, NULL
, NULL
, NULL
, ''
, @Quantity
, @UnitPrice
, @UnitSymbol
, @Price
, @DestinationId
, @CreateBy
, GETDATE()
, @UpdateBy
, GETDATE()
) ";
            #endregion
            return dbHelper.ExecuteAsync<Billing>(query, Billing, token);
        }
    }
}
