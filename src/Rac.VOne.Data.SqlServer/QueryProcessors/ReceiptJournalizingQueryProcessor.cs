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
    public class ReceiptJournalizingQueryProcessor :
        IReceiptJournalizingQueryProcessor,
        IReceiptGeneralJournalizingQueryProcessor,
        IUpdateReceiptJournalizingQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public ReceiptJournalizingQueryProcessor(
            IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT r.OutputAt
     , Count(*) [Count]
     , ccy.Code [CurrencyCode]
     , SUM( r.ReceiptAmount ) [Amount]
     , MAX( r.UpdateAt      ) [UpdateAt]
  FROM Receipt r
 INNER JOIN Currency ccy
    ON ccy.Id                   = r.CurrencyId
   AND r.CompanyId              = @CompanyId
   AND r.CurrencyId             = @CurrencyId
   AND r.Apportioned            = 1
   AND r.Approved               = 1
   AND r.DeleteAt               IS NULL";
            if (!option.IsOutputted) query += @"
   AND r.OutputAt               IS NULL ";
            else query += @"
   AND r.OutputAt               IS NOT NULL ";
            if (option.RecordedAtFrom.HasValue) query += @"
   AND r.RecordedAt             >= @RecordedAtFrom ";
            if (option.RecordedAtTo.HasValue)   query += @"
   AND r.RecordedAt             <= @RecordedAtTo ";
            query += @"
 INNER JOIN Category ct
    ON ct.Id                    =  r.ReceiptCategoryId
   AND ct.UseAdvanceReceived    = 0
 GROUP BY r.OutputAt, ccy.Code
 ORDER BY r.OutputAt DESC
";
            return dbHelper.GetItemsAsync<JournalizingSummary>(query, option, token);
        }

        public Task<IEnumerable<ReceiptJournalizing>> ExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            #region query
            var query = @"
DECLARE
  @SuspenceReceiptDepartmentCode        nvarchar(10) --仮受部門コード
, @SuspenceReceiptAccountTitleCode      nvarchar(10) --仮受科目コード
, @SuspenceReceiptSubCode               nvarchar(10) --仮受補助コード
, @ReceiptDepartmentCode                nvarchar(10) --入金部門コード

SET @SuspenceReceiptDepartmentCode        = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = '仮受部門コード')
SET @SuspenceReceiptAccountTitleCode      = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = '仮受科目コード')
SET @SuspenceReceiptSubCode               = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = '仮受補助コード')
SET @ReceiptDepartmentCode                = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = '入金部門コード')

SELECT cm.Code [CompanyCode]
     , r.RecordedAt
     , r.Id [SlipNumber]
     , @ReceiptDepartmentCode    [DebitDepartmentCode]
     , rd.Name [DebitDepartmentName]
     , rat.Code    [DebitAccountTitleCode]
     , rat.Name    [DebitAccountTitleName]
     , ct.SubCode  [DebitSubCode]
     , '' [DebitSubName]
     , @SuspenceReceiptDepartmentCode   [CreditDepartmentCode]
     , sd.Name                          [CreditDepartmentName]
     , @SuspenceReceiptAccountTitleCode [CreditAccountTitleCode]
     , sat.Name                         [CreditAccountTitleName]
     , @SuspenceReceiptSubCode          [CreditSubCode]
     , ''                               [CreditSubName]
     , r.ReceiptAmount                  [Amount]
     , r.Note1                          [Note]
     , cs.Code                          [CustomerCode]
     , cs.Name                          [CustomerName]
     , ''                               [InvoiceCode]
     , ''                               [StaffCode]
     , r.PayerCode
     , r.PayerName
     , r.SourceBankName
     , r.SourceBranchName
     , r.DueAt
     , r.BankCode
     , r.BankName
     , r.BranchCode
     , r.BranchName
     , r.AccountTypeId
     , r.AccountNumber
     , ccy.Code                         [CurrencyCode]
     , r.UpdateAt
  FROM Receipt r
 INNER JOIN Category ct
    ON ct.Id                    = r.ReceiptCategoryId
   AND r.CompanyId              = @CompanyId
   AND r.Apportioned            = 1
   AND r.Approved               = 1
   AND r.DeleteAt               IS NULL
   AND ct.UseAdvanceReceived    = 0";
            if (option.RecordedAtFrom.HasValue) query += @"
   AND r.RecordedAt             >= @RecordedAtFrom ";
            if (option.RecordedAtTo.HasValue)   query += @"
   AND r.RecordedAt             <= @RecordedAtTo ";
            if (option.CurrencyId != 0)         query += @"
   AND r.CurrencyId             = @CurrencyId ";
            if (!(option.OutputAt.Any()))       query += @"
   AND r.OutputAt               IS NULL  ";
            else query += @"
   AND r.OutputAt               IN @OutputAt ";
            query += @"
 INNER JOIN Company cm              ON cm.Id            = r.CompanyId
 INNER JOIN Currency ccy            ON ccy.Id           = r.CurrencyId
  LEFT JOIN ReceiptHeader rh        ON rh.Id            = r.ReceiptHeaderId
  LEFT JOIN AccountTitle rat        ON rat.Id           = ct.AccountTitleId
  LEFT JOIN Department rd           ON rd.CompanyId     = r.CompanyId
                                   AND rd.Code          = @ReceiptDepartmentCode
  LEFT JOIN Department sd           ON sd.CompanyId     = r.CompanyId
                                   AND sd.Code          = @SuspenceReceiptDepartmentCode
  LEFT JOIN AccountTitle sat        ON sat.CompanyId    = r.CompanyId
                                   AND sat.Code         = @SuspenceReceiptAccountTitleCode
  LEFT JOIN Customer cs             ON r.CustomerId = cs.Id
 ORDER BY RecordedAt, SlipNumber
