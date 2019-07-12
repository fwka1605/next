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
    public class BillingBalanceQueryProcessor :
        IBillingBalanceQueryProcessor,
        IAddBillingBalanceQueryProcessor,
        IDeleteBillingBalanceQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingBalanceQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<DateTime?> GetLastCarryOverAtAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT MAX(CarryOverAt) FROM BillingBalance WHERE CompanyId = @CompanyId";
            return dbHelper.ExecuteAsync<DateTime?>(query, new { CompanyId }, token);
        }

        public Task<IEnumerable<BillingBalance>> GetBillingBalancesAsync(int CompanyId, DateTime? LastCarryOverAt, DateTime CarryOverAt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
                    SELECT
                      CompanyId
                    , CurrencyId
                    , CustomerId
                    , StaffId
                    , DepartmentId
                    FROM BillingBalance
                    WHERE CompanyId = @CompanyId

                    UNION

                    SELECT
                      CompanyId
                    , CurrencyId
                    , CustomerId
                    , StaffId
                    , DepartmentId
                    FROM Billing
                    WHERE CompanyId = @CompanyId";
            if (LastCarryOverAt.HasValue)
            {
                query += @" AND BilledAt > @LastCarryOverAt";
            }

            query += @" AND BilledAt <= @CarryOverAt
                    GROUP BY
                      CompanyId
                    , CurrencyId
                    , CustomerId
                    , StaffId
                    , DepartmentId
                    ORDER BY
                      CompanyId
                    , CurrencyId
                    , CustomerId
                    , StaffId
                    , DepartmentId
                ";
            return dbHelper.GetItemsAsync<BillingBalance>(query, new
                {
                    CompanyId,
                    LastCarryOverAt,
                    CarryOverAt
                }, token);
        }


        public Task<decimal> GetBillingAmountAsync(int CompanyId, int CurrencyId, int CustomerId, int StaffId, int DepartmentId, DateTime CarryOverAt, DateTime? LastCarryOverAt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
                    SELECT
                      SUM(BillingAmount)
                      - SUM(CASE WHEN DeleteAt IS NULL THEN 0
                            ELSE RemainAmount END) BillingAmount
                    FROM Billing
                    WHERE CompanyId = @CompanyId
                    AND CurrencyId = @CurrencyId
                    AND CustomerId = @CustomerId
                    AND StaffId = @StaffId
                    AND DepartmentId = @DepartmentId
                    AND BilledAt <= @CarryOverAt";
            query += (LastCarryOverAt.HasValue) ? @" AND BilledAt > @LastCarryOverAt" : string.Empty;
            return dbHelper.ExecuteAsync<decimal>(query, new
                {
                    CompanyId,
                    CurrencyId,
                    CustomerId,
                    StaffId,
                    DepartmentId,
                    CarryOverAt,
                    LastCarryOverAt
                }, token);
        }

        public Task<decimal> GetReceiptAmountAsync(int CompanyId, int CurrencyId, int CustomerId, int StaffId, int DepartmentId, DateTime CarryOverAt, DateTime? LastCarryOverAt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
                    SELECT
                      SUM(Amount + BankTransferFee - CASE WHEN TaxDifference >= 0 THEN 0 ELSE TaxDifference END) ReceiptAmount
                    FROM Matching m
                    INNER JOIN Billing b
                    ON m.BillingId = b.Id
                    INNER JOIN Receipt r
                    ON m.ReceiptId = r.Id
                    WHERE b.CompanyId = @CompanyId
                    AND b.CurrencyId = @CurrencyId
                    AND b.CustomerId = @CustomerId
                    AND b.StaffId = @StaffId
                    AND b.DepartmentId = @DepartmentId
                    AND r.RecordedAt <= @CarryOverAt";
            query += (LastCarryOverAt.HasValue) ? @" AND r.RecordedAt > @LastCarryOverAt" : string.Empty;
            return dbHelper.ExecuteAsync<decimal>(query, new
                {
                    CompanyId,
                    CurrencyId,
                    CustomerId,
                    StaffId,
                    DepartmentId,
                    CarryOverAt,
                    LastCarryOverAt
                }, token);
        }
        public Task<IEnumerable<BillingBalance>> GetLastBillingBalanceAsync(int CompanyId, int CurrencyId, int CustomerId, int StaffId, int DepartmentId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
                    SELECT
                      CompanyId
                    , CurrencyId
                    , CustomerId
                    , StaffId
                    , DepartmentId
                    , BalanceCarriedOver
                    FROM BillingBalance
                    WHERE CompanyId = @CompanyId
                    AND CurrencyId = @CurrencyId
                    AND CustomerId = @CustomerId
                    AND StaffId = @StaffId
                    AND DepartmentId = @DepartmentId
                ";
            return dbHelper.GetItemsAsync<BillingBalance>(query, new
            {
                CompanyId,
                CurrencyId,
                CustomerId,
                StaffId,
                DepartmentId
            }, token);
        }

        public Task<BillingBalance> SaveAsync(BillingBalance BillingBalance, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO BillingBalance
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
VALUES
(
@CompanyId,
@CurrencyId,
@CustomerId,
@StaffId,
@DepartmentId,
@CarryOverAt,
@BalanceCarriedOver,
@CreateBy,
@CreateAt
)
";
            return dbHelper.ExecuteAsync<BillingBalance>(query, BillingBalance, token);
        }

        public Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
                    DELETE BillingBalance
                    WHERE CompanyId = @CompanyId";
            return dbHelper.ExecuteAsync(query, new { CompanyId }, token);
        }

        public Task<IEnumerable<BillingBalance>> RestoreBillingBalanceAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
                    INSERT INTO BillingBalance
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
                    FROM BillingBalanceBack
                    WHERE CompanyId = @CompanyId";
            return dbHelper.GetItemsAsync<BillingBalance>(query, new { CompanyId }, token);
        }

    }
}
