using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class MatchingOrderQueryProcessor : IMatchingOrderQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public MatchingOrderQueryProcessor (IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<int> InitializeAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO MatchingOrder
(CompanyId, TransactionCategory, ItemName, ExecutionOrder, Available, SortOrder, CreateBy, CreateAt, UpdateBy, UpdateAt)
SELECT
@CompanyId, t.TransactionCategory, t.ItemName, t.ExcecutionOrder, t.Available, t.SortOrder, @LoginUserId, GETDATE(), @LoginUserId, GETDATE()
FROM (
          SELECT 1 [TransactionCategory], 'BillingRemainSign'   [ItemName], 1 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
UNION ALL SELECT 1 [TransactionCategory], 'CashOnDueDatesFlag'  [ItemName], 2 [ExcecutionOrder], 1 [Available], 1 [SortOrder]
UNION ALL SELECT 1 [TransactionCategory], 'DueAt'               [ItemName], 3 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
UNION ALL SELECT 1 [TransactionCategory], 'CustomerCode'        [ItemName], 4 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
UNION ALL SELECT 1 [TransactionCategory], 'BilledAt'            [ItemName], 5 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
UNION ALL SELECT 1 [TransactionCategory], 'BillingRemainAmount' [ItemName], 6 [ExcecutionOrder], 1 [Available], 1 [SortOrder]
UNION ALL SELECT 1 [TransactionCategory], 'BillingCategory'     [ItemName], 7 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
UNION ALL SELECT 2 [TransactionCategory], 'NettingFlag'         [ItemName], 1 [ExcecutionOrder], 1 [Available], 1 [SortOrder]
UNION ALL SELECT 2 [TransactionCategory], 'ReceiptRemainSign'   [ItemName], 2 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
UNION ALL SELECT 2 [TransactionCategory], 'RecordedAt'          [ItemName], 3 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
UNION ALL SELECT 2 [TransactionCategory], 'PayerName'           [ItemName], 4 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
UNION ALL SELECT 2 [TransactionCategory], 'SourceBankName'      [ItemName], 5 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
UNION ALL SELECT 2 [TransactionCategory], 'SourceBranchName'    [ItemName], 6 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
UNION ALL SELECT 2 [TransactionCategory], 'ReceiptRemainAmount' [ItemName], 7 [ExcecutionOrder], 1 [Available], 1 [SortOrder]
UNION ALL SELECT 2 [TransactionCategory], 'ReceiptCategory'     [ItemName], 8 [ExcecutionOrder], 1 [Available], 0 [SortOrder]
) t
";
            return dbHelper.ExecuteAsync(query, new {
                CompanyId   = companyId,
                LoginUserId = loginUserId,
            }, token);
        }
    }
}
