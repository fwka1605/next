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
    public class MatchingJournalizingQueryProcessor :
        IMatchingJournalizingQueryProcessor,
        IMatchingJournalizingSummaryQueryProcessor,
        IMatchingGeneralJournalizingQueryProcessor,
        IMatchedReceiptQueryProcessor,
        IUpdateMatchingJournalizingQueryProcessor,
        IMatchingJournalizingDetailQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public MatchingJournalizingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            #region query

            var matchingHeaderCondition = "";
            var receiptCondition = "";
            var receiptExcludeCondition = "";

            if (option.IsOutputted)
            {
                matchingHeaderCondition += @" AND m.OutputAt  IS NOT NULL";
                receiptCondition += @" AND r.OutputAt  IS NOT NULL";
                receiptExcludeCondition += @" AND re.OutputAt IS NOT NULL";
            }
            else
            {
                matchingHeaderCondition += @" AND m.OutputAt  IS NULL";
                receiptCondition += @" AND r.OutputAt  IS NULL AND r.DeleteAt IS NULL";
                receiptExcludeCondition += @" AND re.OutputAt IS NULL";
            }

            if (option.RecordedAtFrom.HasValue)
            {
                matchingHeaderCondition += @" AND m.RecordedAt >= @RecordedAtFrom";
                receiptCondition += @" AND r.RecordedAt >= @RecordedAtFrom";
                receiptExcludeCondition += @" AND r.RecordedAt >= @RecordedAtFrom";

            }
            if (option.RecordedAtTo.HasValue)
            {
                matchingHeaderCondition += @" AND m.RecordedAt <= @RecordedAtTo";
                receiptCondition += @" AND r.RecordedAt <= @RecordedAtTo";
                receiptExcludeCondition += @" AND r.RecordedAt <= @RecordedAtTo";
            }
            var query = $@"
SELECT OutputAt
     , COUNT(*)           [Count]
     , ccy.Code           [CurrencyCode]
     , SUM( Amount   )    [Amount]
     , MAX( t.UpdateAt )  [UpdateAt]
FROM (
      SELECT m.OutputAt
           , mh.CurrencyId
           , m.Amount
           , m.UpdateAt
        FROM MatchingHeader mh
       INNER JOIN Matching m
          ON mh.Id            = m.MatchingHeaderId
         AND mh.CompanyId     = @CompanyId
         AND mh.CurrencyId    = @CurrencyId
         AND mh.Approved      = 1
{matchingHeaderCondition}";
            if (!option.IsGeneral) query += $@"
      UNION ALL
      SELECT m.OutputAt
           , mh.CurrencyId
           , m.BankTransferFee [Amount]
           , m.UpdateAt
        FROM MatchingHeader mh
       INNER JOIN Matching m
          ON mh.Id            = m.MatchingHeaderId
         AND mh.CompanyId     = @CompanyId
         AND mh.CurrencyId    = @CurrencyId
         AND mh.Approved      = 1
         AND m.BankTransferFee <> 0
{matchingHeaderCondition}
      UNION ALL
      SELECT m.OutputAt
           , mh.CurrencyId
           , ABS(m.TaxDifference) [Amount]
           , m.UpdateAt
        FROM MatchingHeader mh
       INNER JOIN Matching m
          ON mh.Id            = m.MatchingHeaderId
         AND mh.CompanyId     = @CompanyId
         AND mh.CurrencyId    = @CurrencyId
         AND mh.Approved      = 1
         AND m.TaxDifference <> 0
{matchingHeaderCondition}";
            if (!option.IsGeneral && option.UseDiscount) query += $@"
      UNION ALL
      SELECT m.OutputAt
           , mh.CurrencyId
           , mbd.DiscountAmount [Amount]
           , m.UpdateAt
        FROM MatchingHeader mh
       INNER JOIN Matching m
          ON mh.Id            = m.MatchingHeaderId
         AND mh.CompanyId     = @CompanyId
         AND mh.CurrencyId    = @CurrencyId
         AND mh.Approved      = 1
{matchingHeaderCondition}
       INNER JOIN MatchingBillingDiscount mbd
          ON m.Id             = mbd.MatchingId";
            query += $@"
      UNION ALL
      SELECT r.OutputAt
           , r.CurrencyId
           , r.ReceiptAmount [Amount]
           , r.UpdateAt
        FROM Receipt r
       INNER JOIN Category ct
          ON ct.Id          = r.ReceiptCategoryId
         AND r.CompanyId    = @CompanyId
         AND r.CurrencyId   = @CurrencyId
         AND ct.UseAdvanceReceived = 1
         AND r.Apportioned  = 1
         AND r.Approved     = 1
         AND r.DeleteAt     IS NULL
{receiptCondition}
      UNION ALL
      SELECT re.OutputAt
           , r.CurrencyId
           , re.ExcludeAmount [Amount]
           , re.UpdateAt
        FROM Receipt r
       INNER JOIN ReceiptExclude re
          ON r.Id           = re.ReceiptId
         AND r.CompanyId    = @CompanyId
         AND r.CurrencyId   = @CurrencyId
         AND r.Apportioned  = 1
         AND r.Approved     = 1
         AND r.DeleteAt     IS NULL
{receiptExcludeCondition}
       ) t
 INNER JOIN Currency ccy
    ON ccy.Id       = t.CurrencyId
 GROUP BY OutputAt, ccy.Code
 ORDER BY OutputAt DESC";
            #endregion
            return dbHelper.GetItemsAsync<JournalizingSummary>(query, option, token);
        }
        public Task<IEnumerable<MatchingJournalizing>> ExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            #region query
            var matchingCondition = "";
            var receiptCondition = "";
            var excludeCondition = "";

            //未出力時
            if (option.OutputAt == null || !option.OutputAt.Any())
            {
                matchingCondition = @" AND m.OutputAt IS NULL";
                receiptCondition = @" AND r.OutputAt IS NULL";
                excludeCondition = @" AND re.OutputAt IS NULL";
            }
            else
            {
                //再出力
                matchingCondition = @" AND m.OutputAt IN @OutputAt";
                receiptCondition = @" AND r.OutputAt IN @OutputAt";
                excludeCondition = @" AND re.OutputAt IN @OutputAt";
            }

            if (option.RecordedAtFrom.HasValue)
            {
                matchingCondition += @" AND m.RecordedAt >= @RecordedAtFrom";
                receiptCondition += @" AND r.RecordedAt >= @RecordedAtFrom";
                excludeCondition += @" AND r.RecordedAt >= @RecordedAtFrom";

            }
            if (option.RecordedAtTo.HasValue)
            {
                matchingCondition += @" AND m.RecordedAt <= @RecordedAtTo";
                receiptCondition += @" AND r.RecordedAt <= @RecordedAtTo";
                excludeCondition += @" AND r.RecordedAt <= @RecordedAtTo";
            }
            var query = $@"
DECLARE
  @SuspenceReceiptDepartmentCode        nvarchar(10) --仮受部門コード
, @SuspenceReceiptAccountTitleCode      nvarchar(10) --仮受科目コード
, @SuspenceReceiptSubCode               nvarchar(10) --仮受補助コード
, @FeeDepartmentCode                    nvarchar(10) --振込手数料部門コード
, @FeeAccountTitleCode                  nvarchar(10) --振込手数料科目コード
, @FeeSubCode                           nvarchar(10) --振込手数料補助コード
, @DebitTaxDifferenceDepartmentCode     nvarchar(10) --借方消費税誤差部門コード
, @DebitTaxDifferenceAccountTitleCode   nvarchar(10) --借方消費税誤差科目コード
, @DebitTaxDifferenceSubCode            nvarchar(10) --借方消費税誤差補助コード
, @CreditTaxDifferenceDepartmentCode    nvarchar(10) --貸方消費税誤差部門コード
, @CreditTaxDifferenceAccountTitleCode  nvarchar(10) --貸方消費税誤差科目コード
, @CreditTaxDifferenceSubCode           nvarchar(10) --貸方消費税誤差補助コード

SET @SuspenceReceiptDepartmentCode        = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'仮受部門コード')
SET @SuspenceReceiptAccountTitleCode      = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'仮受科目コード')
SET @SuspenceReceiptSubCode               = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'仮受補助コード')
SET @FeeDepartmentCode                    = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'振込手数料部門コード')
SET @FeeAccountTitleCode                  = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'振込手数料科目コード')
SET @FeeSubCode                           = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'振込手数料補助コード')
SET @DebitTaxDifferenceDepartmentCode     = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'借方消費税誤差部門コード')
SET @DebitTaxDifferenceAccountTitleCode   = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'借方消費税誤差科目コード')
SET @DebitTaxDifferenceSubCode            = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'借方消費税誤差補助コード')
SET @CreditTaxDifferenceDepartmentCode    = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'貸方消費税誤差部門コード')
SET @CreditTaxDifferenceAccountTitleCode  = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'貸方消費税誤差科目コード')
SET @CreditTaxDifferenceSubCode           = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'貸方消費税誤差補助コード')

