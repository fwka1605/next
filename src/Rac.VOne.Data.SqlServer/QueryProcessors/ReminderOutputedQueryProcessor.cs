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
    public class ReminderOutputedQueryProcessor : 
        IReminderOutputedQueryProcessor,
        IReminderOutputedExistsQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReminderOutputedQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<int> AddAsync(ReminderOutputed ReminderOutputed, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO ReminderOutputed (
  OutputNo
, BillingId
, RemainAmount
, ReminderTemplateId
, OutputAt
, DestinationId
)
VALUES(
  @OutputNo
, @BillingId
, @RemainAmount
, @ReminderTemplateId
, @OutputAt
, @DestinationId
)
";
            return dbHelper.ExecuteAsync(query, ReminderOutputed, token);
        }

        public Task<int> AddReminderHistoryAsync(int loginUserId, int ReminderId, DateTime outputAt, decimal reminderAmount, CancellationToken token = default(CancellationToken))
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
SELECT
  @ReminderId
, StatusId
, Memo
, 1
, @InputType
, @RemainAmount
, @LoginUserId
, @OutputAt
FROM Reminder
WHERE Id = @ReminderId
";
            return dbHelper.ExecuteAsync(query, 
                new {
                    LoginUserId = loginUserId,
                    ReminderId = ReminderId,
                    InputType = (int)ReminderHistory.ReminderHistoryInputType.Output,
                    RemainAmount = reminderAmount,
                    OutputAt = outputAt
                }, token);
        }

        public Task<int> UpdateReminderAsync(int reminderId, DateTime outputAt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Reminder
   SET OutputAt = @OutputAt
 WHERE Id = @ReminderId
   AND OutputAt IS NULL
";
            return dbHelper.ExecuteAsync(query, new { ReminderId = reminderId, OutputAt = outputAt }, token);
        }

        public Task<IEnumerable<ReminderOutputed>> GetItemsAsync(ReminderOutputedSearch search, CancellationToken token = default(CancellationToken))
        {
            var condition = new StringBuilder();

            if (search.OutputAtFrom != null)
                condition.AppendLine("AND o.OutputAt >= @OutputAtFrom");
            if (search.OutputAtTo != null)
            {
                search.OutputAtTo = search.OutputAtTo.Value.AddDays(1);
                condition.AppendLine("AND o.OutputAt < @OutputAtTo");
            }
            if (search.OutputNoFrom != null)
                condition.AppendLine("AND o.OutputNo >= @OutputNoFrom");
            if (search.OutputNoTo != null)
                condition.AppendLine("AND o.OutputNo <= @OutputNoTo");
            if (!string.IsNullOrEmpty(search.CustomerCodeFrom))
                condition.AppendLine("AND cs.Code >= @CustomerCodeFrom");
            if (!string.IsNullOrEmpty(search.CustomerCodeTo))
                condition.AppendLine("AND cs.Code <= @CustomerCodeTo");

            var destinationSelect = new StringBuilder();
            var destinationJoin = new StringBuilder();
            var destinationGroup = new StringBuilder();

            if (search.UseDestinationSummarized)
            {
                destinationSelect.AppendLine(", d.Id [DestinationId]");
                destinationSelect.AppendLine(", COALESCE(d.Code, '') [DestinationCode]");
                destinationSelect.AppendLine(", CASE WHEN d.Id IS NULL THEN");
                destinationSelect.AppendLine("     IIF (COALESCE(cs.PostalCode,'') <> '','〒' + cs.PostalCode, '') + ' ' + cs.Name + cs.Address1 + cs.Address2 + ' ' + cs.DestinationDepartmentName + ' ' + cs.CustomerStaffName + cs.Honorific");
                destinationSelect.AppendLine("  ELSE");
                destinationSelect.AppendLine("     IIF (COALESCE(d.PostalCode,'') <> '','〒' + d.PostalCode, '') + ' ' + d.Name + d.Address1 + d.Address2 + ' ' + d.DepartmentName + ' ' + d.Addressee + d.Honorific");
                destinationSelect.AppendLine("  END [DestinationDisplay]");

                destinationJoin.AppendLine("LEFT JOIN Destination d");
                destinationJoin.AppendLine("ON d.CompanyId = @CompanyId");
                destinationJoin.AppendLine("AND d.Id = o.DestinationId");

                destinationGroup.AppendLine(", cs.PostalCode, cs.Address1, cs.Address2, cs.DestinationDepartmentName, cs.CustomerStaffName, cs.Honorific");
                destinationGroup.AppendLine(", d.Id, d.Code, d.Name, d.PostalCode, d.Address1, d.Address2, d.DepartmentName, d.Addressee, d.Honorific");
            }

            var query = $@"
SELECT
  o.OutputAt
, o.OutputNo
, cs.Id      [CustomerId]
, cs.Code    [CustomerCode]
, cs.Name    [CustomerName]
, COUNT(*) BillingCount
, SUM(b.BillingAmount) BillingAmount
, SUM(o.RemainAmount)  RemainAmount
, o.ReminderTemplateId
{destinationSelect.ToString()}
FROM ReminderOutputed o
INNER JOIN Billing b
ON o.BillingId = b.Id
AND b.CompanyId = @CompanyId
INNER JOIN Currency ccy
ON b.CurrencyId = ccy.Id
AND ccy.Code = @CurrencyCode
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
{condition.ToString()}
{destinationJoin.ToString()}
GROUP BY o.OutputAt, o.OutputNo, cs.Id, cs.Code, cs.Name, o.ReminderTemplateId
{destinationGroup.ToString()}
ORDER BY o.OutputAt, o.OutputNo
";
            return dbHelper.GetItemsAsync<ReminderOutputed>(query, search, token);
        }

        public Task<int> GetMaxOutputNoAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT COALESCE(MAX(o.OutputNo), 0)
FROM ReminderOutputed o
INNER JOIN Billing b
ON o.BillingId = b.Id
AND b.CompanyId = @CompanyId
";

            return dbHelper.ExecuteAsync<int>(query, new { CompanyId = companyId }, token);
        }

        public async Task<bool> ExistDestinationAsync(int DestinationId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 FROM ReminderOutputed WHERE DestinationId = @DestinationId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { DestinationId }, token)).HasValue;
        }

    }
}
