using static Rac.VOne.Common.Constants;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReminderQueryProcessor :
        IReminderQueryProcessor,
        IAddReminderQueryProcessor,
        IUpdateReminderQueryProcessor,
        IUpdateBillingReminderQueryProcessor,
        IDeleteReminderSummaryQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReminderQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        public Task<IEnumerable<Reminder>> GetItemsAsync(ReminderSearch search, ReminderCommonSetting setting, IEnumerable<ReminderSummarySetting> summary, CancellationToken token = default(CancellationToken))
        {
            var columns = new StringBuilder();
            var grouping = new StringBuilder();

            columns.AppendLine("  @CompanyId [CompanyId]");
            columns.AppendLine(", ccy.Id [CurrencyId]");
            columns.AppendLine(", ccy.Id [CurrencyId]");
            columns.AppendLine(", ccy.Code [CurrencyCode]");

            grouping.AppendLine("  ccy.Id");
            grouping.AppendLine(", ccy.Code");

            var comma = ", ";

            var baseDate = "";
            if (setting.CalculateBaseDate == (int)CalculateBaseDate.OriginalDueAt) baseDate = "COALESCE(b.OriginalDueAt, b.DueAt)";
            if (setting.CalculateBaseDate == (int)CalculateBaseDate.DueAt)         baseDate = "b.DueAt";
            if (setting.CalculateBaseDate == (int)CalculateBaseDate.BilledAt)      baseDate = "b.BilledAt";
            if (search.ReminderManaged)
            {
                baseDate = $"COALESCE(r.CalculateBaseDate, {baseDate})";
            }

            var arrearsDays = $"DATEDIFF(DAY, {baseDate}, @CalculateBaseDate)";
            if (setting.IncludeOnTheDay == 1) arrearsDays += " + 1";

            foreach (var s in summary.Where(x => x.Available == 1).OrderBy(x => x.DisplayOrder))
            {
                columns.Append(comma);
                grouping.Append(comma);

                switch (s.ColumnName)
                {
                    case "CalculateBaseDate":
                        columns.AppendLine(baseDate + " [CalculateBaseDate]");
                        grouping.AppendLine(baseDate);
                        break;
                    case "CustomerCode":
                        columns.AppendLine("cs.Id [CustomerId]");
                        columns.AppendLine(", cs.Code [CustomerCode]");
                        columns.AppendLine(", cs.Name [CustomerName]");
                        columns.AppendLine(", COUNT(*) [DetailCount]");
                        columns.AppendLine(", SUM(b.RemainAmount) [RemainAmount]");
                        columns.AppendLine(", SUM(CASE WHEN b.ReminderId IS NULL THEN NULL ELSE b.RemainAmount END) [ReminderAmount]");
                        grouping.AppendLine("cs.Id");
                        grouping.AppendLine(", cs.Code");
                        grouping.AppendLine(", cs.Name");

                        if (search.ReminderManaged)
                        {
                            columns.AppendLine($", CASE WHEN b.ReminderId IS NULL THEN NULL ELSE {arrearsDays} END [ArrearsDays]");
                            grouping.AppendLine(", b.ReminderId");
                        }
                        else
                        {
                            columns.AppendLine($", {arrearsDays} [ArrearsDays]");
                        }
                        break;
                    case "ClosingAt":
                        columns.AppendLine("b.ClosingAt");
                        grouping.AppendLine("b.ClosingAt");
                        break;
                    case "InvoiceCode":
                        columns.AppendLine("b.InvoiceCode");
                        grouping.AppendLine("b.InvoiceCode");
                        break;
                    case "CollectCategory":
                        columns.AppendLine("cct.Id [CollectCategoryId]");
                        columns.AppendLine(", cct.Code [CollectCategoryCode]");
                        columns.AppendLine(", cct.Name [CollectCategoryName]");
                        grouping.AppendLine("cct.Id");
                        grouping.AppendLine(", cct.Code");
                        grouping.AppendLine(", cct.Name");
                        break;
                    case "DestinationCode":
                        columns.AppendLine("ds.Id [DestinationId]");
                        columns.AppendLine(", ds.Code [DestinationCode]");
                        columns.AppendLine(", CASE WHEN ds.Id IS NULL THEN ");
                        columns.AppendLine("    IIF (COALESCE(cs.PostalCode,'') <> '','〒' + cs.PostalCode, '') + ' ' + cs.Name + cs.Address1 + cs.Address2 + ' ' + cs.DestinationDepartmentName + ' ' + cs.CustomerStaffName + cs.Honorific");
                        columns.AppendLine("  ELSE");
                        columns.AppendLine("    IIF (COALESCE(ds.PostalCode,'') <> '','〒' + ds.PostalCode, '') + ' ' + ds.Name + ds.Address1 + ds.Address2 + ' ' + ds.DepartmentName + ' ' + ds.Addressee + ds.Honorific");
                        columns.AppendLine("  END [DestinationDisplay]");
                        grouping.AppendLine("ds.Id");
                        grouping.AppendLine(", ds.Code");
                        grouping.AppendLine(", ds.PostalCode");
                        grouping.AppendLine(", ds.Name");
                        grouping.AppendLine(", ds.Address1");
                        grouping.AppendLine(", ds.Address2");
                        grouping.AppendLine(", ds.DepartmentName");
                        grouping.AppendLine(", ds.Addressee");
                        grouping.AppendLine(", ds.Honorific");
                        grouping.AppendLine(", cs.PostalCode");
                        grouping.AppendLine(", cs.Address1");
                        grouping.AppendLine(", cs.Address2");
                        grouping.AppendLine(", cs.DestinationDepartmentName");
                        grouping.AppendLine(", cs.CustomerStaffName");
                        grouping.AppendLine(", cs.Honorific");
                        break;
                    case "Department":
                        columns.AppendLine("d.Id [DepartmentId]");
                        columns.AppendLine(", d.Code [DepartmentCode]");
                        columns.AppendLine(", d.Name [DepartmentName]");
                        grouping.AppendLine("d.Id");
                        grouping.AppendLine(", d.Code");
                        grouping.AppendLine(", d.Name");
                        break;
                    case "Staff":
                        columns.AppendLine("s.Id [StaffId]");
                        columns.AppendLine(", s.Code [StaffCode]");
                        columns.AppendLine(", s.Name [StaffName]");
                        grouping.AppendLine("s.Id");
                        grouping.AppendLine(", s.Code");
                        grouping.AppendLine(", s.Name");
                        break;
                }

                comma = ", ";
            }

            if (search.ReminderManaged)
            {
                columns.AppendLine(", r.Id");
                columns.AppendLine(", r.Memo");
                columns.AppendLine(", r.OutputAt");
                columns.AppendLine(", st.Id [StatusId]");
                columns.AppendLine(", st.Code [StatusCode]");
                columns.AppendLine(", st.Name [StatusName]");
                columns.AppendLine(", cs.CustomerStaffName");
                columns.AppendLine(", cs.Note [CustomerNote]");
                columns.AppendLine(", cs.Tel  [CustomerTel]");
                columns.AppendLine(", cs.ExcludeReminderPublish");
                grouping.AppendLine(", r.Id");
                grouping.AppendLine(", r.OutputAt");
                grouping.AppendLine(", r.Memo");
                grouping.AppendLine(", st.Id");
                grouping.AppendLine(", st.Code");
                grouping.AppendLine(", st.Name");
                grouping.AppendLine(", cs.CustomerStaffName");
                grouping.AppendLine(", cs.Note");
                grouping.AppendLine(", cs.Tel");
                grouping.AppendLine(", cs.ExcludeReminderPublish");
            }

            var conditions = new StringBuilder();
            if (search.ExistsMemo)
                conditions.AppendLine("AND bm.Memo IS NOT NULL");
            if (!string.IsNullOrEmpty(search.BillingMemo))
            {
                search.BillingMemo = Sql.GetWrappedValue(search.BillingMemo);
                conditions.AppendLine("AND bm.Memo LIKE @BillingMemo COLLATE JAPANESE_CI_AS");
            }
            if (search.ArrearDaysFrom.HasValue || search.ArrearDaysTo.HasValue)
            {
                var arrearDaysCondition = new StringBuilder();

                arrearDaysCondition.AppendLine("b.ReminderId IS NULL");
                arrearDaysCondition.AppendLine("OR (b.ReminderId IS NOT NULL");

                if (search.ArrearDaysFrom.HasValue)
                    arrearDaysCondition.AppendLine($"AND {arrearsDays} >= @ArrearDaysFrom");
                if (search.ArrearDaysTo.HasValue)
                    arrearDaysCondition.AppendLine($"AND {arrearsDays} <= @ArrearDaysTo");
                arrearDaysCondition.AppendLine(")");

                conditions.AppendLine($"AND ({arrearDaysCondition.ToString()})");
            }
            if (!string.IsNullOrEmpty(search.DepartmentCodeFrom))
                conditions.AppendLine("AND d.Code >= @DepartmentCodeFrom");
            if (!string.IsNullOrEmpty(search.DepartmentCodeTo))
                conditions.AppendLine("AND d.Code <= @DepartmentCodeTo");
            if (!string.IsNullOrEmpty(search.StaffCodeFrom))
                conditions.AppendLine("AND s.Code >= @StaffCodeFrom");
            if (!string.IsNullOrEmpty(search.StaffCodeTo))
                conditions.AppendLine("AND s.Code <= @StaffCodeTo");
            if (!string.IsNullOrEmpty(search.CustomerCodeFrom))
                conditions.AppendLine("AND cs.Code >= @CustomerCodeFrom");
            if (!string.IsNullOrEmpty(search.CustomerCodeTo))
                conditions.AppendLine("AND cs.Code <= @CustomerCodeTo");
            if (search.ReminderManaged)
            {
                //conditions.AppendLine("AND b.ReminderId IS NOT NULL");
            }
            else
            {
                conditions.AppendLine($"AND {arrearsDays} > 0");
                conditions.AppendLine("AND b.ReminderId IS NULL");
                conditions.AppendLine("AND b.RemainAmount <> 0");
                conditions.AppendLine($"AND b.InputType <> {(int)BillingInputType.CashOnDueDate}");
                conditions.AppendLine("AND (cct.UseAccountTransfer = 0 OR cct.UseAccountTransfer = 1 AND b.ResultCode = 0)");
            }
                
            if (search.OutputFlag != null)
            {
                conditions.AppendLine($"AND r.OutputAt IS {(search.OutputFlag == 0 ? "" : "NOT")} NULL");
            }
            if (search.Status != 0)
            {
                if (search.Status < 0)
                    conditions.AppendLine("AND st.Completed = 0");
                else
                    conditions.AppendLine("AND st.Id = @Status");
            }
            if (search.RemoveExcludeReminderPublishCustomer)
            {
                conditions.AppendLine("AND cs.ExcludeReminderPublish = 0");
            }

            var having = new StringBuilder();
            var flags = (AssignmentFlagChecked)search.AssignmentFlg;
            
            if (!flags.HasFlag(AssignmentFlagChecked.All))
            {
                if (flags.HasFlag(AssignmentFlagChecked.NoAssignment))
                {
                    having.Append(having.Length == 0 ? "HAVING ( " : "OR ");
                    having.AppendLine("SUM(b.RemainAmount) = SUM(b.BillingAmount)");
                }
                if (flags.HasFlag(AssignmentFlagChecked.PartAssignment))
                {
                    having.Append(having.Length == 0 ? "HAVING ( " : "OR ");
                    having.AppendLine("(SUM(b.RemainAmount) <> SUM(b.BillingAmount) AND SUM(b.RemainAmount) <> 0)");
                }
                if (flags.HasFlag(AssignmentFlagChecked.FullAssignment))
                {
                    having.Append(having.Length == 0 ? "HAVING ( " : "OR ");
                    having.AppendLine("SUM(b.RemainAmount) = 0");
                }
                if (having.Length > 0) having.AppendLine(") ");
            }

            var order = new StringBuilder();
            order.AppendLine("ORDER BY");
            if (search.ReminderManaged)
            {
                order.AppendLine("  CASE WHEN r.Id IS NULL THEN 1 ELSE 0 END ASC");
                order.AppendLine($", {baseDate}");
            }
            else
            {
                order.AppendLine("  cs.Code");
            }

            var query = $@"
SELECT
{columns.ToString()}
FROM Billing b
INNER JOIN Currency ccy
ON b.CompanyId = @CompanyId
AND b.CompanyId = ccy.CompanyId
AND b.CurrencyId = ccy.Id
AND ccy.Code = @CurrencyCode
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
INNER JOIN Department d
ON b.DepartmentId = d.Id
INNER JOIN Staff s
ON b.StaffId = s.Id
INNER JOIN Category cct
ON b.CollectCategoryId = cct.Id
LEFT JOIN Destination ds
ON b.DestinationId = ds.Id
LEFT JOIN BillingMemo bm
ON b.Id = bm.BillingId
LEFT JOIN Reminder r
ON b.ReminderId = r.Id
LEFT JOIN StatusMaster st
ON r.StatusId = st.Id

WHERE b.DeleteAt IS NULL
{conditions.ToString()}
GROUP BY
{grouping.ToString()}
{having.ToString()}
{order.ToString()}
";
            return dbHelper.GetItemsAsync<Reminder>(query, search, token);
        }

        public Task<IEnumerable<ReminderSummary>> GetSummaryItemsAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken))
        {
            var conditions = new StringBuilder();
            var having = new StringBuilder();

            if (!string.IsNullOrEmpty(search.CustomerCodeFrom))
                conditions.AppendLine("AND cs.Code >= @CustomerCodeFrom");
            if (!string.IsNullOrEmpty(search.CustomerCodeTo))
                conditions.AppendLine("AND cs.Code <= @CustomerCodeTo");
            if (!string.IsNullOrEmpty(search.CustomerName))
            {
                search.CustomerName = Sql.GetWrappedValue(search.CustomerName);
                conditions.AppendLine("AND cs.Name LIKE @CustomerName");
            }

            if (search.ArrearDaysFrom.HasValue || search.ArrearDaysTo.HasValue)
            {
                var baseDate = "";
                var arrearDaysCondition = new StringBuilder();

                if (setting.CalculateBaseDate == (int)CalculateBaseDate.OriginalDueAt) baseDate = "COALESCE(b.OriginalDueAt, b.DueAt)";
                if (setting.CalculateBaseDate == (int)CalculateBaseDate.DueAt)         baseDate = "b.DueAt";
                if (setting.CalculateBaseDate == (int)CalculateBaseDate.BilledAt)      baseDate = "b.BilledAt";

                var arrearDays = $"DATEDIFF(DAY, {baseDate}, @CalculateBaseDate)";
                if (setting.IncludeOnTheDay == 1) arrearDays += " + 1";

                arrearDaysCondition.AppendLine("b.ReminderId IS NULL");
                arrearDaysCondition.AppendLine("OR (b.ReminderId IS NOT NULL");

                if (search.ArrearDaysFrom.HasValue)
                    arrearDaysCondition.AppendLine($"AND {arrearDays} >= @ArrearDaysFrom");

                if (search.ArrearDaysTo.HasValue)
                    arrearDaysCondition.AppendLine($"AND {arrearDays} <= @ArrearDaysTo");

                arrearDaysCondition.AppendLine(")");
                conditions.AppendLine($"AND ({arrearDaysCondition.ToString()})");
            }

            if (!search.ContainReminderAmountZero)
            {
                having.AppendLine("HAVING SUM(CASE WHEN b.ReminderId IS NULL THEN NULL ELSE b.RemainAmount END) <> 0");
            }

            if (search.RemoveExcludeReminderPublishCustomer)
            {
                conditions.AppendLine("AND cs.ExcludeReminderPublish = 0");
            }

            var query = $@"
SELECT 
  rs.Id
, cs.Id   CustomerId
, cs.Code CustomerCode
, cs.Name CustomerName
, COUNT(DISTINCT ReminderId) ReminderCount
, SUM(CASE WHEN b.ReminderId IS NULL THEN 0 ELSE 1 END) BillingCount
, ccy.Id   CurrencyId
, ccy.Code CurrencyCode
, SUM(b.RemainAmount) RemainAmount
, SUM(CASE WHEN b.ReminderId IS NULL THEN NULL ELSE b.RemainAmount END) ReminderAmount
, rs.Memo
, cs.DestinationDepartmentName
, cs.CustomerStaffName
, cs.Note CustomerNote
, cs.Tel  CustomerTel
, cs.Fax  CustomerFax
, cs.ExcludeReminderPublish
FROM Billing b
INNER JOIN Currency ccy
ON b.CurrencyId = ccy.Id
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
INNER JOIN ReminderSummary rs
ON cs.Id = rs.CustomerId
AND ccy.Id = rs.CurrencyId
WHERE b.CompanyId = @CompanyId
AND ccy.Code = @CurrencyCode
AND b.DeleteAt IS NULL
{conditions.ToString()}
GROUP BY rs.Id, cs.Id, cs.Code, cs.Name, ccy.Id, ccy.Code, cs.CustomerStaffName, rs.Memo, cs.Note, cs.Tel, cs.Fax, cs.ExcludeReminderPublish, cs.DestinationDepartmentName
{having.ToString()}
ORDER BY cs.Code
";
            return dbHelper.GetItemsAsync<ReminderSummary>(query, search, token);
        }

        public Task<IEnumerable<ReminderSummary>> GetSummaryItemsByDestinationAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken))
        {
            var conditions = new StringBuilder();
            var having = new StringBuilder();

            if (!string.IsNullOrEmpty(search.CustomerCodeFrom))
                conditions.AppendLine("AND cs.Code >= @CustomerCodeFrom");
            if (!string.IsNullOrEmpty(search.CustomerCodeTo))
                conditions.AppendLine("AND cs.Code <= @CustomerCodeTo");
            if (!string.IsNullOrEmpty(search.CustomerName))
            {
                search.CustomerName = Sql.GetWrappedValue(search.CustomerName);
                conditions.AppendLine("AND cs.Name LIKE @CustomerName");
            }

            if (search.ArrearDaysFrom.HasValue || search.ArrearDaysTo.HasValue)
            {
                var baseDate = "";
                var arrearDaysCondition = new StringBuilder();

                if (setting.CalculateBaseDate == (int)CalculateBaseDate.OriginalDueAt) baseDate = "COALESCE(b.OriginalDueAt, b.DueAt)";
                if (setting.CalculateBaseDate == (int)CalculateBaseDate.DueAt) baseDate = "b.DueAt";
                if (setting.CalculateBaseDate == (int)CalculateBaseDate.BilledAt) baseDate = "b.BilledAt";

                var arrearDays = $"DATEDIFF(DAY, {baseDate}, @CalculateBaseDate)";
                if (setting.IncludeOnTheDay == 1) arrearDays += " + 1";

                arrearDaysCondition.AppendLine("b.ReminderId IS NULL");
                arrearDaysCondition.AppendLine("OR (b.ReminderId IS NOT NULL");

                if (search.ArrearDaysFrom.HasValue)
                    arrearDaysCondition.AppendLine($"AND {arrearDays} >= @ArrearDaysFrom");

                if (search.ArrearDaysTo.HasValue)
                    arrearDaysCondition.AppendLine($"AND {arrearDays} <= @ArrearDaysTo");

                arrearDaysCondition.AppendLine(")");
                conditions.AppendLine($"AND ({arrearDaysCondition.ToString()})");
            }

            if (!search.ContainReminderAmountZero)
            {
                having.AppendLine("HAVING SUM(CASE WHEN b.ReminderId IS NULL THEN NULL ELSE b.RemainAmount END) <> 0");
            }

            if (search.RemoveExcludeReminderPublishCustomer)
            {
                conditions.AppendLine("AND cs.ExcludeReminderPublish = 0");
            }

            var query = $@"
SELECT 
  rs.Id
, cs.Id   CustomerId
, cs.Code CustomerCode
, cs.Name CustomerName
, COUNT(DISTINCT ReminderId) ReminderCount
, SUM(CASE WHEN b.ReminderId IS NULL THEN 0 ELSE 1 END) BillingCount
, ccy.Id   CurrencyId
, ccy.Code CurrencyCode
, SUM(b.RemainAmount) RemainAmount
, SUM(CASE WHEN b.ReminderId IS NULL THEN NULL ELSE b.RemainAmount END) ReminderAmount
, rs.Memo
, cs.CustomerStaffName
, cs.Note CustomerNote
, cs.Tel  CustomerTel
, cs.Fax  CustomerFax
, cs.ExcludeReminderPublish
, ds.Id   [DestinationId]
, ds.Code [DestinationCode]
FROM Billing b
INNER JOIN Currency ccy
ON b.CurrencyId = ccy.Id
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
INNER JOIN ReminderSummary rs
ON cs.Id = rs.CustomerId
AND ccy.Id = rs.CurrencyId
LEFT JOIN Destination ds
ON b.DestinationId = ds.Id
WHERE b.CompanyId = @CompanyId
AND ccy.Code = @CurrencyCode
AND b.DeleteAt IS NULL
{conditions.ToString()}
GROUP BY rs.Id, cs.Id, cs.Code, cs.Name, ccy.Id, ccy.Code, cs.CustomerStaffName, rs.Memo, cs.Note, cs.Tel, cs.Fax, cs.ExcludeReminderPublish, ds.Id, ds.Code
{having.ToString()}
ORDER BY cs.Code
";
            return dbHelper.GetItemsAsync<ReminderSummary>(query, search, token);
        }

        public Task<Reminder> AddAsync(Reminder Reminder, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
INSERT INTO Reminder (
  CompanyId
, CurrencyId
, CustomerId
, CalculateBaseDate
, StatusId
, Memo
, OutputAt
)
OUTPUT inserted.*
  VALUES (
  @CompanyId
, @CurrencyId
, @CustomerId
, @CalculateBaseDate
, @StatusId
, ''
, NULL
)
";
            return dbHelper.ExecuteAsync<Reminder>(query, Reminder, token);
        }

        public Task<ReminderSummary> AddSummaryAsync(ReminderSummary ReminderSummary, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
IF NOT EXISTS(
SELECT *
FROM ReminderSummary
WHERE CustomerId = @CustomerId
AND CurrencyId = @CurrencyId
)
INSERT INTO ReminderSummary (
  CustomerId
, CurrencyId
, Memo
)
OUTPUT inserted.*
  VALUES (
  @CustomerId
, @CurrencyId
, @Memo
)
";
            return dbHelper.ExecuteAsync<ReminderSummary>(query, ReminderSummary, token);
        }

        public Task<IEnumerable<Billing>> UpdateAsync(Reminder reminder, ReminderCommonSetting setting, IEnumerable<ReminderSummarySetting> summary, CancellationToken token = default(CancellationToken))
        {
            var baseDate = "COALESCE(OriginalDueAt, DueAt)";
            if (setting.CalculateBaseDate == (int)CalculateBaseDate.DueAt)    baseDate = "DueAt";
            if (setting.CalculateBaseDate == (int)CalculateBaseDate.BilledAt) baseDate = "BilledAt";

            var condition = new StringBuilder();

            foreach(var s in summary.Where(x => x.Available == 1))
            {
                if (s.ColumnName == "CalculateBaseDate" || s.ColumnName == "CustomerCode" || s.ColumnName == "CurrencyCode")
                    continue;

                var c = "AND ";
                if (s.ColumnName == "ClosingAt")       c += "ClosingAt = @ClosingAt";
                if (s.ColumnName == "InvoiceCode")     c += "InvoiceCode = @InvoiceCode";
                if (s.ColumnName == "CollectCategory") c += "CollectCategoryId = @CollectCategoryId";
                if (s.ColumnName == "DestinationCode") c += "COALESCE(DestinationId, 0) = COALESCE(@DestinationId, 0)";
                if (s.ColumnName == "Department")      c += "DepartmentId = @DepartmentId";
                if (s.ColumnName == "Staff")           c += "StaffId = @StaffId";

                condition.AppendLine(c);
            }

            var query = $@"
UPDATE Billing
   SET ReminderId = @Id
OUTPUT Inserted.*
 WHERE CustomerId = @CustomerId
   AND {baseDate} = @CalculateBaseDate
   AND CurrencyId = @CurrencyId
   AND ReminderId IS NULL
   AND DeleteAt IS NULL
   AND RemainAmount <> 0
   {condition}
";
            return dbHelper.GetItemsAsync<Billing>(query, reminder, token);
        }

        public Task<int> CancelAsync(int reminderId, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
 UPDATE Billing
    SET ReminderId = NULL
 WHERE ReminderId = @reminderId
";
            return dbHelper.ExecuteAsync(query, new { reminderId}, token);
        }

        public Task<Reminder> UpdateStatusAsync(Reminder Reminder, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Reminder
   SET StatusId = @StatusId
     , Memo = @Memo
OUTPUT inserted.*
 WHERE Id = @Id
";
            return dbHelper.ExecuteAsync<Reminder>(query, Reminder, token);
        }

        public Task<ReminderSummary> UpdateSummaryStatusAsync(ReminderSummary ReminderSummary, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE ReminderSummary
   SET Memo = @Memo
OUTPUT inserted.*
 WHERE Id = @Id
";
            return dbHelper.ExecuteAsync<ReminderSummary>(query, ReminderSummary, token);
        }

        public Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForPrintAsync(int companyId, IEnumerable<int> reminderIds, CancellationToken token = default(CancellationToken))
        {
            var order = "ORDER BY b.SalesAt, b.Id ASC";
            var query = $@"
SELECT
  ROW_NUMBER() OVER ({order}) RowNumber
, b.Id
, b.ReminderId
, b.CurrencyId
, b.CustomerId
, cs.Code       [CustomerCode]
, cs.PostalCode [CustomerPostalCode]
, cs.Address1   [CustomerAddress1]
, cs.Address2   [CustomerAddress2]
, cs.Name       [CustomerName]
, cs.CustomerStaffName
, cs.ReceiveAccountId1 [CustomerReceiveAccount1]
, cs.ReceiveAccountId2 [CustomerReceiveAccount2]
, cs.ReceiveAccountId3 [CustomerReceiveAccount3]
, b.BilledAt
, b.SalesAt
, b.DueAt
, COALESCE(b.OriginalDueAt, b.DueAt) [OriginalDueAt]
, b.Note1
, b.Note2
, b.Note3
, b.Note4
, b.Note5
, b.Note6
, b.Note7
, b.Note8
, b.BillingAmount
, b.RemainAmount
, s.Name [StaffName]
, s.Tel  [StaffTel]
, s.Fax  [StaffFax]
, cs.DestinationDepartmentName [DestinationDepartmentName]
, cs.CustomerStaffName         [DestinationAddressee]
, cs.Honorific                 [DestinationHonorific]
FROM Billing b
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
AND b.CompanyId = @CompanyId
AND b.ReminderId IN (SELECT Id FROM @ReminderIds)
AND b.DeleteAt IS NULL
AND b.RemainAmount <> 0
INNER JOIN Staff s
ON b.StaffId = s.Id
{order}
";
            return dbHelper.GetItemsAsync<ReminderBilling>(query, new {
                CompanyId = companyId,
                ReminderIds = reminderIds.GetTableParameter()
            }, token);
        }

        public Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForPrintByDestinationCodeAsync(Reminder reminder, CancellationToken token = default(CancellationToken))
        {
            var conditions = new StringBuilder();
            if (!reminder.DestinationIds.Any())
            {
                conditions.AppendLine("AND d.Id IS NULL");
                conditions.AppendLine("AND b.ReminderId IN (SELECT Id FROM @Ids)");
            }
            else if (!reminder.NoDestination && reminder.DestinationIds.Any())
                conditions.AppendLine("AND d.Id IN (SELECT Id FROM @DestinationIds)");
            else
                conditions.AppendLine("AND (d.Id IN (SELECT Id FROM @DestinationIds) OR d.Id IS NULL)");

            var order = "ORDER BY b.SalesAt, b.Id ASC";
            var query = $@"
SELECT
  ROW_NUMBER() OVER ({order}) RowNumber
, b.Id
, b.ReminderId
, b.CurrencyId
, b.CustomerId
, cs.Code       [CustomerCode]
, cs.PostalCode [CustomerPostalCode]
, cs.Address1   [CustomerAddress1]
, cs.Address2   [CustomerAddress2]
, cs.Name       [CustomerName]
, cs.CustomerStaffName
, cs.ReceiveAccountId1 [CustomerReceiveAccount1]
, cs.ReceiveAccountId2 [CustomerReceiveAccount2]
, cs.ReceiveAccountId3 [CustomerReceiveAccount3]
, b.BilledAt
, b.SalesAt
, b.DueAt
, COALESCE(b.OriginalDueAt, b.DueAt) [OriginalDueAt]
, b.Note1
, b.Note2
, b.Note3
, b.Note4
, b.Note5
, b.Note6
, b.Note7
, b.Note8
, b.BillingAmount
, b.RemainAmount
, s.Name [StaffName]
, s.Tel  [StaffTel]
, s.Fax  [StaffFax]
, ds.Id             [DestinationId]
, ds.Name           [DestinationName]
, ds.PostalCode     [DestinationPostalCode]
, ds.Address1       [DestinationAddress1]
, ds.Address2       [DestinationAddress2]
, COALESCE(ds.DepartmentName, cs.DestinationDepartmentName) [DestinationDepartmentName]
, COALESCE(ds.Addressee, cs.CustomerStaffName)              [DestinationAddressee]
, COALESCE(ds.Honorific, cs.Honorific)                      [DestinationHonorific]
FROM Billing b
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
AND b.CompanyId = @CompanyId
AND b.DeleteAt IS NULL
AND b.RemainAmount <> 0
INNER JOIN Staff s
ON b.StaffId = s.Id
LEFT JOIN Destination d
ON b.DestinationId = d.Id
LEFT JOIN Destination ds
ON ds.Id = @DestinationIdInput
WHERE b.CustomerId = @CustomerIds
{conditions}
{order}
";
            return dbHelper.GetItemsAsync<ReminderBilling>(query, new
            {
                CompanyId = reminder.CompanyId,
                Ids = reminder.Ids.GetTableParameter(),
                DestinationIdInput = reminder.DestinationIdInput,
                CustomerIds = reminder.CustomerId,
                DestinationIds = reminder.DestinationIds.GetTableParameter(),
            }, token);
        }

        public Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForSummaryPrintAsync(int companyId, IEnumerable<int> customerIds, CancellationToken token = default(CancellationToken))
        {
            var order = "ORDER BY b.CustomerId, b.SalesAt, b.Id ASC";
            var query = $@"
SELECT 
  ROW_NUMBER() OVER (PARTITION BY b.CustomerId {order}) RowNumber
, b.Id
, b.ReminderId
, b.CurrencyId
, b.CustomerId
, cs.Code       [CustomerCode]
, cs.PostalCode [CustomerPostalCode]
, cs.Address1   [CustomerAddress1]
, cs.Address2   [CustomerAddress2]
, cs.Name       [CustomerName]
, cs.CustomerStaffName
, cs.ReceiveAccountId1 [CustomerReceiveAccount1]
, cs.ReceiveAccountId2 [CustomerReceiveAccount2]
, cs.ReceiveAccountId3 [CustomerReceiveAccount3]
, b.BilledAt
, b.SalesAt
, b.DueAt
, COALESCE(b.OriginalDueAt, b.DueAt) [OriginalDueAt]
, b.Note1
, b.Note2
, b.Note3
, b.Note4
, b.Note5
, b.Note6
, b.Note7
, b.Note8
, b.BillingAmount
, b.RemainAmount
, s.Name [StaffName]
, s.Tel  [StaffTel]
, s.Fax  [StaffFax]
, cs.DestinationDepartmentName [DestinationDepartmentName]
, cs.CustomerStaffName         [DestinationAddressee]
, cs.Honorific                 [DestinationHonorific]
FROM Billing b
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
AND b.CompanyId = @CompanyId
AND b.CustomerId IN (SELECT Id FROM @CustomerIds)
AND b.ReminderId IS NOT NULL
AND b.DeleteAt IS NULL
AND b.RemainAmount <> 0
INNER JOIN Staff s
ON b.StaffId = s.Id
{order}
";
            return dbHelper.GetItemsAsync<ReminderBilling>(query, new {
                CompanyId = companyId,
                CustomerIds = customerIds.GetTableParameter(),
            }, token);
        }

        public Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForSummaryPrintByDestinationCodeAsync(ReminderSummary reminderSummary, CancellationToken token = default(CancellationToken))
        {
            var conditions = new StringBuilder();
            if (!reminderSummary.DestinationIds.Any())
                conditions.AppendLine("AND d.Id IS NULL");
            else if (!reminderSummary.NoDestination && reminderSummary.DestinationIds.Any())
                conditions.AppendLine("AND d.Id IN (SELECT Id FROM @DestinationIds)");
            else
                conditions.AppendLine("AND (d.Id IN (SELECT Id FROM @DestinationIds) OR d.Id IS NULL)");

            var order = "ORDER BY b.CustomerId, b.SalesAt, b.Id ASC";
            var query = $@"
SELECT 
  ROW_NUMBER() OVER (PARTITION BY b.CustomerId {order}) RowNumber
, b.Id
, b.ReminderId
, b.CurrencyId
, b.CustomerId
, cs.Code       [CustomerCode]
, cs.PostalCode [CustomerPostalCode]
, cs.Address1   [CustomerAddress1]
, cs.Address2   [CustomerAddress2]
, cs.Name       [CustomerName]
, cs.CustomerStaffName
, cs.ReceiveAccountId1 [CustomerReceiveAccount1]
, cs.ReceiveAccountId2 [CustomerReceiveAccount2]
, cs.ReceiveAccountId3 [CustomerReceiveAccount3]
, b.BilledAt
, b.SalesAt
, b.DueAt
, COALESCE(b.OriginalDueAt, b.DueAt) [OriginalDueAt]
, b.Note1
, b.Note2
, b.Note3
, b.Note4
, b.Note5
, b.Note6
, b.Note7
, b.Note8
, b.BillingAmount
, b.RemainAmount
, s.Name [StaffName]
, s.Tel  [StaffTel]
, s.Fax  [StaffFax]
, ds.Id             [DestinationId]
, ds.Name           [DestinationName]
, ds.PostalCode     [DestinationPostalCode]
, ds.Address1       [DestinationAddress1]
, ds.Address2       [DestinationAddress2]
, COALESCE(ds.DepartmentName, cs.DestinationDepartmentName) [DestinationDepartmentName]
, COALESCE(ds.Addressee, cs.CustomerStaffName)              [DestinationAddressee]
, COALESCE(ds.Honorific, cs.Honorific)                      [DestinationHonorific]
FROM Billing b
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
AND b.CompanyId = @CompanyId
AND b.ReminderId IS NOT NULL
AND b.DeleteAt IS NULL
AND b.RemainAmount <> 0
INNER JOIN Staff s
ON b.StaffId = s.Id
LEFT JOIN Destination d
ON b.DestinationId = d.Id
LEFT JOIN Destination ds
ON ds.Id = @DestinationIdInput
WHERE b.CustomerId IN (SELECT Id FROM @CustomerIds)
{conditions}
{order}
";
            return dbHelper.GetItemsAsync<ReminderBilling>(query, new {
                CompanyId = reminderSummary.CompanyId,
                DestinationIdInput = reminderSummary.DestinationIdInput,
                CustomerIds = reminderSummary.CustomerIds.GetTableParameter(),
                DestinationIds = reminderSummary.DestinationIds.GetTableParameter(),
            }, token);
        }

        public Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForReprintAsync(int companyId, ReminderOutputed reminderOutputed, CancellationToken token = default(CancellationToken))
        {
            var order = "ORDER BY ro.OutputNo, b.SalesAt, b.Id ASC";
            var query = $@"
SELECT 
  ROW_NUMBER() OVER (PARTITION BY ro.OutputNo {order}) RowNumber
, b.Id
, b.ReminderId
, b.CurrencyId
, b.CustomerId
, cs.Code       [CustomerCode]
, cs.PostalCode [CustomerPostalCode]
, cs.Address1   [CustomerAddress1]
, cs.Address2   [CustomerAddress2]
, cs.Name       [CustomerName]
, cs.CustomerStaffName
, cs.ReceiveAccountId1 [CustomerReceiveAccount1]
, cs.ReceiveAccountId2 [CustomerReceiveAccount2]
, cs.ReceiveAccountId3 [CustomerReceiveAccount3]
, b.BilledAt
, b.SalesAt
, b.DueAt
, COALESCE(b.OriginalDueAt, b.DueAt) [OriginalDueAt]
, b.Note1
, b.Note2
, b.Note3
, b.Note4
, b.Note5
, b.Note6
, b.Note7
, b.Note8
, b.BillingAmount
, ro.RemainAmount
, s.Name [StaffName]
, s.Tel  [StaffTel]
, s.Fax  [StaffFax]
, ro.OutputNo
, cs.DestinationDepartmentName [DestinationDepartmentName]
, cs.CustomerStaffName         [DestinationAddressee]
, cs.Honorific                 [DestinationHonorific]
FROM Billing b
INNER JOIN ReminderOutputed ro
ON b.Id = ro.BillingId
AND ro.OutputNo IN (SELECT Id FROM @OutputNos)
AND b.CompanyId = @CompanyId
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
INNER JOIN Staff s
ON b.StaffId = s.Id
{order}
";
            return dbHelper.GetItemsAsync<ReminderBilling>(query, new {
                CompanyId = companyId,
                OutputNos = reminderOutputed.OutputNos.GetTableParameter()
            }, token);
        }

        public Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForReprintByDestinationAsync(int companyId, ReminderOutputed reminderOutputed, CancellationToken token = default(CancellationToken))
        {
            var order = "ORDER BY ro.OutputNo, b.SalesAt, b.Id ASC";
            var query = $@"
SELECT 
  ROW_NUMBER() OVER (PARTITION BY ro.OutputNo {order}) RowNumber
, b.Id
, b.ReminderId
, b.CurrencyId
, b.CustomerId
, cs.Code       [CustomerCode]
, cs.PostalCode [CustomerPostalCode]
, cs.Address1   [CustomerAddress1]
, cs.Address2   [CustomerAddress2]
, cs.Name       [CustomerName]
, cs.CustomerStaffName
, cs.ReceiveAccountId1 [CustomerReceiveAccount1]
, cs.ReceiveAccountId2 [CustomerReceiveAccount2]
, cs.ReceiveAccountId3 [CustomerReceiveAccount3]
, b.BilledAt
, b.SalesAt
, b.DueAt
, COALESCE(b.OriginalDueAt, b.DueAt) [OriginalDueAt]
, b.Note1
, b.Note2
, b.Note3
, b.Note4
, b.Note5
, b.Note6
, b.Note7
, b.Note8
, b.BillingAmount
, ro.RemainAmount
, s.Name [StaffName]
, s.Tel  [StaffTel]
, s.Fax  [StaffFax]
, ro.OutputNo
, ds.Id             [DestinationId]
, ds.Name           [DestinationName]
, ds.PostalCode     [DestinationPostalCode]
, ds.Address1       [DestinationAddress1]
, ds.Address2       [DestinationAddress2]
, COALESCE(ds.DepartmentName, cs.DestinationDepartmentName) [DestinationDepartmentName]
, COALESCE(ds.Addressee, cs.CustomerStaffName)              [DestinationAddressee]
, COALESCE(ds.Honorific, cs.Honorific)                      [DestinationHonorific]
FROM Billing b
INNER JOIN ReminderOutputed ro
ON b.Id = ro.BillingId
AND ro.OutputNo IN (SELECT Id FROM @OutputNo)
AND b.CompanyId = @CompanyId
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
INNER JOIN Staff s
ON b.StaffId = s.Id
LEFT JOIN Destination ds
ON ds.CompanyId = @CompanyId
AND ds.Id = ro.DestinationId
AND ds.Id In (SELECT Id FROM @DestinationIds)
{order}
";
            return dbHelper.GetItemsAsync<ReminderBilling>(query, new
            {
                CompanyId = companyId,
                OutputNo = reminderOutputed.OutputNos.GetTableParameter(),
                DestinationIds = reminderOutputed.DestinationIds.GetTableParameter(),
            }, token);
        }

        public Task<IEnumerable<ReminderHistory>> GetReminderListAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken))
        {
            var builder = new StringBuilder();
            if (search.ReminderManaged)
            {

                builder.Append(@"
SELECT rh.Id
     , r.Id    AS ReminderId
     , cs.Code AS CustomerCode
     , cs.Name AS CustomerName
     , cc.Code AS CurrencyCode
     , r.CalculateBaseDate
     , rh.ReminderAmount
     , CASE WHEN r.Id IS NULL THEN NULL
            ELSE DATEDIFF(DAY, r.CalculateBaseDate, @CalculateBaseDate)
        END AS ArrearsDays
     , rh.InputType
     , st.Code AS StatusCode
     , st.Name AS StatusName
     , rh.Memo
     , rh.OutputFlag
     , rh.CreateAt
     , lu.Name AS CreateByName
  FROM ReminderHistory rh
  LEFT JOIN Reminder r
    ON rh.ReminderId = r.Id
  LEFT JOIN Customer cs
    ON r.CustomerId = cs.Id
  LEFT JOIN StatusMaster st
    ON rh.StatusId = st.Id
  LEFT JOIN LoginUser lu
    ON rh.CreateBy = lu.Id
  LEFT JOIN Currency cc
    ON r.CurrencyId = cc.Id
 WHERE r.CompanyId = @CompanyId
   AND cc.Code     = @CurrencyCode
");
            }
            else
            {
                builder.Append(@"
SELECT rh.Id
     , rh.ReminderSummaryId    AS ReminderId
     , cs.Code AS CustomerCode
     , cs.Name AS CustomerName
     , cc.Code AS CurrencyCode
     , rh.ReminderAmount
     , rh.InputType
     , rh.Memo
     , rh.CreateAt
     , lu.Name AS CreateByName
      from ReminderSummaryHistory rh
 left join ReminderSummary rs
   on rs.Id = rh.ReminderSummaryId
 left join Customer cs
   on cs.Id = rs.CustomerId
 left join Currency cc
   on cc.Id = rs.CurrencyId
 left join LoginUser lu
   on lu.Id = rh.CreateBy
 WHERE cs.CompanyId = @CompanyId
   AND cc.Code     = @CurrencyCode
");
            }
            if (search.CreateAtFrom.HasValue)
                builder.AppendLine("  AND rh.CreateAt >= @CreateAtFrom");
            if (search.CreateAtTo.HasValue)
                builder.AppendLine("  AND rh.CreateAt <= @CreateAtTo");
            if(!string.IsNullOrEmpty(search.CreateByCode))
                builder.AppendLine("  AND lu.Code = @CreateByCode");
            if (!string.IsNullOrEmpty(search.CustomerCodeFrom))
                builder.AppendLine("  AND cs.Code >= @CustomerCodeFrom");
            if (!string.IsNullOrEmpty(search.CustomerCodeTo))
                builder.AppendLine("  AND cs.Code <= @CustomerCodeTo");
            if (!string.IsNullOrWhiteSpace(search.ReminderMemo))
            {
                search.ReminderMemo = Sql.GetWrappedValue(search.ReminderMemo);
                builder.AppendLine("AND rh.Memo LIKE @ReminderMemo");
            }

            if (search.ReminderManaged)
            {
                var arrearsDays = "DATEDIFF(DAY, r.CalculateBaseDate, @CalculateBaseDate)";
                if (setting.IncludeOnTheDay == 1) arrearsDays += " + 1";

                if (search.ArrearDaysFrom.HasValue)
                    builder.AppendLine($"AND {arrearsDays} >= @ArrearDaysFrom");
                if (search.ArrearDaysTo.HasValue)
                    builder.AppendLine($"AND {arrearsDays} <= @ArrearDaysTo");
                if (search.OutputFlag.HasValue)
                    builder.AppendLine($"AND r.OutputAt IS {(search.OutputFlag == 0 ? "" : "NOT")} NULL");

                if (search.Status != 0)
                {
                    if (search.Status < 0)
                        builder.AppendLine("AND st.Completed = 0");
                    else
                        builder.AppendLine("AND st.Id = @Status");
                }

                //消込フラグ
                var flags = (AssignmentFlagChecked)search.AssignmentFlg;
                if (!flags.HasFlag(AssignmentFlagChecked.All))
                {
                    if (flags.HasFlag(AssignmentFlagChecked.NoAssignment))
                    {
                        builder.AppendLine(@"
AND EXISTS (SELECT 1
                 FROM Billing b
                WHERE b.ReminderId = r.Id
                  AND b.AssignmentFlag <> 2)
");
                    }
                    if (flags.HasFlag(AssignmentFlagChecked.FullAssignment))
                    {
                        builder.AppendLine(@"
AND NOT EXISTS (SELECT 1
                 FROM Billing b
                WHERE b.ReminderId = r.Id
                  AND b.AssignmentFlag <> 2)
");
                    }
                }

                builder.AppendLine("ORDER BY cs.Code, r.Id, rh.CreateAt DESC");
            }
            else
            {
                builder.AppendLine("ORDER BY cs.Code, rh.CreateAt DESC");
            }

            return dbHelper.GetItemsAsync<ReminderHistory>(builder.ToString(), search, token);
        }

        public Task<IEnumerable<Reminder>> GetCancelDecisionItemsAsync(ReminderSearch search, ReminderCommonSetting setting, IEnumerable<ReminderSummarySetting> summary, CancellationToken token = default(CancellationToken))
        {
            var columns = new StringBuilder();
            var grouping = new StringBuilder();

            columns.AppendLine("  @CompanyId [CompanyId]");
            columns.AppendLine(", ccy.Id [CurrencyId]");
            columns.AppendLine(", ccy.Code [CurrencyCode]");
            columns.AppendLine(", r.Id");

            grouping.AppendLine("  ccy.Id");
            grouping.AppendLine(", ccy.Code");
            grouping.AppendLine(", r.Id");

            var comma = ", ";

            var baseDate = "";
            if (setting.CalculateBaseDate == (int)CalculateBaseDate.OriginalDueAt) baseDate = "COALESCE(b.OriginalDueAt, b.DueAt)";
            if (setting.CalculateBaseDate == (int)CalculateBaseDate.DueAt) baseDate = "b.DueAt";
            if (setting.CalculateBaseDate == (int)CalculateBaseDate.BilledAt) baseDate = "b.BilledAt";

            var arrearsDays = $"DATEDIFF(DAY, {baseDate}, @CalculateBaseDate)";
            if (setting.IncludeOnTheDay == 1) arrearsDays += " + 1";

            foreach (var s in summary.Where(x => x.Available == 1).OrderBy(x => x.DisplayOrder))
            {
                columns.Append(comma);
                grouping.Append(comma);

                switch (s.ColumnName)
                {
                    case "CalculateBaseDate":
                        columns.AppendLine(baseDate + " [CalculateBaseDate]");
                        grouping.AppendLine(baseDate);
                        break;
                    case "CustomerCode":
                        columns.AppendLine("cs.Id [CustomerId]");
                        columns.AppendLine(", cs.Code [CustomerCode]");
                        columns.AppendLine(", cs.Name [CustomerName]");
                        columns.AppendLine(", COUNT(*) [DetailCount]");
                        columns.AppendLine(", SUM(b.RemainAmount) [RemainAmount]");
                        columns.AppendLine(", SUM(CASE WHEN b.ReminderId IS NULL THEN NULL ELSE b.RemainAmount END) [ReminderAmount]");
                        columns.AppendLine($", {arrearsDays} [ArrearsDays]");
                        grouping.AppendLine("cs.Id");
                        grouping.AppendLine(", cs.Code");
                        grouping.AppendLine(", cs.Name");
                        break;
                    case "ClosingAt":
                        columns.AppendLine("b.ClosingAt");
                        grouping.AppendLine("b.ClosingAt");
                        break;
                    case "InvoiceCode":
                        columns.AppendLine("b.InvoiceCode");
                        grouping.AppendLine("b.InvoiceCode");
                        break;
                    case "CollectCategory":
                        columns.AppendLine("cct.Id [CollectCategoryId]");
                        columns.AppendLine(", cct.Code [CollectCategoryCode]");
                        columns.AppendLine(", cct.Name [CollectCategoryName]");
                        grouping.AppendLine("cct.Id");
                        grouping.AppendLine(", cct.Code");
                        grouping.AppendLine(", cct.Name");
                        break;
                    case "DestinationCode":
                        columns.AppendLine("ds.Id [DestinationId]");
                        columns.AppendLine(", ds.Code [DestinationCode]");
                        columns.AppendLine(", CASE WHEN ds.Id IS NULL THEN ");
                        columns.AppendLine("    IIF (COALESCE(cs.PostalCode,'') <> '','〒' + cs.PostalCode, '') + ' ' + cs.Name + cs.Address1 + cs.Address2 + ' ' + cs.DestinationDepartmentName + ' ' + cs.CustomerStaffName + cs.Honorific");
                        columns.AppendLine("  ELSE");
                        columns.AppendLine("    IIF (COALESCE(ds.PostalCode,'') <> '','〒' + ds.PostalCode, '') + ' ' + ds.Name + ds.Address1 + ds.Address2 + ' ' + ds.DepartmentName + ' ' + ds.Addressee + ds.Honorific");
                        columns.AppendLine("  END [DestinationDisplay]");
                        grouping.AppendLine("ds.Id");
                        grouping.AppendLine(", ds.Code");
                        grouping.AppendLine(", ds.PostalCode");
                        grouping.AppendLine(", ds.Name");
                        grouping.AppendLine(", ds.Address1");
                        grouping.AppendLine(", ds.Address2");
                        grouping.AppendLine(", ds.DepartmentName");
                        grouping.AppendLine(", ds.Addressee");
                        grouping.AppendLine(", ds.Honorific");
                        grouping.AppendLine(", cs.PostalCode");
                        grouping.AppendLine(", cs.Address1");
                        grouping.AppendLine(", cs.Address2");
                        grouping.AppendLine(", cs.DestinationDepartmentName");
                        grouping.AppendLine(", cs.CustomerStaffName");
                        grouping.AppendLine(", cs.Honorific");
                        break;
                    case "Department":
                        columns.AppendLine("d.Id [DepartmentId]");
                        columns.AppendLine(", d.Code [DepartmentCode]");
                        columns.AppendLine(", d.Name [DepartmentName]");
                        grouping.AppendLine("d.Id");
                        grouping.AppendLine(", d.Code");
                        grouping.AppendLine(", d.Name");
                        break;
                    case "Staff":
                        columns.AppendLine("s.Id [StaffId]");
                        columns.AppendLine(", s.Code [StaffCode]");
                        columns.AppendLine(", s.Name [StaffName]");
                        grouping.AppendLine("s.Id");
                        grouping.AppendLine(", s.Code");
                        grouping.AppendLine(", s.Name");
                        break;
                }

                comma = ", ";
            }

            var conditions = new StringBuilder();
            if (search.ExistsMemo)
                conditions.AppendLine("AND bm.Memo IS NOT NULL");
            if (!string.IsNullOrEmpty(search.BillingMemo))
            {
                search.BillingMemo = Sql.GetWrappedValue(search.BillingMemo);
                conditions.AppendLine("AND bm.Memo LIKE @BillingMemo COLLATE JAPANESE_CI_AS");
            }
            if (search.ArrearDaysFrom.HasValue || search.ArrearDaysTo.HasValue)
            {
                var arrearDaysCondition = new StringBuilder();

                arrearDaysCondition.AppendLine("b.ReminderId IS NULL");
                arrearDaysCondition.AppendLine("OR (b.ReminderId IS NOT NULL");

                if (search.ArrearDaysFrom.HasValue)
                    arrearDaysCondition.AppendLine($"AND {arrearsDays} >= @ArrearDaysFrom");
                if (search.ArrearDaysTo.HasValue)
                    arrearDaysCondition.AppendLine($"AND {arrearsDays} <= @ArrearDaysTo");
                arrearDaysCondition.AppendLine(")");

                conditions.AppendLine($"AND ({arrearDaysCondition.ToString()})");
            }
            if (!string.IsNullOrEmpty(search.DepartmentCodeFrom))
                conditions.AppendLine("AND d.Code >= @DepartmentCodeFrom");
            if (!string.IsNullOrEmpty(search.DepartmentCodeTo))
                conditions.AppendLine("AND d.Code <= @DepartmentCodeTo");
            if (!string.IsNullOrEmpty(search.StaffCodeFrom))
                conditions.AppendLine("AND s.Code >= @StaffCodeFrom");
            if (!string.IsNullOrEmpty(search.StaffCodeTo))
                conditions.AppendLine("AND s.Code <= @StaffCodeTo");
            if (!string.IsNullOrEmpty(search.CustomerCodeFrom))
                conditions.AppendLine("AND cs.Code >= @CustomerCodeFrom");
            if (!string.IsNullOrEmpty(search.CustomerCodeTo))
                conditions.AppendLine("AND cs.Code <= @CustomerCodeTo");

            var query = $@"
SELECT
{columns.ToString()}
FROM Reminder r
INNER JOIN Billing b
ON b.ReminderId = r.Id
INNER JOIN Currency ccy
ON b.CompanyId = @CompanyId
AND b.CompanyId = ccy.CompanyId
AND b.CurrencyId = ccy.Id
AND ccy.Code = @CurrencyCode
INNER JOIN Customer cs
ON b.CustomerId = cs.Id
INNER JOIN Department d
ON b.DepartmentId = d.Id
INNER JOIN Staff s
ON b.StaffId = s.Id
INNER JOIN Category cct
ON b.CollectCategoryId = cct.Id
LEFT JOIN Destination ds
ON b.DestinationId = ds.Id
LEFT JOIN BillingMemo bm
ON b.Id = bm.BillingId
LEFT JOIN StatusMaster st
ON r.StatusId = st.Id
WHERE r.OutputAt IS NULL
AND b.DeleteAt IS NULL
AND DATEDIFF(DAY, b.DueAt, @CalculateBaseDate) > 0
{conditions.ToString()}
GROUP BY
{grouping.ToString()}
HAVING (SUM(b.RemainAmount) = SUM(b.BillingAmount))
ORDER BY cs.Code
";

            return dbHelper.GetItemsAsync<Reminder>(query, search, token);
        }

        public Task<int> DeleteReminderSummaryAsync(Reminder reminder, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE ReminderSummary
 WHERE CustomerId = @CustomerId
   AND CurrencyId = @CurrencyId
";
            return dbHelper.ExecuteAsync(query, reminder, token);
        }

    }
}