SELECT t.CompanyCode
     , t.RecordedAt
     , DENSE_RANK() OVER(ORDER BY t.CreateAt, t.Id) [SlipNumber]
     , t.DebitDepartmentCode
     , dd.Name  [DebitDepartmentName]
     , t.DebitAccountTitleCode
     , dat.Name [DebitAccountTitleName]
     , t.DebitSubCode
     , t.DebitSubName
     , t.CreditDepartmentCode
     , cd.Name  [CreditDepartmentName]
     , t.CreditAccountTitleCode
     , cat.Name [CreditAccountTitleName]
     , t.CreditSubCode
     , t.CreditSubName
     , t.Amount
     , t.Note
     , t.CustomerCode
     , cs.Name [CustomerName]
     , t.InvoiceCode
     , t.StaffCode
     , t.PayerCode
     , t.PayerName
     , t.SourceBankName
     , t.SourceBranchName
     , t.DueDate
     , t.BankCode
     , t.BankName
     , t.BranchCode
     , t.BranchName
     , t.AccountTypeId
     , t.AccountNumber
     , t.TaxClassId
     , t.CreateAt
     , t.CurrencyCode
     , t.Approved
     , t.Id
     , t.MatchingMemo
  FROM (
       --通常消込
       SELECT cm.Code [CompanyCode]
            , m.RecordedAt
            , mh.Id
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN ''
              ELSE @SuspenceReceiptDepartmentCode END   [DebitDepartmentCode]
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN COALESCE(rat.Code, '')
              ELSE @SuspenceReceiptAccountTitleCode END [DebitAccountTitleCode]
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN COALESCE(rct.SubCode, '')
              ELSE @SuspenceReceiptSubCode END          [DebitSubCode]
            , '' [DebitSubName]
            , CASE WHEN b.InputType = 3 THEN @SuspenceReceiptDepartmentCode
              ELSE d.Code END [CreditDepartmentCode]
            , CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rat.Code, '')
              ELSE bat.Code END [CreditAccountTitleCode]
            , CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rc.SubCode, '')
              ELSE '' END [CreditSubCode]
            , '' [CreditSubName]
            , m.Amount
            , b.Note1       [Note]
            , cs.Code       [CustomerCode]
            , b.InvoiceCode [InvoiceCode]
            , st.Code       [StaffCode]
            , ''            [PayerCode]
            , cs.Name       [PayerName]
            , ''            [SourceBankName]
            , ''            [SourceBranchName]
            , NULL          [DueDate]
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , rct.TaxClassId
            , m.CreateAt
            , ccy.Code      [CurrencyCode]
            , mh.Approved
            , mh.Memo       [MatchingMemo]
         FROM MatchingHeader mh
        INNER JOIN Matching m       ON mh.Id            = m.MatchingHeaderId
                                   AND mh.CompanyId     = @CompanyId
                                   AND mh.CurrencyId    = @CurrencyId
{matchingCondition}
        INNER JOIN Company cm                   ON mh.CompanyId = cm.Id
        INNER JOIN Receipt r                    ON m.ReceiptId = r.Id
        INNER JOIN Billing b                    ON m.BillingId = b.Id
        INNER JOIN Customer cs                  ON b.CustomerId = cs.Id
        INNER JOIN Staff st                     ON b.StaffId = st.Id
        INNER JOIN Department d                 ON b.DepartmentId = d.Id
        INNER JOIN Category rct                 ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Category cct                 ON b.CollectCategoryId = cct.Id
        INNER JOIN Currency ccy                 ON mh.CurrencyId = ccy.Id
         LEFT JOIN AccountTitle rat             ON rct.AccountTitleId = rat.Id
         LEFT JOIN AccountTitle bat             ON b.DebitAccountTitleId = bat.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN BillingScheduledIncome bsi   ON b.Id = bsi.BillingId
         LEFT JOIN Matching bsi_m               ON bsi.MatchingId = bsi_m.Id
         LEFT JOIN Receipt bsi_r                ON bsi_m.ReceiptId = bsi_r.Id
         LEFT JOIN Category bsi_rc              ON bsi_r.ReceiptCategoryId = bsi_rc.Id
         LEFT JOIN AccountTitle bsi_rat         ON bsi_rc.AccountTitleId = bsi_rat.Id

       --手数料
       UNION ALL
       SELECT cm.Code              [CompanyCode]
            , m.RecordedAt
            , mh.Id
            , @FeeDepartmentCode    [DebitDepartmentCode]
            , @FeeAccountTitleCode  [DebitAccountTitleCode]
            , @FeeSubCode           [DebitSubCode]
            , ''                    [DebitSubName]
            , CASE WHEN b.InputType = 3 THEN @SuspenceReceiptDepartmentCode
              ELSE d.Code END       [CreditDepartmentCode]
            , CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rat.Code, '')
              ELSE bat.Code END     [CreditAccountTitleCode]
            , CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rc.SubCode, '')
              ELSE '' END           [CreditSubCode]
            , ''                    [CreditSubName]
            , m.BankTransferFee     [Amount]
            , b.Note1               [Note]
            , cs.Code               [CustomerCode]
            , b.InvoiceCode
            , st.Code               [StaffCode]
            , ''                    [PayerCode]
            , cs.Name               [PayerName]
            , ''                    [SourceBankName]
            , ''                    [SourceBranchName]
            , NULL                  [DueDate]
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , rct.TaxClassId
            , m.CreateAt
            , ccy.Code      [CurrencyCode]
            , mh.Approved
            , mh.Memo       [MatchingMemo]
         FROM MatchingHeader mh
        INNER JOIN Matching m                   ON mh.Id = m.MatchingHeaderId
                                               AND mh.CompanyId = @CompanyId
                                               AND mh.CurrencyId = @CurrencyId
                                               AND m.BankTransferFee <> 0
{matchingCondition}
        INNER JOIN Company cm                   ON mh.CompanyId = cm.Id
        INNER JOIN Receipt r                    ON m.ReceiptId = r.Id
        INNER JOIN Billing b                    ON m.BillingId = b.Id
        INNER JOIN Customer cs                  ON b.CustomerId = cs.Id
        INNER JOIN Staff st                     ON b.StaffId = st.Id
        INNER JOIN Department d                 ON b.DepartmentId = d.Id
        INNER JOIN Category rct                 ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Category cct                 ON b.CollectCategoryId = cct.Id
        INNER JOIN Currency ccy                 ON mh.CurrencyId = ccy.Id
         LEFT JOIN AccountTitle bat             ON b.DebitAccountTitleId = bat.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN BillingScheduledIncome bsi   ON b.Id = bsi.BillingId
         LEFT JOIN Matching bsi_m               ON bsi.MatchingId = bsi_m.Id
         LEFT JOIN Receipt bsi_r                ON bsi_m.ReceiptId = bsi_r.Id
         LEFT JOIN Category bsi_rc              ON bsi_r.ReceiptCategoryId = bsi_rc.Id
         LEFT JOIN AccountTitle bsi_rat         ON bsi_rc.AccountTitleId = bsi_rat.Id

       --消費税誤差
       UNION ALL
       SELECT cm.Code [CompanyCode]
            , m.RecordedAt
            , mh.Id
            , CASE WHEN m.TaxDifference < 0 THEN @DebitTaxDifferenceDepartmentCode
                ELSE CASE WHEN rct.UseAdvanceReceived = 1 THEN ''
                ELSE @SuspenceReceiptDepartmentCode END END [DebitDepartmentCode]
            , CASE WHEN m.TaxDifference < 0 THEN @DebitTaxDifferenceAccountTitleCode
                ELSE CASE WHEN rct.UseAdvanceReceived = 1 THEN COALESCE(rat.Code, '')
                ELSE @SuspenceReceiptAccountTitleCode END END [DebitAccountTitleCode]
            , CASE WHEN m.TaxDifference < 0 THEN @DebitTaxDifferenceSubCode
                ELSE CASE WHEN rct.UseAdvanceReceived = 1 THEN COALESCE(rct.SubCode, '')
                ELSE @SuspenceReceiptSubCode END END [DebitSubCode]
            , ''                   [DebitSubName]
            , CASE WHEN m.TaxDifference > 0 THEN @CreditTaxDifferenceDepartmentCode
              ELSE CASE WHEN b.InputType = 3 THEN @SuspenceReceiptDepartmentCode
                   ELSE d.Code END 
              END [CreditDepartmentCode]
            , CASE WHEN m.TaxDifference > 0 THEN @CreditTaxDifferenceAccountTitleCode
              ELSE CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rat.Code, '')
                   ELSE bat.Code END
              END [CreditAccountTitleCode]
            , CASE WHEN m.TaxDifference > 0 THEN @CreditTaxDifferenceSubCode
              ELSE CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rc.SubCode, '')
                   ELSE '' END
              END [CreditSubCode]
            , ''                    [CreditSubName]
            , CASE WHEN m.TaxDifference < 0 THEN -1 * m.TaxDifference ELSE m.TaxDifference END [Amount]
            , b.Note1               [Note]
            , cs.Code               [CustomerCode]
            , b.InvoiceCode
            , st.Code               [StaffCode]
            , ''                    [PayerCode]
            , cs.Name               [PayerName]
            , ''                    [SourceBankName]
            , ''                    [SourceBranchName]
            , NULL                  [DueDate]
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , rct.TaxClassId
            , m.CreateAt
            , ccy.Code      [CurrencyCode]
            , mh.Approved
            , mh.Memo       [MatchingMemo]
         FROM MatchingHeader mh
        INNER JOIN Matching m                   ON mh.Id = m.MatchingHeaderId
                                               AND mh.CompanyId = @CompanyId
                                               AND mh.CurrencyId = @CurrencyId
                                               AND m.TaxDifference <> 0
{matchingCondition}
        INNER JOIN Company cm                   ON mh.CompanyId = cm.Id
        INNER JOIN Receipt r                    ON m.ReceiptId = r.Id
        INNER JOIN Billing b                    ON m.BillingId = b.Id
        INNER JOIN Customer cs                  ON b.CustomerId = cs.Id
        INNER JOIN Staff st                     ON b.StaffId = st.Id
        INNER JOIN Department d                 ON b.DepartmentId = d.Id
        INNER JOIN Category rct                 ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Category cct                 ON b.CollectCategoryId = cct.Id
        INNER JOIN Currency ccy                 ON mh.CurrencyId = ccy.Id
         LEFT JOIN AccountTitle rat             ON rct.AccountTitleId = rat.Id
         LEFT JOIN AccountTitle bat             ON b.DebitAccountTitleId = bat.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN BillingScheduledIncome bsi   ON b.Id = bsi.BillingId
         LEFT JOIN Matching bsi_m               ON bsi.MatchingId = bsi_m.Id
         LEFT JOIN Receipt bsi_r                ON bsi_m.ReceiptId = bsi_r.Id
         LEFT JOIN Category bsi_rc              ON bsi_r.ReceiptCategoryId = bsi_rc.Id
         LEFT JOIN AccountTitle bsi_rat         ON bsi_rc.AccountTitleId = bsi_rat.Id
";

            //歩引額
            //（歩引機能ON の場合のみ）
            if (option.UseDiscount) query += $@"
       UNION ALL
       SELECT 
              cm.Code    [CompanyCode]
            , m.RecordedAt
            , mh.Id
            , cdd.Code   [DebitDepartmentCode]
            , cdat.Code  [DebitAccountTitleCode]
            , cd.SubCode [DebitSubCode]
            , ''         [DebitSubName]
            , CASE WHEN b.InputType = 3 THEN @SuspenceReceiptDepartmentCode
                   ELSE d.Code   END [CreditDepartmentCode]
            , CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rat.Code, '')
                   ELSE bat.Code END [CreditAccountTitleCode]
            , CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rc.SubCode, '')
                   ELSE '' END       [CreditSubCode]
            , '' [CreditSubName]
            , mbd.DiscountAmount [Amount]
            , b.Note1 [Note]
            , cs.Code [CustomerCode]
            , b.InvoiceCode
            , st.Code [StaffCode]
            , ''            [PayerCode]
            , cs.Name       [PayerName]
            , ''            [SourceBankName]
            , ''            [SourceBranchName]
            , NULL          [DueDate]
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , rct.TaxClassId
            , m.CreateAt
            , ccy.Code      [CurrencyCode]
            , mh.Approved
            , mh.Memo       [MatchingMemo]
         FROM MatchingHeader mh
        INNER JOIN Matching m                   ON mh.Id = m.MatchingHeaderId
                                               AND mh.CompanyId = @CompanyId
                                               AND mh.CurrencyId = @CurrencyId
{matchingCondition}
        INNER JOIN Company cm                   ON mh.CompanyId = cm.Id
        INNER JOIN MatchingBillingDiscount mbd  ON m.Id = mbd.MatchingId
        INNER JOIN Receipt r                    ON m.ReceiptId = r.Id
        INNER JOIN Billing b                    ON m.BillingId = b.Id
        INNER JOIN Customer cs                  ON b.CustomerId = cs.Id
        INNER JOIN Staff st                     ON b.StaffId = st.Id
        INNER JOIN Department d                 ON b.DepartmentId = d.Id
        INNER JOIN Category rct                 ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Category cct                 ON b.CollectCategoryId = cct.Id
        INNER JOIN Currency ccy                 ON mh.CurrencyId = ccy.Id
        INNER JOIN CustomerDiscount cd          ON b.CustomerId = cd.CustomerId
                                               AND mbd.DiscountType = cd.[Sequence]
         LEFT JOIN Department cdd               ON cd.DepartmentId = cdd.Id
         LEFT JOIN AccountTitle cdat            ON cd.AccountTitleId = cdat.Id
         LEFT JOIN AccountTitle bat             ON b.DebitAccountTitleId = bat.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN BillingScheduledIncome bsi   ON b.Id = bsi.BillingId
         LEFT JOIN Matching bsi_m               ON bsi.MatchingId = bsi_m.Id
         LEFT JOIN Receipt bsi_r                ON bsi_m.ReceiptId = bsi_r.Id
         LEFT JOIN Category bsi_rc              ON bsi_r.ReceiptCategoryId = bsi_rc.Id
         LEFT JOIN AccountTitle bsi_rat         ON bsi_rc.AccountTitleId = bsi_rat.Id
