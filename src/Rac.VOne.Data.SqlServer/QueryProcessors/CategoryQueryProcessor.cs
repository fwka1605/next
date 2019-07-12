using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CategoryQueryProcessor :
        ICategoryByCodeQueryProcessor,
        IAddCategoryQueryProcessor,
        ICategoriesQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CategoryQueryProcessor(
                IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public async Task<bool> ExistAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
WHERE EXISTS(
      SELECT 1
          FROM Category 
      WHERE AccountTitleId = @AccountTitleId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { AccountTitleId })).HasValue;
        }

        public async Task<bool> ExistPaymentAgencyAsync(int PaymentAgencyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
WHERE EXISTS(
      SELECT 1
          FROM Category 
      WHERE PaymentAgencyId = @PaymentAgencyId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { PaymentAgencyId })).HasValue;
        }

        public Task<Category> SaveAsync(Category category, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO Category AS target 
USING ( 
    SELECT      @Id         Id
) AS source
ON (
        target.Id           = source.Id
)
WHEN MATCHED THEN
    UPDATE SET 
                Name = @Name
              , AccountTitleId = (
                    CASE
                       WHEN @AccountTitleId != 0 THEN @AccountTitleId
                       ELSE NULL
                    END
                )
              , SubCode                     = @SubCode
              , Note                        = @Note
              , TaxClassId                  = @TaxClassId
              , UseLimitDate                = @UseLimitDate
              , UseLongTermAdvanceReceived  = @UseLongTermAdvanceReceived
              , UseCashOnDueDates           = @UseCashOnDueDates
              , UseAccountTransfer          = @UseAccountTransfer
              , PaymentAgencyId = (
                    CASE
                       WHEN @PaymentAgencyId != 0 THEN @PaymentAgencyId
                       ELSE NULL
                    END
                )
              , UseDiscount                 = @UseDiscount
              , UseAdvanceReceived          = @UseAdvanceReceived
              , ForceMatchingIndividually   = @ForceMatchingIndividually
              , UseInput                    = @UseInput
              , MatchingOrder               = @MatchingOrder
              , UpdateBy                    = @UpdateBy
              , UpdateAt                    = GETDATE()
              , ExternalCode                = @ExternalCode
              , ExcludeInvoicePublish       = @ExcludeInvoicePublish
WHEN NOT MATCHED THEN
    INSERT (  CompanyId
           ,  CategoryType
           ,  Code
           ,  Name
           ,  AccountTitleId
           ,  SubCode
           ,  Note
           ,  TaxClassId
           ,  UseLimitDate
           ,  UseLongTermAdvanceReceived
           ,  UseCashOnDueDates
           ,  UseAccountTransfer
           ,  PaymentAgencyId
           ,  UseDiscount
           ,  UseAdvanceReceived
           ,  ForceMatchingIndividually
           ,  UseInput
           ,  MatchingOrder
           ,  CreateBy
           ,  CreateAt
           ,  UpdateBy
           ,  UpdateAt
           ,  ExternalCode
           )
    VALUES ( @CompanyId
           , @CategoryType
           , @Code
           , @Name
           , (
                CASE
                    WHEN  @AccountTitleId != 0 THEN  @AccountTitleId
                    ELSE NULL
                END
            )
           , @SubCode
           , @Note
           , @TaxClassId
           , @UseLimitDate
           , @UseLongTermAdvanceReceived
           , @UseCashOnDueDates
           , @UseAccountTransfer
           , (
                CASE
                    WHEN  @PaymentAgencyId != 0 THEN  @PaymentAgencyId
                    ELSE NULL
                END
            )
           , @UseDiscount
           , @UseAdvanceReceived
           , @ForceMatchingIndividually
           , @UseInput
           , @MatchingOrder
           , @CreateBy
           , GETDATE()
           , @UpdateBy
           , GETDATE()
           , @ExternalCode
           )
OUTPUT inserted.*; ";
            #endregion
            return dbHelper.ExecuteAsync<Category>(query, category, token);
        }

        public Task<int> InitializeAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO Category
(CompanyId
, CategoryType
, Code
, Name
, AccountTitleId
, SubCode
, Note
, TaxClassId
, UseLimitDate
, UseLongTermAdvanceReceived
, UseCashOnDueDates
, UseAccountTransfer
, PaymentAgencyId
, UseDiscount
, UseAdvanceReceived
, ForceMatchingIndividually
, UseInput
, MatchingOrder
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  @CompanyId
, CategoryType
, Code
, Name
, NULL
, ''
, ''
, TaxClassId
, UseLimitDate
, 0
, 0
, 0
, NULL
, 0
, UseAdvanceReceived
, 0
, UseInput
, 0
, @LoginUserId
, GETDATE()
, @LoginUserId
, GETDATE()
From CategoryBase
";
            return dbHelper.ExecuteAsync(query, new { CompanyId = companyId, LoginUserId = loginUserId }, token);
        }


        public Task<IEnumerable<Category>> GetAsync(
            CategorySearch option,
            CancellationToken token = default(CancellationToken))
        {
            string query = @"
SELECT      ct.*
          , ac.Code [AccountTitleCode] , ac.Name [AccountTitleName]
          , pa.Code [PaymentAgencyCode], pa.Name [PaymentAgencyName]
          , tc.Name [TaxClassName]
FROM        Category ct
LEFT JOIN   AccountTitle ac     ON ac.Id        = ct.AccountTitleId
LEFT JOIN   PaymentAgency pa    ON pa.Id        = ct.PaymentAgencyId
LEFT JOIN   TaxClass tc         ON tc.Id        = ct.TaxClassId
WHERE       ct.Id                   = ct.Id";
            if (option.CompanyId.HasValue) query += @"
AND         ct.CompanyId            = @CompanyId";
            if (option.CategoryType.HasValue) query += @"
AND         ct.CategoryType         = @CategoryType";
            if (option.UseInput.HasValue) query += @"
AND         ct.UseInput             = @UseInput";
            if (option.UseLimitDate.HasValue) query += @"
AND         ct.UseLimitDate         = @UseLimitDate";
            if (option.UseAccountTransfer.HasValue) query += @"
AND         ct.UseAccountTransfer   = @UseAccountTransfer";
            if (!string.IsNullOrWhiteSpace(option.Name))
            {
                option.Name = Sql.GetWrappedValue(option.Name);
                query += @"
AND         ct.Name                 LIKE @Name";
            }
            if (option.Ids?.Any() ?? false) query += @"
AND         ct.Id                   IN (SELECT Id   FROM @Ids)";
            if (option.Codes?.Any() ?? false) query += @"
AND         ct.Code                 IN (SELECT Code FROM @Codes)";
            query += @"
ORDER BY    ct.CompanyId            ASC
          , ct.CategoryType         ASC
          , ct.Code                 ASC";
            return dbHelper.GetItemsAsync<Category>(query, new {
                            option.CompanyId,
                            option.CategoryType,
                            option.UseInput,
                            option.UseLimitDate,
                            option.UseAccountTransfer,
                            option.Name,
                Ids     =   option.Ids.GetTableParameter(),
                Codes   =   option.Codes.GetTableParameter(),
            }, token);
        }
    }
}
