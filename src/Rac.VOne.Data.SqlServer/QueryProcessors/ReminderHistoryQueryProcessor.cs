using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReminderHistoryQueryProcessor : 
        IReminderHistoryQueryProcessor,
        IAddReminderHistoryQueryProcessor,
        IUpdateReminderHistoryQueryProcessor,
        IDeleteReminderHistoryQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReminderHistoryQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<ReminderHistory> AddAsync(ReminderHistory ReminderHistory, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
INSERT INTO ReminderHistory (
  ReminderId
, StatusId
, Memo
, OutputFlag
, InputType
, ReminderAmount
, CreateBy
, CreateAt
)
OUTPUT inserted.*
  VALUES (
  @ReminderId
, @StatusId
, @Memo
, @OutputFlag
, @InputType
, @ReminderAmount
, @CreateBy
, GETDATE()
)
";
            return dbHelper.ExecuteAsync<ReminderHistory>(query, ReminderHistory, token);
        }

        public Task<ReminderSummaryHistory> AddSummaryAsync(ReminderSummaryHistory ReminderSummaryHistory, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO ReminderSummaryHistory (
  ReminderSummaryId
, Memo
, InputType
, ReminderAmount
, CreateBy
, CreateAt
)
OUTPUT inserted.*
VALUES (
  @ReminderSummaryId
, @Memo
, @InputType
, @ReminderAmount
, @CreateBy
, GETDATE()
)
";
            return dbHelper.ExecuteAsync<ReminderSummaryHistory>(query, ReminderSummaryHistory, token);
        }

        public Task<IEnumerable<ReminderHistory>> GetItemsByReminderIdAsync(int reminderId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT
  h.Id
, h.ReminderId
, st.Code [StatusCode]
, st.Name [StatusName]
, h.Memo
, h.OutputFlag
, h.InputType
, h.ReminderAmount
, h.CreateBy
, l.Name [CreateByName]
, h.CreateAt
, st.Id [StatusId]
FROM ReminderHistory h
INNER JOIN StatusMaster st
ON h.StatusId = st.Id
LEFT JOIN LoginUser l
ON h.CreateBy = l.Id
WHERE ReminderId = @ReminderId
ORDER BY h.CreateAt DESC, h.Id DESC
";
            return dbHelper.GetItemsAsync<ReminderHistory>(query, new { ReminderId = reminderId }, token);
        }

        public Task<IEnumerable<ReminderSummaryHistory>> GetSummaryItemsByReminderSummaryIdAsync(int reminderSummaryId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT
  rh.Id
, rh.ReminderSummaryId
, rh.Memo
, rh.InputType
, rh.ReminderAmount
, l.Name [CreateByName]
, rh.CreateAt
FROM ReminderSummaryHistory rh
LEFT JOIN LoginUser l
ON rh.CreateBy = l.Id
WHERE rh.ReminderSummaryId = @ReminderSummaryId
ORDER BY rh.CreateAt DESC, rh.Id DESC
";
            return dbHelper.GetItemsAsync<ReminderSummaryHistory>(query, new { ReminderSummaryId = reminderSummaryId }, token);
        }

        public Task<ReminderHistory> UpdateReminderHistoryAsync(ReminderHistory reminderHistory, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE ReminderHistory
   SET StatusId = @StatusId
     , Memo = @Memo
OUTPUT inserted.*
 WHERE Id = @Id
";
            return dbHelper.ExecuteAsync<ReminderHistory>(query, reminderHistory, token);
        }

        public Task<ReminderSummaryHistory> UpdateReminderSummaryHistoryAsync(ReminderSummaryHistory reminderSummaryHistory, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE ReminderSummaryHistory
   SET Memo = @Memo
OUTPUT inserted.*
 WHERE Id = @Id
";
            return dbHelper.ExecuteAsync<ReminderSummaryHistory>(query, reminderSummaryHistory, token);
        }

        public Task<int> DeleteByReminderIdAsync(int reminderId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE ReminderHistory
 WHERE ReminderId = @reminderId";
            return dbHelper.ExecuteAsync(query, new { reminderId }, token);
        }

        public Task<int> DeleteReminderSummaryHistoryAsync(Reminder reminder, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE ReminderSummaryHistory
WHERE Id IN (SELECT Id FROM ReminderSummary
                WHERE CustomerId = @CustomerId
                  AND CurrencyId = @CurrencyId)
";
            return dbHelper.ExecuteAsync(query, reminder, token);
        }


    }
}