";

            query += $@"
       --前受仕訳
       UNION ALL
       SELECT 
              cm.Code                          [CompanyCode]
            , r.RecordedAt
            , r.Id
            , @SuspenceReceiptDepartmentCode   [DebitDepartmentCode]
            , @SuspenceReceiptAccountTitleCode [DebitAccountTitleCode]
            , @SuspenceReceiptSubCode          [DebitSubCode]
            , ''                               [DebitSubName]
            , ''                               [CreditDepartmentCode]
            , COALESCE(rat.Code, '')           [CreditAccountTitleCode]
            , COALESCE(rct.SubCode, '')        [CreditSubCode]
            , ''                               [CreditSubName]
            , r.ReceiptAmount                  [Amount]
            , r.Note1                          [Note]
            , cs.Code                          [CustomerCode]
            , ''                               [InvoiceCode]
            , ''                               [StaffCode]
            , r.PayerCode
            , r.PayerName
            , r.SourceBankName
            , r.SourceBranchName
            , r.DueAt
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , NULL                           [TaxClassId]
            , r.CreateAt
            , ccy.Code                       [CurrencyCode]
            , 1                              [Approved]
            , ''                             [MatchingMemo]
         FROM Receipt r
        INNER JOIN Category rct                 ON r.ReceiptCategoryId  = rct.Id
                                               AND r.CompanyId          = @CompanyId
                                               AND r.CurrencyId         = @CurrencyId
                                               AND rct.UseAdvanceReceived = 1
                                               AND r.Apportioned = 1
                                               AND r.DeleteAt IS NULL
{receiptCondition}
        INNER JOIN Company cm                   ON r.CompanyId = cm.Id
        INNER JOIN Currency ccy                 ON r.CurrencyId = ccy.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN AccountTitle rat             ON rct.AccountTitleId = rat.Id
         LEFT JOIN Customer cs                  ON r.CustomerId = cs.Id

       --対象外仕訳
       UNION ALL
       SELECT 
              cm.Code [CompanyCode]
            , r.RecordedAt
            , r.Id
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN ''
                   ELSE @SuspenceReceiptDepartmentCode END   [DebitDepartmentCode]
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN COALESCE(rat.Code, '')
                   ELSE @SuspenceReceiptAccountTitleCode END [DebitAccountTitleCode]
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN COALESCE(rct.SubCode, '')
                   ELSE @SuspenceReceiptSubCode END          [DebitSubCode]
            , '' [DebitSubName]
            , @SuspenceReceiptDepartmentCode   [CreditDepartmentCode]
            , COALESCE(reat.Code, '')          [CreditAccountTitleCode]
            , COALESCE(rect.SubCode, '')       [CreditSubCode]
            , ''                               [CreditSubName]
            , re.ExcludeAmount                 [Amount]
            , rect.Note
            , cs.Code                          [CustomerCode]
            , ''                               [InvoiceCode]
            , ''                               [StaffCode]
            , r.PayerCode
            , r.PayerName
            , r.SourceBankName
            , r.SourceBranchName
            , r.DueAt
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , NULL                           [TaxClassId]
            , re.CreateAt
            , ccy.Code                       [CurrencyCode]
            , 1                              [Approved]
            , ''                             [MatchingMemo]
         FROM Receipt r
        INNER JOIN ReceiptExclude re            ON r.Id = re.ReceiptId
                                               AND r.CompanyId = @CompanyId
                                               AND r.CurrencyId = @CurrencyId
                                               AND r.Apportioned = 1
                                               AND r.DeleteAt IS NULL
{excludeCondition}
        INNER JOIN Company cm                   ON r.CompanyId = cm.Id
        INNER JOIN Category rect                ON re.ExcludeCategoryId = rect.Id
        INNER JOIN Category rct                 ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Currency ccy                 ON r.CurrencyId = ccy.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN AccountTitle rat             ON rct.AccountTitleId = rat.Id
         LEFT JOIN AccountTitle reat            ON rect.AccountTitleId = reat.Id
         LEFT JOIN Customer cs                  ON r.CustomerId = cs.Id
       ) t
  LEFT JOIN Department dd                   ON dd.CompanyId = @CompanyId
                                           AND t.DebitDepartmentCode = dd.Code
  LEFT JOIN AccountTitle dat                ON dat.CompanyId = @CompanyId
                                           AND t.DebitAccountTitleCode = dat.Code
  LEFT JOIN Department cd                   ON cd.CompanyId = @CompanyId
                                           AND t.CreditDepartmentCode = cd.Code
  LEFT JOIN AccountTitle cat                ON cat.CompanyId = @CompanyId
                                           AND t.CreditAccountTitleCode = cat.Code
  LEFT JOIN Customer cs                     ON cs.CompanyId = @CompanyId
                                           AND cs.Code = t.CustomerCode
  ORDER BY t.CreateAt, t.Id
";
            #endregion
            return dbHelper.GetItemsAsync<MatchingJournalizing>(query, option, token);
        }
        public Task<IEnumerable<GeneralJournalizing>> GetGeneralJournalizingAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var query =
            #region 1 matching journalizing
@"
SELECT 1 [JournalizingType]
     , mh.CompanyId
     , mh.CurrencyId
     , ccy.[Code]           [CurrencyCode]
     , ccy.[Precision]
     , m.Amount
     , mh.Approved
     , m.CreateAt
     , m.ReceiptId
     , m.BillingId
     , m.Id                 [MatchingId]
     , msi.ReceiptId        [ScheduledIncomeReceiptId]
     , msi.BillingId        [ScheduledIncomeBillingId]
     , msi.MatchingHeaderId [ScheduledIncomeMatchingHeaderId]
     , null                 [AdvanceReceivedBackupId]
     , null                 [ReceiptExcludeId]
     , rcs.Id               [ReceiptCustomerId]
     , rpcs.Id              [ReceiptParentCustomerId]
     , bcs.Id               [BillingCustomerId]
     , bpcs.Id              [BillingParentCustomerId]
     , bdp.Id               [BillingDepartmentId]
     , bdp.StaffId          [BillingDepartmentStaffId]
     , bst.Id               [BillingStaffId]
     , bst.DepartmentId     [BillingStaffDepartmentId]
     , m.MatchingHeaderId
  FROM Matching m
 INNER JOIN MatchingHeader mh           ON mh.Id            = m.MatchingHeaderId
                                       AND mh.CompanyId     = @CompanyId
                                       AND mh.CurrencyId    = @CurrencyId
 INNER JOIN Receipt r                   ON r.Id             = m.ReceiptId
  LEFT JOIN ReceiptHeader rh            ON rh.Id            = r.ReceiptHeaderId
 INNER JOIN Billing b                   ON b.Id             = m.BillingId
 INNER JOIN Currency ccy                ON ccy.Id           = mh.CurrencyId
  LEFT JOIN BillingScheduledIncome bsi  ON b.Id             = bsi.BillingId
  LEFT JOIN Matching msi                ON msi.Id           = bsi.MatchingId
  LEFT JOIN Customer rcs                ON rcs.Id           = r.CustomerId
  LEFT JOIN CustomerGroup rcsg          ON rcs.Id           = rcsg.ChildCustomerId
  LEFT JOIN Customer rpcs               ON rpcs.Id          = rcsg.ParentCustomerId
 INNER JOIN Customer bcs                ON bcs.Id           = b.CustomerId
  LEFT JOIN CustomerGroup bcsg          ON bcs.Id           = bcsg.ChildCustomerId
  LEFT JOIN Customer bpcs               ON bpcs.Id          = bcsg.ParentCustomerId
  LEFT JOIN Department bdp              ON bdp.Id           = b.DepartmentId
  LEFT JOIN Staff bst                   ON bst.Id           = b.StaffId
 WHERE m.Id             = m.Id";
            if (option.RecordedAtFrom.HasValue) query += @"
   AND m.RecordedAt    >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue) query += @"
   AND m.RecordedAt    <= @RecordedAtTo";
            if (!(option.OutputAt?.Any() ?? false)) query += @"
   AND m.OutputAt      IS NULL";
            else query += @"
   AND m.OutputAt      IN @OutputAt";
            #endregion
            query +=
            #region 2 advance received occured journalizing
@"
 UNION ALL
SELECT 2 [JournalizingType]
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
     , null                 [MatchingHeaderId]
  FROM Receipt r
 INNER JOIN Receipt sr                  ON sr.Id            = r.OriginalReceiptId /* source receipt */
                                       AND r.CompanyId      = @CompanyId
                                       AND r.CurrencyId     = @CurrencyId
                                       AND r.Apportioned    = 1
                                       AND r.DeleteAt       IS NULL
 INNER JOIN Category rct                ON rct.Id           = r.ReceiptCategoryId
                                       AND rct.UseAdvanceReceived = 1
  LEFT JOIN ReceiptHeader rh            ON rh.Id            = r.ReceiptHeaderId
 INNER JOIN Currency ccy                ON ccy.Id           = r.CurrencyId
  LEFT JOIN Customer rcs                ON rcs.Id           = r.CustomerId
  LEFT JOIN CustomerGroup rcsg          ON rcs.Id           = rcsg.ChildCustomerId
  LEFT JOIN Customer rpcs               ON rpcs.Id          = rcsg.ParentCustomerId
  LEFT JOIN AdvanceReceivedBackup ar    ON ar.OriginalReceiptId = r.OriginalReceiptId
 WHERE r.Id             = r.Id
   AND ar.Id            IS NULL";
            if (option.RecordedAtFrom.HasValue) query += @"
   AND r.RecordedAt    >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue) query += @"
   AND r.RecordedAt    <= @RecordedAtTo";
            if (!(option.OutputAt?.Any() ?? false)) query += @"
   AND r.OutputAt      IS NULL";
            else query += @"
   AND r.OutputAt      IN @OutputAt";
            #endregion
            query +=
            #region 3 advance received transfer journalizing
@"
 UNION ALL
SELECT 3 [JournalizingType]
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
     , ar.Id                [AdvanceReceivedBackupId]
     , null                 [ReceiptExcludeId]
     , rcs.Id               [ReceiptCustomerId]
     , rpcs.Id              [ReceiptParentCustomerId]
     , null                 [BillingCustomerId]
     , null                 [BillingParentCustomerId]
     , null                 [BillingDepartmentId]
     , null                 [BillingDepartmentStaffId]
     , null                 [BillingStaffId]
     , null                 [BillingStaffDepartmentId]
     , null                 [MatchingHeaderId]
  FROM Receipt r
 INNER JOIN Receipt sr                  ON sr.Id            = r.OriginalReceiptId /* source receipt */
                                       AND r.CompanyId      = @CompanyId
                                       AND r.CurrencyId     = @CurrencyId
                                       AND r.Apportioned    = 1
                                       AND r.DeleteAt       IS NULL
 INNER JOIN Category rct                ON rct.Id           = r.ReceiptCategoryId
                                       AND rct.UseAdvanceReceived = 1
 INNER JOIN AdvanceReceivedBackup ar    ON ar.OriginalReceiptId = r.OriginalReceiptId
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
            #endregion
            query +=
            #region 4 receipt exclude
@"
 UNION ALL
