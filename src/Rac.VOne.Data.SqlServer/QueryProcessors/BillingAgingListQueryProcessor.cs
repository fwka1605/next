using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BillingAgingListQueryProcessor :
        IBillingAgingListQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public BillingAgingListQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        private string GetQueryInitializeTempTable(BillingAgingListSearch option)
        {
            var builder = new StringBuilder();
            builder.Append(@"
IF EXISTS (SELECT * FROM tempdb..sysobjects
 WHERE id = object_id(N'tempdb..#BillingAging'))
DROP TABLE #BillingAging;

CREATE TABLE #BillingAging
( [CompanyId]               INT             NOT NULL
, [CurrencyId]              INT             NOT NULL
, [ParentCustomerId]        INT             NOT NULL
, [CustomerId]              INT             NOT NULL
, [StaffId]                 INT             NOT NULL
, [DepartmentId]            INT             NOT NULL
, [YearMonth]               DATE            NOT NULL
, [Balance]                 NUMERIC(18, 5)      NULL  DEFAULT(0)
, [BillingAmount]           NUMERIC(18, 5)      NULL  DEFAULT(0)
, [BillingMatchingAmount]   NUMERIC(18, 5)      NULL  DEFAULT(0)
, [ReceiptAmount]           NUMERIC(18, 5)      NULL  DEFAULT(0)
, [MatchingAmount]          NUMERIC(18, 5)      NULL  DEFAULT(0));

CREATE INDEX IdxBillingAging ON #BillingAging
( [CompanyId]
, [CurrencyId]
, [DepartmentId]
, [StaffId]
, [ParentCustomerId]
, [CustomerId]
, [YearMonth])
");
            if (option.BillingRemainType > 0)
                builder.Append(@"
IF EXISTS (SELECT * FROM tempdb..sysobjects
 WHERE id = object_id(N'tempdb..#WorkReceipt'))
DROP TABLE #WorkReceipt;

CREATE TABLE #WorkReceipt
( [ReceiptId]               BIGINT              NULL
, [CompanyId]               INT                 NULL
, [CurrencyId]              INT                 NULL
, [CustomerId]              INT                 NULL
, [StaffId]                 INT                 NULL
, [DepartmentId]            INT                 NULL
, [YearMonth]               DATETIME2(3)        NULL
, [RecordedAt]              DATETIME2(3)        NULL
, [ReceiptAmount]           NUMERIC(18, 5)      NULL
, [MatchingAmount]          NUMERIC(18, 5)      NULL
, [HasMany]                 INT                 NULL
);

IF EXISTS (SELECT * FROM tempdb..sysobjects
 WHERE id = object_id(N'tempdb..#CustomerGroupIds'))
DROP TABLE #CustomerGroupIds;

CREATE TABLE #CustomerGroupIds
( [CustomerId]          INT             NOT NULL);
CREATE INDEX IdxCustomerGroupIds ON #CustomerGroupIds
( [CustomerId] );

IF EXISTS (SELECT * FROM tempdb..sysobjects
 WHERE id = object_id(N'tempdb..#HeaderIds'))
DROP TABLE #HeaderIds;

CREATE TABLE #HeaderIds
( [Id]          BIGINT          NOT NULL);
CREATE INDEX IdxHeaderIds ON #HeaderIds
( [Id] );

");
            return builder.ToString();
        }

        private string GetQueryInsertBillingReamin(BillingAgingListSearch option)
        {
            var deptId = option.RequireDepartmentSubtotal   ? "dp.[Id]" : "0";
            var stafId = option.RequireStaffSubtotal        ? "st.[Id]" : "0";
            var deptAlias = option.IsMasterStaff ? "st" : "b";
            var stafAlias = option.IsMasterStaff ? "cs" : "b";
            var targetDate = option.UseBilledAt ? "b.[BilledAt]" : "b.[SalesAt]";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #BillingAging
     ( [CompanyId]
     , [CurrencyId]
     , [YearMonth]
     , [ParentCustomerId]
     , [CustomerId]
     , [StaffId]
     , [DepartmentId]
     , [BillingAmount]
     , [BillingMatchingAmount]
      )
SELECT b.[CompanyId]
     , b.[CurrencyId]
     , b.[YearMonth]
     , COALESCE(pcs.[Id], CASE cs.[IsParent] WHEN 1 THEN cs.[Id] ELSE 0 END) [ParentCustomerId]
     , b.[CustomerId]
     , b.[StaffId]
     , b.[DepartmentId]
     , b.[BillingAmount]
     , b.[BillingMatchingAmount]
  FROM (
       SELECT b.[CompanyId]
            , b.[CurrencyId]
            , b.[CustomerId]
            , CASE WHEN {targetDate} BETWEEN @ym4f AND @ym4t THEN @ym4t
                   WHEN {targetDate} BETWEEN @ym3f AND @ym3t THEN @ym3t
                   WHEN {targetDate} BETWEEN @ym2f AND @ym2t THEN @ym2t
                   WHEN {targetDate} BETWEEN @ym1f AND @ym1t THEN @ym1t
                   WHEN {targetDate} BETWEEN @ym0f AND @ym0t THEN @ym0t
                END  [YearMonth]
            , {stafId} [StaffId]
            , {deptId} [DepartmentId]
            , SUM( b.[BillingAmount]
                 - CASE WHEN b.[DeleteAt] = b.[DeleteAt] THEN b.[RemainAmount] ELSE 0 END ) [BillingAmount]
            , SUM(COALESCE(m.[BillingMatchingAmount], 0))   [BillingMatchingAmount]
         FROM [dbo].[Billing] b
         LEFT JOIN (
              SELECT m.[BillingId]
                   , SUM( m.[Amount]
                        + m.[BankTransferFee]
                        - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
                        + COALESCE(mbd.[DiscountAmount], 0)
                        ) [BillingMatchingAmount]
                FROM [dbo].[Matching] m
               INNER JOIN [dbo].[Billing] b      ON b.[Id]          = m.[BillingId]
                                                AND b.[CompanyId]   = @CompanyId
                                                AND b.[Approved]    = 1
                                                AND b.[InputType]   <> 3
                                                AND m.[RecordedAt]  <= @ym0t
                LEFT JOIN (
                     SELECT mbd.[MatchingId]
                          , SUM( mbd.[DiscountAmount] ) [DiscountAmount]
                       FROM [dbo].[MatchingBillingDiscount] mbd
                      GROUP BY mbd.[MatchingId]
                     ) mbd
                  ON mbd.[MatchingId]      = m.[Id]
               GROUP BY m.[BillingId]
              ) m
           ON m.[BillingId]    = b.[Id]
        INNER JOIN [dbo].[Customer] cs      ON cs.[Id]           = b.[CustomerId]
                                           AND cs.[Code]        >= COALESCE(@CustomerCodeFrom, cs.[Code])
                                           AND cs.[Code]        <= COALESCE(@CustomerCodeTo  , cs.[Code])
                                           AND cs.[ClosingDay]   = COALESCE(@ClosingDay, cs.[ClosingDay])
        INNER JOIN [dbo].[Staff] st         ON st.[Id]           = {stafAlias}.[StaffId]
                                           AND st.[Code]        >= COALESCE(@StaffCodeFrom, st.[Code])
                                           AND st.[Code]        <= COALESCE(@StaffCodeTo  , st.[Code])
        INNER JOIN [dbo].[Department] dp    ON dp.[Id]           = {deptAlias}.[DepartmentId]
                                           AND dp.[Code]        >= COALESCE(@DepartmentCodeFrom, dp.[Code])
                                           AND dp.[Code]        <= COALESCE(@DepartmentCodeTo  , dp.[Code])
        WHERE b.CompanyId      = @CompanyId");
            if (option.CurrencyId.HasValue) builder.Append(@"
          AND b.CurrencyId     = @CurrencyId");
            builder.Append($@"
          AND {targetDate} BETWEEN @ym4f AND @ym0t /* target date */
          AND b.[Approved]    = 1
          AND b.[InputType]   <> 3
        GROUP BY
              b.[CompanyId]
            , b.[CurrencyId]
            , b.[CustomerId]
            , CASE WHEN {targetDate} BETWEEN @ym4f AND @ym4t THEN @ym4t
                   WHEN {targetDate} BETWEEN @ym3f AND @ym3t THEN @ym3t
                   WHEN {targetDate} BETWEEN @ym2f AND @ym2t THEN @ym2t
                   WHEN {targetDate} BETWEEN @ym1f AND @ym1t THEN @ym1t
                   WHEN {targetDate} BETWEEN @ym0f AND @ym0t THEN @ym0t
                END");
            if (option.RequireStaffSubtotal)
                builder.Append(@"
            , st.[Id]");
            if (option.RequireDepartmentSubtotal)
                builder.Append(@"
            , dp.[Id]");
            builder.Append(@"
       ) b
 INNER JOIN [dbo].[Customer] cs         ON  cs.[Id]     = b.[CustomerId]
  LEFT JOIN [dbo].[CustomerGroup] csg   ON  cs.[Id]     = csg.[ChildCustomerId]
  LEFT JOIN [dbo].[Customer] pcs        ON pcs.[Id]     = csg.[ParentCustomerId]
 OPTION ( FORCE ORDER )

");
            return builder.ToString();
        }

        private string GetQueryInsertReceiptRemain(BillingAgingListSearch option)
        {
            var deptId = option.RequireDepartmentSubtotal   ? "dp.[Id]" : "0";
            var stafId = option.RequireStaffSubtotal        ? "st.[Id]" : "0";
            var deptAlias = option.IsMasterStaff ? "st" : "b";
            var stafAlias = option.IsMasterStaff ? "cs" : "b";
            var targetDate = option.UseBilledAt ? "b.[BilledAt]" : "b.[SalesAt]";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #CustomerGroupIds
SELECT DISTINCT csg.[ParentCustomerId]
  FROM [dbo].[CustomerGroup] csg
 INNER JOIN [dbo].[Customer] cs     ON cs.[Id]          = csg.[ParentCustomerId]
                                   AND cs.[CompanyId]   = @CompanyId
 UNION ALL
SELECT DISTINCT csg.[ChildCustomerId]
  FROM [dbo].[CustomerGroup] csg
 INNER JOIN [dbo].[Customer] cs     ON cs.[Id]          = csg.[ChildCustomerId]
                                   AND cs.[CompanyId]   = @CompanyId

INSERT INTO #HeaderIds
SELECT DISTINCT m.MatchingHeaderId
  FROM [dbo].[Matching] m
 INNER JOIN [dbo].[Billing] b       ON b.[Id]           = m.[BillingId]
                                   AND b.[CompanyId]    = @CompanyId");
            if (option.CurrencyId.HasValue) builder.Append(@"
                                   AND b.[CurrencyId]   = @CurrencyId");
            builder.Append($@"
                                   AND b.[Approved]     = 1
                                   AND b.[InputType]   <> 3
 INNER JOIN [dbo].[Receipt] r       ON r.[Id]           = m.[ReceiptId]
                                   AND r.[RecordedAt]   BETWEEN @ym4f AND @ym0t
                                   AND r.[OriginalReceiptId]    IS NULL
 INNER JOIN [dbo].[Customer] cs     ON cs.[Id]          = b.[CustomerId]
                                   AND cs.[CompanyId]   = @CompanyId
                                   AND cs.[Code]       >= COALESCE(@CustomerCodeFrom    , cs.[Code])
                                   AND cs.[Code]       <= COALESCE(@CustomerCodeTo      , cs.[Code])
                                   AND cs.[ClosingDay]  = COALESCE(@ClosingDay, cs.[ClosingDay])
 INNER JOIN [dbo].[Staff] st        ON st.[Id]          = {stafAlias}.[StaffId]
                                   AND st.[CompanyId]   = @CompanyId
                                   AND st.[Code]       >= COALESCE(@StaffCodeFrom       , st.[Code])
                                   AND st.[Code]       <= COALESCE(@StaffCodeTo         , st.[Code])
 INNER JOIN [dbo].[Department] dp   ON dp.[Id]          = {deptAlias}.[DepartmentId]
                                   AND dp.[CompanyId]   = @CompanyId
                                   AND dp.[Code]       >= COALESCE(@DepartmentCodeFrom  , dp.[Code])
                                   AND dp.[Code]       <= COALESCE(@DepartmentCodeTo    , dp.[Code])
 OPTION ( FORCE ORDER )

INSERT INTO #WorkReceipt
     ( [ReceiptId]
     , [CompanyId]
     , [CurrencyId]
     , [CustomerId]
     , [StaffId]
     , [DepartmentId]
     , [YearMonth]
     , [RecordedAt]
     , [ReceiptAmount]
     , [MatchingAmount]
     , [HasMany] )
SELECT r.[ReceiptId]
     , r.[CompanyId]
     , r.[CurrencyId]
     , r.[CustomerId]
     , r.[StaffId]
     , r.[DepartmentId]
     , r.[YearMonth]
     , r.[RecordedAt]
     , CASE r.[ReceiptSeq] WHEN 1 THEN r.[ReceiptAmount] ELSE 0 END [ReceiptAmount]
     , r.[MatchingAmount]
     , r.[HasMany]
  FROM (
        SELECT r.*
             , ROW_NUMBER() OVER (
                      PARTITION BY r.[ReceiptId]
                      ORDER BY r.[CustomerId] ) [ReceiptSeq]
             , COUNT(1) OVER ( PARTITION BY r.[ReceiptId] ) [HasMany]
          FROM (
               SELECT r.[Id] [ReceiptId]
                    , MIN( r.[CompanyId]  )     [CompanyId]
                    , MIN( r.[CurrencyId] )     [CurrencyId]
                    , b.[CustomerId]
                    , {stafId}    [StaffId]
                    , {deptId}    [DepartmentId]
                    , MIN(
                      CASE WHEN r.[RecordedAt] BETWEEN @ym4f AND @ym4t THEN @ym4t
                           WHEN r.[RecordedAt] BETWEEN @ym3f AND @ym3t THEN @ym3t
                           WHEN r.[RecordedAt] BETWEEN @ym2f AND @ym2t THEN @ym2t
                           WHEN r.[RecordedAt] BETWEEN @ym1f AND @ym1t THEN @ym1t
                           WHEN r.[RecordedAt] BETWEEN @ym0f AND @ym0t THEN @ym0t
                       END )                    [YearMonth]
                    , MIN( r.[RecordedAt] )     [RecordedAt]
                    , MIN( r.[ReceiptAmount] )
                    + SUM ( m.[BankTransferFee]
                          - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
                          + COALESCE( mbd.[DiscountAmount], 0) ) [ReceiptAmount]
                    , SUM ( m.[Amount]
                          + m.[BankTransferFee]
                          - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
                          + COALESCE( mbd.[DiscountAmount], 0) ) [MatchingAmount]
                 FROM [dbo].[Matching] m
                INNER JOIN [dbo].[Receipt] r             ON r.[Id]              = m.[ReceiptId]
                                                        AND r.[CompanyId]       = @CompanyId");
            if (option.CurrencyId.HasValue) builder.Append(@"
                                                        AND r.[CurrencyId]      = @CurrencyId");
            builder.Append($@"
                                                        AND m.[MatchingHeaderId] IN (SELECT Id FROM #HeaderIds)
                                                        AND r.[RecordedAt]      BETWEEN @ym4f AND @ym0t
                                                        AND r.[OriginalReceiptId] IS NULL
                INNER JOIN [dbo].[Billing] b             ON b.[Id]              = m.[BillingId]
                                                        AND b.[Approved]        = 1
                                                        AND b.[InputType]      <> 3
                                                        AND b.[CustomerId] NOT IN ( /* not exists customer group */
                                                              SELECT CustomerId  FROM #CustomerGroupIds )
                INNER JOIN [dbo].[Customer] cs           ON cs.[Id]      = b.[CustomerId]
                INNER JOIN [dbo].[Staff] st              ON st.[Id]      = {stafAlias}.[StaffId]
                INNER JOIN [dbo].[Department] dp         ON dp.[Id]      = {deptAlias}.[DepartmentId]
                 LEFT JOIN (
                      SELECT mbd.[MatchingId]
                           , SUM( mbd.[DiscountAmount] )   [DiscountAmount]
                        FROM [dbo].[MatchingBillingDiscount] mbd
                       GROUP BY
                             mbd.[MatchingId]
                      ) mbd
                   ON mbd.[MatchingId]         = m.[Id]
                 LEFT JOIN [dbo].[Matching] m2 /* limitation first matching */
                   ON m2.[ReceiptId]           = m.[ReceiptId]
                  AND m2.[MatchingHeaderId]    < m.[MatchingHeaderId]
                WHERE m2.[Id]                 IS NULL
                GROUP BY
                      r.[Id]
                    , b.[CustomerId]");
            if (option.RequireStaffSubtotal) builder.Append(@"
                    , st.[Id]");
            if (option.RequireDepartmentSubtotal) builder.Append(@"
                    , dp.[Id]");
            builder.Append($@"

                UNION ALL

               SELECT r.[Id] [ReceiptId]
                    , MIN( r.[CompanyId]  )     [CompanyId]
                    , MIN( r.[CurrencyId] )     [CurrencyId]
                    , b.[CustomerId]
                    , {stafId}    [StaffId]
                    , {deptId}    [DepartmentId]
                    , MIN(
                      CASE WHEN r.[RecordedAt] BETWEEN @ym4f AND @ym4t THEN @ym4t
                           WHEN r.[RecordedAt] BETWEEN @ym3f AND @ym3t THEN @ym3t
                           WHEN r.[RecordedAt] BETWEEN @ym2f AND @ym2t THEN @ym2t
                           WHEN r.[RecordedAt] BETWEEN @ym1f AND @ym1t THEN @ym1t
                           WHEN r.[RecordedAt] BETWEEN @ym0f AND @ym0t THEN @ym0t
                       END )                    [YearMonth]
                    , MIN( r.[RecordedAt] )     [RecordedAt]
                    , MIN( r.[ReceiptAmount] )
                    + SUM ( m.[BankTransferFee]
                          - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
                          + COALESCE( mbd.[DiscountAmount], 0) ) [ReceiptAmount]
                    , SUM( m.[Amount]
                         + m.[BankTransferFee]
                         - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
                         + COALESCE( mbd.[DiscountAmount], 0) ) [MatchingAmount]
                 FROM [dbo].[Matching] m
                INNER JOIN [dbo].[Receipt] r             ON r.[Id]              = m.[ReceiptId]
                                                        AND r.[CompanyId]       = @CompanyId");
            if (option.CurrencyId.HasValue) builder.Append(@"
                                                        AND r.[CurrencyId]      = @CurrencyId");
            builder.Append($@"
                                                        AND m.[MatchingHeaderId] IN (SELECT Id FROM #HeaderIds)
                                                        AND r.[RecordedAt]      BETWEEN @ym4f AND @ym0t
                                                        AND r.[OriginalReceiptId] IS NULL
                INNER JOIN [dbo].[Billing] b             ON b.[Id]               = m.[BillingId]
                                                        AND b.[Approved]        = 1
                                                        AND b.[InputType]       <> 3
                                                        AND b.[CustomerId]      IN ( /* exist customer group */
                                                            SELECT CustomerId FROM #CustomerGroupIds )
                INNER JOIN [dbo].[Customer] cs           ON cs.[Id]      = b.[CustomerId]
                INNER JOIN [dbo].[Staff] st              ON st.[Id]      = {stafAlias}.[StaffId]
                INNER JOIN [dbo].[Department] dp         ON dp.[Id]      = {deptAlias}.[DepartmentId]
                 LEFT JOIN (
                      SELECT mbd.[MatchingId]
                           , SUM( mbd.[DiscountAmount] )   [DiscountAmount]
                       FROM [dbo].[MatchingBillingDiscount] mbd
                      GROUP BY
                            mbd.[MatchingId]
                      ) mbd
                   ON mbd.[MatchingId]         = m.[Id]
                 LEFT JOIN [dbo].[Matching] m2
                   ON m2.[ReceiptId]           = m.[ReceiptId]
                  AND m2.[MatchingHeaderId]    < m.[MatchingHeaderId]
                WHERE m2.[Id]                 IS NULL
                GROUP BY
                      r.[Id]
                    , b.[CustomerId]");
            if (option.RequireStaffSubtotal)
                builder.Append(@"
                    , st.[Id]");
            if (option.RequireDepartmentSubtotal)
                builder.Append(@"
                    , dp.[Id]");
            builder.Append($@"
               ) r
       ) r
 OPTION ( FORCE ORDER )

INSERT INTO #BillingAging
     ( [CompanyId]
     , [CurrencyId]
     , [YearMonth]
     , [ParentCustomerId]
     , [CustomerId]
     , [StaffId]
     , [DepartmentId]
     , [ReceiptAmount] )
SELECT wr.[CompanyId]
     , wr.[CurrencyId]
     , wr.[YearMonth]
     , COALESCE( pcs.[Id], CASE cs.[IsParent] WHEN 1 THEN cs.[Id] ElSE 0 END ) [ParentCustomerId]
     , wr.[CustomerId]
     , wr.[StaffId]
     , wr.[DepartmentId]
     , wr.[ReceiptAmount]
  FROM (
       SELECT wr.[CompanyId]
            , wr.[CurrencyId]
            , wr.[CustomerId]
            , wr.[StaffId]
            , wr.[DepartmentId]
            , wr.[YearMonth]
            , CASE WHEN wr.[HasMany] > 1 THEN wr.[MatchingAmount] ELSE wr.[ReceiptAmount] END [ReceiptAmount]
         FROM #WorkReceipt wr
        UNION ALL
       SELECT wr.[CompanyId]
            , wr.[CurrencyId]
            , wr.[CustomerId]
            , wr.[StaffId]
            , wr.[DepartmentId]
            , wr.[YearMonth]
            , wr.[ReceiptAmount]
         FROM (
              SELECT wr.[CompanyId]
                   , wr.[CurrencyId]
                   , COALESCE(ccsg.[ParentCustomerId], pcsg.[ParentCustomerId], wr.[CustomerId] ) [CustomerId]
                   , wr.[StaffId]
                   , wr.[DepartmentId]
                   , wr.[RecordedAt]
                   , wr.[YearMonth]
                   , SUM( wr.[ReceiptAmount] - wr.[MatchingAmount] ) [ReceiptAmount]
                FROM #WorkReceipt wr
                LEFT JOIN (
                     SELECT DISTINCT csg.[ParentCustomerId]
                       FROM [dbo].[CustomerGroup] csg
                     ) pcsg
                  ON pcsg.[ParentCustomerId]  = wr.[CustomerId]
                LEFT JOIN [dbo].[CustomerGroup] ccsg
                  ON ccsg.[ChildCustomerId]   = wr.[CustomerId]
               WHERE wr.[HasMany] > 1
               GROUP BY
                     wr.[CompanyId]
                   , wr.[CurrencyId]
                   , COALESCE(ccsg.[ParentCustomerId], pcsg.[ParentCustomerId], wr.[CustomerId] )
                   , wr.[StaffId]
                   , wr.[DepartmentId]
                   , wr.[RecordedAt]
                   , wr.[YearMonth]
              ) wr
       ) wr
  LEFT JOIN [dbo].[Customer] cs         ON cs.[Id]      = wr.[CustomerId]
  LEFT JOIN [dbo].[CustomerGroup] csg   ON cs.[Id]      = csg.[ChildCustomerId]
  LEFT JOIN [dbo].[Customer] pcs        ON pcs.[Id]     = csg.[ParentCustomerId]
 OPTION ( FORCE ORDER )

");
            return builder.ToString();
        }

        private string GetQueryInsertMatchingReamin(BillingAgingListSearch option)
        {
            var deptId = option.RequireDepartmentSubtotal   ? "dp.[Id]" : "0";
            var stafId = option.RequireStaffSubtotal        ? "st.[Id]" : "0";
            var deptAlias = option.IsMasterStaff ? "st" : "b";
            var stafAlias = option.IsMasterStaff ? "cs" : "b";
            var targetDate = option.UseBilledAt ? "b.[BilledAt]" : "b.[SalesAt]";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #BillingAging
     ( [CompanyId]
     , [CurrencyId]
     , [YearMonth]
     , [ParentCustomerId]
     , [CustomerId]
     , [StaffId]
     , [DepartmentId]
     , [MatchingAmount]
       )
SELECT m.[CompanyId]
     , m.[CurrencyId]
     , CASE WHEN m.[RecordedAt] BETWEEN @ym4f AND @ym4t THEN @ym4t
            WHEN m.[RecordedAt] BETWEEN @ym3f AND @ym3t THEN @ym3t
            WHEN m.[RecordedAt] BETWEEN @ym2f AND @ym2t THEN @ym2t
            WHEN m.[RecordedAt] BETWEEN @ym1f AND @ym1t THEN @ym1t
            WHEN m.[RecordedAt] BETWEEN @ym0f AND @ym0t THEN @ym0t
        END [YearMonth]
     , COALESCE(pcs.[Id], CASE cs.[IsParent] WHEN 1 THEN cs.[Id] ELSE 0 END) [ParentCustomerId]
     , m.[CustomerId]
     , m.[StaffId]
     , m.[DepartmentId]
     , m.[MatchingAmount]
  FROM (
       SELECT b.[CompanyId]
            , b.[CurrencyId]
            , b.[CustomerId]
            , {stafId}  [StaffId]
            , {deptId}  [DepartmentId]
            , CASE WHEN m.[RecordedAt] < {targetDate} THEN {targetDate} ELSE m.[RecordedAt] END [RecordedAt]
            , SUM( m.[Amount]
                 + m.[BankTransferFee]
                 - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
                 + COALESCE(mbd.[DiscountAmount], 0)
                 ) [MatchingAmount]
         FROM [dbo].[Matching] m
        INNER JOIN [dbo].[Billing] b         ON b.[Id]           = m.[BillingId]
                                            AND b.[CompanyId]    = @CompanyId");
            if (option.CurrencyId.HasValue) builder.Append(@"
                                            AND b.[CurrencyId]   = @CurrencyId");
            builder.Append($@"
                                            AND b.[Approved]     = 1
                                            AND b.[InputType]    <> 3
                                            AND CASE WHEN m.[RecordedAt] < {targetDate} THEN {targetDate} ELSE m.[RecordedAt] END
                                                BETWEEN @ym4f AND @ym0t
         LEFT JOIN (
              SELECT mbd.[MatchingId]
                   , SUM(mbd.[DiscountAmount]) [DiscountAmount]
                FROM [dbo].[MatchingBillingDiscount] mbd
               GROUP BY mbd.[MatchingId]
              ) mbd
           ON mbd.[MatchingId] = m.[Id]
        INNER JOIN [dbo].[Customer] cs       ON cs.[Id]           = b.[CustomerId]
                                            AND cs.[Code]        >= COALESCE(@CustomerCodeFrom, cs.[Code])
                                            AND cs.[Code]        <= COALESCE(@CustomerCodeTo  , cs.[Code])
                                            AND cs.[ClosingDay]   = COALESCE(@ClosingDay, cs.[ClosingDay])
        INNER JOIN [dbo].[Staff] st          ON st.[Id]           = {stafAlias}.[StaffId]
                                            AND st.[Code]        >= COALESCE(@StaffCodeFrom, st.[Code])
                                            AND st.[Code]        <= COALESCE(@StaffCodeTo  , st.[Code])
        INNER JOIN [dbo].[Department] dp     ON dp.[Id]           = {deptAlias}.[DepartmentId]
                                            AND dp.[Code]        >= COALESCE(@DepartmentCodeFrom, dp.[Code])
                                            AND dp.[Code]        <= COALESCE(@DepartmentCodeTo  , dp.[Code])
        GROUP BY
              b.[CompanyId]
            , b.[CurrencyId]
            , b.[CustomerId]");
            if (option.RequireStaffSubtotal)
                builder.Append(@"
            , st.[Id]");
            if (option.RequireDepartmentSubtotal)
                builder.Append(@"
            , dp.[Id]");
            builder.Append($@"
            , CASE WHEN m.[RecordedAt] < {targetDate} THEN {targetDate} ELSE m.[RecordedAt] END
       ) m
 INNER JOIN [dbo].[Customer] cs         ON cs.[Id]      = m.[CustomerId]
  LEFT JOIN [dbo].[CustomerGroup] csg   ON cs.[Id]      = csg.[ChildCustomerId]
  LEFT JOIN [dbo].[Customer] pcs        ON pcs.[Id]     = csg.[ParentCustomerId]
  OPTION ( FORCE ORDER )

");
            return builder.ToString();
        }

        private string GetQuerySelectBillingAgingList(BillingAgingListSearch option)
        {
            var builder = new StringBuilder();
            builder.Append(@"
SELECT ba.*
     , cr.[Code] [CurrencyCode]
     , cs.[Code] [CustomerCode]
     , COALESCE(dp.[Code] , N'') [DepartmentCode]
     , COALESCE(st.[Code] , N'') [StaffCode]
     , COALESCE(pcs.[Code], N'') [ParentCustomerCode]
     , COALESCE(cs.[Name] , N'得意先コード不明'  ) [CustomerName]
     , COALESCE(dp.[Name] , N'請求部門コード不明') [DepartmentName]
     , COALESCE(st.[Name] , N'担当者コード不明'  ) [StaffName]
     , COALESCE(pcs.[Name], N'') [ParentCustomerName]
  FROM (
       SELECT ba.[CompanyId]
            , ba.[CurrencyId]
            , CASE WHEN ba2.[HasAnyChildren]    = 0
                     OR ba.[ParentCustomerId]   = 0
                     OR @ConsiderCustomerGroup  = 0   THEN NULL ELSE ba.[ParentCustomerId] END [ParentCustomerId]
            , ba.[CustomerId]
            , ba.[StaffId]
            , ba.[DepartmentId]
            , ba.[Balance]
            , ba.[BillingAmountK]
            , ba.[BillingAmount0]
            , ba.[BillingAmount1]
            , ba.[BillingAmount2]
            , ba.[BillingAmount3]
            , ba.[BillingAmount4]
            , ba.[ReceiptAmountK]
            , ba.[ReceiptAmount0]
            , ba.[ReceiptAmount1]
            , ba.[ReceiptAmount2]
            , ba.[ReceiptAmount3]
            , ba.[ReceiptAmount4]
            , ba.[BillingMatchingAmountK]
            , ba.[BillingMatchingAmount0]
            , ba.[BillingMatchingAmount1]
            , ba.[BillingMatchingAmount2]
            , ba.[BillingMatchingAmount3]
            , ba.[BillingMatchingAmount4]
            , ba.[MatchingAmountK]
            , ba.[MatchingAmount0]
            , ba.[MatchingAmount1]
            , ba.[MatchingAmount2]
            , ba.[MatchingAmount3]
            , ba.[MatchingAmount4]
         FROM (
              SELECT ba.[CompanyId]
                   , ba.[CurrencyId]
                   , ba.[ParentCustomerId]
                   , ba.[CustomerId]
                   , ba.[StaffId]
                   , ba.[DepartmentId]
                   , SUM( ba.[Balance] ) [Balance]
                   , SUM( CASE WHEN ba.[YearMonth] IN (@ym4t, @ym3t, @ym2t, @ym1t)    THEN ba.[BillingAmount]         ELSE 0 END ) [BillingAmountK]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym0t                            THEN ba.[BillingAmount]         ELSE 0 END ) [BillingAmount0]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym1t                            THEN ba.[BillingAmount]         ELSE 0 END ) [BillingAmount1]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym2t                            THEN ba.[BillingAmount]         ELSE 0 END ) [BillingAmount2]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym3t                            THEN ba.[BillingAmount]         ELSE 0 END ) [BillingAmount3]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym4t                            THEN ba.[BillingAmount]         ELSE 0 END ) [BillingAmount4]
                   , SUM( CASE WHEN ba.[YearMonth] IN (@ym4t, @ym3t, @ym2t, @ym1t)    THEN ba.[ReceiptAmount]         ELSE 0 END ) [ReceiptAmountK]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym0t                            THEN ba.[ReceiptAmount]         ELSE 0 END ) [ReceiptAmount0]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym1t                            THEN ba.[ReceiptAmount]         ELSE 0 END ) [ReceiptAmount1]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym2t                            THEN ba.[ReceiptAmount]         ELSE 0 END ) [ReceiptAmount2]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym3t                            THEN ba.[ReceiptAmount]         ELSE 0 END ) [ReceiptAmount3]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym4t                            THEN ba.[ReceiptAmount]         ELSE 0 END ) [ReceiptAmount4]
                   , SUM( CASE WHEN ba.[YearMonth] IN (@ym4t, @ym3t, @ym2t, @ym1t)    THEN ba.[BillingMatchingAmount] ELSE 0 END ) [BillingMatchingAmountK]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym0t                            THEN ba.[BillingMatchingAmount] ELSE 0 END ) [BillingMatchingAmount0]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym1t                            THEN ba.[BillingMatchingAmount] ELSE 0 END ) [BillingMatchingAmount1]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym2t                            THEN ba.[BillingMatchingAmount] ELSE 0 END ) [BillingMatchingAmount2]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym3t                            THEN ba.[BillingMatchingAmount] ELSE 0 END ) [BillingMatchingAmount3]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym4t                            THEN ba.[BillingMatchingAmount] ELSE 0 END ) [BillingMatchingAmount4]
                   , SUM( CASE WHEN ba.[YearMonth] IN (@ym4t, @ym3t, @ym2t, @ym1t)    THEN ba.[MatchingAmount]        ELSE 0 END ) [MatchingAmountK]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym0t                            THEN ba.[MatchingAmount]        ELSE 0 END ) [MatchingAmount0]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym1t                            THEN ba.[MatchingAmount]        ELSE 0 END ) [MatchingAmount1]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym2t                            THEN ba.[MatchingAmount]        ELSE 0 END ) [MatchingAmount2]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym3t                            THEN ba.[MatchingAmount]        ELSE 0 END ) [MatchingAmount3]
                   , SUM( CASE WHEN ba.[YearMonth] = @ym4t                            THEN ba.[MatchingAmount]        ELSE 0 END ) [MatchingAmount4]
                FROM #BillingAging ba
               WHERE ba.[CompanyId] = @CompanyId
               GROUP BY
                     ba.[CompanyId]
                   , ba.[CurrencyId]
                   , ba.[DepartmentId]
                   , ba.[StaffId]
                   , ba.[ParentCustomerId]
                   , ba.[CustomerId]
              ) ba
   INNER JOIN (
         SELECT ba.[CompanyId]
              , ba.[CurrencyId]
              , ba.[StaffId]
              , ba.[DepartmentId]
              , ba.[ParentCustomerId]
              , SUM( CASE WHEN ba.[ParentCustomerId] = 0 OR ba.[ParentCustomerId] = ba.[CustomerId] THEN 0 ELSE 1 END ) [HasAnyChildren]
           FROM #BillingAging ba
          GROUP BY
                ba.[CompanyId]
              , ba.[CurrencyId]
              , ba.[DepartmentId]
              , ba.[StaffId]
              , ba.[ParentCustomerId]
              ) ba2
       ON ba.[CompanyId]        = ba2.[CompanyId]
      AND ba.[CurrencyId]       = ba2.[CurrencyId]
      AND ba.[StaffId]          = ba2.[StaffId]
      AND ba.[DepartmentId]     = ba2.[DepartmentId]
      AND ba.[ParentCustomerId] = ba2.[ParentCustomerId]
     ) ba
  LEFT JOIN [dbo].[Customer] cs         ON cs.[Id]      = ba.[CustomerId]
  LEFT JOIN [dbo].[Department] dp       ON dp.[Id]      = ba.[DepartmentId]
  LEFT JOIN [dbo].[Staff] st            ON st.[Id]      = ba.[StaffId]
  LEFT JOIN [dbo].[Currency] cr         ON cr.[Id]      = ba.[CurrencyId]
  LEFT JOIN [dbo].[Customer] pcs        ON pcs.[Id]     = ba.[ParentCustomerId]
 ORDER BY cr.[Code]");
            if (option.RequireDepartmentSubtotal)
                builder.Append(@"
     , CASE WHEN dp.[Code] IS NULL THEN 1 ELSE 0 END, dp.[Code]");
            if (option.RequireStaffSubtotal)
                builder.Append(@"
     , CASE WHEN st.[Code] IS NULL THEN 1 ELSE 0 END, st.[Code]");
            builder.Append(@"
     , COALESCE(pcs.[Code], cs.[Code])
 OPTION ( FORCE ORDER )

");
            return builder.ToString();
        }

        public Task<IEnumerable<BillingAgingList>> GetAsync(BillingAgingListSearch searchOption,
            IProgressNotifier notifier = null, CancellationToken token = default(CancellationToken))
        {
            return dbHelper.ExecuteQueriesAsync(async connection =>
            {
                await dbHelper.ExecuteAsync(connection, GetQueryInitializeTempTable(searchOption), searchOption, token);
                notifier?.UpdateState();
                await dbHelper.ExecuteAsync(connection, GetQueryInsertBillingReamin(searchOption), searchOption, token);
                notifier?.UpdateState();
                if (searchOption.BillingRemainType > 0)
                {
                    await dbHelper.ExecuteAsync(connection, GetQueryInsertReceiptRemain(searchOption), searchOption, token);
                    notifier?.UpdateState();
                }
                await dbHelper.ExecuteAsync(connection, GetQueryInsertMatchingReamin(searchOption), searchOption, token);
                notifier?.UpdateState();
                var items = await dbHelper.QueryAsync<BillingAgingList>(connection, GetQuerySelectBillingAgingList(searchOption), searchOption, token);
                notifier?.UpdateState();
                return items;
            });
        }

        public Task<IEnumerable<BillingAgingListDetail>> GetDetailsAsync(BillingAgingListSearch SearchOption, CancellationToken token)
        {
            var staffJoin = SearchOption.IsMasterStaff ? "cs.[StaffId]" : "b.[StaffId]";
            var departmentJoin = SearchOption.IsMasterStaff ? "st.[DepartmentId]" : "b.[DepartmentId]";
            var targetDate = SearchOption.TargetDate == 0 ? "b.[BilledAt]" : "b.[SalesAt]";
            var builder = new StringBuilder();
            builder.Append($@"
IF EXISTS (SELECT * FROM tempdb..sysobjects
 WHERE id = object_id(N'tempdb..#BillingAgingDetail'))
DROP   TABLE #BillingAgingDetail;

CREATE TABLE #BillingAgingDetail
([Id]                   BIGINT          NOT NULL
,[BillingAmount]        NUMERIC(18, 5)  NOT NULL);

CREATE INDEX IdxBillingAgingDetail ON #BillingAgingDetail
( [Id] );

INSERT INTO #BillingAgingDetail
SELECT b.[Id]
     , b.[BillingAmount]
     - CASE WHEN b.[DeleteAt] = b.[DeleteAt] THEN b.[RemainAmount] ELSE 0 END [BillingAmount]
  FROM [dbo].[Billing] b
 INNER JOIN [dbo].[Customer] cs
    ON cs.[Id]          = b.[CustomerId]
   AND {targetDate} BETWEEN @ymf AND @ymt
 INNER JOIN [dbo].[Staff] st
    ON st.[Id]          = {staffJoin}
 INNER JOIN [dbo].[Department] dp
    ON dp.[Id]          = {departmentJoin}
 WHERE b.[CompanyId]    = @CompanyId");
            if (SearchOption.CurrencyId.HasValue) builder.Append(@"
   AND b.[CurrencyId]   = @CurrencyId");
            builder.Append($@"
   AND (    b.[CustomerId]  = @CustomerId
         OR b.[CustomerId] IN (
            SELECT csg.[ChildCustomerId]
              FROM [dbo].[CustomerGroup] csg
             WHERE csg.[ParentCustomerId]   = @CustomerId )
       )
   AND b.[BillingAmount] <> CASE WHEN b.[DeleteAt] = b.[DeleteAt] THEN b.[RemainAmount] ELSE 0 END");
            if (!string.IsNullOrEmpty(SearchOption.CustomerCodeFrom)) builder.Append(@"
   AND cs.[Code]       >= @CustomerCodeFrom");
            if (!string.IsNullOrEmpty(SearchOption.CustomerCodeTo)) builder.Append(@"
   AND cs.[Code]       <= @CustomerCodeTo");
            if (SearchOption.ClosingDay.HasValue) builder.Append(@"
   AND cs.[ClosingDay]  = @ClosingDay");
            if (!string.IsNullOrEmpty(SearchOption.StaffCodeFrom)) builder.Append(@"
   AND st.[Code]       >= @StaffCodeFrom");
            if (!string.IsNullOrEmpty(SearchOption.StaffCodeTo)) builder.Append(@"
   AND st.[Code]       <= @StaffCodeTo");
            if (!string.IsNullOrEmpty(SearchOption.DepartmentCodeFrom)) builder.Append(@"
   AND dp.[Code]       >= @DepartmentCodeFrom");
            if (!string.IsNullOrEmpty(SearchOption.DepartmentCodeTo)) builder.Append(@"
   AND dp.[Code]       <= @DepartmentCodeTo");

            builder.Append($@"
SELECT b.[Id]
     , b.[CompanyId]
     , b.[CurrencyId]
     , b.[BilledAt]
     , b.[DueAt]
     , b.[SalesAt]
     , bad.[BillingAmount]
     , bad.[BillingAmount]
     - COALESCE(m.[MatchingAmount], 0) [RemainAmount]
     , b.[InvoiceCode]
     , b.[Note1] [Note]
     , cs.[Code] [CustomerCode]
     , cs.[Name] [CustomerName]
     , st.[Code] [StaffCode]
     , st.[Name] [StaffName]
     , dp.[Code] [DepartmentCode]
     , dp.[Name] [DepartmentName]
     ,  c.[Code] [CurrencyCode]
  FROM #BillingAgingDetail bad
 INNER JOIN [dbo].[Billing] b       ON b.[Id]       = bad.[Id]
 INNER JOIN [dbo].[Customer] cs     ON cs.[Id]      = b.[CustomerId]
 INNER JOIN [dbo].[Staff] st        ON st.[Id]      = {staffJoin}
 INNER JOIN [dbo].[Department] dp   ON dp.[Id]      = {departmentJoin}
 INNER JOIN [dbo].[Currency] c      ON c.[Id]       = b.[CurrencyId]
  LEFT JOIN (
       SELECT m.[BillingId]
            , SUM( m.[Amount]
                 + m.[BankTransferFee]
                 - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
                 + COALESCE(mbd.[DiscountAmount], 0)
                 ) [MatchingAmount]
         FROM #BillingAgingDetail bad
        INNER JOIN [dbo].[Matching] m
           ON bad.[Id]      = m.[BillingId]
          AND m.[RecordedAt] <= @ym0t
         LEFT JOIN (
              SELECT mbd.[MatchingId]
                   , SUM( mbd.[DiscountAmount] ) [DiscountAmount]
                FROM [dbo].[MatchingBillingDiscount] mbd
               GROUP BY mbd.[MatchingId]
              ) mbd
           ON mbd.[MatchingId]      = m.[Id]
        GROUP BY m.[BillingId]
       ) m
    ON m.[BillingId]    = b.[Id]
 WHERE bad.[BillingAmount] - COALESCE(m.[MatchingAmount], 0) <> 0");
            return dbHelper.GetItemsAsync<BillingAgingListDetail>(builder.ToString(), SearchOption, token);
        }

    }
}
