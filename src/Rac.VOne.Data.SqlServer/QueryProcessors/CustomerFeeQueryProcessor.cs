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
    public class CustomerFeeQueryProcessor :
        ICustomerFeeQueryProcessor,
        IAddCustomerFeeQueryProcessor,
        IDeleteCustomerFeeQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CustomerFeeQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        private string GetQueryForPrint()
        {
            return @"
SELECT 
      cmp.Code as CompanyCode
    , cmp.Name as CompanyName
    , cs.Code  as CustomerCode
    , cr.Code  as CurrencyCode
    , cr.Precision as CurrencyPrecision
    , cs.Name  as CustomerName
    , cf.Fee1 as Fee1
    , cf.Fee2 as Fee2
    , cf.Fee3 as Fee3
    , cf.UpdateAt1 as UpdateAt1
    , cf.UpdateAt2 as UpdateAt2
    , cf.UpdateAt3 as UpdateAt3
 FROM (
      SELECT cf.CompanyId
           , cf.CustomerId
           , cf.CurrencyId
           , MAX(case cf.RowNumber when 1 then cf.Fee      else null end) as Fee1
           , MAX(case cf.RowNumber when 2 then cf.Fee      else null end) as Fee2
           , MAX(case cf.RowNumber when 3 then cf.Fee      else null end) as Fee3
           , MAX(case cf.RowNumber when 1 then cf.UpdateAt else null end) as UpdateAt1
           , MAX(case cf.RowNumber when 2 then cf.UpdateAt else null end) as UpdateAt2
           , MAX(case cf.RowNumber when 3 then cf.UpdateAt else null end) as UpdateAt3
        FROM (
             SELECT cs.CompanyId
                  , cf.CustomerId
                  , cf.CurrencyId
                  , cf.Fee
                  , cf.UpdateAt
                  , ROW_NUMBER() over (
                    PARTITION BY cs.CompanyId, cf.CustomerId, cf.CurrencyId
                        ORDER BY cf.UpdateAt DESC, cf.Fee
                    ) as RowNumber
               FROM CustomerFee cf
              INNER JOIN Customer cs
                 ON cs.Id            = cf.CustomerId
                AND cs.CompanyId     = @CompanyId
             ) cf
       WHERE cf.RowNumber <= 3
       GROUP BY
             cf.CompanyId
           , cf.CustomerId
           , cf.CurrencyId
      ) cf
INNER JOIN Customer cs      ON cs.Id        = cf.CustomerId
INNER JOIN Currency cr      ON cr.Id        = cf.CurrencyId
INNER JOIN Company cmp      ON cmp.Id       = cf.CompanyId
WHERE cf.Fee1 IS NOT NULL
ORDER BY
      cs.Code asc
    , cr.Code asc";
        }

        private string GetQueryStandard(CustomerFeeSearch option)
        {
            var query = @"
SELECT      cf.*
          , cs.CompanyId
          , cs.Code as CustomerCode
          , cr.Code as CurrencyCode
          , cm.Code as CompanyCode
FROM        CustomerFee cf
INNER JOIN  Customer cs             ON cs.Id        = cf.CustomerId
INNER JOIN  Currency cr             ON cr.Id        = cf.CurrencyId
INNER JOIN  Company cm              ON cm.Id        = cs.CompanyId
WHERE       cf.CustomerId           = cf.CustomerId";
            if (option.CompanyId.HasValue) query += @"
AND         cs.CompanyId            = @CompanyId";
            if (option.CustomerId.HasValue) query += @"
AND         cf.CustomerId           = @CustomerId";
            if (option.CurrencyId.HasValue) query += @"
AND         cf.CurrencyId           = @CurrencyId";

            query += @"
 ORDER BY
       cm.Code
     , cs.Code
     , cr.Code";

            return query;
        }

        public Task<IEnumerable<CustomerFee>> GetAsync(CustomerFeeSearch option, CancellationToken token =default(CancellationToken))
        {
            var query = option.ForPrint
                ? GetQueryForPrint()
                : GetQueryStandard(option);

            return dbHelper.GetItemsAsync<CustomerFee>(query, option, token);
        }

        public Task<CustomerFee> SaveAsync(CustomerFee fee, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO CustomerFee target
USING (
    SELECT @CustomerId  [CustomerId]
         , @CurrencyId  [CurrencyId]
         , @Fee         [Fee]
      ) source
ON    (
        target.CustomerId   = source.CustomerId
    AND target.CurrencyId   = source.CurrencyId
    AND target.Fee          = source.Fee
)
WHEN MATCHED THEN
    UPDATE
       SET UpdateBy     = @UpdateBy
         , UPdateAt     = GETDATE()
WHEN NOT MATCHED THEN
    INSERT
    ( CustomerId,  CurrencyId,  Fee,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt)
    VALUES
    (@CustomerId, @CurrencyId, @Fee, @CreateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*;
";
            return dbHelper.ExecuteAsync<CustomerFee>(query, fee, token);
        }

        public Task<int> DeleteAsync(CustomerFeeSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE      cf
FROM        CustomerFee cf
INNER JOIN  Customer cs         ON cs.Id        = cf.CustomerId
INNER JOIN  Currency cr         ON cr.Id        = cf.CurrencyId
WHERE       cf.CustomerId       = cf.CustomerId";
            if (option.CompanyId.HasValue) query += @"
AND         cs.CompanyId        = @CompanyId";
            if (option.CustomerId.HasValue) query += @"
AND         cf.CustomerId       = @CustomerId";
            if (option.CurrencyId.HasValue) query += @"
AND         cf.CurrencyId       = @CurrencyId";
            if (option.Fee.HasValue) query += @"
AND         cf.Fee              = @Fee";

            return dbHelper.ExecuteAsync(query, option, token);
        }
    }
}