SELECT 4 [JournalizingType]
     , r.CompanyId
     , r.CurrencyId
     , ccy.[Code]           [CurrencyCode]
     , ccy.[Precision]
     , re.ExcludeAmount     [Amount]
     , r.Approved
     , r.CreateAt
     , r.Id                 [ReceiptId]
     , null                 [BillingId]
     , null                 [MatchingId]
     , null                 [ScheduledIncomeReceiptId]
     , null                 [ScheduledIncomeBillingId]
     , null                 [ScheduledIncomeMatchingHeaderId]
     , null                 [AdvanceReceivedBackupId]
     , re.Id                [ReceiptExcludeId]
     , rcs.Id               [ReceiptCustomerId]
     , rpcs.Id              [ReceiptParentCustomerId]
     , null                 [BillingCustomerId]
     , null                 [BillingParentCustomerId]
     , null                 [BillingDepartmentId]
     , null                 [BillingDepartmentStaffId]
     , null                 [BillingStaffId]
     , null                 [BillingStaffDepartmentId]
     , null                 [MatchingHeaderId]
  FROM Receipt r
  LEFT JOIN ReceiptHeader rh            ON rh.Id            = r.ReceiptHeaderId
 INNER JOIN ReceiptExclude re           ON r.Id             = re.ReceiptId
                                       AND r.CompanyId      = @CompanyId
                                       AND r.CurrencyId     = @CurrencyId
                                       AND r.Apportioned    = 1
                                       AND r.DeleteAt       IS NULL
 INNER JOIN Currency ccy                ON ccy.Id           = r.CurrencyId
  LEFT JOIN Customer rcs                ON rcs.Id           = r.CustomerId
  LEFT JOIN CustomerGroup rcsg          ON rcs.Id           = rcsg.ChildCustomerId
  LEFT JOIN Customer rpcs               ON rpcs.Id          = rcsg.ParentCustomerId
 WHERE re.Id            = re.Id";
            if (option.RecordedAtFrom.HasValue) query += @"
   AND r.RecordedAt    >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue) query += @"
   AND r.RecordedAt    <= @RecordedAtTo";
            if (!(option.OutputAt?.Any() ?? false)) query += @"
   AND re.OutputAt     IS NULL";
            else query += @"
   AND re.OutputAt     IN @OutputAt";

            #endregion
            query += @"
 ORDER BY CreateAt, MatchingHeaderId, ReceiptId";
            return dbHelper.GetItemsAsync<GeneralJournalizing>(query, option, token);
        }

        public Task<IEnumerable<MatchedReceipt>> GetMatchedReceiptsAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var conditionMatching = "";
            var conditionReceipt = "";

            conditionMatching = (option.OutputAt == null || !option.OutputAt.Any())
                ? @" AND m.OutputAt IS NULL"
                : @" AND m.OutputAt IN @OutputAt";
            conditionReceipt = (option.OutputAt == null || !option.OutputAt.Any())
                ? @" AND r.OutputAt IS NULL"
                : @" AND r.OutputAt IN @OutputAt";

            if (option.RecordedAtFrom.HasValue)
            {
                conditionMatching += @" AND m.RecordedAt >= @RecordedAtFrom";
                conditionReceipt += @" AND r.RecordedAt >= @RecordedAtFrom";
            }
            if (option.RecordedAtTo.HasValue)
            {
                conditionMatching += @" AND m.RecordedAt <= @RecordedAtTo";
                conditionReceipt += @" AND r.RecordedAt <= @RecordedAtTo";
            }
            var query = $@"
DECLARE
  @FeeCategoryCode          varchar(2) = 'CH'
, @DebitTaxCategoryCode     varchar(2) = 'BT'
, @CreditTaxCategoryCode    varchar(2) = 'RT'
, @DiscountCategoryCode1    varchar(2) = 'D1'
, @DiscountCategoryCode2    varchar(2) = 'D2'
, @DiscountCategoryCode3    varchar(2) = 'D3'
, @DiscountCategoryCode4    varchar(2) = 'D4'
, @DiscountCategoryCode5    varchar(2) = 'D5'

