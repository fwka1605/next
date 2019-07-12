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
    public class CollectionScheduleQueryProcessor :
        ICollectionScheduleQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CollectionScheduleQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task< IEnumerable<CollectionSchedule>> GetAsync(CollectionScheduleSearch searchOption, CancellationToken token, IProgressNotifier notifier)
        {
            return dbHelper.ExecuteQueriesAsync(async connection =>
            {
                await dbHelper.ExecuteAsync(connection, CreateInitializeTempTable(searchOption), searchOption, token);
                notifier?.UpdateState();
                await dbHelper.ExecuteAsync(connection, CreateInsertBillingData(searchOption), searchOption, token);
                notifier?.UpdateState();
                await dbHelper.ExecuteAsync(connection, CreateInsertMatchingData(searchOption), searchOption, token);
                notifier?.UpdateState();
                var items = await dbHelper.QueryAsync<CollectionSchedule>(connection, CreateSelectCollectionScheduleData(searchOption), searchOption, token);
                notifier?.UpdateState();
                return items;
            });
        }
        private string CreateInitializeTempTable(CollectionScheduleSearch option)
        {
            var builder = new StringBuilder();
            builder.Append(@"
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id = object_id(N'tempdb..#List'))
    DROP TABLE #List;
CREATE TABLE #List
([CustomerId]           INT                 NOT NULL
,[CollectCategoryId]    INT                 NOT NULL
,[StaffId]              INT                 NOT NULL
,[YearMonth]            DATETIME2(3)        NOT NULL
,[BillingAmount]        NUMERIC(18, 5)      NOT NULL DEFAULT(0)
,[MatchingAmount]       NUMERIC(18, 5)      NOT NULL DEFAULT(0)
);
CREATE INDEX IdxList ON #List
([CustomerId]           ASC
,[CollectCategoryId]    ASC
,[StaffId]              ASC
,[YearMonth]            ASC);
");
            return builder.ToString();
        }

        private string CreateInsertBillingData(CollectionScheduleSearch option)
        {
            var staff      = !option.UseMasterStaff ? "b.[StaffId]"      :
                option.DisplayParent ? "COALESCE(pcs.[StaffId], cs.[StaffId])" : "cs.[StaffId]";
            var department = option.UseMasterStaff ? "st.[DepartmentId]" : "b.[DepartmentId]";
            var customerId = option.DisplayParent ? "COALESCE(pcs.[Id], cs.[Id])" : "cs.[Id]";
            var customerCode = option.DisplayParent ? "COALESCE(pcs.[Code], cs.[Code])" : "cs.[Code]";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #List
([CustomerId]
,[CollectCategoryId]
,[StaffId]
,[YearMonth]
,[BillingAmount]
)
SELECT {customerId} [CustomerId]
     , b.[CollectCategoryId]
     , {staff}
     , CASE
         WHEN b.[DueAt] <= @dtprt               THEN @dtprt
         WHEN b.[DueAt] BETWEEN @dt0f AND @dt0t THEN @dt0t
         WHEN b.[DueAt] BETWEEN @dt1f AND @dt1t THEN @dt1t
         WHEN b.[DueAt] BETWEEN @dt2f AND @dt2t THEN @dt2t
         WHEN b.[DueAt] >= @dt3f                THEN @dt3t
       END [YearMonth]
     , SUM( b.[BillingAmount]
          - CASE WHEN b.[DeleteAt] IS NULL THEN 0 ELSE b.[RemainAmount] END
       ) [BillingAmount]
  FROM [dbo].[Billing] b
 INNER JOIN [dbo].[Customer] cs
    ON cs.[Id]          = b.[CustomerId]
   AND b.[CompanyId]    = @CompanyId
   AND b.[Approved]     = 1
   AND b.[InputType]   <> 3
   AND b.[BilledAt]    <= @dt0t");
            if (option.DisplayParent)
                builder.Append(@"
  LEFT JOIN [dbo].[CustomerGroup] csg
    ON csg.[ChildCustomerId]    = b.[CustomerId]
  LEFT JOIN [dbo].[Customer] pcs
    ON pcs.[Id]         = csg.[ParentCustomerId]");
            builder.Append($@"
 INNER JOIN [dbo].[Staff] st
    ON st.[Id]          = {staff}");
            if (!string.IsNullOrEmpty(option.StaffCodeFrom))
                builder.Append(@"
   AND st.[Code]       >= @StaffCodeFrom");
            if (!string.IsNullOrEmpty(option.StaffCodeTo))
                builder.Append(@"
   AND st.[Code]       <= @StaffCodeTo");
            builder.Append($@"
 INNER JOIN [dbo].[Department] dp
    ON dp.[Id]          = {department}");
            if (!string.IsNullOrEmpty(option.DepartmentCodeFrom))
                builder.Append(@"
   AND dp.[Code]       >= @DepartmentCodeFrom");
            if (!string.IsNullOrEmpty(option.DepartmentCodeTo))
                builder.Append(@"
   AND dp.[Code]       <= @DepartmentCodeTo");
            builder.Append(@"
 WHERE b.[Id]           = b.[Id]");
            if (!string.IsNullOrEmpty(option.CustomerCodeFrom))
                builder.Append($@"
   AND {customerCode}       >= @CustomerCodeFrom");
            if (!string.IsNullOrEmpty(option.CustomerCodeTo))
                builder.Append($@"
   AND {customerCode}       <= @CustomerCodeTo");
            builder.Append($@"
 GROUP BY
       {customerId}
     , b.[CollectCategoryId]
     , {staff}
     , CASE
         WHEN b.[DueAt] <= @dtprt               THEN @dtprt
         WHEN b.[DueAt] BETWEEN @dt0f AND @dt0t THEN @dt0t
         WHEN b.[DueAt] BETWEEN @dt1f AND @dt1t THEN @dt1t
         WHEN b.[DueAt] BETWEEN @dt2f AND @dt2t THEN @dt2t
         WHEN b.[DueAt] >= @dt3f                THEN @dt3t
       END");
            return builder.ToString();
        }

        private string CreateInsertMatchingData( CollectionScheduleSearch option)
        {
            var staff       = !option.UseMasterStaff ? "b.[StaffId]"      :
                option.DisplayParent  ? "COALESCE(pcs.[StaffId], cs.[StaffId])" : "cs.[StaffId]";
            var department  = option.UseMasterStaff ? "st.[DepartmentId]" : "b.[DepartmentId]";
            var customerId = option.DisplayParent ? "COALESCE(pcs.[Id], cs.[Id])" : "cs.[Id]";
            var customerCode = option.DisplayParent ? "COALESCE(pcs.[Code], cs.[Code])" : "cs.[Code]";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #List
([CustomerId]
,[CollectCategoryId]
,[StaffId]
,[YearMonth]
,[MatchingAmount]
)
SELECT {customerId} [CustomerId]
     , b.[CollectCategoryId]
     , {staff}
     , CASE
         WHEN b.[DueAt] <= @dtprt               THEN @dtprt
         WHEN b.[DueAt] BETWEEN @dt0f AND @dt0t THEN @dt0t
         WHEN b.[DueAt] BETWEEN @dt1f AND @dt1t THEN @dt1t
         WHEN b.[DueAt] BETWEEN @dt2f AND @dt2t THEN @dt2t
         WHEN b.[DueAt] >= @dt3f                THEN @dt3t
       END [YearMonth]
     , SUM( m.[Amount]
          + m.[BankTransferFee]
          - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
          + COALESCE( mbd.[DiscountAmount], 0 ) ) [MatchingAmount]
  FROM [dbo].[Billing] b
 INNER JOIN [dbo].[Matching] m
    ON b.[Id]           = m.[BillingId]
   AND b.[CompanyId]    = @CompanyId
   AND b.[Approved]     = 1
   AND b.[InputType]   <> 3
   AND b.[BilledAt]    <= @dt0t
   AND m.[RecordedAt]  <= @dt0t
 INNER JOIN [dbo].[Customer] cs
    ON cs.[Id]          = b.[CustomerId]");
            if (option.DisplayParent)
                builder.Append(@"
  LEFT JOIN [dbo].[CustomerGroup] csg
    ON csg.[ChildCustomerId]    = b.[CustomerId]
  LEFT JOIN [dbo].[Customer] pcs
    ON pcs.[Id]         = csg.[ParentCustomerId]");
            builder.Append($@"
 INNER JOIN [dbo].[Staff] st
    ON st.[Id]          = {staff}");
            if (!string.IsNullOrEmpty(option.StaffCodeFrom))
                builder.Append(@"
   AND st.[Code]       >= @StaffCodeFrom");
            if (!string.IsNullOrEmpty(option.StaffCodeTo))
                builder.Append(@"
   AND st.[Code]       <= @StaffCodeTo");
            builder.Append($@"
 INNER JOIN [dbo].[Department] dp
    ON dp.[Id]          = {department}");
            if (!string.IsNullOrEmpty(option.DepartmentCodeFrom))
                builder.Append(@"
   AND dp.[Code]       >= @DepartmentCodeFrom");
            if (!string.IsNullOrEmpty(option.DepartmentCodeTo))
                builder.Append(@"
   AND dp.[Code]       <= @DepartmentCodeTo");

            builder.Append($@"
  LEFT JOIN (
       SELECT mbd.[MatchingId]
            , SUM( mbd.[DiscountAmount] ) [DiscountAmount]
         FROM [dbo].[Matching] m
        INNER JOIN [dbo].[Billing] b
           ON b.[Id]            = m.[BillingId]
          AND b.[CompanyId]     = @CompanyId
          AND b.[Approved]      = 1
          AND b.[InputType]    <> 3
          AND b.[BilledAt]     <= @dt0t
        INNER JOIN [dbo].[MatchingBillingDiscount] mbd
           ON m.[Id]        = mbd.[MatchingId]
        GROUP BY
              mbd.[MatchingId]
       ) mbd
    ON m.[Id]           = mbd.[MatchingId]
 WHERE b.[Id]           = b.[Id]");
            if (!string.IsNullOrEmpty(option.CustomerCodeFrom))
                builder.Append($@"
   AND {customerCode}       >= @CustomerCodeFrom");
            if (!string.IsNullOrEmpty(option.CustomerCodeTo))
                builder.Append($@"
   AND {customerCode}       <= @CustomerCodeTo");
            builder.Append($@"
 GROUP BY
       {customerId}
     , b.[CollectCategoryId]
     , {staff}
     , CASE
         WHEN b.[DueAt] <= @dtprt               THEN @dtprt
         WHEN b.[DueAt] BETWEEN @dt0f AND @dt0t THEN @dt0t
         WHEN b.[DueAt] BETWEEN @dt1f AND @dt1t THEN @dt1t
         WHEN b.[DueAt] BETWEEN @dt2f AND @dt2t THEN @dt2t
         WHEN b.[DueAt] >= @dt3f                THEN @dt3t
       END");
            return builder.ToString();
        }

        private string CreateSelectCollectionScheduleData(CollectionScheduleSearch option)
        {
            var builder = new StringBuilder();
            builder.Append(@"
SELECT ls.*
     , cs.[Code] [CustomerCode]
     , cs.[Name] [CustomerName]
     , cs.[ClosingDay]
     , cs.[CollectCategoryId]   [CustomerCollectCategoryId]
     , cs.[SightOfBill]         [CustomerSightOfBill]
     , dp.[Code] [DepartmentCode]
     , dp.[Name] [DepartmentName]
     , st.[Code] [StaffCode]
     , st.[Name] [StaffName]
     , ct.[Code] [CollectCategoryCode]
     , ct.[Name] [CollectCategoryName]
  FROM (
       SELECT ls.[CustomerId]
            , ls.[StaffId]
            , ls.[CollectCategoryId]
            , SUM( CASE ls.[YearMonth] WHEN @dtprt  THEN ls.[BillingAmount]  ELSE 0 END )
            - SUM( CASE ls.[YearMonth] WHEN @dtprt  THEN ls.[MatchingAmount] ELSE 0 END ) [UncollectedAmountLast]
            , SUM( CASE ls.[YearMonth] WHEN @dt0t   THEN ls.[BillingAmount]  ELSE 0 END )
            - SUM( CASE ls.[YearMonth] WHEN @dt0t   THEN ls.[MatchingAmount] ELSE 0 END ) [UncollectedAmount0]
            , SUM( CASE ls.[YearMonth] WHEN @dt1t   THEN ls.[BillingAmount]  ELSE 0 END )
            - SUM( CASE ls.[YearMonth] WHEN @dt1t   THEN ls.[MatchingAmount] ELSE 0 END ) [UncollectedAmount1]
            , SUM( CASE ls.[YearMonth] WHEN @dt2t   THEN ls.[BillingAmount]  ELSE 0 END )
            - SUM( CASE ls.[YearMonth] WHEN @dt2t   THEN ls.[MatchingAmount] ELSE 0 END ) [UncollectedAmount2]
            , SUM( CASE ls.[YearMonth] WHEN @dt3t   THEN ls.[BillingAmount]  ELSE 0 END )
            - SUM( CASE ls.[YearMonth] WHEN @dt3t   THEN ls.[MatchingAmount] ELSE 0 END ) [UncollectedAmount3]
         FROM #List ls
        GROUP BY
              ls.[CustomerId]
            , ls.[StaffId]
            , ls.[CollectCategoryId]
       ) ls
 INNER JOIN [dbo].[Customer] cs
    ON cs.[Id]                  = ls.[CustomerId]
 INNER JOIN [dbo].[Category] ct
    ON ct.[Id]                  = ls.[CollectCategoryId]
 INNER JOIN [dbo].[Staff] st
    ON st.[Id]                  = ls.[StaffId]
 INNER JOIN [dbo].[Department] dp
    ON dp.[Id]                  = st.[DepartmentId]
 WHERE NOT (ls.[UncollectedAmountLast]  = 0
        AND ls.[UncollectedAmount0]     = 0
        AND ls.[UncollectedAmount1]     = 0
        AND ls.[UncollectedAmount2]     = 0
        AND ls.[UncollectedAmount3]     = 0 )
 ORDER BY
       dp.[Code] ASC
     , st.[Code] ASC
     , cs.[Code] ASC
     , ct.[Code] ASC");
            return builder.ToString();
        }
    }
}
