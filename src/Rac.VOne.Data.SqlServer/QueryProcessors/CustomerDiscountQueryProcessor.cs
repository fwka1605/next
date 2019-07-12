using System;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CustomerDiscountQueryProcessor :
        ICustomerDiscountQueryProcessor,
        IAddCustomerDiscountQueryProcessor,
        IDeleteCustomerDiscountQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CustomerDiscountQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        public Task<CustomerDiscount> SaveAsync(CustomerDiscount CustomerDiscount, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO CustomerDiscount AS Cd 
USING ( 
    SELECT 
        @CustomerId AS CustomerId 
) AS Target 
ON ( 
    Cd.CustomerId = @CustomerId 
    AND Cd.Sequence = @Sequence
) 
WHEN MATCHED THEN 
    UPDATE SET 
    CustomerId = @CustomerId,
    Sequence = @Sequence,
    Rate = @Rate,
    RoundingMode = @RoundingMode,
    MinValue = @MinValue,
    DepartmentId = @DepartmentId,
    AccountTitleId = @AccountTitleId,
    SubCode = @SubCode, 
    UpdateBy = @UpdateBy,
    UpdateAt = GETDATE() 

WHEN NOT MATCHED THEN 
    INSERT
    (CustomerId
    ,Sequence
    ,Rate
    ,RoundingMode
    ,MinValue
    ,DepartmentId
    ,AccountTitleId
    ,SubCode
    ,CreateBy
    ,CreateAt
    ,UpdateBy
    ,UpdateAt
    )
    VALUES
    (@CustomerId
    ,@Sequence
    ,@Rate
    ,@RoundingMode
    ,@MinValue
    ,@DepartmentId
    ,@AccountTitleId
    ,@SubCode
    ,@CreateBy
    ,GETDATE()
    ,@UpdateBy
    ,GETDATE()
    )
OUTPUT inserted.*;";
            #endregion
            return dbHelper.ExecuteAsync<CustomerDiscount>(query, CustomerDiscount, token);
        }
        public Task<IEnumerable<CustomerDiscount>> GetAsync(int customerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        cd.*
            , d.Code   DepartmentCode
            , d.Name   DepartmentName
            , acc.Code AccountTitleCode
            , acc.Name AccountTitleName
FROM        CustomerDiscount cd
LEFT JOIN   AccountTitle acc    ON acc.Id   = cd.AccountTitleId
LEFT JOIN   Department d        ON d.Id     = cd.DepartmentId
WHERE       cd.CustomerId = @customerId";
            return dbHelper.GetItemsAsync<CustomerDiscount>(query, new { customerId }, token);
        }
        public async Task<bool> ExistAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1
         FROM CustomerDiscount 
        WHERE AccountTitleId = @AccountTitleId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { AccountTitleId })).HasValue;
        }

        public Task<int> DeleteAsync(CustomerDiscount discount, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE      CustomerDiscount
WHERE       CompanyId           = @CompanyId";
            if (discount.Sequence > 0) query += @"
AND         [Sequence]          = @Sequence";
            return dbHelper.ExecuteAsync(query, discount, token);
        }

        public Task<IEnumerable<CustomerDiscount>> GetItemsAsync(CustomerSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT            cd.*
                , cm.[Code]     [CompanyCode]
                , cs.[Code]     [CustomerCode]
                , cs.[Name]     [CustomerName]
                , dp1.[Code]    [DepartmentCode1]
                , dp2.[Code]    [DepartmentCode2]
                , dp3.[Code]    [DepartmentCode3]
                , dp4.[Code]    [DepartmentCode4]
                , dp5.[Code]    [DepartmentCode5]
                , ac1.[Code]    [AccountTitleCode1]
                , ac2.[Code]    [AccountTitleCode2]
                , ac3.[Code]    [AccountTitleCode3]
                , ac4.[Code]    [AccountTitleCode4]
                , ac5.[Code]    [AccountTitleCode5]
FROM            (
SELECT            cd.CustomerId
                , MIN( cd.MinValue          ) [MinValue]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 1 THEN cd.Rate                          ELSE -1 END ), -1 ) [Rate1]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 2 THEN cd.Rate                          ELSE -1 END ), -1 ) [Rate2]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 3 THEN cd.Rate                          ELSE -1 END ), -1 ) [Rate3]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 4 THEN cd.Rate                          ELSE -1 END ), -1 ) [Rate4]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 5 THEN cd.Rate                          ELSE -1 END ), -1 ) [Rate5]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 1 THEN cd.RoundingMode                  ELSE -1 END ), -1 ) [RoundingMode1]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 2 THEN cd.RoundingMode                  ELSE -1 END ), -1 ) [RoundingMode2]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 3 THEN cd.RoundingMode                  ELSE -1 END ), -1 ) [RoundingMode3]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 4 THEN cd.RoundingMode                  ELSE -1 END ), -1 ) [RoundingMode4]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 5 THEN cd.RoundingMode                  ELSE -1 END ), -1 ) [RoundingMode5]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 1 THEN COALESCE(cd.DepartmentId  , -1 ) ELSE -1 END ), -1 ) [DepartmentId1]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 2 THEN COALESCE(cd.DepartmentId  , -1 ) ELSE -1 END ), -1 ) [DepartmentId2]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 3 THEN COALESCE(cd.DepartmentId  , -1 ) ELSE -1 END ), -1 ) [DepartmentId3]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 4 THEN COALESCE(cd.DepartmentId  , -1 ) ELSE -1 END ), -1 ) [DepartmentId4]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 5 THEN COALESCE(cd.DepartmentId  , -1 ) ELSE -1 END ), -1 ) [DepartmentId5]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 1 THEN COALESCE(cd.AccountTitleId, -1 ) ELSE -1 END ), -1 ) [AccountTitleId1]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 2 THEN COALESCE(cd.AccountTitleId, -1 ) ELSE -1 END ), -1 ) [AccountTitleId2]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 3 THEN COALESCE(cd.AccountTitleId, -1 ) ELSE -1 END ), -1 ) [AccountTitleId3]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 4 THEN COALESCE(cd.AccountTitleId, -1 ) ELSE -1 END ), -1 ) [AccountTitleId4]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 5 THEN COALESCE(cd.AccountTitleId, -1 ) ELSE -1 END ), -1 ) [AccountTitleId5]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 1 THEN cd.SubCode ELSE CHAR(9) END ), CHAR(9) ) [SubCode1]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 2 THEN cd.SubCode ELSE CHAR(9) END ), CHAR(9) ) [SubCode2]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 3 THEN cd.SubCode ELSE CHAR(9) END ), CHAR(9) ) [SubCode3]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 4 THEN cd.SubCode ELSE CHAR(9) END ), CHAR(9) ) [SubCode4]
                , NULLIF( MAX( CASE cd.[Sequence] WHEN 5 THEN cd.SubCode ELSE CHAR(9) END ), CHAR(9) ) [SubCode5]
