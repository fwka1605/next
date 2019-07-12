using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class MatchingHistorySearchQueryProcessor :
        IMatchingHistorySearchQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public MatchingHistorySearchQueryProcessor (IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        #region query getasync

        private string GetQueryInitializeTempTable()
        {
            return @"
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id = object_id(N'tempdb..#headerIds'))
DROP TABLE #headerIds;

CREATE TABLE #headerIds
([Id]                   BIGINT
,PRIMARY KEY ([Id]))

IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id = object_id(N'tempdb..#billing'))
DROP TABLE #billing;

CREATE TABLE #billing
(
   MatchingHeaderId             BIGINT  NOT NULL
 , RowNumber                    INT     NOT NULL
 , BillingId                    BIGINT
 , RowType                      INT
 , BillingAmount                NUMERIC(18, 5)
 , BillingAmountExcludingTax    NUMERIC(18, 5)
 , TaxAmount                    NUMERIC(18, 5)
 , MatchingAmount               NUMERIC(18, 5)
 , BillingRemain                NUMERIC(18, 5)
 , BillingCategoryId            INT
 , DiscountType                 INT
 , PRIMARY KEY (
      MatchingHeaderId
    , RowNumber
   )
)
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id = object_id(N'tempdb..#receipt'))
DROP TABLE #receipt;

CREATE TABLE #receipt
(
   MatchingHeaderId             BIGINT  NOT NULL
 , RowNumber                    INT     NOT NULL
 , ReceiptId                    BIGINT
 , RowType                      INT
 , ReceiptAmount                NUMERIC(18, 5)
 , ReceiptRemain                NUMERIC(18, 5)
 , AdvanceReceivedOccured       INT
 , ReceiptCategoryId            INT
 , PRIMARY KEY (
    MatchingHeaderId
  , RowNumber
  )
)

IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id = object_id(N'tempdb..#matching'))
DROP TABLE #matching;

CREATE TABLE #matching
(
   MatchingHeaderId             BIGINT NOT NULL
 , RowNumber                    INT    NOT NULL
 , BillingId                    BIGINT
 , BillingAmount                NUMERIC(18, 5)
 , BillingAmountExcludingTax    NUMERIC(18, 5)
 , TaxAmount                    NUMERIC(18, 5)
 , MatchingAmount               NUMERIC(18, 5)
 , BillingRemain                NUMERIC(18, 5)
 , BillingCategoryCode          NVARCHAR(20)
 , BillingCategoryName          NVARCHAR(40)
 , ReceiptId                    BIGINT
 , ReceiptAmount                NUMERIC(18, 5)
 , ReceiptRemain                NUMERIC(18, 5)
 , AdvanceReceivedOccured       INT
 , ReceiptCategoryCode          NVARCHAR(20)
 , ReceiptCategoryName          NVARCHAR(40)
 , PRIMARY KEY (MatchingHeaderId, RowNumber)
)

