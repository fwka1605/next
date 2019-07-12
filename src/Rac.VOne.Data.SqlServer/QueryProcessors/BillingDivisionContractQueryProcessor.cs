using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BillingDivisionContractQueryProcessor :
        IBillingDivisionContractQueryProcessor,
        IAddBillingDivisionContractQueryProcessor,
        IUpdateBillingDivisionContractQueryProcessor,
        IDeleteBillingDivisionContractQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingDivisionContractQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<BillingDivisionContract>> GetItemsAsync(
            BillingDivisionContractSearch option,
            CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        BillingDivisionContract c
WHERE       c.Id                = c.Id";
            if (option.CompanyId.HasValue) query += @"
AND         c.CompanyId         = @CompanyId";
            if (option.CustomerId.HasValue) query += @"
AND         c.CustomerId        = @CustomerId";
            if (!string.IsNullOrWhiteSpace(option.ContractNumber)) query += @"
AND         c.ContractNumber    = @ContractNumber";
            if (option.CustomerIds?.Any() ?? false) query += @"
AND         c.CustomerId        IN (SELECT Id FROM @CustomerIds)";
            if (option.BillingIds?.Any() ?? false) query += @"
AND         c.BillingId         IN (SELECT Id FROM @BillingIds)";
            query += @"
ORDER BY    c.CompanyId         ASC
          , c.CustomerId        ASC
          , c.ContractNumber    ASC";
            return dbHelper.GetItemsAsync<BillingDivisionContract>(query, new {
                                    option.CompanyId,
                                    option.CustomerId,
                                    option.ContractNumber,
                CustomerIds     =   option.CustomerIds.GetTableParameter(),
                BillingIds      =   option.BillingIds.GetTableParameter(),
            }, token);
        }

        private string GetQueryUpdateBilling(BillingDivisionContract contract)
        {
            var cancel = contract.ContractNumber == null;
            var query = new StringBuilder($@"
UPDATE BillingDivisionContract
   SET UpdateBy     = @UpdateBy 
     , UpdateAt     = GETDATE()
     , BillingId    = {(cancel ? "NULL" : "@BillingId")}
 WHERE CompanyId    = @ComanyId
   AND CustomerId   = @CustomerId");

            if (cancel)
                query.Append(@"
   AND BillingId    = @BillingId");
            else
                query.Append(@"
   AND ContractNumber = @ContractNumer");
            return query.ToString();
        }

        public Task<int> UpdateBillingAsync(BillingDivisionContract contract, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync(GetQueryUpdateBilling(contract), contract, token);

        private string GetQueryUpdateWithBillingId() => @"
UPDATE BillingDivisionContract
SET
    BillingId = NULL
  , UpdateBy = @UpdateBy
  , UpdateAt = GETDATE()
WHERE BillingId = @BillingId";

        public Task<int> UpdateWithBillingIdAsync(long billingId, int loginUserId, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync(GetQueryUpdateWithBillingId(), new {
                BillingId   = billingId,
                UpdateBy    = loginUserId,
            }, token);

        public Task<int> DeleteWithBillingIdAsync(long BillingId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE BillingDivisionContract
WHERE BillingId = @BillingId";
            return dbHelper.ExecuteAsync(query, new { BillingId }, token);
        }

        public Task<long> GetNewContractNumberAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      COALESCE( MAX( CASE WHEN ContractNumber IS NULL THEN 0 ELSE CAST( ContractNumber AS BIGINT ) END ), 0 ) + 1 [MaxContractNumber]
FROM        BillingDivisionContract
WHERE       CompanyId   = @CompanyId";
            return dbHelper.ExecuteAsync<long>(query, new { CompanyId = companyId }, token);
        }

        public Task<BillingDivisionContract> SaveAsync(BillingDivisionContract billingDivisionContract, CancellationToken token = default(CancellationToken))
        {
            #region insert query
            var query = @"
INSERT INTO BillingDivisionContract
(CompanyId
,CustomerId
,ContractNumber
,FirstDateType
,Monthly
,BasisDay
,DivisionCount
,RoundingMode
,RemainsApportionment
,BillingId
,BillingAmount
,Comfirm
,CancelDate
,CreateBy
,CreateAt
,UpdateBy
,UpdateAt
)
VALUES
(@CompanyId
,@CustomerId
,@ContractNumber
,@FirstDateType
,@Monthly
,@BasisDay
,@DivisionCount
,@RoundingMode
,@RemainsApportionment
,@BillingId
,@BillingAmount
,@Comfirm
,@CancelDate
,@CreateBy
,GETDATE()
,@UpdateBy
,GETDATE())";
            #endregion
            return dbHelper.ExecuteAsync<BillingDivisionContract>(query, billingDivisionContract, token);
        }
    }
}
