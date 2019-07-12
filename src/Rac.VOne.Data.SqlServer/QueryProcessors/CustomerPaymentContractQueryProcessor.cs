using System;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CustomerPaymentContractQueryProcessor :
        IAddCustomerPaymentContractQueryProcessor,
        ICustomerPaymentContractQueryProcessor,
        IDeleteCustomerPaymentContractQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CustomerPaymentContractQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<CustomerPaymentContract> SaveAsync(CustomerPaymentContract contract, CancellationToken token = default(CancellationToken))
        {
            if (contract.GreaterThanCollectCategoryId2 == 0) contract.GreaterThanCollectCategoryId2 = null;
            if (contract.GreaterThanCollectCategoryId3 == 0) contract.GreaterThanCollectCategoryId3 = null;
            #region merge query
            var query = @"
MERGE INTO CustomerPaymentContract AS target
      USING (
          SELECT
              @CustomerId AS CustomerId 

      ) AS source 
      ON ( 
          target.CustomerId = source.CustomerId 
      ) 
      WHEN MATCHED THEN 
          UPDATE SET 
            CustomerId                      = @CustomerId
          , ThresholdValue                  = @ThresholdValue
          , LessThanCollectCategoryId       = @LessThanCollectCategoryId
          , GreaterThanCollectCategoryId1   = @GreaterThanCollectCategoryId1
          , GreaterThanRate1                = @GreaterThanRate1
          , GreaterThanRoundingMode1        = @GreaterThanRoundingMode1
          , GreaterThanSightOfBill1         = @GreaterThanSightOfBill1
          , GreaterThanCollectCategoryId2   = @GreaterThanCollectCategoryId2
          , GreaterThanRate2                = @GreaterThanRate2
          , GreaterThanRoundingMode2        = @GreaterThanRoundingMode2
          , GreaterThanSightOfBill2         = @GreaterThanSightOfBill2
          , GreaterThanCollectCategoryId3   = @GreaterThanCollectCategoryId3
          , GreaterThanRate3                = @GreaterThanRate3
          , GreaterThanRoundingMode3        = @GreaterThanRoundingMode3
          , GreaterThanSightOfBill3         = @GreaterThanSightOfBill3
          , UpdateBy                        = @UpdateBy
          , UpdateAt                        = GETDATE()
WHEN NOT MATCHED THEN
   INSERT (CustomerId
  ,ThresholdValue
  ,LessThanCollectCategoryId
  ,GreaterThanCollectCategoryId1
  ,GreaterThanRate1
  ,GreaterThanRoundingMode1
  ,GreaterThanSightOfBill1
  ,GreaterThanCollectCategoryId2
  ,GreaterThanRate2
  ,GreaterThanRoundingMode2
  ,GreaterThanSightOfBill2
  ,GreaterThanCollectCategoryId3
  ,GreaterThanRate3
  ,GreaterThanRoundingMode3
  ,GreaterThanSightOfBill3
  ,CreateBy
  ,CreateAt
  ,UpdateBy
  ,UpdateAt)
 
  VALUES (
  @CustomerId
  ,@ThresholdValue
  ,@LessThanCollectCategoryId
  ,@GreaterThanCollectCategoryId1
  ,@GreaterThanRate1
  ,@GreaterThanRoundingMode1
  ,@GreaterThanSightOfBill1
  ,@GreaterThanCollectCategoryId2
  ,@GreaterThanRate2
  ,@GreaterThanRoundingMode2
  ,@GreaterThanSightOfBill2
  ,@GreaterThanCollectCategoryId3
  ,@GreaterThanRate3
  ,@GreaterThanRoundingMode3
  ,@GreaterThanSightOfBill3
  ,@CreateBy
  ,GETDATE()
  ,@UpdateBy
  ,GETDATE())
OUTPUT inserted.*;";
            #endregion
            return dbHelper.ExecuteAsync<CustomerPaymentContract>(query, contract, token);
        }


        public async Task<bool> ExistCategoryAsync(int Id, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT TOP(1) 1
  FROM CustomerPaymentContract 
 WHERE LessThanCollectCategoryId        = @Id
    OR GreaterThanCollectCategoryId1    = @Id
    OR GreaterThanCollectCategoryId2    = @Id
    OR GreaterThanCollectCategoryId3    = @Id";
            return (await dbHelper.ExecuteAsync<int?>(query, new { Id })).HasValue;
        }

        public Task<int> DeleteAsync(int CustomerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"DELETE CustomerPaymentContract WHERE CustomerId = @CustomerId";
            return dbHelper.ExecuteAsync(query, new { CustomerId }, token);
        }

        public Task<IEnumerable<CustomerPaymentContract>> GetAsync(IEnumerable<int> ids, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT cp.*
,ltc.Code       LessThanCode
,ltc.Name       LessThanName
,gtc1.Code      GreaterThanCode1
,gtc1.Name      GreaterThanName1
,gtc2.Code      GreaterThanCode2
,gtc2.Name      GreaterThanName2
,gtc3.Code      GreaterThanCode3
,gtc3.Name      GreaterThanName3
,cate.Code  as CollectCategoryCode
,cate.Name  as CollectCategoryName
  FROM CustomerPaymentContract cp
  LEFT JOIN Category ltc        ON ltc.Id   = cp.LessThanCollectCategoryId
  LEFT JOIN Category gtc1       ON gtc1.Id  = cp.GreaterThanCollectCategoryId1
  LEFT JOIN Category gtc2       ON gtc2.Id  = cp.GreaterThanCollectCategoryId2
  LEFT JOIN Category gtc3       ON gtc3.Id  = cp.GreaterThanCollectCategoryId3
  LEFT JOIN Customer cus        ON cus.Id  = cp.CustomerId
  LEFT JOIN Category cate       ON cus.CollectCategoryId = cate.Id
WHERE CustomerId IN (SELECT Id FROM @CustomerId);";
            return dbHelper.GetItemsAsync<CustomerPaymentContract>(query, new { CustomerId = ids.GetTableParameter() }, token);
        }

    }
}