SELECT t.CompanyCode
     , ROW_NUMBER() OVER (ORDER BY t.CustomerCode, t.Id, t.ReceiptCategoryCode) [SlipNumber]
     , t.CustomerCode
     , t.CustomerName
     , t.InvoiceCode
     , t.BilledAt
     , t.ReceiptCategoryCode
     , t.ReceiptCategoryName
     , t.RecordedAt
     , t.DueAt
     , t.Amount
     , t.DepartmentCode
     , t.DepartmentName
     , t.CurrencyCode
     , t.ReceiptAmount
     , t.Id
     , t.BillingNote1
     , t.BillingNote2
     , t.BillingNote3
     , t.BillingNote4
     , t.ReceiptNote1
     , t.ReceiptNote2
     , t.ReceiptNote3
     , t.ReceiptNote4
     , t.BillNumber
     , t.BillBankCode
     , t.BillBranchCode
     , t.BillDrawAt
     , t.BillDrawer
     , t.BillingMemo
     , t.ReceiptMemo
     , t.MatchingMemo
     , t.BankCode
     , t.BankName
     , t.BranchCode
     , t.BranchName
     , t.AccountNumber
     , t.SourceBankName
     , t.SourceBranchName
     , t.VirtualBranchCode
     , t.VirtualAccountNumber
     , t.SectionCode
     , t.SectionName
     , t.ReceiptCategoryExternalCode
     , t.OriginalReceiptId
     , t.JournalizingCategory
  FROM (
       SELECT cm.Code [CompanyCode]
            , r.Id
            , cs.Code [CustomerCode]
            , cs.Name [CustomerName]
            , b.InvoiceCode
            , b.BilledAt
            , rct.Code [ReceiptCategoryCode]
            , rct.Name [ReceiptCategoryName]
            , r.RecordedAt
            , r.DueAt
            , m.Amount
            , d.Code [DepartmentCode]
            , d.Name [DepartmentName]
            , ccy.Code [CurrencyCode]
            , r.ReceiptAmount [ReceiptAmount]
            , b.Note1 [BillingNote1]
            , b.Note2 [BillingNote2]
            , b.Note3 [BillingNote3]
            , b.Note4 [BillingNote4]
            , r.Note1 [ReceiptNote1]
            , r.Note2 [ReceiptNote2]
            , r.Note3 [ReceiptNote3]
            , r.Note4 [ReceiptNote4]
            , r.BillNumber
            , r.BillBankCode
            , r.BillBranchCode
            , r.BillDrawAt
            , r.BillDrawer
            , bm.Memo [BillingMemo]
            , rm.Memo [ReceiptMemo]
            , mh.Memo [MatchingMemo]
            , r.BankCode
            , r.BankName
            , r.BranchCode
            , r.BranchName
            , r.AccountNumber
            , r.SourceBankName
            , r.SourceBranchName
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN LEFT(r.PayerCode, LEN(r.PayerCode) - 7) ELSE '' END ELSE '' END [VirtualBranchCode]
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN RIGHT(r.PayerCode, 7) ELSE r.PayerCode END ELSE '' END [VirtualAccountNumber]
            , s.Code [SectionCode]
            , s.Name [SectionName]
            , rct.ExternalCode [ReceiptCategoryExternalCode]
            , r.OriginalReceiptId
            , '消込' [JournalizingCategory]
         FROM MatchingHeader mh
        INNER JOIN Matching m
           ON mh.Id            = m.MatchingHeaderId
          AND mh.CompanyId     = @CompanyId
          AND mh.CurrencyId    = @CurrencyId
          AND mh.Approved      = 1
{conditionMatching}
        INNER JOIN Company cm           ON mh.CompanyId = cm.Id
        INNER JOIN Billing b            ON m.BillingId = b.Id
        INNER JOIN Receipt r            ON m.ReceiptId = r.Id
        INNER JOIN Category rct         ON r.ReceiptCategoryId = rct.Id
                    {(option.ContainAdvanceReceivedMatching ? "" : "AND rct.UseAdvanceReceived = 0")}
        INNER JOIN Customer cs          ON b.CustomerId = cs.Id
        INNER JOIN Department d         ON b.DepartmentId = d.Id
        INNER JOIN Currency ccy         ON mh.CurrencyId = ccy.Id
         LEFT JOIN BillingMemo bm       ON bm.BillingId = b.Id
         LEFT JOIN ReceiptMemo rm       ON rm.ReceiptId = r.Id
         LEFT JOIN ReceiptHeader rh     ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN Section s            ON r.SectionId = s.Id

        UNION ALL

       SELECT cm.Code [CompanyCode]
            , r.Id
            , cs.Code [CustomerCode]
            , cs.Name [CustomerName]
            , b.InvoiceCode
            , b.BilledAt
            , @FeeCategoryCode [ReceiptCategoryCode]
            , '振込手数料' [ReceiptCategoryName]
            , r.RecordedAt
            , r.DueAt
            , m.BankTransferFee [Amount]
            , d.Code [DepartmentCode]
            , d.Name [DepartmentName]
            , ccy.Code [CurrencyCode]
            , r.ReceiptAmount [ReceiptAmount]
            , b.Note1 [BillingNote1]
            , b.Note2 [BillingNote2]
            , b.Note3 [BillingNote3]
            , b.Note4 [BillingNote4]
            , r.Note1 [ReceiptNote1]
            , r.Note2 [ReceiptNote2]
            , r.Note3 [ReceiptNote3]
            , r.Note4 [ReceiptNote4]
            , r.BillNumber
            , r.BillBankCode
            , r.BillBranchCode
            , r.BillDrawAt
            , r.BillDrawer
            , bm.Memo [BillingMemo]
            , rm.Memo [ReceiptMemo]
            , mh.Memo [MatchingMemo]
            , r.BankCode
            , r.BankName
            , r.BranchCode
            , r.BranchName
            , r.AccountNumber
            , r.SourceBankName
            , r.SourceBranchName
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN LEFT(r.PayerCode, LEN(r.PayerCode) - 7) ELSE '' END ELSE '' END [VirtualBranchCode]
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN RIGHT(r.PayerCode, 7) ELSE r.PayerCode END ELSE '' END [VirtualAccountNumber]
            , s.Code [SectionCode]
            , s.Name [SectionName]
            , @FeeCategoryCode [ReceiptCategoryExternalCode]
            , r.OriginalReceiptId
            , '消込' [JournalizingCategory]
         FROM MatchingHeader mh
        INNER JOIN Matching m
           ON mh.Id           = m.MatchingHeaderId
          AND mh.CompanyId    = @CompanyId
          AND mh.CurrencyId   = @CurrencyId
          AND mh.Approved     = 1
          AND m.BankTransferFee <> 0
{conditionMatching}
        INNER JOIN Company cm           ON mh.CompanyId = cm.Id
        INNER JOIN Billing b            ON m.BillingId = b.Id
        INNER JOIN Receipt r            ON m.ReceiptId = r.Id
        INNER JOIN Customer cs          ON b.CustomerId = cs.Id
        INNER JOIN Department d         ON b.DepartmentId = d.Id
        INNER JOIN Currency ccy         ON mh.CurrencyId = ccy.Id
         LEFT JOIN BillingMemo bm       ON bm.BillingId = b.Id
         LEFT JOIN ReceiptMemo rm       ON rm.ReceiptId = r.Id
         LEFT JOIN ReceiptHeader rh     ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN Section s            ON r.SectionId = s.Id

        UNION ALL

       SELECT cm.Code [CompanyCode]
            , r.Id
            , cs.Code [CustomerCode]
            , cs.Name [CustomerName]
            , b.InvoiceCode
            , b.BilledAt
            , CASE WHEN m.TaxDifference > 0 THEN @DebitTaxCategoryCode ELSE @CreditTaxCategoryCode END [ReceiptCategoryCode]
            , '消費税誤差' [ReceiptCategoryName]
            , r.RecordedAt
            , r.DueAt
            , ABS(m.TaxDifference) [Amount]
            , d.Code [DepartmentCode]
            , d.Name [DepartmentName]
            , ccy.Code [CurrencyCode]
            , r.ReceiptAmount [ReceiptAmount]
            , b.Note1 [BillingNote1]
            , b.Note2 [BillingNote2]
            , b.Note3 [BillingNote3]
            , b.Note4 [BillingNote4]
            , r.Note1 [ReceiptNote1]
            , r.Note2 [ReceiptNote2]
            , r.Note3 [ReceiptNote3]
            , r.Note4 [ReceiptNote4]
            , r.BillNumber
            , r.BillBankCode
            , r.BillBranchCode
            , r.BillDrawAt
            , r.BillDrawer
            , bm.Memo [BillingMemo]
            , rm.Memo [ReceiptMemo]
            , mh.Memo [MatchingMemo]
            , r.BankCode
            , r.BankName
            , r.BranchCode
            , r.BranchName
            , r.AccountNumber
            , r.SourceBankName
            , r.SourceBranchName
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN LEFT(r.PayerCode, LEN(r.PayerCode) - 7) ELSE '' END ELSE '' END [VirtualBranchCode]
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN RIGHT(r.PayerCode, 7) ELSE r.PayerCode END ELSE '' END [VirtualAccountNumber]
            , s.Code [SectionCode]
            , s.Name [SectionName]
            , CASE WHEN m.TaxDifference > 0 THEN @DebitTaxCategoryCode ELSE @CreditTaxCategoryCode END [ReceiptCategoryExternalCode]
            , r.OriginalReceiptId
            , '消込' [JournalizingCategory]
         FROM MatchingHeader mh
        INNER JOIN Matching m
           ON mh.Id             = m.MatchingHeaderId
          AND mh.CompanyId      = @CompanyId
          AND mh.CurrencyId     = @CurrencyId
          AND mh.Approved       = 1
          AND m.TaxDifference <> 0
{conditionMatching}
        INNER JOIN Company cm           ON mh.CompanyId = cm.Id
        INNER JOIN Billing b            ON m.BillingId = b.Id
        INNER JOIN Receipt r            ON m.ReceiptId = r.Id
        INNER JOIN Customer cs          ON b.CustomerId = cs.Id
        INNER JOIN Department d         ON b.DepartmentId = d.Id
        INNER JOIN Currency ccy         ON mh.CurrencyId = ccy.Id
         LEFT JOIN BillingMemo bm       ON bm.BillingId = b.Id
         LEFT JOIN ReceiptMemo rm       ON rm.ReceiptId = r.Id
         LEFT JOIN ReceiptHeader rh     ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN Section s            ON r.SectionId = s.Id

        UNION ALL

       SELECT cm.Code [CompanyCode]
            , r.Id
            , cs.Code [CustomerCode]
            , cs.Name [CustomerName]
            , b.InvoiceCode
            , b.BilledAt
            , CASE mbd.DiscountType 
                WHEN 1 THEN @DiscountCategoryCode1
                WHEN 2 THEN @DiscountCategoryCode2
                WHEN 3 THEN @DiscountCategoryCode3
                WHEN 4 THEN @DiscountCategoryCode4
                WHEN 5 THEN @DiscountCategoryCode5
                ELSE '' END [ReceiptCategoryCode]
            , '歩引額' + CONVERT(varchar, mbd.DiscountType) [ReceiptCategoryName]
            , r.RecordedAt
            , r.DueAt
            , mbd.DiscountAmount [Amount]
            , d.Code [DepartmentCode]
            , d.Name [DepartmentName]
            , ccy.Code [CurrencyCode]
            , r.ReceiptAmount [ReceiptAmount]
            , b.Note1 [BillingNote1]
            , b.Note2 [BillingNote2]
            , b.Note3 [BillingNote3]
            , b.Note4 [BillingNote4]
            , r.Note1 [ReceiptNote1]
            , r.Note2 [ReceiptNote2]
            , r.Note3 [ReceiptNote3]
            , r.Note4 [ReceiptNote4]
            , r.BillNumber
            , r.BillBankCode
            , r.BillBranchCode
            , r.BillDrawAt
            , r.BillDrawer
            , bm.Memo [BillingMemo]
            , rm.Memo [ReceiptMemo]
            , mh.Memo [MatchingMemo]
            , r.BankCode
            , r.BankName
            , r.BranchCode
            , r.BranchName
            , r.AccountNumber
            , r.SourceBankName
            , r.SourceBranchName
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN LEFT(r.PayerCode, LEN(r.PayerCode) - 7) ELSE '' END ELSE '' END [VirtualBranchCode]
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN RIGHT(r.PayerCode, 7) ELSE r.PayerCode END ELSE '' END [VirtualAccountNumber]
            , s.Code [SectionCode]
            , s.Name [SectionName]
            , CASE mbd.DiscountType 
                WHEN 1 THEN @DiscountCategoryCode1
                WHEN 2 THEN @DiscountCategoryCode2
                WHEN 3 THEN @DiscountCategoryCode3
                WHEN 4 THEN @DiscountCategoryCode4
                WHEN 5 THEN @DiscountCategoryCode5
                ELSE '' END [ReceiptCategoryExternalCode]
            , r.OriginalReceiptId
            , '消込' [JournalizingCategory]
         FROM MatchingHeader mh
        INNER JOIN Matching m
           ON mh.Id             = m.MatchingHeaderId
          AND mh.CompanyId      = @CompanyId
          AND mh.CurrencyId     = @CurrencyId
{conditionMatching}
        INNER JOIN Company cm           ON mh.CompanyId = cm.Id
        INNER JOIN MatchingBillingDiscount mbd  ON m.Id = mbd.MatchingId
        INNER JOIN Billing b            ON m.BillingId = b.Id
        INNER JOIN Receipt r            ON m.ReceiptId = r.Id
        INNER JOIN Customer cs          ON b.CustomerId = cs.Id
        INNER JOIN Department d         ON b.DepartmentId = d.Id
        INNER JOIN Currency ccy         ON mh.CurrencyId = ccy.Id
         LEFT JOIN BillingMemo bm       ON bm.BillingId = b.Id
         LEFT JOIN ReceiptMemo rm       ON rm.ReceiptId = r.Id
         LEFT JOIN ReceiptHeader rh     ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN Section s            ON r.SectionId = s.Id
";

            #region contains advance received occured
            if (option.ContainAdvanceReceivedOccured) query += $@"
        UNION ALL /*前受計上*/

       SELECT cm.Code [CompanyCode]
            , r.Id
            , cs.Code [CustomerCode]
            , cs.Name [CustomerName]
            , '' [InvoiceCode]
            , NULL [BilledAt]
            , rct.Code [ReceiptCategoryCode]
            , rct.Name [ReceiptCategoryName]
            , r.RecordedAt
            , r.DueAt
            , r.ReceiptAmount [Amount]
            , NULL [DepartmentCode]
            , NULL [DepartmentName]
            , ccy.Code [CurrencyCode]
            , r.ReceiptAmount [ReceiptAmount]
            , '' [BillingNote1]
            , '' [BillingNote2]
            , '' [BillingNote3]
            , '' [BillingNote4]
            , r.Note1 [ReceiptNote1]
            , r.Note2 [ReceiptNote2]
            , r.Note3 [ReceiptNote3]
            , r.Note4 [ReceiptNote4]
            , r.BillNumber
            , r.BillBankCode
            , r.BillBranchCode
            , r.BillDrawAt
            , r.BillDrawer
            , NULL [BillingMemo]
            , r.Memo [ReceiptMemo]
            , NULL [MatchingMemo]
            , r.BankCode
            , r.BankName
            , r.BranchCode
            , r.BranchName
            , r.AccountNumber
            , r.SourceBankName
            , r.SourceBranchName
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN LEFT(r.PayerCode, LEN(r.PayerCode) - 7) ELSE '' END ELSE '' END [VirtualBranchCode]
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN RIGHT(r.PayerCode, 7) ELSE r.PayerCode END ELSE '' END [VirtualAccountNumber]
            , s.Code [SectionCode]
            , s.Name [SectionName]
            , rct.ExternalCode [ReceiptCategoryExternalCode]
            , r.OriginalReceiptId
            , '前受計上' [JournalizingCategory]
         FROM (
              SELECT ar.Id
                   , ar.CompanyId
                   , ar.CustomerId
                   , ar.ReceiptCategoryId
                   , ar.RecordedAt
                   , ar.DueAt
                   , ar.ReceiptAmount
                   , ar.CurrencyId
                   , ar.Note1
                   , ar.Note2
                   , ar.Note3
                   , ar.Note4
                   , ar.BillNumber
                   , ar.BillBankCode
                   , ar.BillBranchCode
                   , ar.BillDrawAt
                   , ar.BillDrawer
                   , ar.Memo
                   , ar.BankCode
                   , ar.BankName
                   , ar.BranchCode
                   , ar.BranchName
                   , ar.AccountNumber
                   , ar.SourceBankName
                   , ar.SourceBranchName
                   , ar.PayerCode
                   , ar.SectionId
                   , ar.OriginalReceiptId
                   , ar.InputType
                   , ar.OutputAt
                FROM AdvanceReceivedBackup ar
                LEFT JOIN ReceiptHeader rh          ON ar.ReceiptHeaderId = rh.Id

               UNION ALL

              SELECT r.Id
                   , r.CompanyId
                   , r.CustomerId
                   , r.ReceiptCategoryId
                   , r.RecordedAt
                   , r.DueAt
                   , r.ReceiptAmount
                   , r.CurrencyId
                   , r.Note1
                   , r.Note2
                   , r.Note3
                   , r.Note4
                   , r.BillNumber
                   , r.BillBankCode
                   , r.BillBranchCode
                   , r.BillDrawAt
                   , r.BillDrawer
                   , rm.Memo
                   , r.BankCode
                   , r.BankName
                   , r.BranchCode
                   , r.BranchName
                   , r.AccountNumber
                   , r.SourceBankName
                   , r.SourceBranchName
                   , r.PayerCode
                   , r.SectionId
                   , r.OriginalReceiptId
                   , r.InputType
                   , r.OutputAt
                FROM Receipt r
               INNER JOIN Category rct      ON r.ReceiptCategoryId = rct.Id
                 AND rct.UseAdvanceReceived = 1
                 AND NOT EXISTS(
                     SELECT *
                     FROM AdvanceReceivedBackup bk
                     WHERE r.OriginalReceiptId = r.Id
                 )
                LEFT JOIN ReceiptHeader rh  ON r.ReceiptHeaderId = rh.Id
                LEFT JOIN ReceiptMemo rm    ON rm.ReceiptId = r.Id
            ) r
        INNER JOIN Company cm   ON r.CompanyId = cm.Id
               AND r.CompanyId = @CompanyId
               AND r.CurrencyId = @CurrencyId
{conditionReceipt}
        INNER JOIN Customer cs          ON r.CustomerId = cs.Id
        INNER JOIN Receipt rbk          ON r.OriginalReceiptId = rbk.Id
        INNER JOIN Category rct         ON rbk.ReceiptCategoryId = rct.Id
        INNER JOIN Currency ccy         ON r.CurrencyId = ccy.Id
         LEFT JOIN Section s            ON r.SectionId = s.Id

        UNION ALL /*前受振替・分割*/

       SELECT cm.Code [CompanyCode]
            , r.Id
            , cs.Code [CustomerCode]
            , cs.Name [CustomerName]
            , '' [InvoiceCode]
            , NULL [BilledAt]
            , rct.Code [ReceiptCategoryCode]
            , rct.Name [ReceiptCategoryName]
            , r.RecordedAt
            , r.DueAt
            , r.ReceiptAmount [Amount]
            , NULL [DepartmentCode]
            , NULL [DepartmentName]
            , ccy.Code [CurrencyCode]
            , r.ReceiptAmount [ReceiptAmount]
            , '' [BillingNote1]
            , '' [BillingNote2]
            , '' [BillingNote3]
            , '' [BillingNote4]
            , r.Note1 [ReceiptNote1]
            , r.Note2 [ReceiptNote2]
            , r.Note3 [ReceiptNote3]
            , r.Note4 [ReceiptNote4]
            , r.BillNumber
            , r.BillBankCode
            , r.BillBranchCode
            , r.BillDrawAt
            , r.BillDrawer
            , NULL [BillingMemo]
            , rm.Memo [ReceiptMemo]
            , NULL [MatchingMemo]
            , r.BankCode
            , r.BankName
            , r.BranchCode
            , r.BranchName
            , r.AccountNumber
            , r.SourceBankName
            , r.SourceBranchName
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN LEFT(r.PayerCode, LEN(r.PayerCode) - 7) ELSE '' END ELSE '' END [VirtualBranchCode]
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN RIGHT(r.PayerCode, 7) ELSE r.PayerCode END ELSE '' END [VirtualAccountNumber]
            , s.Code [SectionCode]
            , s.Name [SectionName]
            , rct.ExternalCode [ReceiptCategoryExternalCode]
            , r.OriginalReceiptId
            , '前受振替' [JournalizingCategory]
         FROM Receipt r
        INNER JOIN Company cm       ON r.CompanyId = cm.Id
          AND r.CompanyId = @CompanyId
          AND r.CurrencyId = @CurrencyId
          AND EXISTS (
              SELECT *
              FROM AdvanceReceivedBackup bk
              WHERE r.OriginalReceiptId = bk.OriginalReceiptId )
{conditionReceipt}
        INNER JOIN Category rct         ON r.ReceiptCategoryId = rct.Id
                                       AND rct.UseAdvanceReceived = 1
        INNER JOIN Currency ccy         ON r.CurrencyId = ccy.Id
         LEFT JOIN Customer cs          ON r.CustomerId = cs.Id
         LEFT JOIN ReceiptMemo rm       ON rm.ReceiptId = r.Id
         LEFT JOIN ReceiptHeader rh     ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN Section s            ON r.SectionId = s.Id
        UNION ALL /*前受計上の戻し*/
       SELECT cm.Code [CompanyCode]
            , r.Id
            , cs.Code [CustomerCode]
            , cs.Name [CustomerName]
            , '' [InvoiceCode]
            , NULL [BilledAt]
            , rct.Code [ReceiptCategoryCode]
            , rct.Name [ReceiptCategoryName]
            , r.RecordedAt
            , r.DueAt
            , r.ReceiptAmount * (-1) [Amount]
            , NULL [DepartmentCode]
            , NULL [DepartmentName]
            , ccy.Code [CurrencyCode]
            , r.ReceiptAmount * (-1) [ReceiptAmount]
            , '' [BillingNote1]
            , '' [BillingNote2]
            , '' [BillingNote3]
            , '' [BillingNote4]
            , r.Note1 [ReceiptNote1]
            , r.Note2 [ReceiptNote2]
            , r.Note3 [ReceiptNote3]
            , r.Note4 [ReceiptNote4]
            , r.BillNumber
            , r.BillBankCode
            , r.BillBranchCode
            , r.BillDrawAt
            , r.BillDrawer
            , NULL [BillingMemo]
            , r.Memo [ReceiptMemo]
            , NULL [MatchingMemo]
            , r.BankCode
            , r.BankName
            , r.BranchCode
            , r.BranchName
            , r.AccountNumber
            , r.SourceBankName
            , r.SourceBranchName
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN LEFT(r.PayerCode, LEN(r.PayerCode) - 7) ELSE '' END ELSE '' END [VirtualBranchCode]
            , CASE r.InputType WHEN 1 THEN CASE WHEN LEN(r.PayerCode) > 7 THEN RIGHT(r.PayerCode, 7) ELSE r.PayerCode END ELSE '' END [VirtualAccountNumber]
            , s.Code [SectionCode]
            , s.Name [SectionName]
            , rct.ExternalCode [ReceiptCategoryExternalCode]
            , r.OriginalReceiptId
            , '前受振替' [JournalizingCategory]
         FROM AdvanceReceivedBackup r
        INNER JOIN Company cm           ON r.CompanyId = cm.Id
               AND r.CompanyId = @CompanyId
               AND r.CurrencyId = @CurrencyId
{conditionReceipt}
        INNER JOIN Category rct         ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Currency ccy         ON r.CurrencyId = ccy.Id
         LEFT JOIN Customer cs          ON r.CustomerId = cs.Id
         LEFT JOIN ReceiptHeader rh     ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN Section s            ON r.SectionId = s.Id
";
            #endregion
            query += @"
       ) t
 ORDER BY t.CustomerCode, t.Id, t.ReceiptCategoryCode
