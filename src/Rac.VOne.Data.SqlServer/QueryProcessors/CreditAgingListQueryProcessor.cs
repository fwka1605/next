using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CreditAgingListQueryProcessor :
        ICreditAgingListQueryProcessor
    {

        private readonly IDbHelper dbHelper;

        public CreditAgingListQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        private string CreateInitializeTempTable(CreditAgingListSearch option)
        {
            var builder = new StringBuilder();
            builder.Append(@"
IF EXISTS (SELECT * FROM tempdb..sysobjects
WHERE id = object_id(N'tempdb..#WorkCredit'))
    DROP TABLE #WorkCredit;

CREATE TABLE #WorkCredit
([CompanyId]        INT             NOT NULL
,[DepartmentId]     INT             NOT NULL
,[StaffId]          INT             NOT NULL
,[ParentCustomerId] INT             NOT NULL
,[CustomerId]       INT             NOT NULL
,[YearMonth]        DATETIME2(3)    NOT NULL
,[DataType]         INT                 NULL
,[Balance]          NUMERIC(18, 5)  NOT NULL DEFAULT (0)
,[BillingAmount]    NUMERIC(18, 5)  NOT NULL DEFAULT (0)
,[ReceiptAmount]    NUMERIC(18, 5)  NOT NULL DEFAULT (0)
,[MatchingAmount]   NUMERIC(18, 5)  NOT NULL DEFAULT (0)
,[ReceivableAmount] NUMERIC(18, 5)  NOT NULL DEFAULT (0)
);

CREATE INDEX [IdxWorkCredit] ON [#WorkCredit]
([CompanyId]        ASC
,[DepartmentId]     ASC
,[StaffId]          ASC
,[ParentCustomerId] ASC
,[CustomerId]       ASC
,[YearMonth]        ASC
);
");
            if (option.ConsiderReceiptAmount)
            builder.Append(@"
IF EXISTS (SELECT * FROM tempdb..sysobjects
 WHERE id = object_id(N'tempdb..#WorkReceipt'))
    DROP TABLE #WorkReceipt;

CREATE TABLE #WorkReceipt
([Id]               BIGINT
,[CompanyId]        INT
,[DepartmentId]     INT
,[StaffId]          INT
,[CustomerId]       INT
,[Multiple]         INT
,[YearMonth]        DATETIME2
,[ReceiptAmount]    NUMERIC(18, 5)
,[MatchingAmount]   NUMERIC(18, 5));
");
            return builder.ToString();
        }
        private string CreateInsertReceivable(CreditAgingListSearch option)
        {
            var deptId = option.RequireDepartmentTotal ? "dp.[Id]" : "0";
            var stafId = option.RequireStaffTotal      ? "st.[Id]" : "0";
            var deptAlias = option.UseMasterStaff      ? "st" : "b";
            var stafAlias = option.UseMasterStaff      ? "cs" : "b";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #WorkCredit
([CompanyId]
,[DepartmentId]
,[StaffId]
,[ParentCustomerId]
,[CustomerId]
,[YearMonth]
,[ReceivableAmount] )

SELECT u.[CompanyId]
     , u.[DepartmentId]
     , u.[StaffId]
     , COALESCE( csg.[ParentCustomerId] , CASE MIN( cs.[IsParent] ) WHEN 1 THEN u.[CustomerId] ELSE 0 END ) [ParentCustomerId]
     , u.[CustomerId]
     , u.[YearMonth]
     , SUM( u.[ReceivableAmount] ) [ReceivableAmount]
  FROM (

        SELECT b.[CompanyId]
             , {deptId}  [DepartmentId]
             , {stafId}  [StaffId]
             , b.[CustomerId]
             , CASE WHEN r.[DueAt] BETWEEN @ym1f AND @ym1t THEN @ym1t
                    WHEN r.[DueAt] BETWEEN @ym2f AND @ym2t THEN @ym2t
                    WHEN r.[DueAt] BETWEEN @ym3f AND @ym3t THEN @ym3t
                    WHEN @ym4f <= r.[DueAt]                THEN @ym4t
               END [YearMonth]
             , m.[Amount]   [ReceivableAmount]
          FROM [dbo].[Matching] m
         INNER JOIN [dbo].[Receipt] r            ON r.[Id]              = m.[ReceiptId]
                                                AND r.[Approved]        = 1
                                                AND r.[CompanyId]       = @CompanyId
                                                AND r.[RecordedAt]      <= @ym0t
                                                AND r.[DueAt]           >= @ym1f
         INNER JOIN [dbo].[Billing] b            ON b.[Id]              = m.[BillingId]
                                                AND b.[Approved]        = 1
                                                AND b.[InputType]       <> 3
         INNER JOIN [dbo].[Customer] cs          ON cs.[Id]             = b.[CustomerId]
                                                AND cs.[Code]           >= COALESCE(@CustomerCodeFrom, cs.[Code])
                                                AND cs.[Code]           <= COALESCE(@CustomerCodeTo  , cs.[Code])
                                                AND cs.[ClosingDay]     = COALESCE(@ClosingDay, cs.[ClosingDay])
         INNER JOIN [dbo].[Staff] st             ON st.[Id]             = {stafAlias}.[StaffId]
                                                AND st.[Code]           >= COALESCE(@StaffCodeFrom, st.[Code])
                                                AND st.[Code]           <= COALESCE(@StaffCodeTo  , st.[Code])
         INNER JOIN [dbo].[Department] dp        ON dp.[Id]              = {deptAlias}.[DepartmentId]
                                                AND dp.[Code]           >= COALESCE(@DepartmentCodeFrom, dp.[Code])
                                                AND dp.[Code]           <= COALESCE(@DepartmentCodeTo  , dp.[Code])
         INNER JOIN [dbo].[Category] rct         ON rct.[Id]            = r.[ReceiptCategoryId]
                                                AND (
                                                         rct.[UseLimitDate]  = 1
                                                     OR  rct.[UseAdvanceReceived] = 1
                                                     AND EXISTS (
                                                         SELECT 1
                                                           FROM [dbo].[Receipt] br
                                                          INNER JOIN [dbo].[Category] brct
                                                            ON brct.[Id]     = br.[ReceiptCategoryId]
                                                           AND br.[Id]       = r.[OriginalReceiptId]
                                                           AND brct.[UseLimitDate]  = 1 )
                                                    )

         UNION ALL

        SELECT r.[CompanyId]
             , {deptId}  [DepartmentId]
             , {stafId}  [StaffId]
             , cs.[Id]  [CustomerId]
             , CASE WHEN r.[DueAt] BETWEEN @ym1f AND @ym1t THEN @ym1t
                    WHEN r.[DueAt] BETWEEN @ym2f AND @ym2t THEN @ym2t
                    WHEN r.[DueAt] BETWEEN @ym3f AND @ym3t THEN @ym3t
                    WHEN @ym4f <= r.[DueAt]                THEN @ym4t
               END [YearMonth]
             , r.[RemainAmount] [ReceivableAmount]
          FROM [dbo].[Receipt] r
         INNER JOIN [dbo].[Customer] cs          ON cs.[Id]             = r.[CustomerId]
                                                AND r.[CompanyId]       = @CompanyId
                                                AND r.[RecordedAt]      <= @ym0t
                                                AND r.[DueAt]           >= @ym1f
                                                AND r.[Approved]        = 1
                                                AND r.[DeleteAt]        IS NULL
                                                AND cs.[Code]           >= @CustomerCodeFrom
                                                AND cs.[Code]           <= @CustomerCodeTo
                                                AND cs.[ClosingDay]     = COALESCE(@ClosingDay, cs.[ClosingDay])
         INNER JOIN [dbo].[Staff] st             ON st.[Id]             = cs.[StaffId]
                                                AND st.[Code]           >= @StaffCodeFrom
                                                AND st.[Code]           <= @StaffCodeTo
         INNER JOIN [dbo].[Department] dp        ON dp.[Id]             = st.[DepartmentId]
                                                AND dp.[Code]           >= @DepartmentCodeFrom
                                                AND dp.[Code]           <= @DepartmentCodeTo
         INNER JOIN (
               SELECT r.[Id] [ReceiptId]
                    , MAX(
                      CASE WHEN r.[CustomerId] = b.[CustomerId]
                            AND dp.[Id]        = b.[DepartmentId]
                            AND st.[Id]        = b.[StaffId]
                           THEN 0 ELSE 1 END
                         ) [Multiple]
                 FROM [dbo].[Receipt] r
                INNER JOIN [dbo].[Matching] m        ON r.[Id]              = m.[ReceiptId]
                                                    AND r.[CompanyId]       = @CompanyId
                                                    AND r.[RecordedAt]      <= @ym0t
                                                    AND r.[DueAt]           >= @ym1f
                                                    AND r.[Approved]        = 1
                                                    AND r.[AssignmentFlag]  < 2
                                                    AND r.[DeleteAt]        IS NULL
                INNER JOIN [dbo].[Billing] b         ON b.[Id]              = m.[BillingId]
                INNER JOIN [dbo].[Customer] cs       ON cs.[Id]             = r.[CustomerId]
                                                    AND cs.[Code]           >= COALESCE(@CustomerCodeFrom   , cs.[Code])
                                                    AND cs.[Code]           <= COALESCE(@CustomerCodeTo     , cs.[Code])
                                                    AND cs.[ClosingDay]     = COALESCE(@ClosingDay, cs.[ClosingDay])
                INNER JOIN [dbo].[Staff] st          ON st.[Id]             = cs.[StaffId]
                                                    AND st.[Code]           >= COALESCE(@StaffCodeFrom      , st.[Code])
                                                    AND st.[Code]           <= COALESCE(@StaffCodeTo        , st.[Code])
                INNER JOIN [dbo].[Department] dp     ON dp.[Id]             = st.[DepartmentId]
                                                    AND dp.[Code]           >= COALESCE(@DepartmentCodeFrom , dp.[Code])
                                                    AND dp.[Code]           <= COALESCE(@DepartmentCodeTo   , dp.[Code])
                GROUP BY
                      r.[Id]
               ) m
            ON r.[Id]                = m.[ReceiptId]
         INNER JOIN [dbo].[Category] rct        ON rct.[Id]             = r.[ReceiptCategoryId]
           AND (    rct.[UseLimitDate]  = 1
                OR  rct.[UseAdvanceReceived] = 1
                AND EXISTS (
                    SELECT 1
                      FROM [dbo].[Receipt] br
                     INNER JOIN [dbo].[Category] brct
                        ON brct.[Id]            = br.[ReceiptCategoryId]
                       AND br.[Id]              = r.[OriginalReceiptId]
                       AND brct.[UseLimitDate] = 1 )
               )

         UNION ALL

        SELECT b.[CompanyId]
             , {deptId} [DepartmentId]
             , {stafId} [StaffId]
             , b.[CustomerId]
             , @ym0t [YearMonth]
             , b.[RemainAmount] [ReceivableAmount]
          FROM [dbo].[Billing] b
         INNER JOIN [dbo].[Customer] cs          ON cs.[Id]             = b.[CustomerId]
                                                AND b.[CompanyId]       = @CompanyId
                                                AND b.[Approved]        = 1
                                                AND b.[InputType]       = 3
                                                AND b.[AssignmentFlag]  < 2
                                                AND b.[DeleteAt]        IS NULL
                                                AND b.[DueAt]           <= @ym0t
                                                AND cs.[Code]           >= COALESCE(@CustomerCodeFrom, cs.[Code])
                                                AND cs.[Code]           <= COALESCE(@CustomerCodeTo  , cs.[Code])
                                                AND cs.[ClosingDay]     = COALESCE(@ClosingDay, cs.[ClosingDay])
         INNER JOIN [dbo].[Staff] st             ON st.[Id]             = {stafAlias}.[StaffId]
                                                AND st.[Code]           >= COALESCE(@StaffCodeFrom, st.[Code])
                                                AND st.[Code]           <= COALESCE(@StaffCodeTo  , st.[Code])
         INNER JOIN [dbo].[Department] dp        ON dp.[Id]             = {deptAlias}.[DepartmentId]
                                                AND dp.[Code]           >= COALESCE(@DepartmentCodeFrom, dp.[Code])
                                                AND dp.[Code]           <= COALESCE(@DepartmentCodeTo  , dp.[Code])

        ) u
 INNER JOIN [dbo].[Customer] cs         ON cs.[Id]                  = u.[CustomerId]
  LEFT JOIN [dbo].[CustomerGroup] csg   ON csg.[ChildCustomerId]    = cs.[Id]
 GROUP BY
       u.[CompanyId]
     , u.[DepartmentId]
     , u.[StaffId]
     , u.[CustomerId]
     , csg.[ParentCustomerId]
     , u.[YearMonth]
");
            return builder.ToString();
        }
        private string CreateInsertBilling(CreditAgingListSearch option)
        {
            var deptId = option.RequireDepartmentTotal ? "dp.[Id]" : "0";
            var stafId = option.RequireStaffTotal ? "st.[Id]" : "0";
            var deptAlias = option.UseMasterStaff ? "st" : "b";
            var stafAlias = option.UseMasterStaff ? "cs" : "b";
            var targetDate = option.UseBilledAt ? "b.[BilledAt]" : "b.[SalesAt]";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #WorkCredit
([CompanyId]
,[DepartmentId]
,[StaffId]
,[ParentCustomerId]
,[CustomerId]
,[YearMonth]
,[BillingAmount] )
SELECT b.[CompanyId]
     , b.[DepartmentId]
     , b.[StaffId]
     , COALESCE( csg.[ParentCustomerId], CASE cs.[IsParent] WHEN 1 THEN cs.[Id] ELSE 0 END ) [ParentCustomerId]
     , b.[CustomerId]
     , b.[YearMonth]
     , b.[BillingAmount]
  FROM (SELECT b.[CompanyId]
             , {deptId} [DepartmentId]
             , {stafId} [StaffId]
             , b.[CustomerId]
             , @ym0t [YearMonth]
             , SUM( b.[BillingAmount]
                  - CASE WHEN b.[DeleteAt] IS NULL THEN 0 ELSE b.[RemainAmount] END ) [BillingAmount]
          FROM [dbo].[Billing] b
         INNER JOIN [dbo].[Customer] cs          ON cs.[Id]              = b.[CustomerId]
                                                AND b.[CompanyId]        = @CompanyId
                                                AND {targetDate}         BETWEEN @ympf AND @ym0t
                                                AND b.[Approved]         = 1
                                                AND b.[InputType]       <> 3
                                                AND cs.[Code]           >= COALESCE(@CustomerCodeFrom, cs.[Code])
                                                AND cs.[Code]           <= COALESCE(@CustomerCodeTo  , cs.[Code])
                                                AND cs.[ClosingDay]     = COALESCE(@ClosingDay, cs.[ClosingDay])
         INNER JOIN [dbo].[Staff] st             ON st.[Id]              = {stafAlias}.[StaffId]
                                                AND st.[Code]           >= COALESCE(@StaffCodeFrom, st.[Code])
                                                AND st.[Code]           <= COALESCE(@StaffCodeTo  , st.[Code])
         INNER JOIN [dbo].[Department] dp        ON dp.[Id]              = {deptAlias}.[DepartmentId]
                                                AND dp.[Code]           >= COALESCE(@DepartmentCodeFrom, dp.[Code])
                                                AND dp.[Code]           <= COALESCE(@DepartmentCodeTo  , dp.[Code])
         GROUP BY
               b.[CompanyId]");
            if (option.RequireDepartmentTotal) builder.Append(@"
             , dp.[Id]");
            if (option.RequireStaffTotal) builder.Append(@"
             , st.[Id]");
            builder.Append($@"
             , b.[CustomerId]
       ) b
 INNER JOIN [dbo].[Customer] cs             ON cs.[Id]                  = b.[CustomerId]
  LEFT JOIN [dbo].[CustomerGroup] csg       ON csg.[ChildCustomerId]    = cs.[Id]
");
            return builder.ToString();
        }
        private string CreateInsertReceipt(CreditAgingListSearch option)
        {
            var deptId = option.RequireDepartmentTotal ? "dp.[Id]" : "0";
            var stafId = option.RequireStaffTotal ? "st.[Id]" : "0";
            var deptAlias = option.UseMasterStaff ? "st" : "b";
            var stafAlias = option.UseMasterStaff ? "cs" : "b";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #WorkReceipt
SELECT r.[Id] [ReceiptId]
     , r.[CompanyId]
     , {deptId} [DepartmentId]
     , {stafId} [StaffId]
     , cs.[Id] [CustomerId]
     , COUNT(1) [Multiple]
     , @ym0t [YearMonth]
     , CASE ROW_NUMBER() OVER
            (PARTITION BY r.[Id]
                 ORDER BY");
            if (option.RequireDepartmentTotal) builder.Append(" dp.[Id],");
            if (option.RequireStaffTotal) builder.Append(" st.[Id],");
            builder.Append($@" cs.[Id] )
       WHEN 1 THEN MIN( r.[ReceiptAmount] )
          + SUM(m.[BankTransferFee]
              - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
              + COALESCE( mbd.[DiscountAmount], 0 ) )
       ELSE 0 END [ReceiptAmount]
     , SUM( m.[Amount]
          + m.[BankTransferFee]
          - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
          + COALESCE( mbd.[DiscountAmount], 0 ) ) [MatchingAmount]
  FROM [dbo].[Receipt] r
 INNER JOIN [dbo].[Matching] m       ON r.[Id]               = m.[ReceiptId]
                                    AND r.[CompanyId]        = @CompanyId
                                    AND r.[RecordedAt]       BETWEEN @ympf AND @ym0t
                                    AND r.[Approved]         = 1
 INNER JOIN [dbo].[Billing] b        ON b.[Id]               = m.[BillingId]
                                    AND b.[Approved]         = 1
                                    AND b.[InputType]       <> 3
 INNER JOIN [dbo].[Customer] cs      ON cs.[Id]              = b.[CustomerId]
                                    AND cs.[Code]           >= COALESCE(@CustomerCodeFrom, cs.[Code])
                                    AND cs.[Code]           <= COALESCE(@CustomerCodeTo  , cs.[Code])
                                    AND cs.[ClosingDay]      = COALESCE(@ClosingDay, cs.[ClosingDay])
 INNER JOIN [dbo].[Staff] st         ON st.[Id]              = {stafAlias}.[StaffId]
                                    AND st.[Code]           >= COALESCE(@StaffCodeFrom, st.[Code])
                                    AND st.[Code]           <= COALESCE(@StaffCodeTo  , st.[Code])
 INNER JOIN [dbo].[Department] dp    ON dp.[Id]              = {deptAlias}.[DepartmentId]
                                    AND dp.[Code]           >= COALESCE(@DepartmentCodeFrom, dp.[Code])
                                    AND dp.[Code]           <= COALESCE(@DepartmentCodeTo  , dp.[Code])
  LEFT JOIN (
       SELECT mbd.[MatchingId]
            , SUM( mbd.[DiscountAmount] ) [DiscountAmount]
         FROM [dbo].[MatchingBillingDiscount] mbd
        GROUP BY
              mbd.[MatchingId]
       ) mbd
    ON m.[Id]               = mbd.[MatchingId]
  LEFT JOIN [dbo].[Matching] m2
    ON m2.[ReceiptId]           = m.[ReceiptId]
   AND m2.[MatchingHeaderId]    < m.[MatchingHeaderId]
  LEFT JOIN [dbo].[Matching] mav
    ON mav.[ReceiptId]      = r.[OriginalReceiptId]
 INNER JOIN [dbo].[Category] rct
    ON rct.[Id]             = r.[ReceiptCategoryId]
 WHERE m2.[Id]              IS NULL
   AND (
            rct.[UseAdvanceReceived] = 0
        OR  mav.[Id]            IS NULL
       )
 GROUP BY
       r.[Id]
     , r.[CompanyId]");
            if (option.RequireDepartmentTotal) builder.Append(@"
     , dp.[Id]");
            if (option.RequireStaffTotal) builder.Append(@"
     , st.[Id]");
            builder.Append(@"
     , cs.[Id];

INSERT INTO #WorkCredit
([CompanyId]
,[DepartmentId]
,[StaffId]
,[ParentCustomerId]
,[CustomerId]
,[YearMonth]
,[ReceiptAmount] )
SELECT wr.[CompanyId]
     , wr.[DepartmentId]
     , wr.[StaffId]
     , COALESCE(csg.[ParentCustomerId], CASE MIN( cs.[IsParent] ) WHEN 1 THEN wr.[CustomerId] ELSE 0 END ) [ParentCustomerId]
     , wr.[CustomerId]
     , wr.[YearMonth]
     , SUM( wr.[ReceiptAmount] ) [ReceiptAmount]
  FROM (
        SELECT wr.[CompanyId]
             , wr.[DepartmentId]
             , wr.[StaffId]
             , wr.[CustomerId]
             , wr.[YearMonth]
             , CASE wr.[Multiple] WHEN 1 THEN wr.[ReceiptAmount] ELSE wr.[MatchingAmount] END [ReceiptAmount]
          FROM #WorkReceipt wr
         UNION ALL
        SELECT wr.[CompanyId]
             , wr.[DepartmentId]
             , wr.[StaffId]
             , COALESCE(pcsg.[ParentCustomerId], ccsg.[ParentCustomerId], wr.[CustomerId] ) [CustomerId]
             , wr.[YearMonth]
             , wr.[ReceiptAmount] - wr.[MatchingAmount]
          FROM #WorkReceipt wr
          LEFT JOIN (
               SELECT DISTINCT csg.[ParentCustomerId]
                 FROM [dbo].[CustomerGroup] csg
               ) pcsg
            ON pcsg.[ParentCustomerId]  = wr.[CustomerId]
          LEFT JOIN [dbo].[CustomerGroup] ccsg
            ON ccsg.[ChildCustomerId]   = wr.[CustomerId]
         WHERE wr.[Multiple]            > 1
        ) wr
 INNER JOIN [dbo].[Customer] cs         ON cs.[Id]                  = wr.[CustomerId]
  LEFT JOIN [dbo].[CustomerGroup] csg   ON csg.[ChildCustomerId]    = wr.[CustomerId]
 GROUP BY
       wr.[CompanyId]
     , wr.[DepartmentId]
     , wr.[StaffId]
     , csg.[ParentCustomerId]
     , wr.[CustomerId]
     , wr.[YearMonth]
");
            return builder.ToString();
        }
        private string CreateInsertMatching(CreditAgingListSearch option)
        {
            var deptId = option.RequireDepartmentTotal ? "dp.[Id]" : "0";
            var stafId = option.RequireStaffTotal ? "st.[Id]" : "0";
            var deptAlias = option.UseMasterStaff ? "st" : "b";
            var stafAlias = option.UseMasterStaff ? "cs" : "b";
            var targetDate = option.UseBilledAt ? "b.[BilledAt]" : "b.[SalesAt]";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #WorkCredit
([CompanyId]
,[DepartmentId]
,[StaffId]
,[ParentCustomerId]
,[CustomerId]
,[YearMonth]
,[MatchingAmount] )

SELECT m.[CompanyId]
     , m.[DepartmentId]
     , m.[StaffId]
     , COALESCE(csg.[ParentCustomerId], CASE cs.[IsParent] WHEN 1 THEN cs.[Id] ELSE 0 END ) [ParentCustomerId]
     , m.[CustomerId]
     , m.[YearMonth]
     , m.[MatchingAmount]
  FROM (
        SELECT b.[CompanyId]
             , {deptId} [DepartmentId]
             , {stafId} [StaffId]
             , b.[CustomerId]
             , @ym0t [YearMonth]
             , SUM( m.[Amount]
                  + m.[BankTransferFee]
                  - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
                  + COALESCE( mbd.[DiscountAmount], 0 ) ) [MatchingAmount]
          FROM [dbo].[Matching] m
         INNER JOIN [dbo].[Receipt] r        ON r.[Id]               = m.[ReceiptId]
                                            AND r.[CompanyId]        = @CompanyId
                                            AND r.[Approved]         = 1
         INNER JOIN [dbo].[Billing] b        ON b.[Id]               = m.[BillingId]
                                            AND b.[Approved]         = 1
                                            AND b.[InputType]       <> 3
                                            AND CASE WHEN r.[RecordedAt] < {targetDate} THEN {targetDate} ELSE r.[RecordedAt] END
                                                     BETWEEN @ympf AND @ym0t
         INNER JOIN [dbo].[Customer] cs      ON cs.[Id]              = b.[CustomerId]
                                            AND cs.[Code]           >= COALESCE(@CustomerCodeFrom   , cs.[Code])
                                            AND cs.[Code]           <= COALESCE(@CustomerCodeTo     , cs.[Code])
                                            AND cs.[ClosingDay]     = COALESCE(@ClosingDay, cs.[ClosingDay])
         INNER JOIN [dbo].[Staff] st         ON st.[Id]              = {stafAlias}.[StaffId]
                                            AND st.[Code]           >= COALESCE(@StaffCodeFrom      , st.[Code])
                                            AND st.[Code]           <= COALESCE(@StaffCodeTo        , st.[Code])
         INNER JOIN [dbo].[Department] dp    ON dp.[Id]              = {deptAlias}.[DepartmentId]
                                            AND dp.[Code]           >= COALESCE(@DepartmentCodeFrom , dp.[Code])
                                            AND dp.[Code]           <= COALESCE(@DepartmentCodeTo   , dp.[Code])
          LEFT JOIN (
               SELECT mbd.[MatchingId]
                    , SUM( mbd.[DiscountAmount] ) [DiscountAmount]
                 FROM [dbo].[MatchingBillingDiscount] mbd
                GROUP BY
                      mbd.[MatchingId]
               ) mbd
            ON m.[Id]               = mbd.[MatchingId]
         GROUP BY
               b.[CompanyId]");
            if (option.RequireDepartmentTotal) builder.Append(@"
             , dp.[Id]");
            if (option.RequireStaffTotal) builder.Append(@"
             , st.[Id]");
            builder.Append($@"
             , b.[CustomerId]
       ) m
 INNER JOIN [dbo].[Customer] cs             ON cs.[Id]      = m.[CustomerId]
  LEFT JOIN [dbo].[CustomerGroup] csg       ON cs.[Id]      = csg.[ChildCustomerId] 
");
            return builder.ToString();
        }
        private string CreateSelectData(CreditAgingListSearch option)
        {
            var builder = new StringBuilder();
            var requireParentLimit = option.ConsiderCustomerGroup && option.UseParentCustomerCredit;
            builder.Append($@"
SELECT wc.*
     , cs.[Code] [CustomerCode]
     , COALESCE(dp.[Code]  , N'') [DepartmentCode]
     , COALESCE(st.[Code]  , N'') [StaffCode]
     , COALESCE(pcs.[Code] , N'') [ParentCustomerCode]
     , COALESCE(cs.[Name]  , N'得意先コード不明')   [CustomerName]
     , COALESCE(dp.[Name]  , N'請求部門コード不明') [DepartmentName]
     , COALESCE(st.[Name]  , N'担当者コード不明')   [StaffName]
     , COALESCE(pcs.[Name] , N'') [ParentCustomerName]
     , cct.[Name] [CollectCategory]
     , COALESCE(pcct.[Name], N'') [ParentCollectCategoryName]
     , {(requireParentLimit ? "COALESCE(pcs.[CreditLimit] , cs.[CreditLimit])" : "cs.[CreditLimit]")} * 10000 [CreditLimit]
  FROM (
        SELECT wc.[CompanyId]
             , wc.[DepartmentId]
             , wc.[StaffId]
             , CASE WHEN wc2.[HasAnyChildren] = 0 OR wc.[ParentCustomerId] = 0 THEN NULL ELSE wc.[ParentCustomerId] END [ParentCustomerId]
             , wc.[CustomerId]
             , wc.[BillingAmount]
             , wc.[ReceiptAmount]
             , wc.[MatchingAmount]
             , wc.[ReceivableAmount_0]
             , wc.[ReceivableAmount_1]
             , wc.[ReceivableAmount_2]
             , wc.[ReceivableAmount_3]
             , wc.[ReceivableAmount_4]
          FROM (
                SELECT wc.[CompanyId]
                     , wc.[DepartmentId]
                     , wc.[StaffId]
                     , wc.[ParentCustomerId]
                     , wc.[CustomerId]
                     , SUM( CASE wc.[YearMonth] WHEN @ym0t THEN wc.[BillingAmount]      ELSE 0 END ) [BillingAmount]
                     , SUM( CASE wc.[YearMonth] WHEN @ym0t THEN wc.[ReceiptAmount]      ELSE 0 END ) [ReceiptAmount]
                     , SUM( CASE wc.[YearMonth] WHEN @ym0t THEN wc.[MatchingAmount]     ELSE 0 END ) [MatchingAmount]
                     , SUM( CASE wc.[YearMonth] WHEN @ym0t THEN wc.[ReceivableAmount]   ELSE 0 END ) [ReceivableAmount_0]
                     , SUM( CASE wc.[YearMonth] WHEN @ym1t THEN wc.[ReceivableAmount]   ELSE 0 END ) [ReceivableAmount_1]
                     , SUM( CASE wc.[YearMonth] WHEN @ym2t THEN wc.[ReceivableAmount]   ELSE 0 END ) [ReceivableAmount_2]
                     , SUM( CASE wc.[YearMonth] WHEN @ym3t THEN wc.[ReceivableAmount]   ELSE 0 END ) [ReceivableAmount_3]
                     , SUM( CASE wc.[YearMonth] WHEN @ym4t THEN wc.[ReceivableAmount]   ELSE 0 END ) [ReceivableAmount_4]
                  FROM #WorkCredit wc
                 GROUP BY
                       wc.[CompanyId]
                     , wc.[DepartmentId]
                     , wc.[StaffId]
                     , wc.[ParentCustomerId]
                     , wc.[CustomerId]
               ) wc
         INNER JOIN (
                SELECT wc.[CompanyId]
                     , wc.[DepartmentId]
                     , wc.[StaffId]
                     , wc.[ParentCustomerId]
                     , SUM( CASE WHEN wc.[ParentCustomerId] = 0 OR wc.[ParentCustomerId] = wc.[CustomerId] THEN 0 ELSE 1 END ) [HasAnyChildren]
                  FROM #WorkCredit wc
                 GROUP BY
                       wc.[CompanyId]
                     , wc.[DepartmentId]
                     , wc.[StaffId]
                     , wc.[ParentCustomerId]
               ) wc2
            ON wc.[CompanyId]           = wc2.[CompanyId]
           AND wc.[DepartmentId]        = wc2.[DepartmentId]
           AND wc.[StaffId]             = wc2.[StaffId]
           AND wc.[ParentCustomerId]    = wc2.[ParentCustomerId]
       ) wc
 INNER JOIN [dbo].[Customer] cs         ON cs.[Id]      = wc.[CustomerId]
 INNER JOIN [dbo].[Category] cct        ON cct.[Id]     = cs.[CollectCategoryId]
  LEFT JOIN [dbo].[Department] dp       ON dp.[Id]      = wc.[DepartmentId]
  LEFT JOIN [dbo].[Staff] st            ON st.[Id]      = wc.[StaffId]
  LEFT JOIN [dbo].[Customer] pcs        ON pcs.[Id]     = wc.[ParentCustomerId]
  LEFT JOIN [dbo].[Category] pcct       ON pcct.[Id]    = pcs.[CollectCategoryId]
 ORDER BY
       wc.[CompanyId]
     , dp.[Code]
     , st.[Code]
     , pcs.[Code]
     , cs.[Code]
");
            return builder.ToString();
        }


        public Task<IEnumerable<CreditAgingList>> GetAsync(CreditAgingListSearch searchOption,
            IProgressNotifier notifier = null,
            CancellationToken token = default(CancellationToken))
        {
            return dbHelper.ExecuteQueriesAsync(async connection =>
            {
                await dbHelper.ExecuteAsync(connection, CreateInitializeTempTable(searchOption), searchOption, token);
                notifier?.UpdateState();
                await dbHelper.ExecuteAsync(connection, CreateInsertReceivable(searchOption), searchOption, token);
                notifier?.UpdateState();
                await dbHelper.ExecuteAsync(connection, CreateInsertBilling(searchOption), searchOption, token);
                notifier?.UpdateState();
                if (searchOption.ConsiderReceiptAmount)
                {
                    await dbHelper.ExecuteAsync(connection, CreateInsertReceipt(searchOption), searchOption, token);
                    notifier?.UpdateState();
                }
                await dbHelper.ExecuteAsync(connection, CreateInsertMatching(searchOption), searchOption, token);
                notifier?.UpdateState();
                var items = await dbHelper.QueryAsync<CreditAgingList>(connection, CreateSelectData(searchOption), searchOption, token);
                notifier?.UpdateState();
                return items;
            });
        }
    }
}
