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
    public class AccountTransferDetailQueryProcessor :
        IAddAccountTransferDetailQueryProcessor,
        IAccountTransferDetailQueryProcessor,
        IDeleteAccountTransferDetailQueryProcessor,
        IUpdateBillingAccountTransferLogQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public AccountTransferDetailQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<AccountTransferDetail> AddAsync(AccountTransferDetail detail, CancellationToken token = default(CancellationToken))
        {
            #region insert query
            var query = @"
INSERT INTO [dbo].[AccountTransferDetail]
     ( [AccountTransferLogId]
     , [BillingId]
     , [CompanyId]
     , [CustomerId]
     , [DepartmentId]
     , [StaffId]
     , [BilledAt]
     , [SalesAt]
     , [ClosingAt]
     , [DueAt]
     , [DueAt2nd]
     , [BillingAmount]
     , [InvoiceCode]
     , [Note1]
     , [TransferBankCode]
     , [TransferBankName]
     , [TransferBranchCode]
     , [TransferBranchName]
     , [TransferAccountTypeId]
     , [TransferAccountNumber]
     , [TransferAccountName]
     , [TransferCustomerCode]
     , [TransferNewCode] )
OUTPUT inserted.*
VALUES
     ( @AccountTransferLogId
     , @BillingId
     , @CompanyId
     , @CustomerId
     , @DepartmentId
     , @StaffId
     , @BilledAt
     , @SalesAt
     , @ClosingAt
     , @DueAt
     , @DueAt2nd
     , @BillingAmount
     , @InvoiceCode
     , @Note1
     , @TransferBankCode
     , @TransferBankName
     , @TransferBranchCode
     , @TransferBranchName
     , @TransferAccountTypeId
     , @TransferAccountNumber
     , @TransferAccountName
     , @TransferCustomerCode
     , @TransferNewCode )
";
            #endregion
            return dbHelper.ExecuteAsync<AccountTransferDetail>(query, detail, token);
        }

        public Task<int> DeleteAsync(long AccountTransferLogId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE [dbo].[AccountTransferDetail]
 WHERE [AccountTransferLogId]   = @AccountTransferLogId";
            return dbHelper.ExecuteAsync(query, new { AccountTransferLogId }, token);
        }

        public Task<IEnumerable<AccountTransferDetail>> GetAsync(AccountTransferSearch option, CancellationToken token = default(CancellationToken))
        {
            var history = option.AccountTransferLogIds?.Any() ?? false;
            var builder = new StringBuilder( history ? @"
SELECT atd.[Id]
     , atd.[AccountTransferLogId]
     , atd.[BillingId]
     , atd.[CompanyId]
     , atd.[CustomerId]
     , atd.[DepartmentId]
     , atd.[StaffId]
     , atd.[BilledAt]
     , atd.[SalesAt]
     , atd.[ClosingAt]
     , atd.[DueAt]
     , atd.[DueAt2nd]
     , atd.[BillingAmount]
     , atd.[InvoiceCode]
     , atd.[Note1]
     , atd.[TransferBankCode]
     , atd.[TransferBankName]
     , atd.[TransferBranchCode]
     , atd.[TransferBranchName]
     , atd.[TransferAccountTypeId]
     , atd.[TransferAccountNumber]
     , atd.[TransferAccountName]
     , atd.[TransferCustomerCode]
     , atd.[TransferNewCode]
     , COALESCE(cs.[Code], N'') [CustomerCode]
     , COALESCE(cs.[Name], N'') [CustomerName]
     , COALESCE(dp.[Code], N'') [DepartmentCode]
     , COALESCE(dp.[Name], N'') [DepartmentName]
     , COALESCE(st.[Code], N'') [StaffCode]
     , COALESCE(st.[Name], N'') [StaffName]
     , atl.[CollectCategoryId]
     , atl.[PaymentAgencyId]
     , cc.[Code]    [CollectCategoryCode]
     , cc.[Name]    [CollectCategoryName]
     , pa.[Code]    [PaymentAgencyCode]
     , pa.[Name]    [PaymentAgencyName]
     , pa.[FileFormatId]
     , atl.[DueAt] [NewDueAt]
  FROM [dbo].[AccountTransferDetail] atd
 INNER JOIN [dbo].[AccountTransferLog] atl  ON atl.[Id] = atd.[AccountTransferLogId]
 INNER JOIN [dbo].[Category] cc             ON cc.[Id]  = atl.[CollectCategoryId]
 INNER JOIN [dbo].[PaymentAgency] pa        ON pa.[Id]  = atl.[PaymentAgencyId]
  LEFT JOIN [dbo].[Customer] cs             ON cs.[Id]  = atd.[CustomerId]
  LEFT JOIN [dbo].[Department] dp           ON dp.[Id]  = atd.[DepartmentId]
  LEFT JOIN [dbo].[Staff] st                ON st.[Id]  = atd.[StaffId]
 WHERE atl.[Id] IN (SELECT [Id] FROM @Ids)
" : @"
SELECT ROW_NUMBER() OVER ( ORDER BY b.[Id] ) [Id]
     , 0 [AccountTransferLogId]
     , b.[Id] [BillingId]
     , b.[CompanyId]
     , b.[CustomerId]
     , b.[DepartmentId]
     , b.[StaffId]
     , b.[BilledAt]
     , b.[SalesAt]
     , b.[ClosingAt]
     , b.[DueAt]
     , b.[BillingAmount] - b.[OffsetAmount] [BillingAmount]
     , b.[InvoiceCode]
     , b.[Note1]
     , cs.[TransferBankCode]
     , cs.[TransferBankName]
     , cs.[TransferBranchCode]
     , cs.[TransferBranchName]
     , cs.[TransferAccountTypeId]
     , cs.[TransferAccountNumber]
     , cs.[TransferAccountName]
     , cs.[TransferCustomerCode]
     , cs.[TransferNewCode]
     , b.[ResultCode]
     , b.[UpdateAt] [BillingUpdateAt]
     , cs.[Code]    [CustomerCode]
     , cs.[Name]    [CustomerName]
     , dp.[Code]    [DepartmentCode]
     , dp.[Name]    [DepartmentName]
     , st.[Code]    [StaffCode]
     , st.[Name]    [StaffName]
     , b.[CollectCategoryId]
     , cc.[PaymentAgencyId]
     , cc.[Code]    [CollectCategoryCode]
     , cc.[Name]    [CollectCategoryName]
     , pa.[Code]    [PaymentAgencyCode]
     , pa.[Name]    [PaymentAgencyName]
     , pa.[FileFormatId]
  FROM [dbo].[Billing] b
  LEFT JOIN [dbo].[Customer] cs             ON cs.[Id]  = b.[CustomerId]
  LEFT JOIN [dbo].[Department] dp           ON dp.[Id]  = b.[DepartmentId]
  LEFT JOIN [dbo].[Staff] st                ON st.[Id]  = b.[StaffId]
  LEFT JOIN [dbo].[Category] cc             ON cc.[Id]  = b.[CollectCategoryId]
  LEFT JOIN [dbo].[PaymentAgency] pa        ON pa.[Id]  = cc.[PaymentAgencyId]
  LEFT JOIN [dbo].[Currency] cy             ON cy.[Id]  = b.[Id]
 WHERE b.[CompanyId]            = @CompanyId
   AND b.[InputType]            <> 3
   AND b.[CollectCategoryId]    = @CollectCategoryId
   AND b.[CurrencyId]           = @CurrencyId
   AND b.[DeleteAt]             IS NULL
   AND b.[AccountTransferLogId] IS NULL
   AND (b.[ResultCode] IS NULL OR b.[ResultCode] <> N'0')
");
            if (!history)
            {
                if (option.DueAtFrom.HasValue)
                    builder.Append(@"
   AND b.[DueAt]                >= @DueAtFrom");
                if (option.DueAtTo.HasValue)
                    builder.Append(@"
   AND b.[DueAt]                <= @DueAtTo");

            }

            var param = history
                ? (object)new { Ids = option.AccountTransferLogIds.GetTableParameter() }
                : option;

            return dbHelper.GetItemsAsync<AccountTransferDetail>(builder.ToString(), param, token);
        }
        public Task<Billing> UpdateAsync(AccountTransferDetail detail, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE [dbo].[Billing]
   SET [AccountTransferLogId]   = @AccountTransferLogId
     , [RequestDate]            = CAST( GETDATE() AS DATE )
     , [DueAt]                  = @NewDueAt
     , [TransferOriginalDueAt]  = CASE WHEN [TransferOriginalDueAt] IS NULL THEN [DueAt] ELSE [TransferOriginalDueAt] END
     , [UpdateBy]               = @CreateBy
     , [UpdateAt]               = GETDATE()
OUTPUT inserted.*
 WHERE [Id]                     = @BillingId
   AND [UpdateAt]               = @BillingUpdateAt;"
.AppendIfNotAnyRowsAffectedThenRaiseError();
            return dbHelper.ExecuteAsync<Billing>(query, detail, token);
        }

        public Task<int> CancelAsync(long AccountTransferLogId, int LoginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE [dbo].[Billing]
   SET [AccountTransferLogId]   = NULL
     , [RequestDate]            = NULL
     , [DueAt]                  = [TransferOriginalDueAt]
     , [TransferOriginalDueAt]  = NULL
     , [UpdateBy]               = @LoginUserId
     , [UpdateAt]               = GETDATE()
 WHERE [AccountTransferLogId]   = @AccountTransferLogId
";
            return dbHelper.ExecuteAsync(query, new { AccountTransferLogId, LoginUserId }, token);
        }

    }
}
