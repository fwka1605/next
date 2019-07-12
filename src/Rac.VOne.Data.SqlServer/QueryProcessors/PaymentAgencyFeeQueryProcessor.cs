using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Collections.Generic;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class PaymentAgencyFeeQueryProcessor :
        IAddPaymentAgencyFeeQueryProcessor,
        IPaymentAgencyFeeQueryProcessor, 
        IDeletePaymentAgencyFeeQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public PaymentAgencyFeeQueryProcessor(IDbHelper dbHelper)

        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<PaymentAgencyFee>> GetAsync(PaymentAgencyFeeSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        pf.*
            , pa.CompanyId
            , pa.Code       PayemntAgencyCode
            , cr.Code       CurrencyCode
            , cm.Code       CompanyCode
FROM        PaymentAgencyFee pf
INNER JOIN  PaymentAgency pa        ON pa.Id            = pf.PaymentAgencyId
INNER JOIN  Currency cr             ON cr.Id            = pf.CurrencyId
INNER JOIN  Company cm              ON cm.Id            = pa.CompanyId
WHERE       pf.PaymentAgencyId  = pf.PaymentAgencyId";
            if (option.CompanyId.HasValue) query += @"
AND         cm.Id               = @CompanyId";
            if (option.PaymentAgencyId.HasValue) query += @"
AND         pf.PaymentAgencyId  = @PaymentAgencyId";
            if (option.CurrencyId.HasValue) query += @"
AND         pf.CurrencyId       = @CurrencyId";
            query += @"
 ORDER BY
       pa.CompanyId             ASC
     , pa.Code                  ASC
     , cr.Code                  ASC";
            return dbHelper.GetItemsAsync<PaymentAgencyFee>(query, option, token);
        }

        public Task<PaymentAgencyFee> SaveAsync(PaymentAgencyFee fee, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO PaymentAgencyFee target
USING (
    SELECT @PaymentAgencyId     [PaymentAgencyId]
         , @CurrencyId          [CurrencyId]
         , @Fee                 [Fee]
      ) source
ON    (
        target.PaymentAgencyId      = source.PaymentAgencyId
    AND target.CurrencyId           = source.CurrencyId
    AND target.Fee                  = source.Fee
)
WHEN MATCHED THEN
    UPDATE
       SET UpdateBy     = @UpdateBy
         , UPdateAt     = GETDATE()
WHEN NOT MATCHED THEN
    INSERT
    ( PaymentAgencyId,  CurrencyId,  Fee,  CreateBy, CreateAt, UpdateBy, UpdateAt)
    VALUES
    (@PaymentAgencyId, @CurrencyId, @Fee, @CreateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*;
";
            return dbHelper.ExecuteAsync<PaymentAgencyFee>(query, fee, token);
        }

        public Task<int> DeleteAsync(int PaymentAgencyId, int CurrencyId, decimal Fee, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE      PaymentAgencyFee
WHERE       PaymentAgencyId     = @PaymentAgencyId
AND         CurrencyId          = @CurrencyId
AND         Fee                 = @Fee";
            return dbHelper.ExecuteAsync(query, new { PaymentAgencyId, CurrencyId, Fee }, token);
        }

    }
}