";

            return dbHelper.GetItemsAsync<MatchedReceipt>(query, option, token);
        }

        public Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            if (option.OutputAt == null) option.OutputAt = new List<DateTime>();
            var query = @"
UPDATE m
   SET m.OutputAt       = NULL
     , m.UpdateAt       = @UpdateAt
     , m.UpdateBy       = @LoginUserId
  FROM MatchingHeader mh
 INNER JOIN Matching m
    ON mh.Id            = m.MatchingHeaderId
   AND mh.CompanyId     = @CompanyId
   AND mh.CurrencyId    = @CurrencyId
   AND mh.Approved      = 1
   AND m.OutputAt       IN @OutputAt
";
            return dbHelper.ExecuteAsync(query, option, token);
        }
        public Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE m
   SET m.OutputAt       = @UpdateAt
     , m.UpdateAt       = @UpdateAt
     , m.UpdateBy       = @LoginUserId
  FROM MatchingHeader mh
 INNER JOIN Matching m
    ON mh.Id            = m.MatchingHeaderId
   AND mh.CompanyId     = @CompanyId
   AND mh.CurrencyId    = @CurrencyId
   AND mh.Approved      = 1
   AND m.OutputAt       IS NULL
";
            if (option.RecordedAtFrom.HasValue) query += @"
   AND m.RecordedAt >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue)   query += @"
   AND m.RecordedAt <= @RecordedAtTo";
            return dbHelper.ExecuteAsync(query, option, token);
        }

        public Task<IEnumerable<MatchingJournalizingDetail>> GetDetailsAsync(JournalizingOption option, CancellationToken token)
        {
            #region query
            var query = @"
SELECT t.Id
     , t.HeaderId
     , t.Amount
     , t.OutputAt
     , t.CreateAt
     , t.RecordedAt
     , t.PayerName
     , t.ReceiptAmount
     , t.CustomerCode
     , t.CustomerName
     , t.BilledAt
     , t.BillingAmount
     , t.InvoiceCode
     , t.JournalizingType
     , t.CurrencyCode
     , t.UpdateAt
     , DENSE_RANK() OVER ( ORDER BY t.JournalizingType, t.HeaderId ) [GroupIndex]
  FROM ( /* 1 : 消込仕訳 */
       SELECT  m.Id    [Id]
             , mh.Id  [HeaderId]
             , m.Amount [Amount]
             , m.OutputAt
             , mh.CreateAt
             , r.RecordedAt
             , r.PayerName
             , r.ReceiptAmount
             , cs.Code  [CustomerCode]
             , cs.Name  [CustomerName]
             , b.BilledAt
             , b.BillingAmount
             , b.InvoiceCode
             , 1        [JournalizingType]
             , ccy.Code [CurrencyCode]
             , m.UpdateAt
          FROM MatchingHeader mh
         INNER JOIN Matching m
            ON mh.Id            = m.MatchingHeaderId
           AND mh.CompanyId     = @CompanyId
           AND m.OutputAt       IS NOT NULL
           AND mh.CreateAt      >= @CreateAtFrom
           AND mh.CreateAt      <= @CreateAtTo
           AND (@CurrencyId IS NULL OR mh.CurrencyId = @CurrencyId)
           AND EXISTS(
                SELECT 1
                  FROM MatchingHeader mh2
                 INNER JOIN Matching m2
                    ON mh2.Id           = m2.MatchingHeaderId
                   AND mh2.CompanyId    = mh.CompanyId
                 INNER JOIN Billing b2
                    ON mh.Id            = mh2.Id
                   AND m2.BillingId     = b2.Id
                   AND (@CustomerId IS NULL OR b2.CustomerId = @CustomerId)
                )
         INNER JOIN Currency ccy        ON mh.CurrencyId = ccy.Id
         INNER JOIN Receipt r           ON m.ReceiptId = r.Id
         INNER JOIN Billing b           ON m.BillingId = b.Id
         INNER JOIN Customer cs         ON b.CustomerId = cs.Id

         UNION ALL
         /* 2 : 前受計上 */
        SELECT r.Id    [Id]
             , r.Id    [HeaderId]
             , r.ReceiptAmount [Amount]
             , r.OutputAt
             , r.CreateAt
             , r.RecordedAt
             , r.PayerName
             , r.ReceiptAmount
             , cs.Code [CustomerCode]
             , cs.Name [CustomerName]
             , NULL     [BilledAt]
             , NULL     [BillingAmount]
             , ''       [InvoiceCode]
             , 2        [JournalizingType]
             , ccy.Code [CurrencyCode]
             , r.UpdateAt
        FROM Receipt r
       INNER JOIN Category rc
          ON r.CompanyId            = @CompanyId
         AND r.ReceiptCategoryId    = rc.Id
         AND rc.UseAdvanceReceived  = 1
         AND r.OutputAt             IS NOT NULL
         AND r.CreateAt             >= @CreateAtFrom
         AND r.CreateAt             <= @CreateAtTo
         AND (@CurrencyId IS NULL OR r.CurrencyId = @CurrencyId)
         AND (@CustomerId IS NULL OR r.CustomerId = @CustomerId)
       INNER JOIN Currency ccy      ON r.CurrencyId = ccy.Id
        LEFT JOIN Customer cs       ON r.CustomerId = cs.Id
        LEFT JOIN AdvanceReceivedBackup ar  ON r.OriginalReceiptId = ar.OriginalReceiptId
       WHERE ar.Id              IS NULL

         UNION ALL
         /* 3 : 前受振替 */
        SELECT r.Id    [Id]
             , r.Id    [HeaderId]
             , r.ReceiptAmount [Amount]
             , r.OutputAt
             , r.CreateAt
             , r.RecordedAt
             , r.PayerName
             , r.ReceiptAmount
             , cs.Code [CustomerCode]
             , cs.Name [CustomerName]
             , NULL     [BilledAt]
             , NULL     [BillingAmount]
             , ''       [InvoiceCode]
             , 3        [JournalizingType]
             , ccy.Code [CurrencyCode]
             , r.UpdateAt
        FROM Receipt r
       INNER JOIN Category rc
          ON r.CompanyId            = @CompanyId
         AND r.ReceiptCategoryId    = rc.Id
         AND rc.UseAdvanceReceived  = 1
         AND r.OutputAt             IS NOT NULL
         AND r.CreateAt             >= @CreateAtFrom
         AND r.CreateAt             <= @CreateAtTo
         AND (@CurrencyId IS NULL OR r.CurrencyId = @CurrencyId)
         AND (@CustomerId IS NULL OR r.CustomerId = @CustomerId)
       INNER JOIN Currency ccy      ON r.CurrencyId = ccy.Id
        LEFT JOIN Customer cs       ON r.CustomerId = cs.Id
       INNER JOIN AdvanceReceivedBackup ar  ON r.OriginalReceiptId = ar.OriginalReceiptId

         UNION ALL
         /* 4 : 対象外入金 */
        SELECT re.Id    [Id]
             , re.Id    [HeaderId]
             , re.ExcludeAmount [Amount]
             , re.OutputAt
             , re.CreateAt
             , r.RecordedAt
             , r.PayerName
             , r.ReceiptAmount
             , cs.Code  [CustomerCode]
             , cs.Name  [CustomerName]
             , NULL     [BilledAt]
             , NULL     [BillingAmount]
             , ''       [InvoiceCode]
             , 4        [JournalizingType]
             , ccy.Code [CurrencyCode]
             , re.UpdateAt
          FROM ReceiptExclude re
         INNER JOIN Receipt r
            ON re.ReceiptId     = r.Id
           AND r.CompanyId      = @CompanyId
           AND re.OutputAt      IS NOT NULL
           AND re.CreateAt      >= @CreateAtFrom
           AND re.CreateAt      <= @CreateAtTo
           AND (@CurrencyId IS NULL OR r.CurrencyId = @CurrencyId)
           AND (@CustomerId IS NULL OR r.CustomerId = @CustomerId)
         INNER JOIN Currency ccy    ON r.CurrencyId = ccy.Id
          LEFT JOIN Customer cs     ON r.CustomerId = cs.Id
    ) t 
";
            if (option.JournalizingTypes.Any())
                query += @"
 WHERE t.JournalizingType IN  @JournalizingTypes";
            query += @"
 ORDER BY t.OutputAt
     , t.JournalizingType
     , t.Id
     , t.CustomerCode
     , t.CurrencyCode
     , t.BilledAt
     , t.InvoiceCode
     , t.RecordedAt
";
            #endregion
            return dbHelper.GetItemsAsync<MatchingJournalizingDetail>(query, option, token);
        }

        public Task<int> CancelDetailAsync(Matching detail, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Matching 
   SET OutputAt         = NULL
     , UpdateAt         = @UpdateAt
     , UpdateBy         = @UpdateBy
 WHERE MatchingHeaderId = @Id
";
            return dbHelper.ExecuteAsync(query, detail, token);
        }

        public Task<IEnumerable<MatchingJournalizing>> MFExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            #region query
            var matchingCondition = "";
            var receiptCondition = "";
            var excludeCondition = "";
            var creditSubCodeCondition = "";

            //未出力時
            if (option.OutputAt == null || !option.OutputAt.Any())
            {
                matchingCondition = @" AND m.OutputAt IS NULL";
                receiptCondition = @" AND r.OutputAt IS NULL";
                excludeCondition = @" AND re.OutputAt IS NULL";
            }
            else
            {
                //再出力
                matchingCondition = @" AND m.OutputAt IN @OutputAt";
                receiptCondition = @" AND r.OutputAt IN @OutputAt";
                excludeCondition = @" AND re.OutputAt IN @OutputAt";
            }

            if (option.RecordedAtFrom.HasValue)
            {
                matchingCondition += @" AND m.RecordedAt >= @RecordedAtFrom";
                receiptCondition += @" AND r.RecordedAt >= @RecordedAtFrom";
                excludeCondition += @" AND r.RecordedAt >= @RecordedAtFrom";

            }
            if (option.RecordedAtTo.HasValue)
            {
                matchingCondition += @" AND m.RecordedAt <= @RecordedAtTo";
                receiptCondition += @" AND r.RecordedAt <= @RecordedAtTo";
                excludeCondition += @" AND r.RecordedAt <= @RecordedAtTo";
            }

            //補助科目コードに得意先名を出力する
            if (option.OutputCustoemrName)
            {
                creditSubCodeCondition = @"cs.Name";
            }
            else
            {
                creditSubCodeCondition = @"''";
            }

            var query = $@"
