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
    public class MfAggrTransactionQueryProcessor :
        IAddMfAggrTransactionQueryProcessor,
        IMfAggrTransactionQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public MfAggrTransactionQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<MfAggrTransaction> AddAsync(MfAggrTransaction transaction, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO [dbo].[MfAggrTransaction]
(   Id
,   ReceiptId
,   CompanyId
,   CurrencyId
,   Amount
,   AccountId
,   SubAccountId
,   Content
,   PayerCode
,   PayerName
,   PayerNameRaw
,   RecordedAt
,   MfCreatedAt
,   Rate
,   ConvertedAmount
,   ToCurrencyId
,   ExcludeCategoryId
,   CreateBy
,   CreateAt
)
OUTPUT inserted.*
VALUES
(  @Id
,  @ReceiptId
,  @CompanyId
,  @CurrencyId
,  @Amount
,  @AccountId
,  @SubAccountId
,  @Content
,  @PayerCode
,  @PayerName
,  @PayerNameRaw
,  @RecordedAt
,  @MfCreatedAt
,  @Rate
,  @ConvertedAmount
,  @ToCurrencyId
,  @ExcludeCategoryId
,  @CreateBy
,  GETDATE()
)
;
";
            return dbHelper.ExecuteAsync<MfAggrTransaction>(query, transaction, token);
        }

        public Task<IEnumerable<MfAggrTransaction>> GetAsync(MfAggrTransactionSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        tr.*
            , ac.[DisplayName]      [AccountName]
            , sb.[Name]             [SubAccountName]
            , sb.[AccountTypeName]
            , sb.[AccountNumber]
FROM        [dbo].[MfAggrTransaction] tr
INNER JOIN  [dbo].[MfAggrAccount] ac
ON          ac.[Id]                 = tr.[AccountId]
INNER JOIN  [dbo].[MfAggrSubAccount] sb
ON          sb.[Id]                 = tr.[SubAccountId]
LEFT JOIN   [dbo].[BankAccountType] at
ON          at.[Id]                 = sb.[AccountTypeId]
WHERE       tr.[CompanyId]          = @CompanyId";
            if (option.CurrencyId.HasValue) query += @"
AND         tr.[CurrencyId]         = @CurrencyId";
            if (option.RecordedAtFrom.HasValue) query += @"
AND         tr.[RecordedAt]        >= @RecordedAtFrom";
            if (option.RecordedAtTo.HasValue) query += @"
AND         tr.[RecordedAt]        <= @RecordedAtTo";
            if (!string.IsNullOrEmpty(option.AccountName))
            {
                option.AccountName = Sql.GetWrappedValue(option.AccountName);
                query += @"
AND         ac.[DisplayName]     LIKE @AccountName";
            }
            if (!string.IsNullOrEmpty(option.SubAccountName))
            {
                option.SubAccountName = Sql.GetWrappedValue(option.SubAccountName);
                query += @"
AND         sb.[Name]            LIKE @SubAccountName";
            }
            if (!string.IsNullOrEmpty(option.BankCode)) query += @"
AND         ac.[BankCode]           = @BankCode";
            if (!string.IsNullOrEmpty(option.BranchCode)) query += @"
AND         sb.[BranchCode]         = @BranchCode";
            if (!string.IsNullOrEmpty(option.AccountTypeName))
            {
                option.AccountTypeName = Sql.GetWrappedValue(option.AccountTypeName);
                query += @"
AND         sb.[AccountTypeName] LIKE @AccountTypeName";
            }
            if (option.AccountTypeId.HasValue) query += @"
AND         sb.[AccountTypeId]      = @AccountTypeId";
            if (!string.IsNullOrEmpty(option.AccountNumber)) query += @"
AND         sb.[AccountNumber]      = @AccountNumber";
            query += @"
ORDER BY    tr.[Id]        ASC
";
            return dbHelper.GetItemsAsync<MfAggrTransaction>(query, option, token);
        }

        public Task<IEnumerable<long>> GetIdsAsync(CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT [Id] FROM [dbo].[MfAggrTransaction] ORDER BY [Id] ASC";
            return dbHelper.GetItemsAsync<long>(query, null, token);
        }

        public Task<IEnumerable<MfAggrTransaction>> GetLastOneAsync(MfAggrTransactionSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      TOP 1 tr.*
FROM        [dbo].[MfAggrTransaction] tr
WHERE       tr.[CompanyId]     = @CompanyId";
            if (option.CurrencyId.HasValue) query += @"
AND         tr.[CurrencyId]    = @CurrencyId";
            query += @"
ORDER BY    tr.[CreateAt]        DESC
";
            return dbHelper.GetItemsAsync<MfAggrTransaction>(query, option, token);
        }
    }
}
