using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class PeriodicBillingSettingDetailQueryProcessor :
        IPeriodicBillingSettingDetailQueryProcessor,
        IAddPeriodicBillingSettingDetailQueryProcessor,
        IDeletePeriodicBillingSettingDetailQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public PeriodicBillingSettingDetailQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<PeriodicBillingSettingDetail> SaveAsync(PeriodicBillingSettingDetail detail, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO PeriodicBillingSettingDetail
(  PeriodicBillingSettingId,  DisplayOrder
,  BillingCategoryId,  TaxClassId,  DebitAccountTitleId
,  Quantity,  UnitSymbol,  UnitPrice,  Price,  TaxAmount,  BillingAmount
,  Note1,  Note2,  Note3,  Note4,  Note5,  Note6,  Note7,  Note8, Memo
,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt )
OUTPUT inserted.*
VALUES
( @PeriodicBillingSettingId, @DisplayOrder
, @BillingCategoryId, @TaxClassId, @DebitAccountTitleId
, @Quantity, @UnitSymbol, @UnitPrice, @Price, @TaxAmount, @BillingAmount
, @Note1, @Note2, @Note3, @Note4, @Note5, @Note6, @Note7, @Note8, @Memo
, @CreateBy, GETDATE(), @UpdateBy, GETDATE())";

            return dbHelper.ExecuteAsync<PeriodicBillingSettingDetail>(query, detail, token);
        }

        public Task<int> DeleteAsync(long headerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE PeriodicBillingSettingDetail
 WHERE PeriodicBillingSettingId = @headerId";
            return dbHelper.ExecuteAsync(query, new { headerId }, token);
        }

        public Task<IEnumerable<PeriodicBillingSettingDetail>> GetAsync(PeriodicBillingSettingSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT d.*
  FROM PeriodicBillingSettingDetail d
 INNER JOIN PeriodicBillingSetting s    ON s.Id     = d.PeriodicBillingSettingId
 WHERE s.CompanyId              = @CompanyId
   AND s.Id                     IN (SELECT id FROM @Ids)";
            return dbHelper.GetItemsAsync<PeriodicBillingSettingDetail>(query, new {
                        option.CompanyId,
                Ids =   option.Ids?.GetTableParameter()
            }, token);
        }

    }
}