";
            #endregion
            return dbHelper.GetItemsAsync<ReceiptJournalizing>(query, option, token);
        }
        public Task<IEnumerable<GeneralJournalizing>> ExtractGeneralJournalizingAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            #region query 0 : receipt journalizing
            var query = @"
SELECT 0 [JournalizingType]
     , r.CompanyId
     , r.CurrencyId
     , ccy.[Code]           [CurrencyCode]
     , ccy.[Precision]
     , r.ReceiptAmount      [Amount]
     , r.Approved
     , r.CreateAt
     , r.Id                 [ReceiptId]
     , null                 [BillingId]
     , null                 [MatchingId]
     , null                 [ScheduledIncomeReceiptId]
     , null                 [ScheduledIncomeBillingId]
     , null                 [ScheduledIncomeMatchingHeaderId]
     , null                 [AdvanceReceivedBackupId]
     , null                 [ReceiptExcludeId]
     , rcs.Id               [ReceiptCustomerId]
     , rpcs.Id              [ReceiptParentCustomerId]
     , null                 [BillingCustomerId]
     , null                 [BillingParentCustomerId]
     , null                 [BillingDepartmentId]
     , null                 [BillingDepartmentStaffId]
     , null                 [BillingStaffId]
     , null                 [BillingStaffDepartmentId]
  FROM Receipt r
 INNER JOIN Category rct                ON rct.Id           = r.ReceiptCategoryId
                                       AND r.CompanyId      = @CompanyId
                                       AND r.CurrencyId     = @CurrencyId
                                       AND r.Apportioned    = 1
                                       AND r.DeleteAt       IS NULL
                                       AND rct.UseAdvanceReceived = 0
  LEFT JOIN ReceiptHeader rh            ON rh.Id            = r.ReceiptHeaderId
 INNER JOIN Currency ccy                ON ccy.Id           = r.CurrencyId
  LEFT JOIN Customer rcs                ON rcs.Id           = r.CustomerId
  LEFT JOIN CustomerGroup rcsg          ON rcs.Id           = rcsg.ChildCustomerId
  LEFT JOIN Customer rpcs               ON rpcs.Id          = rcsg.ParentCustomerId
 WHERE r.Id             = r.Id";
            if (option.RecordedAtFrom.HasValue) query += @"
   AND r.RecordedAt    >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue) query += @"
   AND r.RecordedAt    <= @RecordedAtTo";
            if (!(option.OutputAt?.Any() ?? false)) query += @"
   AND r.OutputAt      IS NULL";
            else query += @"
   AND r.OutputAt      IN @OutputAt";
            query += @"
 ORDER BY CreateAt, ReceiptId";
            #endregion
            return dbHelper.GetItemsAsync<GeneralJournalizing>(query, option, token);
        }

        public Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE r
   SET r.OutputAt = GETDATE()
     , r.UpdateAt = GETDATE()
     , r.UpdateBy = @LoginUserId
  FROM Receipt r
 INNER JOIN Category ct
    ON ct.Id                    = r.ReceiptCategoryId
   AND r.CompanyId              = @CompanyId
   AND r.OutputAt               IS NULL
   AND r.Apportioned            = 1
   AND r.Approved               = 1
   AND r.DeleteAt               IS NULL
   AND ct.UseAdvanceReceived    = 0";
            if (option.RecordedAtFrom.HasValue) query += @"
   AND r.RecordedAt >= @RecordedAtFrom ";
            if (option.RecordedAtTo.HasValue) query += @"
   AND r.RecordedAt <= @RecordedAtTo ";
            if (option.CurrencyId != 0) query += @"
   AND r.CurrencyId = @CurrencyId ";
            return dbHelper.ExecuteAsync(query, option, token);
        }
        public Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE r
   SET r.OutputAt = NULL
     , r.UpdateAt = GETDATE()
     , r.UpdateBy = @LoginUserId
  FROM Receipt r 
 INNER JOIN Category ct
    ON ct.Id                    = r.ReceiptCategoryId
   AND r.CompanyId              = @CompanyId
   AND r.OutputAt               IN @OutputAt
   AND r.Apportioned            = 1
   AND r.Approved               = 1
   AND r.DeleteAt               IS NULL
   AND ct.UseAdvanceReceived = 0 ";
            if (option.CurrencyId != 0) query += @"
   AND r.CurrencyId             = @CurrencyId";
            return dbHelper.ExecuteAsync(query, option, token);
        }

    }
}
