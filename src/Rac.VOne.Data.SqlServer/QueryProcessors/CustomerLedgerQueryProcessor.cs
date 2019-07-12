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
    public class CustomerLedgerQueryProcessor
        : ICustomerLedgerQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CustomerLedgerQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<CustomerLedger>> GetAsync(CustomerLedgerSearch searchOption, CancellationToken token, IProgressNotifier notifier)
        {
            return dbHelper.ExecuteQueriesAsync(async connection =>
            {
                await dbHelper.ExecuteAsync(connection, CreateInitializeTempTable(searchOption), searchOption, token);
                notifier?.UpdateState();
                await dbHelper.ExecuteAsync(connection, CreateInsertBilling(searchOption), searchOption, token);
                notifier?.UpdateState();
                if (searchOption.RequireReceiptData)
                {
                    await dbHelper.ExecuteAsync(connection, CreateInsertReceipt(searchOption), searchOption, token);
                    notifier?.UpdateState();
                }
                await dbHelper.ExecuteAsync(connection, CreateInsertMatching(searchOption), searchOption, token);
                notifier?.UpdateState();
                var items = await dbHelper.QueryAsync<CustomerLedger>(connection, CreateSelectLedger(searchOption), searchOption, token);
                notifier?.UpdateState();
                return items;
            });
        }

        /// <summary>消込記号用　消込ヘッダー取得</summary>
        /// <param name="searchOption"></param>
        /// <returns>債権代表者ID, 消込ヘッダーID</returns>
        public async Task<IEnumerable<KeyValuePair<int, long>>> GetKeysAsync(CustomerLedgerSearch searchOption, CancellationToken token, IProgressNotifier notifier)
        {
            var items = await dbHelper.GetItemsAsync<KeyValuePair<int, long>>(CreateSelectKeys(searchOption), searchOption, token);
            notifier?.UpdateState();
            return items;
        }

        /// <summary>繰越用 残高取得</summary>
        /// <param name="searchOption"></param>
        /// <returns>債権代表者ID, 繰越残高</returns>
        public async Task<IEnumerable<KeyValuePair<int, decimal>>> GetBalancesAsync(CustomerLedgerSearch searchOption, CancellationToken token, IProgressNotifier notifier)
        {
            var items = await dbHelper.GetItemsAsync<KeyValuePair<int, decimal>>(CreateSelectBalance(searchOption), searchOption, token);
            notifier?.UpdateState();
            return items;
        }


        private string CreateInitializeTempTable(CustomerLedgerSearch option)
        {
            var builder = new StringBuilder();
            builder.Append(@"
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id = object_id(N'tempdb..#Ledger'))
    DROP TABLE #Ledger;
CREATE TABLE #Ledger
([CompanyId]        INT                                         NOT NULL
,[ParentCustomerId] INT                                         NOT NULL
,[RecordedAt]       DATETIME2(3)                                NOT NULL
,[DataType]         INT                                         NOT NULL
,[CustomerId]       INT                                         NOT NULL
,[DepartmentId]     INT                                         NOT NULL
,[SectionId]        INT                                         NOT NULL
,[InvoiceCode]      NVARCHAR(100) COLLATE Japanese_CI_AS_KS_WS      NULL
,[CategoryId]       INT                                         NOT NULL
,[AccountTitleId]   INT                                         NOT NULL
,[CurrencyId]       INT                                         NOT NULL
,[BillingInputId]   BIGINT                                          NULL
,[TransactionId]    BIGINT                                          NULL
,[BillingAmount]    NUMERIC(18, 5)                                  NULL
,[ReceiptAmount]    NUMERIC(18, 5)                                  NULL
,[MatchingAmount]   NUMERIC(18, 5)                                  NULL
,[HeaderId1]        BIGINT                                          NULL
,[HeaderId2]        BIGINT                                          NULL
,[HeaderId3]        BIGINT                                          NULL
,[HeaderId4]        BIGINT                                          NULL
,[HeaderId5]        BIGINT                                          NULL
,[HeaderId6]        BIGINT                                          NULL
,[HeaderId7]        BIGINT                                          NULL
);
");
            return builder.ToString();
        }

        private string CreateInsertBilling(CustomerLedgerSearch option)
        {
            var targetDate   = option.UseBilledAt                       ? "b.[BilledAt]"            : "b.[SalesAt]";
            var department   = option.RequireBillingSubtotal
                || option.RequireBillingInputIdSubotal
                || !option.DisplayDepartment                            ? "0"                       : "MIN( b.[DepartmentId] )";
            var customer     = option.RequireBillingSubtotal            ? "b.[CustomerId]"          : "MIN( b.[CustomerId] )";
            var category     = option.RequireBillingCategorySubtotal    ? "b.[BillingCategoryId]"
                             : option.DisplayBillingCategory            ? "MIN( b.[BillingCategoryId] )" : "0";
            var accountTitle = option.RequireDebitAccountTitleSubtotal  ? "COALESCE(b.[DebitAccountTitleId], 0)"
                             : option.DisplayDebitAccountTitle          ? "MIN( COALESCE(b.[DebitAccountTitleId], 0) )" : "0";
            var invoiceCode  = option.DisplayInvoiceCode                ? "MIN( b.[InvoiceCode] )"  : "N''";
            var inputId      = option.RequireBillingInputIdSubotal      ? "COALESCE( b.[BillingInputId], b.[Id] )" : "MIN( COALESCE( b.[BillingInputId], b.[Id] ) )";
            var transId      = option.RequireBillingInputIdSubotal || option.RequireBillingSubtotal
                                                                        ? "MIN( b.[Id] )"           : "b.[Id]";
            var recordedAt   = option.RequireBillingSubtotal || option.RequireBillingInputIdSubotal
                                                                        ? targetDate                : $"MIN( {targetDate} )";
            var partition
                = option.RequireBillingCategorySubtotal                 ? $"{targetDate}, b.[CustomerId], b.[BillingCategoryId]"
                : option.RequireDebitAccountTitleSubtotal               ? $"{targetDate}, b.[CustomerId], COALESCE(b.[DebitAccountTitleId], 0)"
                : option.RequireBillingInputIdSubotal                   ? $"COALESCE(b.[BillingInputId], b.[Id]), {targetDate}"
                : "b.[Id]";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #Ledger
([CompanyId]
,[ParentCustomerId]
,[RecordedAt]
,[DataType]
,[CustomerId]
,[DepartmentId]
,[SectionId]
,[InvoiceCode]
,[CategoryId]
,[AccountTitleId]
,[CurrencyId]
,[BillingInputId]
,[TransactionId]
,[BillingAmount]
,[HeaderId1],[HeaderId2],[HeaderId3],[HeaderId4],[HeaderId5],[HeaderId6],[HeaderId7]
)
SELECT W.[CompanyId]
     , W.[ParentCustomerId]
     , W.[RecordedAt]
     , 1  [DataType]
     , W.[CustomerId]
     , W.[DepartmentId]
     , 0  [SectionId]
     , W.[InvoiceCode]
     , W.[CategoryId]
     , W.[AccountTitleId]
     , W.[CurrencyId]
     , W.[BillingInputId]
     , W.[TransactionId]
     , SUM( W.[BillingAmount] ) [BillingAmount]
     , MIN( CASE W.[MatchingSeq] WHEN 1 THEN W.[MatchingHeaderId] ELSE NULL END ) [HeaderId1]
     , MIN( CASE W.[MatchingSeq] WHEN 2 THEN W.[MatchingHeaderId] ELSE NULL END ) [HeaderId2]
     , MIN( CASE W.[MatchingSeq] WHEN 3 THEN W.[MatchingHeaderId] ELSE NULL END ) [HeaderId3]
     , MIN( CASE W.[MatchingSeq] WHEN 4 THEN W.[MatchingHeaderId] ELSE NULL END ) [HeaderId4]
     , MIN( CASE W.[MatchingSeq] WHEN 5 THEN W.[MatchingHeaderId] ELSE NULL END ) [HeaderId5]
     , MIN( CASE W.[MatchingSeq] WHEN 6 THEN W.[MatchingHeaderId] ELSE NULL END ) [HeaderId6]
     , MIN( CASE W.[MatchingSeq] WHEN 7 THEN W.[MatchingHeaderId] ELSE NULL END ) [HeaderId7]
  FROM (
SELECT MIN( b.[CompanyId] ) [CompanyId]
     , COALESCE(pcs.[Id], cs.[Id]) [ParentCustomerId]
     , {recordedAt} [RecordedAt]
     , {customer} [CustomerId]
     , {department} [DepartmentId]
     , {invoiceCode} [InvoiceCode]
     , {category} [CategoryId]
     , {accountTitle} [AccountTitleId]
     , MIN( b.[CurrencyId] ) [CurrencyId]
     , {inputId} [BillingInputId]
     , {transId} [TransactionId]
     , SUM( CASE COALESCE(m.[BillingSeq], 1) WHEN 1
            THEN b.[BillingAmount]
               - CASE WHEN b.[DeleteAt] IS NULL THEN 0 ELSE b.[RemainAmount] END
            ELSE 0 END ) [BillingAmount]
     , m.[MatchingHeaderId]
     , ROW_NUMBER() OVER (PARTITION BY {partition} ORDER BY m.[MatchingHeaderId]) [MatchingSeq]
  FROM [dbo].[Billing] b
 INNER JOIN [dbo].[Customer] cs
    ON cs.[Id]          = b.[CustomerId]
   AND b.[CompanyId]    = @CompanyId
   AND b.[CurrencyId]   = @CurrencyId
   AND {targetDate}     BETWEEN @YearMonthFrom AND @YearMonthTo
   AND b.[Approved]     = 1
   AND b.[InputType]   <> 3
   AND ( b.[DeleteAt]   IS NULL
    OR   b.[DeleteAt]   IS NOT NULL AND b.[RemainAmount] <> b.[BillingAmount] )
  LEFT JOIN (
       SELECT b.[Id] [BillingId]
            , m.[MatchingHeaderId]
            , ROW_NUMBER() OVER (PARTITION BY b.[Id] ORDER BY m.[MatchingHeaderId]) [BillingSeq]
         FROM [dbo].[Matching] m
        INNER JOIN [dbo].[Billing] b
           ON b.[Id]            = m.[BillingId]
          AND b.[CompanyId]     = @CompanyId
          AND b.[CurrencyId]    = @CurrencyId
          AND {targetDate}      BETWEEN @YearMonthFrom AND @YearMonthTo
        INNER JOIN [dbo].[Customer] cs
           ON cs.[Id]           = b.[CustomerId]
         LEFT JOIN [dbo].[CustomerGroup] csg
           ON csg.[ChildCustomerId] = b.[CustomerId]
         LEFT JOIN [dbo].[Customer] pcs
           ON pcs.[Id]          = csg.[ParentCustomerId]
        WHERE COALESCE(pcs.[Code], cs.[Code])
                BETWEEN @CustomerCodeFrom AND @CustomerCodeTo");

            if (option.ClosingDay.HasValue)
                builder.Append(@"
          AND COALESCE(pcs.[ClosingDay], cs.[ClosingDay]) = @ClosingDay");

            builder.Append($@"
        GROUP BY
              b.[Id]
            , m.[MatchingHeaderId]
       ) m
    ON m.[BillingId]    = b.[Id]
  LEFT JOIN [dbo].[CustomerGroup] csg
    ON csg.[ChildCustomerId]    = b.[CustomerId]
  LEFT JOIN [dbo].[Customer] pcs
    ON pcs.[Id]                 = csg.[ParentCustomerId]
 WHERE COALESCE(pcs.[Code], cs.[Code])
            BETWEEN @CustomerCodeFrom AND @CustomerCodeTo");
            if (option.ClosingDay.HasValue)
                builder.Append(@"
   AND COALESCE(pcs.[ClosingDay], cs.[ClosingDay]) = @ClosingDay");

            builder.Append($@"
 GROUP BY {partition}
     , COALESCE(pcs.[Id], cs.[Id])
     , m.[MatchingHeaderId]
       ) W
 GROUP BY [CompanyId], [ParentCustomerId], [RecordedAt], [CustomerId], [DepartmentId], [InvoiceCode], [CategoryId], [AccountTitleId], [CurrencyId], [BillingInputId], [TransactionId]
 ORDER BY
       [CompanyId]
     , [ParentCustomerId]
     , [RecordedAt]
     , [CustomerId]
 OPTION ( FORCE ORDER );
");
            return builder.ToString();
        }

        private string CreateInsertReceipt(CustomerLedgerSearch option)
        {
            var recordedAt  = option.DoGroupReceipt ? "r.[RecordedAt]"          : "MIN( r.[RecordedAt] )";
            var category    = option.DoGroupReceipt ? "r.[ReceiptCategoryId]"   : "MIN( r.[ReceiptCategoryId] )";
            var transId     = option.DoGroupReceipt ? "MIN( r.[Id] )"           : "r.[Id]";
            var partition   = option.DoGroupReceipt ? "r.[RecordedAt], r.[ReceiptCategoryId]" : "r.[Id]";
            var section     = !option.DoGroupReceipt && option.DisplaySection ? "MIN( COALESCE( r.[SectionId], 0 ) )" : "0";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #Ledger
([CompanyId]
,[ParentCustomerId]
,[RecordedAt]
,[DataType]
,[CustomerId]
,[DepartmentId]
,[SectionId]
,[InvoiceCode]
,[CategoryId]
,[AccountTitleId]
,[CurrencyId]
,[BillingInputId]
,[TransactionId]
,[ReceiptAmount]
)
SELECT r.[CompanyId]
     , r.[ParentCustomerId]
     , r.[RecordedAt]
     , 2 [DATAKUBUN]
     , r.[CustomerId]
     , 0 [DepartmentId]
     , r.[SectionId]
     , N'' [InvoiceCode]
     , r.[CategoryId]
     , 0 [AccountTitleId]
     , r.[CurrencyId]
     , 0 [BillingInputId]
     , MIN( r.[Transactionid] ) [Transactionid]
     , SUM( CASE r.[ReceiptSeq] WHEN 1 THEN r.[ReceiptAmount] ELSE 0 END
          + r.[MatchingAmount] ) [ReceiptAmount]
  FROM (
SELECT MIN( r.[CompanyId] ) [CompanyId]
     , COALESCE(pcs.[Id], cs.[Id]) [ParentCustomerId]
     , {recordedAt} [RecordedAt]
     , COALESCE(pcs.[Id], cs.[Id]) [CustomerId]
     , {section} [SectionId]
     , {category} [CategoryId]
     , MIN( r.[CurrencyId] ) [CurrencyId]
     , {transId} [Transactionid]
     , ROW_NUMBER() OVER (
            PARTITION BY    {partition}
            ORDER BY        m.[MatchingHeaderId], COALESCE(pcs.[Id], cs.[Id]) ) [ReceiptSeq]
     , MIN( r.[ReceiptAmount] ) [ReceiptAmount]
     , SUM( m.[BankTransferFee]
          - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
          + COALESCE( mbd.[DiscountAmount], 0) ) [MatchingAmount]
  FROM [dbo].[Matching] m
 INNER JOIN [dbo].[Receipt] r
    ON r.[Id]           = m.[ReceiptId]
   AND r.[CompanyId]    = @CompanyId
   AND r.[CurrencyId]   = @CurrencyId
   AND r.[RecordedAt]   BETWEEN @YearMonthFrom AND @YearMonthTo
   AND r.[Approved]     = 1
 INNER JOIN [dbo].[Billing] b
    ON b.[Id]           = m.[BillingId]
   AND b.[Approved]     = 1
   AND b.[InputType]   <> 3
 INNER JOIN [dbo].[Customer] cs
    ON cs.[Id]          = b.[CustomerId]
  LEFT JOIN (
       SELECT mbd.[MatchingId]
            , SUM( mbd.[DiscountAmount] ) [DiscountAmount]
         FROM [dbo].[MatchingBillingDiscount] mbd
        INNER JOIN [dbo].[Matching] m
           ON m.[Id]            = mbd.[MatchingId]
        INNER JOIN [dbo].[MatchingHeader] mh
           ON mh.[Id]           = m.[MatchingHeaderId]
          AND mh.[CompanyId]    = @CompanyId
          AND mh.[CurrencyId]   = @CurrencyId
        GROUP BY
              mbd.[MatchingId]
       ) mbd
    ON mbd.[MatchingId] = m.[Id]
  LEFT JOIN [dbo].[CustomerGroup] csg
    ON csg.[ChildCustomerId]    = b.[CustomerId]
  LEFT JOIN [dbo].[Customer] pcs
    ON pcs.[Id]             = csg.[ParentCustomerId]
  LEFT JOIN [dbo].[Matching] om
    ON om.[ReceiptId]       = r.[OriginalReceiptId]
 WHERE COALESCE(pcs.[Code], cs.[Code])
            BETWEEN @CustomerCodeFrom AND @CustomerCodeTo
   AND om.[Id]              IS NULL");

            if (option.ClosingDay.HasValue)
                builder.Append(@"
   AND COALESCE(pcs.[ClosingDay], cs.[ClosingDay]) = @ClosingDay");

            builder.Append($@"
 GROUP BY
       {partition}
     , m.[MatchingHeaderId], COALESCE(pcs.[Id], cs.[Id])
       ) r
 GROUP BY
       [Companyid], [ParentCustomerId], [RecordedAt], [CustomerId], [SectionId], [CategoryId], [CurrencyId]
HAVING SUM( CASE r.[ReceiptSeq] WHEN 1 THEN r.[ReceiptAmount] ELSE 0 END
          + r.[MatchingAmount] ) <> 0
 ORDER BY
       [Companyid], [ParentCustomerId], [RecordedAt], [CustomerId]
 OPTION (FORCE ORDER );
");
            return builder.ToString();
        }

        private string CreateInsertMatching(CustomerLedgerSearch option)
        {
            var targetDate  = option.UseBilledAt    ? "b.[BilledAt]"            : "b.[SalesAt]";
            var category    = option.DoGroupReceipt ? "r.[ReceiptCategoryId]"   : "MIN( r.[ReceiptCategoryId] )";
            var transId     = option.DoGroupReceipt ? "MIN( r.[Id] )"           : "r.[Id]";
            var partition   = option.DoGroupReceipt ? "r.[ReceiptCategoryId]"   : "r.[Id]";
            var section     = !option.DoGroupReceipt && option.DisplaySection ? "MIN( COALESCE( r.[SectionId], 0 ) )" : "0";
            var recordedAt  = $"CASE WHEN m.[RecordedAt] < {targetDate} THEN {targetDate} ELSE m.[RecordedAt] END";
            var builder = new StringBuilder();
            builder.Append($@"
INSERT INTO #Ledger
([CompanyId]
,[ParentCustomerId]
,[RecordedAt]
,[DataType]
,[CustomerId]
,[DepartmentId]
,[SectionId]
,[InvoiceCode]
,[CategoryId]
,[AccountTitleId]
,[CurrencyId]
,[BillingInputId]
,[TransactionId]
,[MatchingAmount]
,[HeaderId1],[HeaderId2],[HeaderId3],[HeaderId4],[HeaderId5],[HeaderId6],[HeaderId7]
)
SELECT m.[CompanyId]
     , m.[ParentCustomerId]
     , m.[RecordedAt]
     , 3 [DataType]
     , m.[CustomerId]
     , 0 [DepartmentId]
     , m.[SectionId]
     , N'' [InvoiceCode]
     , m.[CategoryId]
     , 0 [AccountTitleId]
     , m.[CurrencyId]
     , 0 [BillingInputId]
     , m.[TransactionId]
     , SUM( m.[MatchingAmount] ) [MatchingAmount]
     , MIN( CASE m.[MatchingSeq] WHEN 1 THEN m.[MatchingHeaderId] ELSE NULL END ) [HeaderId1]
     , MIN( CASE m.[MatchingSeq] WHEN 2 THEN m.[MatchingHeaderId] ELSE NULL END ) [HeaderId2]
     , MIN( CASE m.[MatchingSeq] WHEN 3 THEN m.[MatchingHeaderId] ELSE NULL END ) [HeaderId3]
     , MIN( CASE m.[MatchingSeq] WHEN 4 THEN m.[MatchingHeaderId] ELSE NULL END ) [HeaderId4]
     , MIN( CASE m.[MatchingSeq] WHEN 5 THEN m.[MatchingHeaderId] ELSE NULL END ) [HeaderId5]
     , MIN( CASE m.[MatchingSeq] WHEN 6 THEN m.[MatchingHeaderId] ELSE NULL END ) [HeaderId6]
     , MIN( CASE m.[MatchingSeq] WHEN 7 THEN m.[MatchingHeaderId] ELSE NULL END ) [HeaderId7]
  FROM (
SELECT MIN( r.[CompanyId] ) [CompanyId]
     , MIN( r.[CurrencyId] ) [CurrencyId]
     , COALESCE(pcs.[Id], cs.[Id]) [ParentCustomerId]
     , b.[CustomerId]
     , {transId} [TransactionId]
     , {recordedAt} [RecordedAt]
     , {section} [SectionId]
     , {category} [CategoryId]
     , m.[MatchingHeaderId]
     , SUM( m.[Amount]
          + m.[BankTransferFee]
          - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
          + COALESCE( mbd.[DiscountAmount], 0 ) ) [MatchingAmount]
     , ROW_NUMBER() OVER (
        PARTITION BY {partition}, b.[CustomerId], COALESCE(pcs.[Id], cs.[Id]), {recordedAt}
        ORDER BY m.[MatchingHeaderId] ) [MatchingSeq]
  FROM [dbo].[Matching] m
 INNER JOIN [dbo].[Receipt] r
    ON r.[Id]               = m.[ReceiptId]
   AND r.[CompanyId]        = @CompanyId
   AND r.[CurrencyId]       = @CurrencyId
   AND r.[Approved]         = 1
 INNER JOIN [dbo].[Billing] b
    ON b.[Id]               = m.[BillingId]
   AND {recordedAt}
            BETWEEN @YearMonthFrom AND @YearMonthTo
   AND b.[Approved]         = 1
   AND b.[InputType]       <> 3
 INNER JOIN [dbo].[Customer] cs
    ON cs.[Id]              = b.[CustomerId]
  LEFT JOIN (
       SELECT mbd.[MatchingId]
            , SUM( mbd.[DiscountAmount] ) [DiscountAmount]
         FROM [dbo].[MatchingBillingDiscount] mbd
        INNER JOIN [dbo].[Matching] m
           ON m.[Id]            = mbd.[MatchingId]
        INNER JOIN [dbo].[MatchingHeader] mh
           ON mh.[Id]           = m.[MatchingHeaderId]
          AND mh.[CompanyId]    = @CompanyId
          AND mh.[CurrencyId]   = @CurrencyId
        GROUP BY
              mbd.[MatchingId]
       ) mbd
    ON mbd.[MatchingId]     = m.[Id]
  LEFT JOIN [dbo].[CustomerGroup] csg
    ON csg.[ChildCustomerId]    = b.[CustomerId]
  LEFT JOIN [dbo].[Customer] pcs
    ON pcs.[Id]                 = csg.[ParentCustomerId]
 WHERE COALESCE(pcs.[Code], cs.[Code])
            BETWEEN @CustomerCodeFrom AND @CustomerCodeTo");

            if (option.ClosingDay.HasValue)
                builder.Append(@"
   AND COALESCE(pcs.[ClosingDay], cs.[ClosingDay]) = @ClosingDay");

            builder.Append($@"
 GROUP BY
       {partition}, b.[CustomerId], COALESCE(pcs.[Id], cs.[Id]), {recordedAt}
     , m.[MatchingHeaderId]
       ) m
 GROUP BY
       m.[CompanyId], m.[CurrencyId], m.[ParentCustomerId], m.[RecordedAt], m.[CustomerId], m.[SectionId], m.[CategoryId], m.[TransactionId]
 ORDER BY
       [CompanyId], [ParentCustomerId], [RecordedAt], [CustomerId]
OPTION ( FORCE ORDER );
");
            return builder.ToString();
        }

        private string CreateSelectLedger(CustomerLedgerSearch option)
        {
            var builder = new StringBuilder();
            builder.Append($@"
SELECT ld.[RecordedAt]
     , ld.[ParentCustomerId]
     , pcs.[Code]   [ParentCustomerCode]
     , pcs.[Name]   [ParentCustomerName]
     , cr.[Code]    [CurrencyCode]
     , ld.[DepartmentId]
     , dp.[Code]    [DepartmentCode]
     , dp.[Name]    [DepartmentName]
     , ld.[SectionId]
     , sc.[Code]    [SectionCode]
     , sc.[Name]    [SectionName]
     , ld.[InvoiceCode]
     , ld.[CategoryId]
     , ct.[Name]    [CategoryName]
     , ld.[AccountTitleId]
     , ac.[Name]    [AccountTitleName]
     , ld.[DataType]
     , ld.[BillingInputId]
     , ld.[TransactionId]
     , ld.[CustomerId]
     , cs.[Code]    [CustomerCode]
     , cs.[Name]    [CustomerName]
     , ld.[BillingAmount]
     , ld.[ReceiptAmount]
     , ld.[MatchingAmount]
     , ld.[HeaderId1], ld.[HeaderId2], ld.[HeaderId3], ld.[HeaderId4], ld.[HeaderId5], ld.[HeaderId6], ld.[HeaderId7]
  FROM #Ledger ld
  LEFT JOIN [dbo].[Customer] pcs
    ON pcs.[Id]     = ld.[ParentCustomerId]
  LEFT JOIN [dbo].[Customer] cs
    ON cs.[Id]      = ld.[CustomerId]
  LEFT JOIN [dbo].[Category] ct
    ON ct.Id        = ld.[CategoryId]
  LEFT JOIN [dbo].[Department] dp
    ON dp.[Id]      = ld.[DepartmentId]
  LEFT JOIN [dbo].[Section] sc
    ON sc.[Id]      = ld.[SectionId]
  LEFT JOIN [dbo].[AccountTitle] ac
    ON ac.[Id]      = ld.[AccountTitleId]
  LEFT JOIN [dbo].[Currency] cr
    ON cr.[Id]      = ld.[CurrencyId]
 ORDER BY
       pcs.[Code]
     , ld.[RecordedAt]
     , ld.[DataType]
     , cs.[Code]
     , ld.[BillingInputId]
     , ld.[TransactionId]
");
            return builder.ToString();
        }

        private string CreateSelectBalance(CustomerLedgerSearch option)
        {
            var targetDate = option.UseBilledAt ? "b.[BilledAt]" : "b.[SalesAt]";
            var builder = new StringBuilder();
            builder.Append($@"
SELECT U.[ParentCustomerId] [Key]
     , SUM( U.[CarriedAmount] ) [Value]
  FROM (
        SELECT COALESCE(pcs.[Id], cs.[Id]) [ParentCustomerId]
             , SUM( b.[BillingAmount]
                  - CASE WHEN b.[DeleteAt] IS NULL THEN 0 ELSE b.[RemainAmount] END
                  - COALESCE(m.[MatchingAmount], 0) ) [CarriedAmount]
          FROM [dbo].[Billing] b
         INNER JOIN [dbo].[Customer] cs
            ON cs.[Id]          = b.[CustomerId]
           AND b.[CompanyId]    = @CompanyId
           AND b.[CurrencyId]   = @CurrencyId
           AND {targetDate}     < @YearMonthFrom
           AND b.[Approved]     = 1
           AND b.[InputType]   <> 3
           AND (    b.[DeleteAt]     IS NULL
                OR  b.[DeleteAt]     IS NOT NULL AND b.[BillingAmount] <> b.[RemainAmount] )
          LEFT JOIN (
               SELECT b.[Id] [BillingId]
                    , SUM( m.[Amount]
                         + m.[BankTransferFee]
                         - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
                         + COALESCE( mbd.[DiscountAmount], 0)
                         ) [MatchingAmount]
                 FROM [dbo].[Matching] m
                INNER JOIN [dbo].[Billing] b
                   ON b.[Id]            = m.[BillingId]
                  AND b.[CompanyId]     = @CompanyId
                  AND b.[CurrencyId]    = @CurrencyId
                  AND m.[RecordedAt]    < @YearMonthFrom
                  AND {targetDate}      < @YearMonthFrom
                 LEFT JOIN (
                      SELECT mbd.[MatchingId]
                           , SUM( mbd.[DiscountAmount] ) [DiscountAmount]
                        FROM [dbo].[MatchingBillingDiscount] mbd
                       INNER JOIN [dbo].[Matching] m
                          ON m.[Id]             = mbd.[MatchingId]
                       INNER JOIN [dbo].[MatchingHeader] mh
                          ON mh.[Id]            = m.[MatchingHeaderId]
                         AND mh.[CompanyId]     = @CompanyId
                         AND mh.[CurrencyId]    = @CurrencyId
                       GROUP BY
                             mbd.[MatchingId]
                      ) mbd
                   ON mbd.[MatchingId]  = m.[Id]
                INNER JOIN [dbo].[Customer] cs
                   ON cs.[Id]               = b.[CustomerId]
                 LEFT JOIN [dbo].[CustomerGroup] csg
                   ON csg.[ChildCustomerId] = b.[CustomerId]
                 LEFT JOIN [dbo].[Customer] pcs
                   ON pcs.[Id]              = csg.[ParentCustomerId]
                WHERE COALESCE(pcs.[Code], cs.[Code])
                        BETWEEN @CustomerCodeFrom AND @CustomerCodeTo");
            if (option.ClosingDay.HasValue)
                builder.Append(@"
                  AND COALESCE(pcs.[ClosingDay], cs.[ClosingDay]) = @ClosingDay");

            builder.Append($@"
                GROUP BY
                      b.[Id]
               ) m
            ON b.[Id]           = m.[BillingId]
          LEFT JOIN [dbo].[CustomerGroup] csg
            ON csg.[ChildCustomerId]    = b.[CustomerId]
          LEFT JOIN [dbo].[Customer] pcs
            ON pcs.[Id]                 = csg.[ParentCustomerId]
         WHERE COALESCE(pcs.[Code], cs.[Code])
                    BETWEEN @CustomerCodeFrom AND @CustomerCodeTo");

            if (option.ClosingDay.HasValue)
                builder.Append(@"
           AND COALESCE(pcs.[ClosingDay], cs.[ClosingDay]) = @ClosingDay");

            builder.Append($@"
           AND b.[BillingAmount]    <> COALESCE(m.[MatchingAmount], 0)
                                + CASE WHEN b.[DeleteAt] IS NULL THEN 0 ELSE b.[RemainAmount] END
         GROUP BY
               COALESCE(pcs.[Id], cs.[Id])");

            if (option.UseReceipt)
                builder.Append(@"

UNION ALL

        SELECT r.[ParentCustomerId]
             , SUM( r.[MatchingAmount]
                  - CASE r.[ReceiptSeq] WHEN 1 THEN r.[ReceiptAmount] ELSE 0 END ) [CarriedAmount]
          FROM (
                SELECT r.[Id]
                     , ROW_NUMBER() OVER (PARTITION BY r.[Id] ORDER BY m.[MatchingHeaderId], COALESCE(pcs.[Id], cs.[Id]) ) [ReceiptSeq]
                     , MIN( r.[ReceiptAmount] ) [ReceiptAmount]
                     , COALESCE(pcs.[Id], cs.[Id]) [ParentCustomerId]
                     , SUM( CASE WHEN b.[BilledAt] < @YearMonthFrom THEN m.[Amount] ELSE 0 END ) [MatchingAmount]
                  FROM [dbo].[Matching] m
                 INNER JOIN [dbo].[Receipt] r
                    ON r.[Id]           = m.[ReceiptId]
                   AND r.[CompanyId]    = @CompanyId
                   AND r.[CurrencyId]   = @CurrencyId
                   AND r.[RecordedAt]   < @YearMonthFrom
                 INNER JOIN [dbo].[Billing] b
                    ON b.[Id]           = m.[BillingId]
                 INNER JOIN [dbo].[Customer] cs
                    ON cs.[Id]          = b.[CustomerId]
                  LEFT JOIN [dbo].[CustomerGroup] csg
                    ON csg.[ChildCustomerId]    = b.[CustomerId]
                  LEFT JOIN [dbo].[Customer] pcs
                    ON pcs.[Id]         = csg.[ParentCustomerId]
                 WHERE COALESCE(pcs.[Code], cs.[Code])
                        BETWEEN @CustomerCodeFrom AND @CustomerCodeTo");

            if (option.UseReceipt && option.ClosingDay.HasValue)
                builder.Append(@"
                   AND COALESCE(pcs.[ClosingDay], cs.[ClosingDay]) = @ClosingDay");

            if (option.UseReceipt)
                builder.Append($@"
                 GROUP BY r.[Id]
                     , m.[MatchingHeaderId]
                     , COALESCE(pcs.[Id], cs.[Id])
                HAVING MIN( r.[ReceiptAmount] )
                    <> SUM( CASE WHEN b.[BilledAt] < @YearMonthFrom THEN m.[Amount] ELSE 0 END )
               ) r
         GROUP BY
               r.[ParentCustomerId]");

            builder.Append(@"
       ) U
 GROUP BY
       U.ParentCustomerId
 OPTION ( FORCE ORDER )
");
            return builder.ToString();
        }

        private string CreateSelectKeys(CustomerLedgerSearch option)
        {
            var builder = new StringBuilder();
            builder.Append(@"
SELECT COALESCE(pcs.[Id], cs.[Id]) [Key]
     , m2.MatchingHeaderId [Value]
  FROM [dbo].[Billing] b
 INNER JOIN [dbo].[Customer] cs
    ON cs.[Id]          = b.[CustomerId]
   AND b.[CompanyId]    = @CompanyId
   AND b.[CurrencyId]   = @CurrencyId
   AND b.[BilledAt]     < @YearMonthFrom
   AND b.[Approved]     = 1
   AND b.[InputType]   <> 3
   AND (    b.[DeleteAt]     IS NULL
        OR  b.[DeleteAt]     IS NOT NULL AND b.[BillingAmount] <> b.[RemainAmount] )
  LEFT JOIN (
       SELECT b.[Id] [BillingId]
            , SUM( m.[Amount]
                 + m.[BankTransferFee]
                 - CASE WHEN m.[TaxDifference] < 0 THEN m.[TaxDifference] ELSE 0 END
                 + COALESCE( mbd.[DiscountAmount], 0)
                 ) [MatchingAmount]
         FROM [dbo].[Matching] m
        INNER JOIN [dbo].[Billing] b
           ON b.[Id]            = m.[BillingId]
          AND b.[CompanyId]     = @CompanyId
          AND b.[CurrencyId]    = @CurrencyId
          AND m.[RecordedAt]    < @YearMonthFrom
          AND b.[BilledAt]      < @YearMonthFrom
         LEFT JOIN (
              SELECT mbd.[MatchingId]
                   , SUM( mbd.[DiscountAmount] ) [DiscountAmount]
                FROM [dbo].[MatchingBillingDiscount] mbd
               INNER JOIN [dbo].[Matching] m
                  ON m.[Id]             = mbd.[MatchingId]
               INNER JOIN [dbo].[MatchingHeader] mh
                  ON mh.[Id]            = m.[MatchingHeaderId]
                 AND mh.[CompanyId]     = @CompanyId
                 AND mh.[CurrencyId]    = @CurrencyId
               GROUP BY
                     mbd.[MatchingId]
              ) mbd
           ON mbd.[MatchingId]  = m.[Id]
        INNER JOIN [dbo].[Customer] cs
           ON cs.[Id]               = b.[CustomerId]
         LEFT JOIN [dbo].[CustomerGroup] csg
           ON csg.[ChildCustomerId] = b.[CustomerId]
         LEFT JOIN [dbo].[Customer] pcs
           ON pcs.[Id]              = csg.[ParentCustomerId]
        WHERE COALESCE(pcs.[Code], cs.[Code])
                BETWEEN @CustomerCodeFrom AND @CustomerCodeTo");

            if (option.ClosingDay.HasValue)
                builder.Append(@"
          AND COALESCE(pcs.[ClosingDay], cs.[ClosingDay]) = @ClosingDay");

            builder.Append($@"
        GROUP BY
              b.[Id]
       ) m
    ON b.[Id]           = m.[BillingId]
  LEFT JOIN [dbo].[CustomerGroup] csg
    ON csg.[ChildCustomerId]    = b.[CustomerId]
  LEFT JOIN [dbo].[Customer] pcs
    ON pcs.[Id]                 = csg.[ParentCustomerId]
 INNER JOIN [dbo].[Matching] m2
    ON b.[Id]           = m2.[BillingId]
 WHERE COALESCE(pcs.[Code], cs.[Code])
            BETWEEN @CustomerCodeFrom AND @CustomerCodeTo");

            if (option.ClosingDay.HasValue)
                builder.Append(@"
   AND COALESCE(pcs.[ClosingDay], cs.[ClosingDay]) = @ClosingDay");

            builder.Append($@"
   AND b.[BillingAmount]    <> COALESCE(m.[MatchingAmount], 0)
                        + CASE WHEN b.[DeleteAt] IS NULL THEN 0 ELSE b.[RemainAmount] END
 GROUP BY
       COALESCE(pcs.[Id], cs.[Id])
     , m2.MatchingHeaderId
");
            return builder.ToString();
        }

    }
}