DECLARE
  @SuspenceReceiptDepartmentCode        nvarchar(10) --仮受部門コード
, @SuspenceReceiptAccountTitleCode      nvarchar(10) --仮受科目コード
, @SuspenceReceiptSubCode               nvarchar(10) --仮受補助コード
, @FeeDepartmentCode                    nvarchar(10) --振込手数料部門コード
, @FeeAccountTitleCode                  nvarchar(10) --振込手数料科目コード
, @FeeSubCode                           nvarchar(10) --振込手数料補助コード
, @DebitTaxDifferenceDepartmentCode     nvarchar(10) --借方消費税誤差部門コード
, @DebitTaxDifferenceAccountTitleCode   nvarchar(10) --借方消費税誤差科目コード
, @DebitTaxDifferenceSubCode            nvarchar(10) --借方消費税誤差補助コード
, @CreditTaxDifferenceDepartmentCode    nvarchar(10) --貸方消費税誤差部門コード
, @CreditTaxDifferenceAccountTitleCode  nvarchar(10) --貸方消費税誤差科目コード
, @CreditTaxDifferenceSubCode           nvarchar(10) --貸方消費税誤差補助コード

SET @SuspenceReceiptDepartmentCode        = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'仮受部門コード')
SET @SuspenceReceiptAccountTitleCode      = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'仮受科目コード')
SET @SuspenceReceiptSubCode               = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'仮受補助コード')
SET @FeeDepartmentCode                    = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'振込手数料部門コード')
SET @FeeAccountTitleCode                  = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'振込手数料科目コード')
SET @FeeSubCode                           = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'振込手数料補助コード')
SET @DebitTaxDifferenceDepartmentCode     = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'借方消費税誤差部門コード')
SET @DebitTaxDifferenceAccountTitleCode   = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'借方消費税誤差科目コード')
SET @DebitTaxDifferenceSubCode            = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'借方消費税誤差補助コード')
SET @CreditTaxDifferenceDepartmentCode    = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'貸方消費税誤差部門コード')
SET @CreditTaxDifferenceAccountTitleCode  = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'貸方消費税誤差科目コード')
SET @CreditTaxDifferenceSubCode           = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'貸方消費税誤差補助コード')

SELECT t.CompanyCode
     , t.RecordedAt
     , DENSE_RANK() OVER(ORDER BY t.CreateAt, t.Id) [SlipNumber]
     , t.DebitDepartmentCode
     , dd.Name  [DebitDepartmentName]
     , t.DebitAccountTitleCode
     , dat.Name [DebitAccountTitleName]
     , t.DebitSubCode
     , t.DebitSubName
     , t.CreditDepartmentCode
     , cd.Name  [CreditDepartmentName]
     , t.CreditAccountTitleCode
     , cat.Name [CreditAccountTitleName]
     , t.CreditSubCode
     , t.CreditSubName
     , t.Amount
     , t.Note
     , t.CustomerCode
     , cs.Name [CustomerName]
     , t.InvoiceCode
     , t.StaffCode
     , t.PayerCode
     , t.PayerName
     , t.SourceBankName
     , t.SourceBranchName
     , t.DueDate
     , t.BankCode
     , t.BankName
     , t.BranchCode
     , t.BranchName
     , t.AccountTypeId
     , t.AccountNumber
     , t.TaxClassId
     , t.CreateAt
     , t.CurrencyCode
     , t.Approved
     , t.Id
     , t.MatchingMemo
  FROM (
       --通常消込
       SELECT cm.Code [CompanyCode]
            , m.RecordedAt
            , mh.Id
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN ''
              ELSE @SuspenceReceiptDepartmentCode END   [DebitDepartmentCode]
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN COALESCE(rat.Code, '')
              ELSE @SuspenceReceiptAccountTitleCode END [DebitAccountTitleCode]
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN COALESCE(rct.SubCode, '')
              ELSE @SuspenceReceiptSubCode END          [DebitSubCode]
            , '' [DebitSubName]
            , CASE WHEN b.InputType = 3 THEN @SuspenceReceiptDepartmentCode
              ELSE d.Code END [CreditDepartmentCode]
            , CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rat.Code, '')
              ELSE bcat.Code END [CreditAccountTitleCode]
            , {creditSubCodeCondition} [CreditSubCode]
            , '' [CreditSubName]
            , m.Amount
            , b.Note1       [Note]
            , cs.Code       [CustomerCode]
            , b.InvoiceCode [InvoiceCode]
            , st.Code       [StaffCode]
            , ''            [PayerCode]
            , cs.Name       [PayerName]
            , ''            [SourceBankName]
            , ''            [SourceBranchName]
            , NULL          [DueDate]
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , rct.TaxClassId
            , m.CreateAt
            , ccy.Code      [CurrencyCode]
            , mh.Approved
            , mh.Memo       [MatchingMemo]
         FROM MatchingHeader mh
        INNER JOIN Matching m       ON mh.Id            = m.MatchingHeaderId
                                   AND mh.CompanyId     = @CompanyId
                                   AND mh.CurrencyId    = @CurrencyId
{matchingCondition}
        INNER JOIN Company cm                   ON mh.CompanyId = cm.Id
        INNER JOIN Receipt r                    ON m.ReceiptId = r.Id
        INNER JOIN Billing b                    ON m.BillingId = b.Id
        INNER JOIN Customer cs                  ON b.CustomerId = cs.Id
        INNER JOIN Staff st                     ON b.StaffId = st.Id
        INNER JOIN Department d                 ON b.DepartmentId = d.Id
        INNER JOIN Category rct                 ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Category cct                 ON b.CollectCategoryId = cct.Id
        INNER JOIN Currency ccy                 ON mh.CurrencyId = ccy.Id
        INNER JOIN Category bct                 ON b.BillingCategoryId = bct.Id
         LEFT JOIN AccountTitle bcat            ON bct.AccountTitleId = bcat.Id
         LEFT JOIN AccountTitle rat             ON rct.AccountTitleId = rat.Id
         LEFT JOIN AccountTitle bat             ON b.DebitAccountTitleId = bat.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN BillingScheduledIncome bsi   ON b.Id = bsi.BillingId
         LEFT JOIN Matching bsi_m               ON bsi.MatchingId = bsi_m.Id
         LEFT JOIN Receipt bsi_r                ON bsi_m.ReceiptId = bsi_r.Id
         LEFT JOIN Category bsi_rc              ON bsi_r.ReceiptCategoryId = bsi_rc.Id
         LEFT JOIN AccountTitle bsi_rat         ON bsi_rc.AccountTitleId = bsi_rat.Id

       --手数料
       UNION ALL
       SELECT cm.Code              [CompanyCode]
            , m.RecordedAt
            , mh.Id
            , @FeeDepartmentCode    [DebitDepartmentCode]
            , @FeeAccountTitleCode  [DebitAccountTitleCode]
            , @FeeSubCode           [DebitSubCode]
            , ''                    [DebitSubName]
            , CASE WHEN b.InputType = 3 THEN @SuspenceReceiptDepartmentCode
              ELSE d.Code END       [CreditDepartmentCode]
            , CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rat.Code, '')
              ELSE bcat.Code END [CreditAccountTitleCode]
            , {creditSubCodeCondition} [CreditSubCode]
            , ''                    [CreditSubName]
            , m.BankTransferFee     [Amount]
            , b.Note1               [Note]
            , cs.Code               [CustomerCode]
            , b.InvoiceCode
            , st.Code               [StaffCode]
            , ''                    [PayerCode]
            , cs.Name               [PayerName]
            , ''                    [SourceBankName]
            , ''                    [SourceBranchName]
            , NULL                  [DueDate]
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , rct.TaxClassId
            , m.CreateAt
            , ccy.Code      [CurrencyCode]
            , mh.Approved
            , mh.Memo       [MatchingMemo]
         FROM MatchingHeader mh
        INNER JOIN Matching m                   ON mh.Id = m.MatchingHeaderId
                                               AND mh.CompanyId = @CompanyId
                                               AND mh.CurrencyId = @CurrencyId
                                               AND m.BankTransferFee <> 0
{matchingCondition}
        INNER JOIN Company cm                   ON mh.CompanyId = cm.Id
        INNER JOIN Receipt r                    ON m.ReceiptId = r.Id
        INNER JOIN Billing b                    ON m.BillingId = b.Id
        INNER JOIN Customer cs                  ON b.CustomerId = cs.Id
        INNER JOIN Staff st                     ON b.StaffId = st.Id
        INNER JOIN Department d                 ON b.DepartmentId = d.Id
        INNER JOIN Category rct                 ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Category cct                 ON b.CollectCategoryId = cct.Id
        INNER JOIN Currency ccy                 ON mh.CurrencyId = ccy.Id
        INNER JOIN Category bct                 ON b.BillingCategoryId = bct.Id
         LEFT JOIN AccountTitle bcat            ON bct.AccountTitleId = bcat.Id
         LEFT JOIN AccountTitle bat             ON b.DebitAccountTitleId = bat.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN BillingScheduledIncome bsi   ON b.Id = bsi.BillingId
         LEFT JOIN Matching bsi_m               ON bsi.MatchingId = bsi_m.Id
         LEFT JOIN Receipt bsi_r                ON bsi_m.ReceiptId = bsi_r.Id
         LEFT JOIN Category bsi_rc              ON bsi_r.ReceiptCategoryId = bsi_rc.Id
         LEFT JOIN AccountTitle bsi_rat         ON bsi_rc.AccountTitleId = bsi_rat.Id

       --消費税誤差
       UNION ALL
       SELECT cm.Code [CompanyCode]
            , m.RecordedAt
            , mh.Id
            , CASE WHEN m.TaxDifference < 0 THEN @DebitTaxDifferenceDepartmentCode
              ELSE @SuspenceReceiptDepartmentCode END   [DebitDepartmentCode]
            , CASE WHEN m.TaxDifference < 0 THEN @DebitTaxDifferenceAccountTitleCode
              ELSE @SuspenceReceiptAccountTitleCode END [DebitAccountTitleCode]
            , CASE WHEN m.TaxDifference < 0 THEN @DebitTaxDifferenceSubCode
              ELSE @SuspenceReceiptSubCode END [DebitSubCode]
            , ''                   [DebitSubName]
            , CASE WHEN m.TaxDifference > 0 THEN @CreditTaxDifferenceDepartmentCode
              ELSE CASE WHEN b.InputType = 3 THEN @SuspenceReceiptDepartmentCode
                   ELSE d.Code END 
              END [CreditDepartmentCode]
            , CASE WHEN m.TaxDifference > 0 THEN @CreditTaxDifferenceAccountTitleCode
              ELSE CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rat.Code, '')
                   ELSE bcat.Code END
              END [CreditAccountTitleCode]
            , {creditSubCodeCondition} [CreditSubCode]
            , ''                    [CreditSubName]
            , CASE WHEN m.TaxDifference < 0 THEN -1 * m.TaxDifference ELSE m.TaxDifference END [Amount]
            , b.Note1               [Note]
            , cs.Code               [CustomerCode]
            , b.InvoiceCode
            , st.Code               [StaffCode]
            , ''                    [PayerCode]
            , cs.Name               [PayerName]
            , ''                    [SourceBankName]
            , ''                    [SourceBranchName]
            , NULL                  [DueDate]
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , rct.TaxClassId
            , m.CreateAt
            , ccy.Code      [CurrencyCode]
            , mh.Approved
            , mh.Memo       [MatchingMemo]
         FROM MatchingHeader mh
        INNER JOIN Matching m                   ON mh.Id = m.MatchingHeaderId
                                               AND mh.CompanyId = @CompanyId
                                               AND mh.CurrencyId = @CurrencyId
                                               AND m.TaxDifference <> 0
{matchingCondition}
        INNER JOIN Company cm                   ON mh.CompanyId = cm.Id
        INNER JOIN Receipt r                    ON m.ReceiptId = r.Id
        INNER JOIN Billing b                    ON m.BillingId = b.Id
        INNER JOIN Customer cs                  ON b.CustomerId = cs.Id
        INNER JOIN Staff st                     ON b.StaffId = st.Id
        INNER JOIN Department d                 ON b.DepartmentId = d.Id
        INNER JOIN Category rct                 ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Category cct                 ON b.CollectCategoryId = cct.Id
        INNER JOIN Currency ccy                 ON mh.CurrencyId = ccy.Id
        INNER JOIN Category bct                 ON b.BillingCategoryId = bct.Id
         LEFT JOIN AccountTitle bcat            ON bct.AccountTitleId = bcat.Id
         LEFT JOIN AccountTitle bat             ON b.DebitAccountTitleId = bat.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN BillingScheduledIncome bsi   ON b.Id = bsi.BillingId
         LEFT JOIN Matching bsi_m               ON bsi.MatchingId = bsi_m.Id
         LEFT JOIN Receipt bsi_r                ON bsi_m.ReceiptId = bsi_r.Id
         LEFT JOIN Category bsi_rc              ON bsi_r.ReceiptCategoryId = bsi_rc.Id
         LEFT JOIN AccountTitle bsi_rat         ON bsi_rc.AccountTitleId = bsi_rat.Id
