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
    public class ScheduledPaymentListQueryProcessor : IScheduledPaymentListQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ScheduledPaymentListQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<ScheduledPaymentList>> GetAsync(ScheduledPaymentListSearch option, CancellationToken token = default(CancellationToken))
        { 
            var CalcBaseDate = option.ReportSettings[8].ItemKey;
            var CalcIncludeToday = option.ReportSettings[7].ItemKey;
            var sqlWhere = "";
            var query = "";
            var orderBy = " ccy.DisplayOrder";
            var orderByCustomerSummaryFlag = " ccy.DisplayOrder";
            if (option.BilledAtFrom.HasValue)
            {
                sqlWhere += " AND b.BilledAt >= @BilledAtFrom ";
            }
            if (option.BilledAtTo.HasValue)
            {
                sqlWhere += " AND b.BilledAt <= @BilledAtTo ";
            }
            if (option.DueAtFrom.HasValue)
            {
                sqlWhere += " AND b.DueAt >= @DueAtFrom ";
            }
            if (option.DueAtTo.HasValue)
            {
                sqlWhere += " AND b.DueAt <= @DueAtTo ";
            }
            if (option.ClosingAtFrom.HasValue)
            {
                sqlWhere += " AND b.ClosingAt >= @ClosingAtFrom ";
            }
            if (option.ClosingAtTo.HasValue)
            {
                sqlWhere += " AND b.ClosingAt <= @ClosingAtTo ";
            }
            if (!string.IsNullOrEmpty(option.InvoiceCodeFrom))
            {
                sqlWhere += " AND b.InvoiceCode >= @InvoiceCodeFrom ";
            }
            if (!string.IsNullOrEmpty(option.InvoiceCodeTo))
            {
                sqlWhere += " AND b.InvoiceCode <= @InvoiceCodeTo ";
            }
            if (!string.IsNullOrEmpty(option.InvoiceCode))
            {
                option.InvoiceCode = Sql.GetWrappedValue(option.InvoiceCode);
                sqlWhere += " AND b.InvoiceCode LIKE @InvoiceCode";
            }
            if (!string.IsNullOrEmpty(option.CategoryCode))
            {
                sqlWhere += " AND ct.Code  = @CategoryCode ";
            }
            if (!string.IsNullOrEmpty(option.CurrencyCode))
            {
                sqlWhere += " AND ccy.Code = @CurrencyCode ";
            }
            if (!string.IsNullOrEmpty(option.DepartmentCodeFrom))
            {
                sqlWhere += " AND d.Code >= @DepartmentCodeFrom ";
            }
            if (!string.IsNullOrEmpty(option.DepartmentCodeTo))
            {
                sqlWhere += " AND d.Code <= @DepartmentCodeTo ";
            }
            if (!string.IsNullOrEmpty(option.StaffCodeFrom))
            {
                sqlWhere += " AND st.Code >= @StaffCodeFrom ";
            }
            if (!string.IsNullOrEmpty(option.StaffCodeTo))
            {
                sqlWhere += " AND st.Code <= @StaffCodeTo ";
            }
            if (!string.IsNullOrEmpty(option.CustomerCodeFrom))
            {
                sqlWhere += " AND cs.Code >= @CustomerCodeFrom ";
            }
            if (!string.IsNullOrEmpty(option.CustomerCodeTo))
            {
                sqlWhere += " AND cs.Code <= @CustomerCodeTo ";
            }
            //for order
            if (option.ReportSettings[0].ItemKey == "1")
            {
                orderBy += " ,d.Code";
                orderByCustomerSummaryFlag += " ,d.Code";
            }
            if (option.ReportSettings[1].ItemKey == "1")
            {
                orderBy += " ,st.Code";
                orderByCustomerSummaryFlag += " ,st.Code";
            }
            if (option.ReportSettings[2].ItemKey == "1")
            {
                orderBy += ",cs.Code ";
                orderByCustomerSummaryFlag += " ,cs.Code";
            }
            if (option.ReportSettings[3].ItemKey == "1")
            {
                orderBy += " ,b.DueAt ";
            }
            if (option.ReportSettings[5].ItemKey == "0" && option.ReportSettings[2].ItemKey == "0")
            {
                orderBy += " ,cs.Code";
                orderByCustomerSummaryFlag += " ,cs.Code";
            }
            if (option.ReportSettings[5].ItemKey == "2")
            {
                orderBy += " ,b.Id";
            }
            if (option.ReportSettings[6].ItemKey == "0")
            {
                orderBy += " ,b.BilledAt";
            }
            if (option.ReportSettings[6].ItemKey == "1")
            {
                orderBy += " ,b.SalesAt";
            }
            if (option.ReportSettings[6].ItemKey == "2")
            {
                orderBy += " ,b.ClosingAt";
            }
            if (option.ReportSettings[3].ItemKey == "0" && option.ReportSettings[6].ItemKey == "3")
            {
                orderBy += " ,b.DueAt";
            }
            if (option.ReportSettings[6].ItemKey == "4")
            {
                orderBy += ",COALESCE(b.OriginalDueAt, b.DueAt)";
            }

            if (option.CustomerSummaryFlag)
            {
                query = @"SELECT
                          cs.Code  [CustomerCode]
                        , cs.Name  [CustomerName]
                        , ccy.Code [CurrencyCode]
                        , SUM(b.RemainAmount - b.OffsetAmount) [RemainAmount]
                        , d.Code [DepartmentCode]
                        , d.Name [DepartmentName]
                        , st.Code [StaffCode]
                        , st.Name [StaffName]
                        FROM Billing b
                        INNER JOIN Department d
                        ON b.DepartmentId = d.Id
                        INNER JOIN Staff st
                        ON b.StaffId = st.Id
                        INNER JOIN Customer cs
                        ON b.CustomerId = cs.Id
                        INNER JOIN Category ct
                        ON b.CollectCategoryId = ct.Id
                        INNER JOIN Currency ccy
                        ON b.CurrencyId = ccy.Id
                        WHERE b.CompanyId = @CompanyId
                          AND (b.RemainAmount - b.OffsetAmount) <> 0
                          AND b.DeleteAt IS NULL
                             " + sqlWhere +
                        @" GROUP BY cs.Code, cs.Name, ccy.Code, d.Code, d.Name, st.Code, st.Name, ccy.DisplayOrder
                        ORDER BY"
                            + orderByCustomerSummaryFlag;
            }
            else
            {
                query = @"SELECT
                          b.Id
                        , cs.Code [CustomerCode]
                        , cs.Name [CustomerName]
                        , ccy.Code [CurrencyCode]
                        , b.BilledAt
                        , b.SalesAt
                        , b.ClosingAt
                        , b.DueAt
                        , COALESCE(b.OriginalDueAt, b.DueAt) [OriginalDueAt]
                        , (b.RemainAmount - b.OffsetAmount) [RemainAmount]
                        , CASE WHEN
                            CASE @CalcBaseDate
                              WHEN 0 THEN COALESCE(b.OriginalDueAt, b.DueAt)
                              WHEN 1 THEN b.DueAt
                              ELSE b.BilledAt END < DATEADD(day, CASE @CalcIncludeToday WHEN 1 THEN 1 ELSE 0 END, @BaseDate)
                            THEN '*' ELSE '' END [DelayDivision]
                        , ct.Code [CollectCategoryCode]
                        , ct.Name [CollectCategoryName]
                        , b.InvoiceCode
                        , b.Note1
                        , b.Note2
                        , b.Note3
                        , b.Note4
                        , d.Code [DepartmentCode]
                        , d.Name [DepartmentName]
                        , st.Code [StaffCode]
                        , st.Name [StaffName]
                        FROM Billing b
                        INNER JOIN Department d
                        ON b.DepartmentId = d.Id
                        INNER JOIN Staff st
                        ON b.StaffId = st.Id
                        INNER JOIN Customer cs
                        ON b.CustomerId = cs.Id
                        INNER JOIN Category ct
                        ON b.CollectCategoryId = ct.Id
                        INNER JOIN Currency ccy
                        ON b.CurrencyId = ccy.Id
                        WHERE b.CompanyId = @CompanyId
                          AND (b.RemainAmount - b.OffsetAmount) <> 0
                          AND b.DeleteAt IS NULL
                               " + sqlWhere +
                            @" ORDER BY"
                               + orderBy;
            }

            return dbHelper.GetItemsAsync<ScheduledPaymentList>(query, new
                {
                    CompanyId = option.CompanyId,
                    CalcBaseDate,
                    CalcIncludeToday,
                    BaseDate = option.BaseDate,
                    BilledAtFrom = option.BilledAtFrom,
                    BilledAtTo = option.BilledAtTo,
                    DueAtFrom = option.DueAtFrom,
                    DueAtTo = option.DueAtTo,
                    ClosingAtFrom = option.ClosingAtFrom,
                    ClosingAtTo = option.ClosingAtTo,
                    InvoiceCodeFrom = option.InvoiceCodeFrom,
                    InvoiceCodeTo = option.InvoiceCodeTo,
                    InvoiceCode = option.InvoiceCode,
                    CategoryCode = option.CategoryCode,
                    CurrencyCode = option.CurrencyCode,
                    DepartmentCodeFrom = option.DepartmentCodeFrom,
                    DepartmentCodeTo = option.DepartmentCodeTo,
                    StaffCodeFrom = option.StaffCodeFrom,
                    StaffCodeTo = option.StaffCodeTo,
                    CustomerCodeFrom = option.CustomerCodeFrom,
                    CustomerCodeTo = option.CustomerCodeTo
                }, token);
        }
    }
}