";
        }

        private string GetQueryInitializeMatchingHeaderId(MatchingHistorySearch option)
        {
            var builder = new StringBuilder(@"
INSERT INTO #headerIds
SELECT DISTINCT mh.Id
  FROM [MatchingHeader] mh
 INNER JOIN [Matching] m        ON mh.Id        = m.MatchingHeaderId
 INNER JOIN [Billing] b         ON b.Id         = m.BillingId
 INNER JOIN [Receipt] r         ON r.Id         = m.ReceiptId
 INNER JOIN [Customer] cs       ON cs.Id        = b.CustomerId
 INNER JOIN [Department] dp     ON dp.Id        = b.DepartmentId
 INNER JOIN [Currency] ccy      ON ccy.Id       = mh.CurrencyId
  LEFT JOIN [ReceiptHeader] rh  ON rh.Id        = r.ReceiptHeaderId
  LEFT JOIN [Section] sc        ON sc.Id        = r.SectionId
  LEFT JOIN [LoginUser] lu      ON lu.Id        = mh.CreateBy
");
            if (option.OnlyNonOutput) builder.Append(@"
  LEFT JOIN MatchingOutputed mo ON mh.Id        = mo.MatchingHeaderId");
            builder.Append(@"
 WHERE mh.CompanyId             = @CompanyId");
            if (option.MatchingProcessType.HasValue) builder.Append(@"
   AND mh.MatchingProcessType   = @MatchingProcessType");
            if (option.OnlyNonOutput) builder.Append(@"
   AND mo.MatchingHeaderId      IS NULL");
            if (option.BillingAmountFrom.HasValue) builder.Append(@"
   AND b.BillingAmount         >= @BillingAmountFrom");
            if (option.BillingAmountTo.HasValue) builder.Append(@"
   AND b.BillingAmount         <= @BillingAmountTo");
            if (option.BillingCategoryId.HasValue) builder.Append(@"
   AND b.BillingCategoryId      = @BillingCategoryId");
            if (option.ReceiptAmountFrom.HasValue) builder.Append(@"
   AND r.ReceiptAmount         >= @ReceiptAmountFrom");
            if (option.ReceiptAmountTo.HasValue) builder.Append(@"
   AND r.ReceiptAmount         <= @ReceiptAmountTo");
            if (option.ReceiptIdFrom.HasValue) builder.Append(@"
   AND r.Id                    >= @ReceiptIdFrom");
            if (option.ReceiptIdTo.HasValue) builder.Append(@"
   AND r.Id                    <= @ReceiptIdTo");
            if (option.RecordedAtFrom.HasValue) builder.Append(@"
   AND r.RecordedAt            >= @RecordedAtFrom");
            if (option.RecordedAtTo.HasValue) builder.Append(@"
   AND r.RecordedAt            <= @RecordedAtTo");
            if (option.InputAtFrom.HasValue) builder.Append(@"
   AND r.CreateAt              >= @InputAtFrom");
            if (option.InputAtTo.HasValue) builder.Append(@"
   AND r.CreateAt              <= @InputAtTo");
            if (option.CreateAtFrom.HasValue) builder.Append(@"
   AND m.CreateAt              >= @CreateAtFrom");
            if (option.CreateAtTo.HasValue) builder.Append(@"
   AND m.CreateAt              <= @CreateAtTo");
            if (!string.IsNullOrEmpty(option.CustomerCodeFrom)) builder.Append(@"
   AND cs.Code                 >= @CustomerCodeFrom");
            if (!string.IsNullOrEmpty(option.CustomerCodeTo)) builder.Append(@"
   AND cs.Code                 <= @CustomerCodeTo");
            if (!string.IsNullOrEmpty(option.DepartmentCodeFrom)) builder.Append(@"
   AND dp.Code                 >= @DepartmentCodeFrom");
            if (!string.IsNullOrEmpty(option.DepartmentCodeTo)) builder.Append(@"
   AND dp.Code                 <= @DepartmentCodeTo");
            if (!string.IsNullOrEmpty(option.SectionCodeFrom)) builder.Append(@"
   AND sc.Code                 >= @SectionCodeFrom");
            if (!string.IsNullOrEmpty(option.SectionCodeTo)) builder.Append(@"
   AND sc.Code                 <= @SectionCodeTo");
            if (!string.IsNullOrEmpty(option.LoginUserFrom)) builder.Append(@"
   AND lu.Code                 >= @LoginUserFrom");
            if (!string.IsNullOrEmpty(option.LoginUserTo)) builder.Append(@"
   AND lu.Code                 <= @LoginUserTo");
            if (!string.IsNullOrEmpty(option.BankCode)) builder.Append(@"
   AND r.BankCode              = @BankCode");
            if (!string.IsNullOrEmpty(option.BranchCode)) builder.Append(@"
   AND r.BranchCode            = @BranchCode");
            if (option.AccountType.HasValue) builder.Append(@"
   AND r.AccountTypeId         = @AccountType");
            if (!string.IsNullOrEmpty(option.AccountNumber)) builder.Append(@"
   AND r.AccountNumber         = @AccountNumber");
            if (!string.IsNullOrEmpty(option.CurrencyCode)) builder.Append(@"
   AND ccy.Code                 = @CurrencyCode");

            if (option.UseSectionMaster) builder.Append(@"
   AND r.SectionId IN (SELECT SectionId  FROM SectionWithLoginUser WHERE LoginUserId = @LoginUserId) ");

            if (option.ExistsMemo) builder.Append(@"
   AND mh.Memo                  > N''");
            if (!string.IsNullOrEmpty(option.Memo))
            {
                option.Memo = Sql.GetWrappedValue(option.Memo);
                builder.Append(@"
   AND mh.Memo               LIKE @Memo");
            }
            if (!string.IsNullOrEmpty(option.PayerName))
            {
                option.PayerName = Sql.GetWrappedValue(option.PayerName);
                builder.Append(@"
   AND r.PayerName             LIKE @PayerName");
            }
            return builder.ToString();
        }

        private string GetQueryInsertBillingData()
        {
            return @"
INSERT INTO #billing
SELECT
       MatchingHeaderId
     , ROW_NUMBER()
          OVER(PARTITION BY MatchingHeaderId
                   ORDER BY BillingId, RowType) AS RowNumber
     , BillingId
     , RowType
     , BillingAmount
     , BillingAmountExcludingTax
     , TaxAmount
     , MatchingAmount
     , BillingRemain
     , BillingCategoryId
     , DiscountType
  FROM (
SELECT mh.Id MatchingHeaderId
     , 0 AS RowType
     , m.BillingId
     , MIN(b.BillingAmount)  [BillingAmount]
     , MIN(b.BillingAmount) - MIN(b.TaxAmount)  [BillingAmountExcludingTax]
     , MIN(b.TaxAmount)  [TaxAmount]
     , SUM(m.Amount
          + m.BankTransferFee
          + COALESCE(mbd.DiscountAmount, 0)
          - CASE WHEN m.TaxDifference < 0 THEN m.TaxDifference ELSE 0 END) [MatchingAmount]
     , MIN(m.BillingRemain) AS BillingRemain
     , MIN( b.BillingCategoryId ) [BillingCategoryId]
     , CAST( NULL AS INT ) [DiscountType]
  FROM #headerIds ids
 INNER JOIN MatchingHeader mh           ON ids.Id   = mh.Id
 INNER JOIN Matching m                  ON mh.Id    = m.MatchingHeaderId
 INNER JOIN Billing b                   ON b.Id     = m.BillingId
 INNER JOIN Category ct                 ON ct.Id    = b.BillingCategoryId
  LEFT JOIN (
    SELECT MatchingId
         , SUM(DiscountAmount) DiscountAmount
      FROM MatchingBillingDiscount
     GROUP BY MatchingId
  ) mbd
    ON m.Id = mbd.MatchingId
 GROUP BY mh.Id
     , m.BillingId
     , ct.Code
     , ct.Name
 UNION ALL 
SELECT mh.Id MatchingHeaderId
     , 1 AS RowType
     , m.BillingId
     , NULL  [BillingAmount]
     , NULL  [BillingAmountExcludingTax]
     , NULL  [TaxAmount]
     , SUM(m.TaxDifference) [MatchingAmount]
     , NULL [BillingRemain]
     , NULL [BillingCategoryId]
     , NULL [DiscountType]
  FROM #headerIds ids
 INNER JOIN MatchingHeader mh           ON ids.Id   = mh.Id
 INNER JOIN Matching m                  ON mh.Id    = m.MatchingHeaderId
                                       AND 0        < m.TaxDifference
 GROUP BY mh.Id
     , m.BillingId
 UNION ALL 
SELECT mh.Id MatchingHeaderId
     , 2 AS RowType
     , m.BillingId
     , NULL  [BillingAmount]
     , NULL  [BillingAmountExcludingTax]
     , NULL  [TaxAmount]
     , SUM(mbd.DiscountAmount) * (-1) [MatchingAmount]
     , NULL  [BillingRemain]
     , NULL  [BillingCategoryId]
     , mbd.DiscountType
  FROM #headerIds ids
 INNER JOIN MatchingHeader mh           ON ids.Id   = mh.Id
 INNER JOIN Matching m                  ON mh.Id    = m.MatchingHeaderId
 INNER JOIN MatchingBillingDiscount mbd ON m.Id     = mbd.MatchingId
 GROUP BY mh.Id
     , m.BillingId
     , mbd.DiscountType
) t

";
        }

        private string GetQueryInsertReceiptData()
        {
            return @"
INSERT INTO #receipt
SELECT
  MatchingHeaderId
, ROW_NUMBER()
  OVER(PARTITION BY MatchingHeaderId
           ORDER BY ReceiptId, RowType) AS RowNumber
, ReceiptId
, RowType
, ReceiptAmount
, ReceiptRemain
, AdvanceReceivedOccured
, ReceiptCategoryId
FROM (

SELECT mh.Id MatchingHeaderId
     , 0 AS RowType
     , m.ReceiptId
     , MIN(r.ReceiptAmount) [ReceiptAmount]
     , MIN(m.ReceiptRemain) [ReceiptRemain]
     , MAX(m.AdvanceReceivedOccured) [AdvanceReceivedOccured]
     , MIN( r.ReceiptCategoryId ) [ReceiptCategoryId]
  FROM #headerIds ids
 INNER JOIN MatchingHeader mh       ON ids.Id       = mh.Id
 INNER JOIN Matching m              ON mh.Id        = m.MatchingHeaderId
 INNER JOIN Receipt r               ON r.Id         = m.ReceiptId
 INNER JOIN Category ct             ON ct.Id        = r.ReceiptCategoryId
 GROUP BY mh.Id
     , m.ReceiptId
 UNION ALL 
SELECT mh.Id MatchingHeaderId
     , 1 AS RowType
     , m.ReceiptId
     , SUM(m.BankTransferFee) [ReceiptAmount]
     , NULL [ReceiptRemain]
     , NULL [AdvanceReceivedOccured]
     , NULL [ReceiptCategoryId]
  FROM #headerIds ids
 INNER JOIN MatchingHeader mh       ON ids.Id       = mh.Id
 INNER JOIN Matching m              ON mh.Id        = m.MatchingHeaderId
                                   AND m.BankTransferFee <> 0
 GROUP BY mh.Id
     , m.ReceiptId
 UNION ALL 
SELECT mh.Id MatchingHeaderId
     , 2 AS RowType
     , m.ReceiptId
     , SUM(m.TaxDifference) * (-1) [ReceiptAmount]
     , NULL [ReceiptRemain]
     , NULL [AdvanceReceivedOccured]
     , NULL [ReceiptCategoryId]
  FROM #headerIds ids
 INNER JOIN MatchingHeader mh       ON ids.Id       = mh.Id
 INNER JOIN Matching m              ON mh.Id        = m.MatchingHeaderId
                                   AND m.TaxDifference < 0
 GROUP BY mh.Id
     , m.ReceiptId
) t

";
        }

        private string GetQueryInsertMatchingWork()
        {
            return @"
INSERT INTO #matching
SELECT ks.MatchingHeaderId
     , ks.RowNumber
     , ks.BillingId
     , ks.BillingAmount
     , ks.BillingAmountExcludingTax
     , ks.TaxAmount
     , ks.MatchingAmount
     , ks.BillingRemain
     , CASE ks.RowType
        WHEN 0 THEN bct.[Code]
        WHEN 1 THEN N'BT'
        WHEN 2 THEN N'D' + CAST(ks.DiscountType AS NVARCHAR(1))
       END [BillingCategoryCode]
     , CASE ks.RowType
        WHEN 0 THEN bct.[Name]
        WHEN 1 THEN N'消費税誤差'
        WHEN 2 THEN N'歩引額' + CAST(ks.DiscountType AS NVARCHAR(1))
       END [BillingCategoryName]
     , kn.ReceiptId
     , kn.ReceiptAmount
     , kn.ReceiptRemain
     , kn.AdvanceReceivedOccured
     , CASE kn.RowType
        WHEN 0 THEN rct.[Code]
        WHEN 1 THEN N'CH'
        WHEN 2 THEN N'RT'
       END [ReceiptCategoryCode]
     , CASE kn.RowType
        WHEN 0 THEN rct.[Name]
        WHEN 1 THEN N'手数料'
        WHEN 2 THEN N'消費税誤差'
       END [ReceiptCategoryName]
  FROM #billing ks
  LEFT JOIN #receipt kn
    ON ks.MatchingHeaderId = kn.MatchingHeaderId
   AND ks.RowNumber = kn.RowNumber
  LEFT JOIN Category bct        ON bct.Id   = ks.BillingCategoryId
  LEFT JOIN Category rct        ON rct.Id   = kn.ReceiptCategoryId
 UNION
SELECT kn.MatchingHeaderId
     , kn.RowNumber
     , ks.BillingId
     , ks.BillingAmount
     , ks.BillingAmountExcludingTax
     , ks.TaxAmount
     , ks.MatchingAmount
     , ks.BillingRemain
     , CASE ks.RowType
        WHEN 0 THEN bct.[Code]
        WHEN 1 THEN N'BT'
        WHEN 2 THEN N'D' + CAST(ks.DiscountType AS NVARCHAR(1))
       END [BillingCategoryCode]
     , CASE ks.RowType
        WHEN 0 THEN bct.[Name]
        WHEN 1 THEN N'消費税誤差'
        WHEN 2 THEN N'歩引額' + CAST(ks.DiscountType AS NVARCHAR(1))
       END [BillingCategoryName]
     , kn.ReceiptId
     , kn.ReceiptAmount
     , kn.ReceiptRemain
     , kn.AdvanceReceivedOccured
     , CASE kn.RowType
        WHEN 0 THEN rct.[Code]
        WHEN 1 THEN N'CH'
        WHEN 2 THEN N'RT'
       END [ReceiptCategoryCode]
     , CASE kn.RowType
        WHEN 0 THEN rct.[Name]
        WHEN 1 THEN N'手数料'
        WHEN 2 THEN N'消費税誤差'
       END [ReceiptCategoryName]
  FROM #receipt kn
  LEFT JOIN #billing ks
    ON kn.MatchingHeaderId = ks.MatchingHeaderId
   AND kn.RowNumber = ks.RowNumber
  LEFT JOIN Category bct        ON bct.Id   = ks.BillingCategoryId
  LEFT JOIN Category rct        ON rct.Id   = kn.ReceiptCategoryId
;

";
        }

        private string GetQuerySelectHisotryData(MatchingHistorySearch option)
        {
            var builder = new StringBuilder(@"
SELECT mh.CreateAt
     , mh.Memo     MatchingMemo
     , mh.MatchingProcessType
     , mh.Id
     , sc.Code     SectionCode
     , sc.Name     SectionName
     , d.Code      DepartmentCode
     , d.Name      DepartmentName
     , cs.Code     CustomerCode
     , cs.Name     CustomerName
     , b.BilledAt
     , b.SalesAt
     , b.InvoiceCode
     , tbl.BillingCategoryCode
     , tbl.BillingCategoryName
     , tbl.BillingAmount
     , tbl.BillingAmountExcludingTax
     , tbl.TaxAmount
     , tbl.MatchingAmount
     , tbl.BillingRemain
     , b.Note1     BillingNote1
     , b.Note2     BillingNote2
     , b.Note3     BillingNote3
     , b.Note4     BillingNote4
     , r.RecordedAt
     , tbl.ReceiptId
     , tbl.ReceiptCategoryCode
     , tbl.ReceiptCategoryName
     , tbl.ReceiptAmount
     , tbl.AdvanceReceivedOccured
     , tbl.ReceiptRemain
     , r.PayerName
     , r.BankCode
     , r.BankName
     , r.BranchCode
     , r.BranchName
     , r.AccountNumber
     , r.Note1     ReceiptNote1
     , r.Note2     ReceiptNote2
     , r.Note3     ReceiptNote3
     , r.Note4     ReceiptNote4
     , r.PayerCode
     , u.Code      LoginUserCode
     , u.Name      LoginUserName
     , mh.TaxDifference
     , mh.BankTransferFee
     , b.Id        BillingId
     , tbl.MatchingHeaderId
     , cc.Code  [CollectCategoryCode]
     , cc.Name  [CollectCategoryName]
  FROM #matching tbl
 INNER JOIN MatchingHeader mh
    ON tbl.MatchingHeaderId = mh.Id

  LEFT JOIN Billing b           ON  b.Id    = tbl.BillingId
  LEFT JOIN Receipt r           ON  r.Id    = tbl.ReceiptId
  LEFT JOIN ReceiptHeader rh    ON rh.Id    = r.ReceiptHeaderId
  LEFT JOIN Customer cs         ON cs.Id    = b.CustomerId
  LEFT JOIN Department d        ON  d.Id    = b.DepartmentId
  LEFT JOIN Section sc          ON sc.Id    = r.SectionId
  LEFT JOIN LoginUser u         ON  u.Id    = mh.CreateBy
  LEFT JOIN Category cc         ON cc.Id    = b.CollectCategoryId
");

            if (option.OutputOrder == 0)
            {
                builder.Append(@"
 ORDER BY       mh.CreateAt ASC, tbl.MatchingHeaderId, tbl.RowNumber ");
            }
            else if (option.OutputOrder == 1)
            {
                builder.Append(@"
  LEFT JOIN Customer cs_h       ON cs_h.Id = mh.CustomerId
  LEFT JOIN PaymentAgency pa    ON pa.Id = mh.PaymentAgencyId
 ORDER BY pa.Code, cs_h.Code, tbl.MatchingHeaderId, tbl.RowNumber ");
            }
            else if (option.OutputOrder == 2)
            {
                builder.Append(@"
  LEFT JOIN (
    SELECT mh2.Id MatchingHeaderId
         , MIN(m2.ReceiptId) ReceiptId
      FROM MatchingHeader mh2
     INNER JOIN Matching m2
        ON mh2.CompanyId = @CompanyId
       AND mh2.Id = m2.MatchingHeaderId
     GROUP BY mh2.Id
  ) r_min
    ON r_min.MatchingHeaderId = mh.Id
 ORDER BY r_min.ReceiptId, tbl.MatchingHeaderId, tbl.RowNumber ");
            }
                return builder.ToString();
        }

        #endregion

        public Task<IEnumerable<MatchingHistory>> GetAsync(MatchingHistorySearch searchOption,
            CancellationToken token, IProgressNotifier notifier)
        {
            return dbHelper.ExecuteQueriesAsync(async connection =>
            {
                await dbHelper.ExecuteAsync(connection, GetQueryInitializeTempTable(), null, token);
                notifier?.UpdateState();
                await dbHelper.ExecuteAsync(connection, GetQueryInitializeMatchingHeaderId(searchOption), searchOption, token);
                notifier?.UpdateState();
                await dbHelper.ExecuteAsync(connection, GetQueryInsertBillingData(), null, token);
                notifier?.UpdateState();
                await dbHelper.ExecuteAsync(connection, GetQueryInsertReceiptData(), null, token);
                notifier?.UpdateState();
                await dbHelper.ExecuteAsync(connection, GetQueryInsertMatchingWork(), null, token);
                notifier?.UpdateState();
                var items = await dbHelper.QueryAsync<MatchingHistory>(connection, GetQuerySelectHisotryData(searchOption), searchOption, token);
                notifier?.UpdateState();
                return items;
            });
        }
    }
}