";

            //歩引額
            //（歩引機能ON の場合のみ）
            if (option.UseDiscount) query += $@"
       UNION ALL
       SELECT 
              cm.Code    [CompanyCode]
            , m.RecordedAt
            , mh.Id
            , cdd.Code   [DebitDepartmentCode]
            , cdat.Code  [DebitAccountTitleCode]
            , cd.SubCode [DebitSubCode]
            , ''         [DebitSubName]
            , CASE WHEN b.InputType = 3 THEN @SuspenceReceiptDepartmentCode
                   ELSE d.Code   END [CreditDepartmentCode]
            , CASE WHEN b.InputType = 3 THEN COALESCE(bsi_rat.Code, '')
                   ELSE bcat.Code END [CreditAccountTitleCode]
            , {creditSubCodeCondition} [CreditSubCode]
            , '' [CreditSubName]
            , mbd.DiscountAmount [Amount]
            , b.Note1 [Note]
            , cs.Code [CustomerCode]
            , b.InvoiceCode
            , st.Code [StaffCode]
            , ''            [PayerCode]
            , cs.Name       [PayerName]
            , ''            [SourceBankName]
            , ''            [SourceBranchName]
            , NULL          [DueDate]
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , rct.TaxClassId
            , m.CreateAt
            , ccy.Code      [CurrencyCode]
            , mh.Approved
            , mh.Memo       [MatchingMemo]
         FROM MatchingHeader mh
        INNER JOIN Matching m                   ON mh.Id = m.MatchingHeaderId
                                               AND mh.CompanyId = @CompanyId
                                               AND mh.CurrencyId = @CurrencyId
{matchingCondition}
        INNER JOIN Company cm                   ON mh.CompanyId = cm.Id
        INNER JOIN MatchingBillingDiscount mbd  ON m.Id = mbd.MatchingId
        INNER JOIN Receipt r                    ON m.ReceiptId = r.Id
        INNER JOIN Billing b                    ON m.BillingId = b.Id
        INNER JOIN Customer cs                  ON b.CustomerId = cs.Id
        INNER JOIN Staff st                     ON b.StaffId = st.Id
        INNER JOIN Department d                 ON b.DepartmentId = d.Id
        INNER JOIN Category rct                 ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Category cct                 ON b.CollectCategoryId = cct.Id
        INNER JOIN Currency ccy                 ON mh.CurrencyId = ccy.Id
        INNER JOIN CustomerDiscount cd          ON b.CustomerId = cd.CustomerId
                                               AND mbd.DiscountType = cd.[Sequence]
        INNER JOIN Category bct                 ON b.BillingCategoryId = bct.Id
         LEFT JOIN AccountTitle bcat            ON bct.AccountTitleId = bcat.Id
         LEFT JOIN Department cdd               ON cd.DepartmentId = cdd.Id
         LEFT JOIN AccountTitle cdat            ON cd.AccountTitleId = cdat.Id
         LEFT JOIN AccountTitle bat             ON b.DebitAccountTitleId = bat.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN BillingScheduledIncome bsi   ON b.Id = bsi.BillingId
         LEFT JOIN Matching bsi_m               ON bsi.MatchingId = bsi_m.Id
         LEFT JOIN Receipt bsi_r                ON bsi_m.ReceiptId = bsi_r.Id
         LEFT JOIN Category bsi_rc              ON bsi_r.ReceiptCategoryId = bsi_rc.Id
         LEFT JOIN AccountTitle bsi_rat         ON bsi_rc.AccountTitleId = bsi_rat.Id
";

            query += $@"
       --前受仕訳
       UNION ALL
       SELECT 
              cm.Code                          [CompanyCode]
            , r.RecordedAt
            , r.Id
            , @SuspenceReceiptDepartmentCode   [DebitDepartmentCode]
            , @SuspenceReceiptAccountTitleCode [DebitAccountTitleCode]
            , @SuspenceReceiptSubCode          [DebitSubCode]
            , ''                               [DebitSubName]
            , ''                               [CreditDepartmentCode]
            , COALESCE(rat.Code, '')           [CreditAccountTitleCode]
            , {creditSubCodeCondition} [CreditSubCode]
            , ''                               [CreditSubName]
            , r.ReceiptAmount                  [Amount]
            , r.Note1                          [Note]
            , cs.Code                          [CustomerCode]
            , ''                               [InvoiceCode]
            , ''                               [StaffCode]
            , r.PayerCode
            , r.PayerName
            , r.SourceBankName
            , r.SourceBranchName
            , r.DueAt
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , NULL                           [TaxClassId]
            , r.CreateAt
            , ccy.Code                       [CurrencyCode]
            , 1                              [Approved]
            , ''                             [MatchingMemo]
         FROM Receipt r
        INNER JOIN Category rct                 ON r.ReceiptCategoryId  = rct.Id
                                               AND r.CompanyId          = @CompanyId
                                               AND r.CurrencyId         = @CurrencyId
                                               AND rct.UseAdvanceReceived = 1
                                               AND r.Apportioned = 1
                                               AND r.DeleteAt IS NULL
{receiptCondition}
        INNER JOIN Company cm                   ON r.CompanyId = cm.Id
        INNER JOIN Currency ccy                 ON r.CurrencyId = ccy.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN AccountTitle rat             ON rct.AccountTitleId = rat.Id
         LEFT JOIN Customer cs                  ON r.CustomerId = cs.Id

       --対象外仕訳
       UNION ALL
       SELECT 
              cm.Code [CompanyCode]
            , r.RecordedAt
            , r.Id
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN ''
                   ELSE @SuspenceReceiptDepartmentCode END   [DebitDepartmentCode]
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN COALESCE(rat.Code, '')
                   ELSE @SuspenceReceiptAccountTitleCode END [DebitAccountTitleCode]
            , CASE WHEN rct.UseAdvanceReceived = 1 THEN COALESCE(rct.SubCode, '')
                   ELSE @SuspenceReceiptSubCode END          [DebitSubCode]
            , '' [DebitSubName]
            , @SuspenceReceiptDepartmentCode   [CreditDepartmentCode]
            , COALESCE(reat.Code, '')          [CreditAccountTitleCode]
            , ''                               [CreditSubCode]
            , ''                               [CreditSubName]
            , re.ExcludeAmount                 [Amount]
            , rect.Note
            , cs.Code                          [CustomerCode]
            , ''                               [InvoiceCode]
            , ''                               [StaffCode]
            , r.PayerCode
            , r.PayerName
            , r.SourceBankName
            , r.SourceBranchName
            , r.DueAt
            , r.[BankCode]
            , r.[BankName]
            , r.[BranchCode]
            , r.[BranchName]
            , r.[AccountTypeId]
            , r.[AccountNumber]
            , NULL                           [TaxClassId]
            , re.CreateAt
            , ccy.Code                       [CurrencyCode]
            , 1                              [Approved]
            , ''                             [MatchingMemo]
         FROM Receipt r
        INNER JOIN ReceiptExclude re            ON r.Id = re.ReceiptId
                                               AND r.CompanyId = @CompanyId
                                               AND r.CurrencyId = @CurrencyId
                                               AND r.Apportioned = 1
                                               AND r.DeleteAt IS NULL
{excludeCondition}
        INNER JOIN Company cm                   ON r.CompanyId = cm.Id
        INNER JOIN Category rect                ON re.ExcludeCategoryId = rect.Id
        INNER JOIN Category rct                 ON r.ReceiptCategoryId = rct.Id
        INNER JOIN Currency ccy                 ON r.CurrencyId = ccy.Id
         LEFT JOIN ReceiptHeader rh             ON r.ReceiptHeaderId = rh.Id
         LEFT JOIN AccountTitle rat             ON rct.AccountTitleId = rat.Id
         LEFT JOIN AccountTitle reat            ON rect.AccountTitleId = reat.Id
         LEFT JOIN Customer cs                  ON r.CustomerId = cs.Id
       ) t
  LEFT JOIN Department dd                   ON dd.CompanyId = @CompanyId
                                           AND t.DebitDepartmentCode = dd.Code
  LEFT JOIN AccountTitle dat                ON dat.CompanyId = @CompanyId
                                           AND t.DebitAccountTitleCode = dat.Code
  LEFT JOIN Department cd                   ON cd.CompanyId = @CompanyId
                                           AND t.CreditDepartmentCode = cd.Code
  LEFT JOIN AccountTitle cat                ON cat.CompanyId = @CompanyId
                                           AND t.CreditAccountTitleCode = cat.Code
  LEFT JOIN Customer cs                     ON cs.CompanyId = @CompanyId
                                           AND cs.Code = t.CustomerCode
  ORDER BY t.CreateAt, t.Id
";
            #endregion
            return dbHelper.GetItemsAsync<MatchingJournalizing>(query, option, token);
        }

    }
}
