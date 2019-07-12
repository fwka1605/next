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
    public class BillingBalanceBackQueryProcessor :
        IAddBillingBalanceBackQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingBalanceBackQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<BillingBalanceBack>> SaveAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
                    INSERT INTO BillingBalanceBack
                    (CompanyId,
                    CurrencyId,
                    CustomerId,
                    StaffId,
                    DepartmentId,
                    CarryOverAt,
                    BalanceCarriedOver,
                    CreateBy,
                    CreateAt)
                    OUTPUT inserted.*
                    SELECT
                    CompanyId,
                    CurrencyId,
                    CustomerId,
                    StaffId,
                    DepartmentId,
                    CarryOverAt,
                    BalanceCarriedOver,
                    CreateBy,
                    CreateAt
                    FROM BillingBalance
                    WHERE CompanyId = @CompanyId";
            return dbHelper.GetItemsAsync<BillingBalanceBack>(query, new { CompanyId }, token);
        }
    }
}
