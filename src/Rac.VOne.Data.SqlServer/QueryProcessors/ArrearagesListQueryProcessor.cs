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
    public class ArrearagesListQueryProcessor : IArrearagesListQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public ArrearagesListQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<ArrearagesList>> GetAsync(ArrearagesListSearch option, CancellationToken token = default(CancellationToken))
        {
            // todo : cleanup magic number
            var CalcBaseDate = option.ReportSettings[7].ItemKey;
            var IncludeCalcBaseDate = option.ReportSettings[8].ItemKey;

            var sqlWhere = "";
            var query = "";
            var orderBy = " ccy.DisplayOrder";
            var orderByCustomerSummaryFlag = " ccy.DisplayOrder";
            if (option.CustomerSummaryFlag == false && option.ExistsMemo)
            {
                sqlWhere = " AND bm.Memo IS NOT NULL";
            }
            if (option.CustomerSummaryFlag == false && !string.IsNullOrEmpty(option.BillingMemo))
            {
                option.BillingMemo = Sql.GetWrappedValue(option.BillingMemo);
                sqlWhere += " AND bm.Memo LIKE @BillingMemo";
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
            if (!string.IsNullOrEmpty(option.StaffCodeFrom))
            {
                sqlWhere += " AND st.Code >= @StaffCodeFrom ";
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
                        , SUM(b.RemainAmount) [RemainAmount]
                        , SUM(b.OffsetAmount) [OffsetAmount]
                        , d.Code [DepartmentCode]
                        , d.Name [DepartmentName]
                        , st.Code [StaffCode]
                        , st.Name [StaffName]
                        , @BaseDate [BaseDate]
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
                            AND b.AssignmentFlag <> 2
                            AND b.DeleteAt IS NULL
                            AND b.InputType <> 3
                            AND CASE @CalcBaseDate
                                WHEN 0 THEN COALESCE(b.OriginalDueAt, b.DueAt)
                                WHEN 1 THEN b.DueAt
                                ELSE b.BilledAt END <= DATEADD(day, CASE @IncludeCalcBaseDate WHEN 1 THEN 0 ELSE -1 END, @BaseDate)
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
                            , b.RemainAmount [RemainAmount]
                            , b.OffsetAmount [OffsetAmount]
                            , DATEDIFF(day, 
                                CASE @CalcBaseDate
                                    WHEN 0 THEN COALESCE(b.OriginalDueAt, b.DueAt)
                                    WHEN 1 THEN b.DueAt
                                    ELSE b.BilledAt END,
                                    @BaseDate)
                                + CASE @IncludeCalcBaseDate WHEN 1 THEN 1 ELSE 0 END [ArrearagesDayCount]
                            , ct.Code [CollectCategoryCode]
                            , ct.Name [CollectCategoryName]
                            , b.InvoiceCode
                            , b.Note1
                            , b.Note2
                            , b.Note3
                            , b.Note4
                            , bm.Memo
                            , cs.CustomerStaffName
                            , cs.Note [CustomerNote]
                            , cs.Tel
                            , d.Code [DepartmentCode]
                            , d.Name [DepartmentName]
                            , st.Code [StaffCode]
                            , st.Name [StaffName]
                            , @BaseDate [BaseDate]
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
                            LEFT JOIN BillingMemo bm
                            ON b.Id = bm.BillingId
                            WHERE b.CompanyId = @CompanyId
                                AND b.AssignmentFlag <> 2
                                AND b.DeleteAt IS NULL
                                AND b.InputType <> 3
                                AND CASE @CalcBaseDate
                                    WHEN 0 THEN COALESCE(b.OriginalDueAt, b.DueAt)
                                    WHEN 1 THEN b.DueAt
                                    ELSE b.BilledAt END <= DATEADD(day, CASE @IncludeCalcBaseDate WHEN 1 THEN 0 ELSE -1 END, @BaseDate)
                               " + sqlWhere +
                            @" ORDER BY"
                               + orderBy;
            }

            return dbHelper.GetItemsAsync<ArrearagesList>(query, new
                {
                    CompanyId = option.CompanyId,
                    CalcBaseDate,
                    IncludeCalcBaseDate,
                    BaseDate = option.BaseDate,
                    BillingMemo = option.BillingMemo,
                    CurrencyCode = option.CurrencyCode,
                    DepartmentCodeFrom = option.DepartmentCodeFrom,
                    DepartmentCodeTo = option.DepartmentCodeTo,
                    StaffCodeFrom = option.StaffCodeFrom,
                    StaffCodeTo = option.StaffCodeTo
                }, token);
        }

    }
}
