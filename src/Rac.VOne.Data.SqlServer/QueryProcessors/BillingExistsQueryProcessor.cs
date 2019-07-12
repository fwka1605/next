using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BillingExistsQueryProcessor : IBillingExistsQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingExistsQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public async Task<bool> ExistCreditAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
WHERE EXISTS(
      SELECT 1
        FROM Billing 
       WHERE CreditAccountTitleId = @AccountTitleId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { AccountTitleId }, token)).HasValue;
        }

        public async Task<bool> ExistDebitAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1
         FROM Billing 
        WHERE DebitAccountTitleId = @AccountTitleId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { AccountTitleId }, token)).HasValue;
        }

        public async Task<bool> ExistCollectCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1
         FROM Billing
        WHERE BillingCategoryId = @CategoryId
          OR  CollectCategoryId = @CategoryId
          OR  OriginalCollectCategoryId = @CategoryId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CategoryId }, token)).HasValue;
        }

        public async Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 FROM Billing WHERE DepartmentId = @DepartmentId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { DepartmentId }, token)).HasValue;
        }

        public async Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 FROM Billing WHERE CustomerId = @CustomerId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CustomerId }, token)).HasValue;
        }

        public async Task<bool> ExistBillingCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 FROM Billing  WHERE BillingCategoryId = @CategoryId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CategoryId }, token)).HasValue;
        }

        public async Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
        SELECT 1 FROM Billing WHERE CurrencyId = @CurrencyId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CurrencyId }, token)).HasValue;
        }

        public async Task<bool> ExistStaffAsync(int StaffId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS (
       SELECT 1
         FROM Billing b
        WHERE b.StaffId = @StaffId )";
            return (await dbHelper.ExecuteAsync<int?>(query, new { StaffId }, token)).HasValue;
        }

        public async Task<bool> ExistDestinationAsync(int DestinationId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 FROM Billing WHERE DestinationId = @DestinationId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { DestinationId }, token)).HasValue;
        }

    }
}