FROM            CustomerDiscount cd
GROUP BY          cd.CustomerId
) cd
INNER JOIN      Customer cs         ON  cs.Id   = cd.CustomerId
INNER JOIN      Company cm          ON  cm.Id   = cs.CompanyId
LEFT JOIN       Staff st            ON  st.id   = cs.StaffId
LEFT JOIN       Department dp1      ON dp1.Id   = cd.DepartmentId1
LEFT JOIN       Department dp2      ON dp2.Id   = cd.DepartmentId2
LEFT JOIN       Department dp3      ON dp3.Id   = cd.DepartmentId3
LEFT JOIN       Department dp4      ON dp4.Id   = cd.DepartmentId4
LEFT JOIN       Department dp5      ON dp5.Id   = cd.DepartmentId5
LEFT JOIN       AccountTitle ac1    ON ac1.Id   = cd.AccountTitleId1
LEFT JOIN       AccountTitle ac2    ON ac2.Id   = cd.AccountTitleId2
LEFT JOIN       AccountTitle ac3    ON ac3.Id   = cd.AccountTitleId3
LEFT JOIN       AccountTitle ac4    ON ac4.Id   = cd.AccountTitleId4
LEFT JOIN       AccountTitle ac5    ON ac5.Id   = cd.AccountTitleId5
WHERE           cs.CompanyId        = @CompanyId";
            if (!string.IsNullOrEmpty(option.CustomerCodeFrom)) query += @"
AND             cs.Code             >= @CustomerCodeFrom";
            if (!string.IsNullOrEmpty(option.CustomerCodeTo)) query += @"
AND             cs.Code             <= @CustomerCodeTo";
            if (option.ClosingDay.HasValue) query += @"
AND             cs.ClosingDay       = @ClosingDay";
            if (option.ShareTransferFee.HasValue) query += @"
AND             cs.ShareTransferFee = @ShareTransferFee";
            if (option.UpdateAtFrom.HasValue) query += @"
AND             cs.UpdateAt         >= @UpdateAtFrom";
            if (option.UpdateAtTo.HasValue) query += @"
AND             cs.UpdateAt         <= @UpdateAtTo";
            if (!string.IsNullOrEmpty(option.StaffCodeFrom)) query += @"
AND             st.Code             >= @StaffCodeFrom";
            if (!string.IsNullOrEmpty(option.StaffCodeTo)) query += @"
AND             st.Code             <= @StaffCodeTo";
            query += @"
ORDER BY          cs.CompanyId      ASC
                , cs.Code           ASC";

            return dbHelper.GetItemsAsync<CustomerDiscount>(query, option, token);
        }
    }
}
