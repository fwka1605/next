using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class AccountTitleQueryProcessor :
        IAccountTitleQueryProcessor,
        IAddAccountTitleQueryProcessor,
        IAccountTitleForImportQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public AccountTitleQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        //forImporting
        public Task<IEnumerable<MasterData>> GetImportItemsCategoryAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM AccountTitle acc
 WHERE acc.CompanyId = @CompanyId
   AND acc.Code NOT IN (SELECT Code FROM @Code)
   AND EXISTS (
       SELECT * FROM Category ca
        WHERE ca.AccountTitleId = acc.Id )
";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsCustomerDiscountAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM AccountTitle acc
 WHERE acc.CompanyId = @CompanyId
   AND acc.Code NOT IN (SELECT Code FROM @Code)
   AND EXISTS (
       SELECT * FROM CustomerDiscount cus
       WHERE cus.AccountTitleId = acc.Id )";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsDebitBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM AccountTitle acc
 WHERE acc.CompanyId = @CompanyId
   AND acc.Code NOT IN (SELECT Code FROM @Code)
   AND EXISTS (
       SELECT * FROM Billing b
        WHERE b.DebitAccountTitleId = acc.Id )";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsCreditBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM AccountTitle acc
 WHERE acc.CompanyId = @CompanyId
   AND acc.Code NOT IN (SELECT Code FROM @Code)
   AND EXISTS (
       SELECT * FROM Billing b
        WHERE b.CreditAccountTitleId = acc.Id )";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<AccountTitle> AddAsync(AccountTitle account, CancellationToken token = default(CancellationToken))
        {
            string query = @"
MERGE INTO AccountTitle AS Account 
USING ( 
    SELECT 
     @CompanyId AS CompanyId 
    ,@Code AS Code
) AS Target 
ON ( 
    Account.CompanyId = @CompanyId 
    AND Account.Code = @Code
) 
WHEN MATCHED THEN 
    UPDATE SET
        Name = @Name
        ,ContraAccountCode = (CASE WHEN @ContraAccountCode IS NOT NULL THEN  @ContraAccountCode ELSE '' END )
        ,ContraAccountName = (CASE WHEN @ContraAccountName IS NOT NULL THEN  @ContraAccountName ELSE '' END )
        ,ContraAccountSubCode = (CASE WHEN @ContraAccountSubCode IS NOT NULL THEN  @ContraAccountSubCode ELSE '' END )
        ,UpdateBy = @UpdateBy 
        ,UpdateAt = GETDATE() 
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, Code, Name,ContraAccountCode,ContraAccountName,ContraAccountSubCode, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @Code, @Name,
    (CASE WHEN @ContraAccountCode IS NOT NULL THEN  @ContraAccountCode ELSE '' END ),
    (CASE WHEN @ContraAccountName IS NOT NULL THEN  @ContraAccountName ELSE '' END ),
    (CASE WHEN @ContraAccountSubCode IS NOT NULL THEN  @ContraAccountSubCode ELSE '' END ),
    @UpdateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<AccountTitle>(query, account, token);
        }

        public Task<IEnumerable<AccountTitle>> GetAsync(AccountTitleSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        AccountTitle at
WHERE       at.Id           = at.Id";
            if (option.CompanyId.HasValue) query += @"
AND         at.CompanyId    = @CompanyId";
            if (option.Ids?.Any() ?? false) query += @"
AND         at.Id           IN (SELECT Id   FROM @Ids)";
            if (option.Codes?.Any() ?? false) query += @"
AND         at.Code         IN (SELECT Code FROM @Codes)";
            if (!string.IsNullOrWhiteSpace(option.Name))
            {
                option.Name = Sql.GetWrappedValue(option.Name);
                query += @"
AND         at.Name         LIKE @Name";
            }
            query += @"
ORDER BY    at.CompanyId    ASC
,           at.Code         ASC";

            return dbHelper.GetItemsAsync<AccountTitle>(query, new {
                            option.CompanyId,
                Ids     =   option.Ids.GetTableParameter(),
                Codes   =   option.Codes.GetTableParameter(),
                            option.Name,
            }, token);
        }
    }
}
