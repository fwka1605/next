using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BankAccountQueryProcessor :
        IBankAccountQueryProcessor,
        IAddBankAccountQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BankAccountQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<BankAccount>> GetAsync(BankAccountSearch option, CancellationToken token = default(CancellationToken))
        {


            var query = @"
SELECT      ba.*
          , sc.Code SectionCode          ,  sc.Name SectionName
          , rc.Code ReceiptCategoryCode  ,  rc.Name ReceiptCategoryName
                                         , bat.Name AccountTypeName
FROM        BankAccount ba
LEFT JOIN   Section sc          ON sc.Id        = ba.SectionId
LEFT JOIN   Category rc         ON rc.Id        = ba.ReceiptCategoryId
LEFT JOIN   BankAccountType bat ON bat.Id       = ba.AccountTypeId
WHERE       ba.Id               = ba.Id ";
            if (option.CompanyId.HasValue) query += @"
AND         ba.CompanyId        = @CompanyId";

            if (! string.IsNullOrWhiteSpace(option.SearchWord))
            {
                option.SearchWord = Sql.GetWrappedValue(option.SearchWord);
                query += @"
AND         (ba.BankCode        LIKE @SearchWord
          OR ba.BankName        LIKE @SearchWord
          OR ba.BranchCode      LIKE @SearchWord
          OR ba.BranchName      LIKE @SearchWord
          OR ba.AccountNumber   LIKE @SearchWord)";
            }

            if (option.AccountTypeId > 0) query += @"
AND         ba.AccountTypeId    = @AccountTypeId";
            if (!string.IsNullOrEmpty(option.AccountNumber)) query += @"
AND         ba.AccountNumber    = @AccountNumber";
            if (!string.IsNullOrEmpty(option.BankName)) query += @"
AND         ba.BankName         = @BankName";
            if (!string.IsNullOrEmpty(option.BranchName)) query += @"
AND         ba.BranchName       = @BranchName";
            if (option.ReceiptCategoryId.HasValue) query += @"
AND         ba.ReceiptCategoryId = @ReceiptCategoryId";
            if (option.SectionId.HasValue) query += @"
AND         ba.SectionId        = @SectionId";
            if (option.ImportSkipping.HasValue) query += @"
AND         ba.ImportSkipping = @ImportSkipping";
            if (option.Ids?.Any() ?? false) query += @"
AND         ba.Id               IN (SELECT Id FROM @Ids)";
            if (option.BankCodes?.Any() ?? false) query += @"
AND         ba.BankCode         IN (SELECT Code FROM @BankCodes)";
            if (option.BranchCodes?.Any() ?? false) query += @"
AND         ba.BranchCode       IN (SELECT Code FROM @BranchCodes)";



            query += @"
ORDER BY        ba.CompanyId        ASC
              , ba.BankCode         ASC
              , ba.BranchCode       ASC
              , ba.AccountTypeId    ASC
              , ba.AccountNumber    ASC
";

            return dbHelper.GetItemsAsync<BankAccount>(query, new {
                                    option.CompanyId,
                                    option.AccountTypeId,
                                    option.AccountNumber,
                                    option.BankName,
                                    option.BranchName,
                                    option.ReceiptCategoryId,
                                    option.SectionId,
                                    option.ImportSkipping,
                                    option.SearchWord,
                Ids             =   option.Ids.GetTableParameter(),
                BankCodes       =   option?.BankCodes?.GetTableParameter(),
                BranchCodes     =   option?.BranchCodes?.GetTableParameter(),
            }, token);
        }

        public Task< BankAccount> SaveAsync(BankAccount BankAccount, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO BankAccount AS target 
USING ( 
    SELECT @CompanyId       CompanyId
         , @BankCode        BankCode
         , @BranchCode      BranchCode
         , @AccountTypeId   AccountTypeId
         , @AccountNumber   AccountNumber
) AS source
ON ( 
        target.CompanyId        = @CompanyId 
    AND target.BankCode         = @BankCode 
    AND target.BranchCode       = @BranchCode 
    AND target.AccountTypeId    = @AccountTypeId 
    AND target.AccountNumber    = @AccountNumber 
) 
WHEN MATCHED THEN
    UPDATE SET
           BankName             = @BankName 
         , BranchName           = @BranchName 
         , ReceiptCategoryId    = @ReceiptCategoryId
         , SectionId            = (
                    CASE
                       WHEN @SectionId != 0 THEN @SectionId
                       ELSE NULL
                    END
                )
        , ImportSkipping    = @ImportSkipping
        , UpdateBy          = @UpdateBy 
        , UpdateAt          = GETDATE() 
WHEN NOT MATCHED THEN 
    INSERT ( CompanyId,  BankCode,  BranchCode,  AccountTypeId,  AccountNumber,  BankName,  BranchName,  ReceiptCategoryId
           , SectionId,  ImportSkipping,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt)
    VALUES (@CompanyId, @BankCode, @BranchCode, @AccountTypeId, @AccountNumber, @BankName, @BranchName, @ReceiptCategoryId,
        (
        CASE
            WHEN @SectionId != 0 THEN  @SectionId
            ELSE NULL
        END
        ),              @ImportSkipping, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<BankAccount>(query, BankAccount, token);
        }

        public async Task<bool> ExistCategoryAsnc(int ReceiptCategoryId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 
         FROM BankAccount
        WHERE ReceiptCategoryId = @ReceiptcategoryId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { ReceiptCategoryId })).HasValue;
        }

        public async Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 
         FROM BankAccount
        WHERE SectionId = @SectionId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { SectionId })).HasValue;
        }
    }
}
