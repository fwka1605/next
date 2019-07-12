using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Rac.VOne.Common.Constants;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BillingSearchQueryProcessor :
        IBillingSearchQueryProcessor,
        IBillingSearchForImportQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingSearchQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<Billing>> GetAsync(BillingSearch option, CancellationToken token = default(CancellationToken))
        {
            // Matchingを介したReceipt集計クエリをJOINするか否か(検索速度を考慮)
            var requireRecordedAt = option.RequireRecordedAt;

            var query = new StringBuilder(@"
SELECT
  b.Id
, b.CompanyId
, b.CurrencyId
, b.CustomerId
, b.DepartmentId
, b.StaffId
, b.BillingCategoryId
, b.InputType
, b.BillingInputId
, b.BilledAt
, b.ClosingAt
, b.SalesAt
, b.DueAt
, b.BillingAmount
, b.TaxAmount
, b.AssignmentAmount
, b.RemainAmount
, b.OffsetAmount
, b.AssignmentFlag
, b.Approved
, b.CollectCategoryId
, b.OriginalCollectCategoryId
, b.DebitAccountTitleId
, b.CreditAccountTitleId
, b.OutputAt
, b.PublishAt
, b.InvoiceCode
, b.TaxClassId
, b.Note1
, b.Note2
, b.Note3
, b.Note4
, b.DeleteAt
, b.RequestDate
, b.ResultCode
, b.TransferOriginalDueAt
, b.ScheduledPaymentKey
, b.Quantity
, b.UnitPrice
, b.UnitSymbol
, b.Price
, b.CreateBy
, b.CreateAt
, b.UpdateBy
, b.UpdateAt
, b.AccountTransferLogId
, b.DestinationId
, b.Note5
, b.Note6
, b.Note7
, b.Note8
, b.ReminderId
, b.DueAt BillingDueAt
, CASE WHEN b.OriginalDueAt IS NULL THEN b.DueAt ELSE b.OriginalDueAt END OriginalDueAt
, CASE WHEN b.OriginalDueAt IS NULL THEN NULL ELSE b.DueAt END ModifiedDueAt
, b.RemainAmount - b.OffsetAmount as PaymentAmount 
, (b.RemainAmount - b.OffsetAmount) as TargetAmount
, bmm.Memo
, d.BillingId AS BillingDiscountId
, COALESCE(d.DiscountAmount , 0) DiscountAmount
, COALESCE(d.DiscountAmount1, 0) DiscountAmount1
, COALESCE(d.DiscountAmount2, 0) DiscountAmount2
, COALESCE(d.DiscountAmount3, 0) DiscountAmount3
, COALESCE(d.DiscountAmount4, 0) DiscountAmount4
, COALESCE(d.DiscountAmount5, 0) DiscountAmount5
, c.Code        CustomerCode
, c.Name        CustomerName
, c.Kana        CustomerKana
, c.IsParent
, dp.Code       DepartmentCode
, dp.Name       DepartmentName
, cur.Code      CurrencyCode
, cct.Code      CollectCategoryCode
, cct.Name      CollectCategoryName
, bct.Code      BillingCategoryCode
, bct.Name      BillingCategoryName
, st.Code       StaffCode
, st.Name       StaffName
, us.Code       LoginUserCode
, us.Name       LoginUserName
, CASE WHEN b.OriginalCollectCategoryId IS NULL THEN cct.Code +' : '+ cct.Name  ELSE  cate.Code +' : '+ cate.Name END CategoryCodeAndName
, CASE WHEN b.OriginalCollectCategoryId IS NULL THEN NULL ELSE  cct.Code +' : '+ cct.Name END OrgCategoryCodeAndName
, bdc.ContractNumber
, bdc.Comfirm
, COALESCE(pc.Id  , c.Id  )     [ParentCustomerId]
, COALESCE(pc.Code, c.Code)     [ParentCustomerCode]
, COALESCE(pc.Name, c.Name)     [ParentCustomerName]
, COALESCE(pc.Kana, c.Kana)     [ParentCustomerKana]
, binput.PublishAt AS BillingInputPublishAt
");
            if (requireRecordedAt) query.Append(@"
, mrg.FirstRecordedAt
, mrg.LastRecordedAt");

            query.Append(@"
FROM Billing as b
LEFT JOIN Customer c            ON   c.Id           = b.CustomerId
LEFT JOIN CustomerGroup cg      ON   c.Id           = cg.ChildCustomerId
LEFT JOIN Customer pc           ON  pc.Id           = cg.ParentCustomerId
LEFT JOIN Currency cur          ON cur.Id           = b.CurrencyId
LEFT JOIN Category cate         ON cate.Id          = b.OriginalCollectCategoryId
LEFT JOIN Category cct          ON cct.Id           = b.CollectCategoryId
LEFT JOIN Category bct          ON bct.Id           = b.BillingCategoryId
LEFT JOIN Department dp         ON  dp.Id           = b.DepartmentId
LEFT JOIN Staff st              ON  st.Id           = b.StaffId
LEFT JOIN BillingMemo bmm       ON   b.Id           = bmm.BillingId
LEFT JOIN LoginUser us          ON  us.Id           = b.UpdateBy
LEFT JOIN BillingDivisionContract bdc ON b.Id       = bdc.BillingId
LEFT JOIN BillingInput binput   ON b.BillingInputId = binput.Id
LEFT JOIN (
        SELECT BillingId
        , SUM(DiscountAmount) AS DiscountAmount
        , SUM(CASE WHEN DiscountType = 1 THEN DiscountAmount ELSE 0 END) DiscountAmount1
        , SUM(CASE WHEN DiscountType = 2 THEN DiscountAmount ELSE 0 END) DiscountAmount2
        , SUM(CASE WHEN DiscountType = 3 THEN DiscountAmount ELSE 0 END) DiscountAmount3
        , SUM(CASE WHEN DiscountType = 4 THEN DiscountAmount ELSE 0 END) DiscountAmount4
        , SUM(CASE WHEN DiscountType = 5 THEN DiscountAmount ELSE 0 END) DiscountAmount5
        FROM BillingDiscount 
        GROUP BY BillingId
    ) d
    ON b.Id = d.BillingId
");
            if (requireRecordedAt)
                query.Append(@"
LEFT JOIN (
        SELECT
            m.BillingId,
            MIN(r.RecordedAt) as FirstRecordedAt,
            MAX(r.RecordedAt) as LastRecordedAt
        FROM
            Matching m
            LEFT JOIN Receipt r ON m.ReceiptId = r.Id
        GROUP BY
            m.BillingId
    ) mrg
    ON b.Id = mrg.BillingId
");
            query.Append(@"
WHERE b.CompanyId = @CompanyId");


            if (option.BsBilledAtFrom.HasValue)
                query.AppendLine(" AND b.BilledAt >= @BsBilledAtFrom");

            if (option.BsBilledAtTo.HasValue)
                query.AppendLine(" AND b.BilledAt <= @BsBilledAtTo");

            if (option.BsSalesAtFrom.HasValue)
                query.AppendLine(" AND b.SalesAt >= @BsSalesAtFrom");

            if (option.BsSalesAtTo.HasValue)
                query.AppendLine(" AND b.SalesAt <= @BsSalesAtTo");

            if (!string.IsNullOrEmpty(option.BsCustomerName))
            {
                option.BsCustomerName = Sql.GetWrappedValue(option.BsCustomerName);
                query.AppendLine(" AND  c.Name LIKE @BsCustomerName");
            }

            if (!string.IsNullOrEmpty(option.BsCustomerNameKana))
            {
                option.BsCustomerNameKana = Sql.GetWrappedValue(option.BsCustomerNameKana);
                query.AppendLine(" AND  c.Kana  LIKE @BsCustomerNameKana");
            }

            if (!string.IsNullOrEmpty(option.BsPayerName))
            {
                option.BsPayerName = Sql.GetWrappedValue(option.BsPayerName);
                query.AppendLine(@"
 AND EXISTS (
    SELECT 1 FROM KanaHistoryCustomer khc
    WHERE khc.CustomerId = b.CustomerId AND khc.PayerName like @BsPayerName)");
            }

            if (option.BsClosingAtFrom.HasValue)
                query.AppendLine(" AND b.ClosingAt >= @BsClosingAtFrom");

            if (option.BsClosingAtTo.HasValue)
                query.AppendLine(" AND b.ClosingAt <= @BsClosingAtTo");

            if (!string.IsNullOrEmpty(option.BsScheduledPaymentKey))
            {
                option.BsScheduledPaymentKey = Sql.GetWrappedValue(option.BsScheduledPaymentKey);
                query.AppendLine(" AND b.ScheduledPaymentKey LIKE @BsScheduledPaymentKey");
            }

            if (option.BsDueAtFrom.HasValue)
                query.AppendLine(" AND b.DueAt >=  @BsDueAtFrom");
            if (option.BsDueAtTo.HasValue)
                query.AppendLine(" AND b.DueAt <=  @BsDueAtTo");
            if (!string.IsNullOrEmpty(option.BsInvoiceCodeFrom))
                query.AppendLine(" AND b.InvoiceCode >=  @BsInvoiceCodeFrom");
            if (!string.IsNullOrEmpty(option.BsInvoiceCodeTo))
                query.AppendLine(" AND b.InvoiceCode <=  @BsInvoiceCodeTo");
            if (!string.IsNullOrEmpty(option.BsInvoiceCode))
            {
                option.BsInvoiceCode = Sql.GetWrappedValue(option.BsInvoiceCode);
                query.AppendLine(" AND b.InvoiceCode LIKE @BsInvoiceCode");
            }
            if (!string.IsNullOrEmpty(option.BsCurrencyCode))
                query.AppendLine(" AND cur.Code =  @BsCurrencyCode");
            if (!string.IsNullOrEmpty(option.BsDepartmentCodeFrom))
                query.AppendLine(" AND dp.Code >=  @BsDepartmentCodeFrom");
            if (!string.IsNullOrEmpty(option.BsDepartmentCodeTo))
                query.AppendLine(" AND dp.Code <=  @BsDepartmentCodeTo");
            if (!string.IsNullOrEmpty(option.BsStaffCodeFrom))
                query.AppendLine(" AND st.Code >= @BsStaffCodeFrom");
            if (!string.IsNullOrEmpty(option.BsStaffCodeTo))
                query.AppendLine(" AND st.Code <= @BsStaffCodeTo");

            if (option.IsParentFlg != 0)
            {
                query.AppendLine(@"
AND (
        b.CustomerId IN (SELECT ChildCustomerId FROM CustomerGroup WHERE ParentCustomerId = @CustomerId )
     OR b.CustomerId = @CustomerId
    )");
            }
            else if (!string.IsNullOrEmpty(option.BsCustomerCode))
            {
                query.AppendLine(" AND b.CustomerId = @CustomerId");
            }

            if (!string.IsNullOrEmpty(option.BsCustomerCodeFrom))
                query.AppendLine(" AND c.Code >= @BsCustomerCodeFrom");

            if (!string.IsNullOrEmpty(option.BsCustomerCodeTo))
                query.AppendLine(" AND c.Code <= @BsCustomerCodeTo");
            if (option.BsRemaingAmountFrom.HasValue)
                query.AppendLine(" AND b.RemainAmount >= @BsRemaingAmountFrom");
            if (option.BsRemaingAmountTo.HasValue)
                query.AppendLine(" AND b.RemainAmount <= @BsRemaingAmountTo");
            if (option.BsBillingAmountFrom.HasValue)
                query.AppendLine(" AND b.BillingAmount >= @BsBillingAmountFrom");
            if (option.BsBillingAmountTo.HasValue)
                query.AppendLine(" AND b.BillingAmount <= @BsBillingAmountTo");
            if (option.BsBillingCategoryId != 0)
                query.AppendLine(" AND b.BillingCategoryId  = @BsBillingCategoryId");
            if (option.CollectCategoryId != 0)
                query.AppendLine(" AND b.CollectCategoryId = @CollectCategoryId");
            if (!string.IsNullOrEmpty(option.BsInputType.ToString()) && option.BsInputType != 0)
                query.AppendLine(" AND b.InputType = @BsInputType");
            if (option.ExistsMemo)
                query.AppendLine(" AND bmm.Memo IS NOT NULL");
            if (!string.IsNullOrEmpty(option.BsMemo))
            {
                option.BsMemo = Sql.GetWrappedValue(option.BsMemo);
                query.AppendLine(" AND bmm.Memo LIKE @BsMemo");
            }

            if (option.BsUpdateAtFrom.HasValue)
                query.AppendLine(" AND b.UpdateAt >= @BsUpdateAtFrom");
            if (option.BsUpdateAtTo.HasValue)
                query.AppendLine(" AND b.UpdateAt <= @BsUpdateAtTo");

            if (option.BsUpdateBy.HasValue)
                query.AppendLine(" AND b.UpdateBy = @BsUpdateBy");

            if (!string.IsNullOrEmpty(option.BsNote1))
            {
                option.BsNote1 = Sql.GetWrappedValue(option.BsNote1);
                query.AppendLine(" AND b.Note1 LIKE @BsNote1");
            }
            if (!string.IsNullOrEmpty(option.BsNote2))
            {
                option.BsNote2 = Sql.GetWrappedValue(option.BsNote2);
                query.AppendLine(" AND b.Note2 LIKE @BsNote2");
            }
            if (!string.IsNullOrEmpty(option.BsNote3))
            {
                option.BsNote3 = Sql.GetWrappedValue(option.BsNote3);
                query.AppendLine(" AND b.Note3 LIKE @BsNote3");
            }
            if (!string.IsNullOrEmpty(option.BsNote4))
            {
                option.BsNote4 = Sql.GetWrappedValue(option.BsNote4);
                query.AppendLine(" AND b.Note4 LIKE @BsNote4");
            }
            if (!string.IsNullOrEmpty(option.BsNote5))
            {
                option.BsNote5 = Sql.GetWrappedValue(option.BsNote5);
                query.AppendLine(" AND b.Note5 LIKE @BsNote5");
            }
            if (!string.IsNullOrEmpty(option.BsNote6))
            {
                option.BsNote6 = Sql.GetWrappedValue(option.BsNote6);
                query.AppendLine(" AND b.Note6 LIKE @BsNote6");
            }
            if (!string.IsNullOrEmpty(option.BsNote7))
            {
                option.BsNote7 = Sql.GetWrappedValue(option.BsNote7);
                query.AppendLine(" AND b.Note7 LIKE @BsNote7");
            }
            if (!string.IsNullOrEmpty(option.BsNote8))
            {
                option.BsNote8 = Sql.GetWrappedValue(option.BsNote8);
                query.AppendLine(" AND b.Note8 LIKE @BsNote8");
            }

            var flags = (AssignmentFlagChecked)option.AssignmentFlg;
            var selectedFlags = new List<int>();
            if (flags.HasFlag(AssignmentFlagChecked.NoAssignment)) selectedFlags.Add(0);
            if (flags.HasFlag(AssignmentFlagChecked.PartAssignment)) selectedFlags.Add(1);
            if (flags.HasFlag(AssignmentFlagChecked.FullAssignment)) selectedFlags.Add(2);
            if (selectedFlags.Any() && !flags.HasFlag(AssignmentFlagChecked.All))
                query.AppendLine($" AND b.AssignmentFlag IN ({(string.Join(", ", selectedFlags))})");

            if (!string.IsNullOrEmpty(option.LoginUserCode))
                query.AppendLine(" AND b.UpdateBy = @UserId");

            if (option.UseSectionMaster
                && option.KobetsuType != "Matching")
            {
                query.AppendLine(@"
AND EXISTS (SELECT 1
              FROM SectionWithLoginUser swl
             INNER JOIN SectionWithDepartment swd
                ON swl.LoginUserId  = @LoginUserId
               AND swl.SectionId    = swd.SectionId
               AND swd.DepartmentId = b.DepartmentId )");
            }

            if (option.UseDepartmentWork)
            {
                query.AppendLine(@"
 AND b.DepartmentId IN (
        SELECT DISTINCT wdt.DepartmentId
          FROM WorkDepartmentTarget wdt
         WHERE wdt.ClientKey        = @ClientKey
           AND wdt.UseCollation     = 1 )");
            }

            if (option.RequestDate == 1)
                query.AppendLine(" AND b.RequestDate IS NOT NULL");
            if (option.CurrencyId != 0)
                query.AppendLine(" AND cur.Id = @CurrencyId");

            if (option.ParentCustomerId != 0)
            {
                query.AppendLine(@"
AND b.CustomerId NOT IN (SELECT ChildCustomerId
                           FROM CustomerGroup
                          UNION
                         SELECT ParentCustomerId
                           FROM CustomerGroup)
AND c.IsParent <> 1
                                    ");
            }

            if (option.IsDeleted)
            {
                query.AppendLine(" AND b.DeleteAt IS NOT NULL");
            }
            else
            {
                query.AppendLine(" AND b.DeleteAt IS NULL");
            }

            if (option.BsDeleteAtFrom.HasValue)
                query.AppendLine(" AND b.DeleteAt >= @BsDeleteAtFrom");
            if (option.BsDeleteAtTo.HasValue)
                query.AppendLine(" AND b.DeleteAt <= @BsDeleteAtTo");
            if (option.KobetsuType == "Matching")
                query.AppendLine(" AND cct.UseAccountTransfer = 0");

            if (option.BillingId.HasValue)
                query.AppendLine(" AND b.Id = @BillingId");
            if (option.BillingInputId.HasValue)
                query.AppendLine(" AND b.BillingInputId = @BillingInputId");

            query.Append(@"
 ORDER BY b.Id
 OPTION ( FORCE ORDER )
");
            return dbHelper.GetItemsAsync<Billing>(query.ToString(), option, token);
        }

        public Task<IEnumerable<long>> GetItemsForImportAsync(BillingSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      b.Id
FROM        Billing b
WHERE       b.CompanyId       = @CompanyId
AND         b.AssignmentFlag    IN (0,1)
AND         b.DeleteAt          IS NULL";

            var whereCondition = new StringBuilder();

            if (option.BilledAt.HasValue) query += @"
AND         b.BilledAt          = @BilledAt";
            if (option.BillingAmount != 0) query += @"
AND         b.BillingAmount     = @BillingAmount";
            if (option.TaxAmount != 0) query += @"
AND         b.TaxAmount         = @TaxAmount";
            if (!string.IsNullOrEmpty(option.InvoiceCode)) query += @"
AND         b.InvoiceCode       = @InvoiceCode";
            if (option.Price != 0) query += @"
AND         b.Price             = @Price";
            if (option.TaxClassId.HasValue) query += @"
AND         b.TaxClassId        = @TaxClassId";
            if (!string.IsNullOrEmpty(option.Note1)) query += @"
AND         b.Note1             = @Note1";
            if (!string.IsNullOrEmpty(option.Note2)) query += @"
AND         b.Note2             = @Note2";
            if (!string.IsNullOrEmpty(option.Note3)) query += @"
AND         b.Note3             = @Note3";
            if (!string.IsNullOrEmpty(option.Note4)) query += @"
AND         b.Note4             = @Note4";
            if (!string.IsNullOrEmpty(option.Note5)) query += @"
AND         b.Note5             = @Note5";
            if (!string.IsNullOrEmpty(option.Note6)) query += @"
AND         b.Note6             = @Note6";
            if (!string.IsNullOrEmpty(option.Note7)) query += @"
AND         b.Note7             = @Note7";
            if (!string.IsNullOrEmpty(option.Note8)) query += @"
AND         b.Note8             = @Note8";
            if (option.CustomerId != 0) query += @"
AND         b.CustomerId        = @CustomerId";
            return dbHelper.GetItemsAsync<long>(query, option, token);
        }
    }

}
