using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class MatchingQueryProcessor :
        IMatchingQueryProcessor,
        IAddMatchingQueryProcessor,
        ICancelMatchingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public MatchingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<Billing>> GetBillingsForSequentialMatchingAsync(MatchingBillingSearch searchOption, IEnumerable<MatchingOrder> orders, CancellationToken token = default(CancellationToken))
        {
            var query = new StringBuilder(@"
SELECT b.Id
     , b.CompanyId
     , b.CurrencyId
     , b.CustomerId
     , b.BillingAmount
     , b.RemainAmount
     , b.RemainAmount - b.OffsetAmount TargetAmount
     , b.OffsetAmount
     , b.AssignmentAmount
     , b.BilledAt
     , b.DueAt
     , b.Note1
     , b.Note2
     , b.Note3
     , b.Note4
     , b.InvoiceCode
     , b.InputType
     , b.SalesAt
     , b.ClosingAt
     , b.DepartmentId
     , b.BillingCategoryId
     , b.CollectCategoryId
     , b.DebitAccountTitleId
     , b.CreditAccountTitleId
     , b.TaxClassId
     , b.StaffId
     , b.UpdateBy
     , b.UpdateAt
     , b.ScheduledPaymentKey
     , b.InputType
     , bm.Memo
     , COALESCE(bd.DiscountAmount , 0) DiscountAmount
     , COALESCE(bd.DiscountAmount1, 0) DiscountAmount1
     , COALESCE(bd.DiscountAmount2, 0) DiscountAmount2
     , COALESCE(bd.DiscountAmount3, 0) DiscountAmount3
     , COALESCE(bd.DiscountAmount4, 0) DiscountAmount4
     , COALESCE(bd.DiscountAmount5, 0) DiscountAmount5 
     , bc.Code BillingCategoryCode
     , bc.Name BillingCategoryName
     , dp.Code DepartmentCode
     , dp.Name DepartmentName
     , st.Code StaffCode
     , st.Name StaffName
     , cs.Code CustomerCode
     , cs.Name CustomerName
     , cs.Kana CustomerKana
     , cs.IsParent
     , pcs.Id   ParentCustomerId
     , pcs.Code ParnetCustomerCode
     , pcs.Name ParentCustomerName
FROM WorkBillingTarget wb
 INNER JOIN Billing b           ON  wb.ClientKey        = @ClientKey
                               AND  wb.CustomerId       = COALESCE(@ParentCustomerId, 0)
                               AND  wb.PaymentAgencyId  = COALESCE(@PaymentAgencyId , 0)
                               AND  wb.BillingId        = b.Id
                               AND   b.CurrencyId       = @CurrencyId
  LEFT JOIN Department dp       ON  dp.Id               = b.DepartmentId
  LEFT JOIN Staff st            ON  st.Id               = b.StaffId
  LEFT JOIN BillingMemo bm      ON   b.Id               = bm.BillingId
  LEFT JOIN Category bc         ON  bc.Id               = b.BillingCategoryId
  LEFT JOIN Category cc         ON  cc.Id               = b.CollectCategoryId
  LEFT JOIN Customer cs         ON  cs.Id               = b.CustomerId
  LEFT JOIN Customer pcs        ON pcs.Id               = wb.CustomerId
  LEFT JOIN (
       SELECT bd.BillingId
            , SUM(bd.DiscountAmount) DiscountAmount
            , SUM(CASE bd.DiscountType WHEN 1 THEN bd.DiscountAmount ELSE 0 END) DiscountAmount1
            , SUM(CASE bd.DiscountType WHEN 2 THEN bd.DiscountAmount ELSE 0 END) DiscountAmount2
            , SUM(CASE bd.DiscountType WHEN 3 THEN bd.DiscountAmount ELSE 0 END) DiscountAmount3
            , SUM(CASE bd.DiscountType WHEN 4 THEN bd.DiscountAmount ELSE 0 END) DiscountAmount4
            , SUM(CASE bd.DiscountType WHEN 5 THEN bd.DiscountAmount ELSE 0 END) DiscountAmount5
         FROM BillingDiscount bd
        WHERE bd.AssignmentFlag = 0
         GROUP BY
              bd.BillingId
            ) bd                ON  b.Id        = bd.BillingId
");
            var order = orders.GetOrderByQuery(x =>
            {
                var item = string.Empty;
                switch (x.ItemName)
                {
                    case "BillingRemainSign": item = "CASE WHEN (b.RemainAmount - b.OffsetAmount) < 0 THEN 0 ELSE 1 END"; break;
                    case "CashOnDueDatesFlag": item = "CASE WHEN b.InputType = 3 THEN 1 ELSE 0 END"; break;
                    case "DueAt": item = "b.DueAt"; break;
                    case "CustomerCode": item = "cs.Code"; break;
                    case "BilledAt": item = "b.BilledAt"; break;
                    case "BillingRemainAmount": item = "(b.RemainAmount - b.OffsetAmount) * CASE WHEN (b.RemainAmount - b.OffsetAmount) < 0 THEN -1 ELSE 1 END"; break;
                    case "BillingCategory": item = "bc.MatchingOrder"; break;
                }
                return item;
            });
            if (!string.IsNullOrEmpty(order)) query.Append(order);
            //               query.Append(@"
            //OPTION ( FORCE ORDER )");
            return dbHelper.GetItemsAsync<Billing>(query.ToString(), searchOption, token);
        }
        public Task<IEnumerable<Receipt>> GetReceiptsForSequentialMatchingAsync(MatchingReceiptSearch searchOption, IEnumerable<MatchingOrder> orders, CancellationToken token = default(CancellationToken))
        {
            var query = new StringBuilder(@"
SELECT r.Id
     , r.CompanyId
     , r.CurrencyId
     , r.PayerName
     , r.PayerNameRaw
     , r.ReceiptAmount
     , r.AssignmentAmount
     , r.AssignmentFlag
     , r.RemainAmount
     , r.SourceBankName
     , r.SourceBranchName
     , r.RecordedAt
     , r.ReceiptCategoryId
     , r.CreateAt
     , r.UpdateAt
     , r.DueAt
     , r.CustomerId
     , r.OriginalReceiptId
     , r.Note1
     , r.Note2
     , r.Note3
     , r.Note4
     , r.BillNumber
     , r.BillBankCode
     , r.BillBranchCode
     , r.BillDrawAt
     , r.BillDrawer
     , r.ReceiptHeaderId
     , r.BankCode
     , r.BankName
     , r.BranchCode
     , r.BranchName
     , r.AccountTypeId
     , r.AccountNumber
     , bt.Name AccountTypeName
     , /*rm.Memo*/ CAST(N'' AS NVARCHAR(140))  ReceiptMemo
     , cs.Code CustomerCode
     , cs.Name CustomerName
     , sc.Code SectionCode
     , sc.Name SectionName
     , cr.Code CurrencyCode
     , rc.UseCashOnDueDates
     , rc.Code CategoryCode
     , rc.Name CategoryName
     , rc.UseAdvanceReceived
     , rc.MatchingOrder
     , COALESCE(ec.Name, N'') ExcludeCategoryName
     , n.ReceiptId NettingId
  FROM (
SELECT r.*
  FROM WorkCollation wc
 INNER JOIN WorkReceiptTarget rt    ON wc.ClientKey             = @ClientKey
                                   AND wc.CompanyId             = @CompanyId
                                   AND wc.CurrencyId            = @CurrencyId
                                   AND wc.ParentCustomerId      = COALESCE(@ParentCustomerId, 0)
                                   AND wc.PaymentAgencyId       = COALESCE(@PaymentAgencyId , 0)
                                   AND rt.ClientKey             = wc.ClientKey
                                   AND rt.CompanyId             = wc.CompanyId
                                   AND rt.CurrencyId            = wc.CurrencyId
                                   AND rt.PayerName             = wc.PayerName
                                   AND rt.BankCode              = wc.BankCode
                                   AND rt.BranchCode            = wc.BranchCode
                                   AND rt.PayerCode             = wc.PayerCode
                                   AND rt.SourceBankName        = wc.SourceBankName
                                   AND rt.SourceBranchName      = wc.SourceBranchName
                                   AND rt.CollationKey          = wc.CollationKey
                                   AND rt.CustomerId            = wc.CustomerId
                                   AND rt.CollationType         = wc.CollationType
 INNER JOIN Receipt r               ON  r.Id                    = rt.ReceiptId
  LEFT JOIN ReceiptHeader rh        ON rh.Id                    = r.ReceiptHeaderId
       ) r
  LEFT JOIN ReceiptMemo rm      ON   r.Id       = rm.ReceiptId
  LEFT JOIN Customer cs         ON  cs.Id       = r.CustomerId
  LEFT JOIN PaymentAgency pa    ON  pa.Id       = @PaymentAgencyId
  LEFT JOIN Currency cr         ON  cr.Id       = r.CurrencyId
  LEFT JOIN Section sc          ON  sc.Id       = r.SectionId
  LEFT JOIN BankAccountType bt  ON  bt.Id       = r.AccountTypeId
  LEFT JOIN Category rc         ON  rc.Id       = r.ReceiptCategoryId
  LEFT JOIN Category ec         ON  ec.Id       = r.ExcludeCategoryId
  LEFT JOIN Netting n           ON   r.Id       = n.ReceiptId
");
            if (searchOption.UseScheduledPayment == 1)
                query.Append(@"
 UNION ALL
SELECT COALESCE(n.ReceiptId, 0) [Id]
     , n.CompanyId
     , n.CurrencyId
     , c.Kana   [PayerName]
     , c.Kana   [PayerNameRaw]
     , n.Amount [ReceiptAmount]
     , 0 [AssignmentAmount]
     , 0 [AssignmentFlag]
     , n.Amount [RemainAmount]
     , N'' [SourceBankName]
     , N'' [SourceBranchName]
     , n.RecordedAt
     , n.ReceiptCategoryId
     , null [CreateAt]
     , null [UpdateAt]
     , n.DueAt
     , n.CustomerId
     , null [OriginalReceiptId]
     , n.Note [Note1]
     , null [Note2]
     , null [Note3]
     , null [Note4]
     , null [BillNumber]
     , null [BillBankCode]
     , null [BillBranchCode]
     , null [BillDrawAt]
     , null [BillDrawer]
     , null [ReceiptHeaderId]
     , N''  [BankCode]
     , N''  [BankName]
     , N''  [BranchCode]
     , N''  [BranchName]
     , null [AccountTypeId]
     , null [AccountNumber]
     , null [AccountTypeName]
     , n.[ReceiptMemo] /* performance issue cardinarity estimate */
     , c.Code [CustomerCode]
     , c.Name [CustomerName]
     , s.Code [SectionCode]
     , s.Name [SectionName]
     , cur.Code as CurrencyCode
     , rc.UseCashOnDueDates
     , rc.Code [CategoryCode]
     , rc.Name [CategoryName]
     , rc.UseAdvanceReceived
     , rc.MatchingOrder
     , null [ExcludeCategoryName]
     , n.Id [NettingId]
  FROM WorkNettingTarget nt
 INNER JOIN Netting n           ON nt.ClientKey         = @ClientKey
                               AND nt.NettingId         = n.Id
                               AND n.CurrencyId         = @CurrencyId
 INNER JOIN Customer c          ON c.Id                 = n.CustomerId
 INNER JOIN Category rc         ON rc.Id                = n.ReceiptCategoryId
  LEFT JOIN Section s           ON s.Id                 = n.SectionId
  LEFT JOIN Currency cur        ON cur.Id               = n.CurrencyId
  LEFT JOIN CustomerGroup cg    ON cg.ChildCustomerId   = n.CustomerId
 WHERE EXISTS (
       SELECT 1
         FROM WorkCollation wc
        WHERE wc.ClientKey          = nt.ClientKey
          AND wc.CompanyId          = n.CompanyId
          AND wc.CurrencyId         = n.CurrencyId
          AND wc.ParentCustomerId   = @ParentCustomerId
          AND wc.CustomerId         = n.CustomerId
          AND wc.CollationType      = nt.CollationType )");

            query.Insert(0, @"
SELECT r.*
  FROM (
");
            query.Append(@"
       ) r
");
            var order = orders.GetOrderByQuery(x =>
            {
                var item = string.Empty;
                switch (x.ItemName)
                {
                    case "NettingFlag": item = "CASE WHEN r.NettingId IS NULL THEN 0 ELSE 1 END"; break;
                    case "ReceiptRemainSign": item = "CASE WHEN r.RemainAmount < 0 THEN 0 ELSE 1 END"; break;
                    case "RecordedAt": item = "r.RecordedAt"; break;
                    case "PayerName": item = "r.PayerName"; break;
                    case "SourceBankName": item = "r.SourceBankName"; break;
                    case "SourceBranchName": item = "r.SourceBranchName"; break;
                    case "ReceiptRemainAmount": item = "r.RemainAmount * CASE WHEN r.RemainAmount < 0 THEN -1 ELSE 1 END"; break;
                    case "ReceiptCategory": item = "r.MatchingOrder"; break;
                }
                return item;
            });
            if (!string.IsNullOrEmpty(order))
                query.Append(order);
            //               query.Append(@"
            //OPTION ( FORCE ORDER )");
            return dbHelper.GetItemsAsync<Receipt>(query.ToString(), searchOption, token);
        }


        public Task<IEnumerable<Receipt>> GetReceiptsForMatchingAsync(MatchingReceiptSearch option, CancellationToken token = default(CancellationToken))
        {
            var whereCondition = " ";
            var query = " ";
            var netting = "";

            var order = option.Orders.GetOrderByQuery(x =>
            {
                var item = string.Empty;
                switch (x.ItemName)
                {
                    case "NettingFlag":
                        if (option.UseScheduledPayment == 0) return item;
                        item = "CASE WHEN r.Id IS NULL THEN 0 ELSE 1 END"; break;
                    case "ReceiptRemainSign"    : item = "CASE WHEN r.RemainAmount < 0 THEN 0 ELSE 1 END"; break;
                    case "RecordedAt"           : item = "r.RecordedAt"; break;
                    case "PayerName"            : item = "r.PayerName"; break;
                    case "SourceBankName"       : item = "r.SourceBankName"; break;
                    case "SourceBranchName"     : item = "r.SourceBranchName"; break;
                    case "ReceiptRemainAmount"  : item = "r.RemainAmount * CASE WHEN r.RemainAmount < 0 THEN -1 ELSE 1 END"; break;
                    case "ReceiptCategory"      : item = "r.MatchingOrder"; break;
                }
                return item;
            });
            if (option.CompanyId != 0)
            {
                whereCondition += " WHERE r.CompanyId = @CompanyId";
            }
            if (option.ClientKey != null)
            {
                whereCondition += " AND rt.ClientKey = @ClientKey";
            }
            if (option.RecordedAtFrom.HasValue)
            {
                whereCondition += " AND COALESCE( r.ProcessingAt, r.RecordedAt ) >=  @RecordedAtFrom";
            }
            if (option.RecordedAtTo.HasValue)
            {
                whereCondition += " AND COALESCE( r.ProcessingAt, r.RecordedAt ) <=  @RecordedAtTo";
            }
            if (option.UseCashOnDueDates != 0 && option.BillingDataType == 2)
            {
                whereCondition += " AND r.DueAt IS NULL";
            }
            if (option.UseReceiptSection != 0)
            {
                whereCondition += @"
   AND r.SectionId IN (
       SELECT st.SectionId
         FROM WorkSectionTarget st
        WHERE st.ClientKey = rt.ClientKey
          AND st.UseCollation = 1 )";
            }

            if (option.PaymentAgencyId == 0
            && option.ParentCustomerId == 0)
            {
                whereCondition += @"
   AND r.PayerName = @PayerName";
                netting = @"
 WHERE n.AssignmentFlag = 0
   AND c.Kana = @PayerName
";

            }
            else
            {
                netting = @"
 WHERE n.AssignmentFlag = 0
   AND EXISTS (
       SELECT 1
         FROM WorkCollation co
         WHERE n.CurrencyId         = co.CurrencyId
           AND n.CustomerId         = co.CustomerId
           AND nt.ClientKey         = co.ClientKey
           AND nt.CollationType     = co.CollationType
           AND co.ParentCustomerId  = @ParentCustomerId )";
            }

            whereCondition += @"
   AND r.Apportioned = 1
   AND r.RemainAmount <> 0
   AND r.DeleteAt is null
   AND EXISTS (
       SELECT 1
         FROM WorkCollation c
        WHERE c.ClientKey           = rt.ClientKey
          AND c.CompanyId           = r.CompanyId
          AND c.CurrencyId          = r.CurrencyId
          AND c.ParentCustomerId    = @ParentCustomerId
          AND c.PaymentAgencyId     = @PaymentAgencyId
          AND c.PayerName           = r.PayerName
          AND c.PayerCode           = r.PayerCode
          AND c.BankCode            = r.BankCode
          AND c.BranchCode          = r.BranchCode
          AND c.SourceBankName      = r.SourceBankName
          AND c.SourceBranchName    = r.SourceBranchName
          AND c.CustomerId          = COALESCE(r.CustomerId, 0)
          AND c.Collationkey        = r.CollationKey
          AND c.CollationType       = rt.CollationType )";

            query = @"
SELECT
  r.Id
, r.CompanyId
, r.CurrencyId
, r.PayerName
, r.PayerNameRaw
, r.ReceiptAmount
, r.AssignmentAmount
, r.AssignmentFlag
, r.RemainAmount
, r.SourceBankName
, r.SourceBranchName
, r.RecordedAt
, r.ReceiptCategoryId
, r.CreateAt
, r.DueAt
, r.CustomerId
, r.OriginalReceiptId
, r.Note1
, r.Note2
, r.Note3
, r.Note4
, r.BillNumber
, r.BillBankCode
, r.BillBranchCode
, r.BillDrawAt
, r.BillDrawer
, r.ReceiptHeaderId
, r.BankCode
, r.BankName
, r.BranchCode
, r.BranchName
, r.AccountTypeId
, r.AccountNumber
, r.UpdateAt
, bat.Name as AccountTypeName
, rmemo.Memo as ReceiptMemo
, cu.Name as CustomerName
, cu.Code as CustomerCode
, se.Code as SectionCode
, se.Name as SectionName
, cur.Code as CurrencyCode
, rc.UseCashOnDueDates
, rc.Name as CategoryName
, rc.Code as CategoryCode
, rc.UseAdvanceReceived
, rc.MatchingOrder
, (CASE WHEN r.ExcludeCategoryId IS NOT NULL  THEN (SELECT Name FROM Category WHERE  Id = r.ExcludeCategoryId ) ELSE '' END ) as ExcludeCategoryName
, n.ReceiptId [NettingId]
, r.ProcessingAt
 FROM WorkReceiptTarget rt
INNER JOIN Receipt r            ON   r.Id = rt.ReceiptId
AND r.CurrencyId = @CurrencyId
LEFT JOIN ReceiptMemo rmemo     ON   r.Id = rmemo.ReceiptId
LEFT JOIN Customer cu           ON  cu.Id = r.CustomerId
LEFT JOIN PaymentAgency pa      ON  pa.Id = @PaymentAgencyId
LEFT JOIN Currency as cur       ON cur.Id = r.CurrencyId
LEFT JOIN Section se            ON  se.Id = r.SectionId
LEFT JOIN ReceiptHeader rh      ON  rh.Id = r.ReceiptHeaderId
LEFT JOIN BankAccountType bat   ON bat.Id = r.AccountTypeId
INNER JOIN Category rc          ON  rc.Id = r.ReceiptCategoryId
LEFT JOIN Netting n             ON   r.Id = n.ReceiptId";

            whereCondition += @" UNION ALL
SELECT
  CASE WHEN n.ReceiptId IS NOT NULL THEN n.ReceiptId ELSE 0 END [Id]
, n.CompanyId
, n.CurrencyId
, c.Kana[PayerName]
, c.Kana[PayerNameRaw]
, n.Amount[ReceiptAmount]
, 0 [AssignmentAmount]
, 0 [AssignmentFlag]
, n.Amount [RemainAmount]
, N'' [SourceBankName]
, N'' [SourceBranchName]
, n.RecordedAt
, n.ReceiptCategoryId
, null [CreateAt]
, n.DueAt
, n.CustomerId
, null [OriginalReceiptId]
, n.Note [Note1]
, null [Note2]
, null [Note3]
, null [Note4]
, null [BillNumber]
, null [BillBankCode]
, null [BillBranchCode]
, null [BillDrawAt]
, null [BillDrawer]
, null [ReceiptHeaderId]
, N''  [BankCode]
, N''  [BankName]
, N''  [BranchCode]
, N''  [BranchName]
, null [AccountTypeId]
, null [AccountNumber]
, null [UpdateAt]
, null [AccountTypeName]
, n.[ReceiptMemo]
, c.Name [CustomerName]
, c.Code [CustomerCode]
, s.Code [SectionCode]
, s.Name [SectionName]
, cur.Code as CurrencyCode
, rc.UseCashOnDueDates
, rc.Name [CategoryName]
, rc.Code [CategoryCode]
, rc.UseAdvanceReceived
, rc.MatchingOrder
, null [ExcludeCategoryName]
, n.Id [NettingId]
, null [ProcessingAt]
  FROM WorkNettingTarget nt
 INNER JOIN Netting n           ON nt.ClientKey = @ClientKey
   AND nt.NettingId = n.Id
   AND n.CurrencyId = @CurrencyId
 INNER JOIN Customer c          ON c.Id     = n.CustomerId
 INNER JOIN Category rc         ON rc.Id    = n.ReceiptCategoryId
  LEFT JOIN Section s           ON s.Id     = n.SectionId
  LEFT JOIN Currency as cur     ON n.CurrencyId = cur.Id
  LEFT JOIN CustomerGroup cg    ON cg.ChildCustomerId   = n.CustomerId ";
            query = "SELECT * FROM(" + query + whereCondition + netting + ")r" + order;

            return dbHelper.GetItemsAsync<Receipt>(query, option, token);
        }



        public Task<IEnumerable<Billing>> GetBillingsForMatchingAsync(MatchingBillingSearch option, CancellationToken token = default(CancellationToken))
        {

            var query = $@"
SELECT
  b.Id
, b.CompanyId
, b.CurrencyId
, b.CustomerId
, b.BillingAmount
, b.RemainAmount
, (b.RemainAmount - b.OffsetAmount) as TargetAmount
, b.OffsetAmount
, b.AssignmentAmount
, b.BilledAt
, b.DueAt
, b.Note1
, b.Note2
, b.Note3
, b.Note4
, b.Note5
, b.Note6
, b.Note7
, b.Note8
, cs.Name as CustomerName
, cs.Code as CustomerCode
, cs.Kana as CustomerKana
, cs.IsParent
, b.InvoiceCode
, b.InputType
, b.SalesAt
, b.ClosingAt
, b.DepartmentId
, b.BillingCategoryId
, b.CollectCategoryId
, b.DebitAccountTitleId
, b.CreditAccountTitleId
, b.TaxClassId
, b.StaffId
, b.UpdateBy
, b.UpdateAt
, bc.Code   BillingCategoryCode
, bc.Name   BillingCategoryName
, dep.Name  DepartmentName
, dep.Code  DepartmentCode
, staf.Code StaffCode
, staf.Name StaffName
, bm.Memo
, b.InputType
, b.ScheduledPaymentKey
, COALESCE(d.DiscountAmount , 0) DiscountAmount
, COALESCE(d.DiscountAmount1, 0) DiscountAmount1
, COALESCE(d.DiscountAmount2, 0) DiscountAmount2
, COALESCE(d.DiscountAmount3, 0) DiscountAmount3
, COALESCE(d.DiscountAmount4, 0) DiscountAmount4
, COALESCE(d.DiscountAmount5, 0) DiscountAmount5
, pa.Name PaymentAgencyName
, pa.Kana PaymentAgencyKana
, COALESCE(pcs.Id  , cs.Id  ) ParentCustomerId
, COALESCE(pcs.Code, cs.Code) ParentCustomerCode
, COALESCE(pcs.Name, cs.Name) ParentCustomerName
, COALESCE(pcs.Kana, cs.Kana) ParentCustomerKana
FROM Billing b 
 INNER JOIN Customer cs             ON   cs.Id = b.CustomerId
  LEFT JOIN Department dep          ON  dep.Id = b.DepartmentId
  LEFT JOIN Staff staf              ON staf.Id = b.StaffId
  LEFT JOIN BillingMemo bm          ON    b.Id = bm.BillingId
 INNER JOIN Category bc             ON   bc.Id = b.BillingCategoryId
 INNER JOIN Category cc             ON   cc.Id = b.CollectCategoryId
  LEFT JOIN (
       SELECT BillingId
            , SUM(DiscountAmount) DiscountAmount
            , SUM(CASE WHEN DiscountType = 1 THEN DiscountAmount ELSE 0 END) DiscountAmount1
            , SUM(CASE WHEN DiscountType = 2 THEN DiscountAmount ELSE 0 END) DiscountAmount2
            , SUM(CASE WHEN DiscountType = 3 THEN DiscountAmount ELSE 0 END) DiscountAmount3
            , SUM(CASE WHEN DiscountType = 4 THEN DiscountAmount ELSE 0 END) DiscountAmount4
            , SUM(CASE WHEN DiscountType = 5 THEN DiscountAmount ELSE 0 END) DiscountAmount5
         FROM BillingDiscount
        WHERE AssignmentFlag = 0
         GROUP BY
              BillingId
       ) d                          ON b.Id     = d.BillingId
  LEFT JOIN PaymentAgency pa        ON pa.Id    = cc.PaymentAgencyId
  LEFT JOIN CustomerGroup csg       ON cs.Id    = csg.ChildCustomerId
  LEFT JOIN Customer pcs            ON pcs.Id   = csg.ParentCustomerId
 WHERE b.CompanyId          = @CompanyId
   AND b.DeleteAt           IS NULL
   AND (b.RemainAmount      <> b.OffsetAmount)";
            if (option.DueAtFrom.HasValue) query += @"
   AND b.DueAt              >=  @DueAtFrom";
            if (option.DueAtTo.HasValue) query += @"
   AND b.DueAt              <=  @DueAtTo";
            if (option.CurrencyId.HasValue) query += @"
   AND b.CurrencyId         =  @CurrencyId";
            if (option.PaymentAgencyId != 0) query += @"
   AND pa.Id                = @PaymentAgencyId
   AND b.ResultCode         = 0";
            else query += @"
   AND COALESCE(pcs.Id, cs.Id) = @ParentCustomerId";

            if (option.UseDepartmentWork) query += @"
   AND b.DepartmentId       IN (
       SELECT wdt.DepartmentId
         FROM WorkDepartmentTarget wdt
        WHERE wdt.ClientKey     = @ClientKey
          AND wdt.UseCollation  = 1 )";

            if (option.UseCashOnDueDates == 1)
            {
                if (option.BillingDataType == 1) query += @"
   AND b.InputType          <> 3";
                else if (option.BillingDataType == 2) query += @"
   AND b.InputType          = 3";
            }
            if (option.ParentCustomerId != 0 && option.ParentCustomerId.HasValue) query += @"
   AND cc.UseAccountTransfer = 0";
            else if (option.PaymentAgencyId != 0 && option.PaymentAgencyId.HasValue) query += @"
   AND cc.UseAccountTransfer = 1";
            var order = option.Orders.GetOrderByQuery(x =>
            {
                var item = string.Empty;
                switch (x.ItemName)
                {
                    case "BillingRemainSign"    : item = "CASE WHEN (b.RemainAmount - b.OffsetAmount) < 0 THEN 0 ELSE 1 END"; break;
                    case "CashOnDueDatesFlag"   : item = "CASE WHEN b.InputType = 3 THEN 1 ELSE 0 END"; break;
                    case "DueAt"                : item = "b.DueAt"; break;
                    case "CustomerCode"         : item = "cs.Code"; break;
                    case "BilledAt"             : item = "b.BilledAt"; break;
                    case "BillingRemainAmount"  : item = "(b.RemainAmount - b.OffsetAmount) * CASE WHEN (b.RemainAmount - b.OffsetAmount) < 0 THEN -1 ELSE 1 END"; break;
                    case "BillingCategory"      : item = "bc.MatchingOrder"; break;
                }
                return item;
            });
            if (!string.IsNullOrEmpty(order)) query += order;

            return dbHelper.GetItemsAsync<Billing>(query, new
                {
                    ClientKey = option.ClientKey,
                    CompanyId = option.CompanyId,
                    CurrencyId = option.CurrencyId,
                    DueAtFrom = option.DueAtFrom,
                    DueAtTo = option.DueAtTo,
                    ParentCustomerId = option.ParentCustomerId,
                    PaymentAgencyId = option.PaymentAgencyId,
                    BillingDataType = option.BillingDataType,
                    DepartmentId = option.DepartmentId

                }, token);
        }


        public Task<IEnumerable<MatchingHeader>> SearchMatchedDataAsync(CollationSearch option, CancellationToken token = default(CancellationToken))
        {
            var receiptDisplayOrder = "";
            switch (option.SortOrderDirection)
            {
                case SortOrderColumnType.MinRecordedAt:
                    receiptDisplayOrder = "COALESCE(m.MinReceiptRecordedAt, '9999/12/31') ASC";
                    break;
                case SortOrderColumnType.MaxRecordedAt:
                    receiptDisplayOrder = "COALESCE(m.MaxReceiptRecordedAt, '9999/12/31') DESC";
                    break;
                case SortOrderColumnType.MinReceiptId:
                    receiptDisplayOrder = "m.MinReceiptId ASC";
                    break;
                case SortOrderColumnType.MaxReceiptId:
                    receiptDisplayOrder = "m.MaxReceiptId DESC";
                    break;
                case SortOrderColumnType.PayerNameAsc:
                    receiptDisplayOrder = "m.[PayerName] ASC";
                    break;
                case SortOrderColumnType.PayerNameDesc:
                    receiptDisplayOrder = "m.[PayerName] DESC";
                    break;
                default:
                    receiptDisplayOrder = "COALESCE(pa.[Code], cs.[Code]) ASC";
                    break;
            }

            var query = $@"
SELECT mh.*
     , m.*
     , mh.Amount + mh.BankTransferFee
     - CASE WHEN mh.TaxDifference < 0 THEN mh.TaxDifference ELSE 0 END [BillingAmount]
     , mh.Amount
     + CASE WHEN mh.TaxDifference < 0 THEN 0 ELSE mh.TaxDifference END [ReceiptAmount]
     , cs.[Code] [CustomerCode]
     , cs.[Name] [CustomerName]
     , pa.[Code] [PaymentAgencyCode]
     , pa.[Name] [PaymentAgencyName]
     , cr.[Code] [CurrencyCode]
     , COALESCE(pa.[Code], cs.[Code]) [DispCustomerCode]
     , COALESCE(pa.[Name], cs.[Name]) [DispCustomerName]
     , COALESCE(pa.ShareTransferFee, cs.ShareTransferFee) [ShareTransferFee]
     , ROW_NUMBER() OVER (ORDER BY COALESCE(pa.Code, cs.Code))    [BillingDisplayOrder]
     , ROW_NUMBER() OVER (ORDER BY {receiptDisplayOrder} )        [ReceiptDisplayOrder]
  FROM (
SELECT m.MatchingHeaderId
     , MIN( r.PayerName ) [PayerName]
     , MAX( m.UpdateAt  ) [MatchingUpdateAt]
     , MIN(m.RecordedAt) [MinReceiptRecordedAt]
     , MAX(m.RecordedAt) [MaxReceiptRecordedAt]
     , MIN(m.ReceiptId)  [MinReceiptId]
     , MAX(m.ReceiptId)  [MaxReceiptId]
  FROM Matching m
 INNER JOIN MatchingHeader mh   ON mh.Id                = m.MatchingHeaderId
                               AND mh.CompanyId         = @CompanyId
                               AND mh.CurrencyId        = COALESCE(@CurrencyId, mh.CurrencyId)
                               AND mh.Approved          = @Approved
                               AND m.RecordedAt        >= COALESCE(@RecordedAtFrom, m.RecordedAt)
                               AND m.RecordedAt        <= COALESCE(@RecordedAtTo  , m.RecordedAt)
                               AND mh.CreateAt         >= COALESCE(@CreateAtFrom, mh.CreateAt)
                               AND mh.CreateAt         <= COALESCE(@CreateAtTo  , mh.CreateAt)
 INNER JOIN Receipt r           ON r.Id                 = m.ReceiptId
                               AND (
                                        @UseSectionWork = 0
                                     OR @UseSectionWork = 1 AND r.SectionId IN (
                                        SELECT DISTINCT SectionID
                                          FROM WorkSectionTarget wst
                                         WHERE wst.ClientKey        = @ClientKey
                                           AND wst.UseCollation     = 1 )
                                   )
 INNER JOIN Billing b           ON b.Id                 = m.BillingId
                               AND b.DueAt             >= COALESCE(@DueAtFrom, b.DueAt)
                               AND b.DueAt             <= COALESCE(@DueAtTo, b.DueAt)
                               AND (
                                        @BillingType    = 0
                                     OR @BillingType    = 1 AND b.InputType <> 3
                                     OR @BillingType    = 2 AND b.InputType  = 3
                                   )
                               AND (
                                        @UseDepartmentWork = 0
                                     OR @UseDepartmentWork = 1 AND b.DepartmentId IN (
                                        SELECT DISTINCT DepartmentId
                                          FROM WorkDepartmentTarget wdt
                                         WHERE wdt.ClientKey        = @ClientKey
                                           AND wdt.UseCollation     = 1 )
                                   )
  LEFT JOIN Matching m2         ON m2.MatchingHeaderId  = m.MatchingHeaderId
                               AND m2.OutputAt         IS NOT NULL
 WHERE m2.Id        IS NULL
 GROUP BY m.MatchingHeaderId
       ) m
 INNER JOIN MatchingHeader mh   ON mh.Id                = m.MatchingHeaderId
  LEFT JOIN Customer cs         ON cs.Id                = mh.CustomerId
  LEFT JOIN PaymentAgency pa    ON pa.Id                = mh.PaymentAgencyId
 INNER JOIN Currency cr         ON cr.Id                = mh.CurrencyId
";
            return dbHelper.GetItemsAsync<MatchingHeader>(query, option, token);
        }

        public Task<IEnumerable<Netting>> SearchMatchingNettingAsync(CollationSearch CollationSearch, Collation Collation, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT nt.Id
     , nt.CompanyId
     , nt.CurrencyId
     , nt.ReceiptCategoryId
     , nt.SectionId
     , nt.RecordedAt
     , nt.Amount
     , nt.DueAt
     , nt.ReceiptMemo
     , nt.CustomerId
     , nt.Note
     , COALESCE(pcs.Kana, ccs.Kana) [CustomerKana]
  FROM Netting nt
  LEFT JOIN Currency cy         ON  cy.Id   = nt.CurrencyId
  LEFT JOIN Customer ccs        ON ccs.Id   = nt.CustomerId
  LEFT JOIN CustomerGroup cg    ON ccs.Id   = cg.ChildCustomerId
  LEFT JOIN Customer pcs        ON pcs.Id   = cg.ParentCustomerId
 WHERE nt.CompanyId             = @CompanyId
   AND nt.RecordedAt           >= COALESCE(@RecordedAtFrom, nt.RecordedAt)
   AND nt.RecordedAt           <= @RecordedAtTo
   AND cy.Code                  = @CurrencyCode
   AND COALESCE(pcs.Id, ccs.Id) = @CustomerId
   AND nt.AssignmentFlag        = 0
   AND nt.ReceiptId            IS NULL
";
            return dbHelper.GetItemsAsync<Netting>(query, new
                {
                    RecordedAtFrom = CollationSearch.RecordedAtFrom,
                    RecordedAtTo = CollationSearch.RecordedAtTo,
                    CompanyId = CollationSearch.CompanyId,
                    CustomerId = Collation.CustomerId,
                    CurrencyCode = Collation.CurrencyCode
                }, token);
        }

        #region query create advance received
        private string GetQueryCreateAdvanceReceived() => @"
INSERT INTO Receipt
(CompanyId
,CurrencyId
,ReceiptHeaderId
,ReceiptCategoryId
,CustomerId
,SectionId
,InputType
,Apportioned
,Approved
,Workday
,RecordedAt
,OriginalRecordedAt
,ReceiptAmount
,AssignmentAmount
,RemainAmount
,AssignmentFlag
,PayerCode
,PayerName
,PayerNameRaw
,SourceBankName
,SourceBranchName
,OutputAt
,DueAt
,MailedAt
,OriginalReceiptId
,ExcludeFlag
,ExcludeCategoryId
,ExcludeAmount
,ReferenceNumber
,RecordNumber
,DensaiRegisterAt
,Note1
,Note2
,Note3
,Note4
,BillNumber
,BillBankCode
,BillBranchCode
,BillDrawAt
,BillDrawer
,DeleteAt
,CreateBy
,CreateAt
,UpdateBy
,UpdateAt)
 OUTPUT inserted.* 
VALUES (
@CompanyId
,@CurrencyId
,@ReceiptHeaderId
,@ReceiptCategoryId
,@CustomerId
,@SectionId
,@InputType
,@Apportioned
,@Approved
,@Workday
,@RecordedAt
,@OriginalRecordedAt
,@ReceiptAmount
,@AssignmentAmount
,@RemainAmount
,@AssignmentFlag
,@PayerCode
,@PayerName
,@PayerNameRaw
,@SourceBankName
,@SourceBranchName
,@OutputAt
,@DueAt
,@MailedAt
,@OriginalReceiptId
,@ExcludeFlag
,@ExcludeCategoryId
,@ExcludeAmount
,@ReferenceNumber
,@RecordNumber
,@DensaiRegisterAt
,@Note1
,@Note2
,@Note3
,@Note4
,@BillNumber
,@BillBankCode
,@BillBranchCode
,@BillDrawAt
,@BillDrawer
,@DeleteAt
,@CreateBy
,@CreateAt
,@UpdateBy
,@UpdateAt)
";
        #endregion
        public Task<Receipt> SaveMatchingReceiptAsync(ReceiptInput ReceiptInput, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync<Receipt>(GetQueryCreateAdvanceReceived(), ReceiptInput, token);


        public Task<IEnumerable<Billing>> GetMatchedBillingsForCancelAsync(long headerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT b.Id
     , b.BillingAmount
     , b.RemainAmount
     , b.OffsetAmount
     , b.AssignmentFlag
     , b.DeleteAt
     , m.AssignmentAmount
     , m.TaxDifference
     , m.BankTransferFee
     , m.DiscountAmount
  FROM (
       SELECT m.BillingId
            , SUM(m.Amount)             [AssignmentAmount]
            , SUM(m.TaxDifference)      [TaxDifference]
            , SUM(m.BankTransferFee)    [BankTransferFee]
            , SUM(mbd.DiscountAmount)   [DiscountAmount]
         FROM Matching m
         LEFT JOIN MatchingBillingDiscount mbd
           ON mbd.MatchingId        = m.Id
        WHERE m.MatchingHeaderId    = @headerId
        GROUP BY m.BillingId
              ) m
 INNER JOIN Billing b
    ON b.Id         = m.BillingId
";
            return dbHelper.GetItemsAsync<Billing>(query, new { headerId }, token);
        }

        public Task<IEnumerable<Billing>> GetMatchedBillingsAsync(MatchingBillingSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = " ";
            var order = option.Orders.GetOrderByQuery(x =>
            {
                var item = string.Empty;
                switch (x.ItemName)
                {
                    case "BillingRemainSign"    : item = "CASE WHEN (b.RemainAmount - b.OffsetAmount) < 0 THEN 0 ELSE 1 END "; break;
                    case "CashOnDueDatesFlag"   : item = "CASE b.InputType WHEN 3 THEN 1 ELSE 0 END"; break;
                    case "DueAt"                : item = "b.DueAt"; break;
                    case "CustomerCode"         : item = "cu.Code"; break;
                    case "BilledAt"             : item = "b.BilledAt"; break;
                    case "BillingRemainAmount"  : item = "(b.RemainAmount - b.OffsetAmount) * CASE WHEN (b.RemainAmount - b.OffsetAmount) < 0 THEN -1 ELSE 1 END"; break;
                    case "BillingCategory"      : item = "bc.MatchingOrder"; break;
                }
                return item;
            });
            query = @"
SELECT b.CompanyId
     , b.Id
     , b.CustomerId
     , b.CurrencyId
     , b.BillingAmount
     , b.RemainAmount
     , (b.RemainAmount - b.OffsetAmount) as TargetAmount
     , b.OffsetAmount
     , b.AssignmentAmount
     , b.BilledAt
     , b.DueAt
     , b.Note1
     , b.Note2
     , b.Note3
     , b.Note4
     , b.Note5
     , b.Note6
     , b.Note7
     , b.Note8
     , cu.Name as CustomerName
     , cu.Code as CustomerCode
     , cu.Kana as CustomerKana
     , b.InvoiceCode
     , b.InputType
     , b.SalesAt
     , b.ClosingAt
     , b.DepartmentId
     , b.BillingCategoryId
     , b.CollectCategoryId
     , b.DebitAccountTitleId
     , b.CreditAccountTitleId
     , b.TaxClassId
     , b.StaffId
     , b.UpdateBy
     , bc.Code as BillingCategoryCode
     , bc.Name as BillingCategoryName
     , dep.Name as DepartmentName
     , bm.Memo
     , b.InputType
     , b.ScheduledPaymentKey
     , COALESCE(d.DiscountAmount , 0) DiscountAmount
     , COALESCE(d.DiscountAmount1, 0) DiscountAmount1
     , COALESCE(d.DiscountAmount2, 0) DiscountAmount2
     , COALESCE(d.DiscountAmount3, 0) DiscountAmount3
     , COALESCE(d.DiscountAmount4, 0) DiscountAmount4
     , COALESCE(d.DiscountAmount5, 0) DiscountAmount5
     , mh.TaxDifference
     , mh.BankTransferFee ";

            if (option.PaymentAgencyId != 0 && option.PaymentAgencyId.HasValue)
                query += @" ,pa.Name as PaymentAgencyName, pa.Kana as PaymentAgencyKana, cu.Code as ParentCode, cu.Kana as ParentKana";
            else
            {
                if (option.IsParent == 1)
                    query += @",coalesce(pc.Code, cu.Code) as ParentCode, coalesce(pc.Kana, cu.Kana) as ParentKana";
                else if (option.IsParent == 0 && option.ParentCustomerId != 0 && option.ParentCustomerId.HasValue)
                    query += @",cu.Code as ParentCode, cu.Kana as ParentKana";
            }

            query += @"
  FROM MatchingHeader mh
 INNER JOIN Billing b 
    ON mh.Id = @MatchingHeaderId
   AND EXISTS (
       SELECT 1
         FROM Matching m
        WHERE m.MatchingHeaderId   = mh.Id
          AND m.BillingId          = b.Id )
 INNER JOIN Customer cu ON b.CustomerId = cu.Id 
 INNER JOIN Category cc ON b.CollectCategoryId = cc.Id
  LEFT JOIN Department dep ON dep.Id = b.DepartmentId
  LEFT JOIN BillingMemo bm ON b.Id = bm.BillingId
 INNER JOIN Category bc ON b.BillingCategoryId = bc.Id
  LEFT JOIN (
       SELECT mat.MatchingHeaderId
            , mat.BillingId
            , SUM(mbd.DiscountAmount) DiscountAmount
            , SUM(CASE mbd.DiscountType WHEN 1 THEN mbd.DiscountAmount ELSE 0 END) DiscountAmount1
            , SUM(CASE mbd.DiscountType WHEN 2 THEN mbd.DiscountAmount ELSE 0 END) DiscountAmount2
            , SUM(CASE mbd.DiscountType WHEN 3 THEN mbd.DiscountAmount ELSE 0 END) DiscountAmount3
            , SUM(CASE mbd.DiscountType WHEN 4 THEN mbd.DiscountAmount ELSE 0 END) DiscountAmount4
            , SUM(CASE mbd.DiscountType WHEN 5 THEN mbd.DiscountAmount ELSE 0 END) DiscountAmount5
         FROM MatchingBillingDiscount mbd
        INNER JOIN Matching mat         ON mat.Id   = mbd.MatchingId
        GROUP BY
              mat.MatchingHeaderId
            , mat.BillingId
       ) d
    ON mh.Id    = d.MatchingHeaderId
   AND b.Id     = d.BillingId ";
            if (option.PaymentAgencyId != 0 && option.PaymentAgencyId.HasValue)
            {
                query += @"
 INNER JOIN PaymentAgency pa ON cc.PaymentAgencyId = pa.Id";
            }
            if (option.PaymentAgencyId == 0 || !option.PaymentAgencyId.HasValue)
            {
                query += @"
  LEFT JOIN CustomerGroup cg    ON b.CustomerId = cg.ChildCustomerId
  LEFT JOIN Customer pc         ON pc.Id        = cg.ParentCustomerId ";

            }
            query += @"
 WHERE b.Id IN (SELECT DISTINCT BillingId FROM Matching WHERE  MatchingHeaderId = @MatchingHeaderId) ";
            query += order;

            return dbHelper.GetItemsAsync<Billing>(query, option, token);
        }

        public Task<IEnumerable<Receipt>> GetMatchgedReceiptsAsync(MatchingReceiptSearch option, CancellationToken token = default(CancellationToken))
        {
            var whereCondition = " ";
            var query = " ";
            var order = option.Orders.GetOrderByQuery(x =>
            {
                var item = string.Empty;
                switch (x.ItemName)
                {
                    case "NettingFlag":
                        item = option.UseScheduledPayment == 0
                            ? "r.Id" : "CASE WHEN n.Id IS NULL THEN 0 ELSE 1 END";
                        break;
                    case "ReceiptRemainSign"    : item = "CASE WHEN r.RemainAmount < 0 THEN 0 ELSE 1 END"; break;
                    case "RecordedAt"           : item = "r.RecordedAt"; break;
                    case "PayerName"            : item = "r.PayerName"; break;
                    case "SourceBankName"       : item = "r.SourceBankName"; break;
                    case "SourceBranchName"     : item = "r.SourceBranchName"; break;
                    case "ReceiptRemainAmount"  : item = "r.RemainAmount * CASE WHEN r.RemainAmount < 0 THEN -1 ELSE 1 END"; break;
                    case "ReceiptCategory"      : item = "rc.MatchingOrder"; break;
                }
                return item;
            });
            query = @"SELECT  r.CompanyId
                            , r.CurrencyId
                            , r.Id
                            , r.PayerName
                            , r.PayerNameRaw
                            , r.ReceiptAmount
                            , r.AssignmentAmount
                            , r.RemainAmount
                            , r.SourceBankName
                            , r.SourceBranchName
                            , r.SourceBankName
                            , r.SourceBranchName
                            , r.RecordedAt
                            , r.ReceiptCategoryId
                            , r.CreateAt
                            , rc.UseCashOnDueDates
                            , rc.Name as CategoryName
                            , rc.Code as CategoryCode
                            , ec.Name as ExcludeCategoryName
                            , r.DueAt
                            , r.CustomerId
                            , r.OriginalReceiptId
                            , cu.Name as CustomerName
                            , cu.Code as CustomerCode
                            , r.BankCode
                            , r.BankName
                            , r.BranchCode
                            , r.BranchName
                            , r.AccountTypeId
                            , r.AccountNumber
                            , bat.Name as AccountTypeName
                            , se.Code as SectionCode
                            , se.Name as SectionName
                            , r.Note1
                            , rmemo.Memo as ReceiptMemo
                            , r.Note2
                            , r.Note3
                            , r.Note4
                            , r.BillNumber
                            , r.BillBankCode
                            , r.BillBranchCode
                            , r.BillDrawAt
                            , r.BillDrawer ";
            if (option.PaymentAgencyId != 0 && option.PaymentAgencyId.HasValue)
            {
                query += @", pa.UseFeeTolerance as UseFeeTolerance";
            }
            else if (option.ParentCustomerId != 0 && option.ParentCustomerId.HasValue)
            {
                query += @", cu.UseFeeTolerance as UseFeeTolerance";
            }
            if (option.UseScheduledPayment != 0)
            {
                query += @",n.ReceiptId as NettingId";
            }
            query += @"
 FROM Receipt r
 LEFT JOIN ReceiptMemo rmemo        ON   r.Id = rmemo.ReceiptId
 LEFT JOIN Customer cu              ON  cu.Id = r.CustomerId
 LEFT JOIN PaymentAgency pa         ON  pa.Id = @PaymentAgencyId
 LEFT JOIN Section se               ON  se.Id = r.SectionId
 LEFT JOIN ReceiptHeader rh         ON  rh.Id = r.ReceiptHeaderId
 LEFT JOIN BankAccountType bat      ON bat.Id = r.AccountTypeId
INNER JOIN Category rc              ON  rc.Id = r.ReceiptCategoryId
 LEFT JOIN Category ec              ON  ec.Id = r.ExcludeCategoryId
";

            if (option.UseScheduledPayment != 0)
            {
                query += " LEFT JOIN Netting n ON r.Id = n.ReceiptId";
            }

            whereCondition = " WHERE r.Id IN (SELECT DISTINCT(ReceiptId) FROM Matching WHERE  MatchingHeaderId = @MatchingHeaderId) ";

            query = query + @whereCondition + order;

            return dbHelper.GetItemsAsync<Receipt>(query, new
                {
                    ClientKey = option.ClientKey,
                    CompanyId = option.CompanyId,
                    RecordedAt = option.RecordedAtTo,
                    ParentCustomerId = option.ParentCustomerId,
                    PaymentAgencyId = option.PaymentAgencyId,
                    MatchingHeaderId = option.MatchingHeaderId
                }, token);
        }


        private string GetQuerySelectReceipt() => @"
SELECT
 r.Id
,r.CompanyId
,r.CurrencyId
,r.ExcludeFlag
,r.CustomerId
,r.AssignmentFlag
,r.AssignmentAmount
,r.RecordedAt
,r.PayerCode
,r.PayerName
,r.ReceiptAmount
,r.ExcludeAmount
,r.RemainAmount
,r.InputType
,r.Note1
,r.Note2
,r.Note3
,r.Note4
,r.DueAt
,r.SourceBankName
,r.SourceBranchName
,r.OutputAt
,r.BillNumber
,r.BillBankCode
,r.BillBranchCode
,r.BillDrawAt
,r.BillDrawer
,r.OriginalReceiptId
,r.SectionId
,r.ReceiptCategoryId
,r.BankCode
,r.BankName
,r.BranchCode
,r.BranchName
,r.AccountNumber
,r.AccountName
,r.CreateBy
,r.CreateAt
,r.UpdateBy
,r.UpdateAt
,n.ReceiptId as NettingId
,N'' as AccountTypeName
,rc.UseAdvanceReceived
,rc.Code CategoryCode
,rc.Name CategoryName
,rc.UseCashOnDueDates
,r.ExcludeCategoryId
,ec.Name ExcludeCategoryName
,cs.Code CustomerCode
,cs.Name CustomerName
,cr.Code CurrencyCode
,sc.Code SectionCode
,sc.Name SectionName
,rm.Memo ReceiptMemo
,rm.ReceiptId as ReceiptId
,rx.RecExcOutputAt
FROM Receipt as r
LEFT JOIN ReceiptHeader rh  ON rh.Id        = r.ReceiptHeaderId
LEFT JOIN ReceiptMemo rm    ON rm.ReceiptId = r.Id
LEFT JOIN (
           SELECT rx.ReceiptId
                , MAX( CASE WHEN rx.OutputAt IS NULL THEN 0 ELSE 1 END ) [RecExcOutputAt]
             FROM ReceiptExclude rx
            GROUP BY
                  rx.ReceiptId
          ) rx              ON rx.ReceiptId = r.Id
LEFT JOIN Netting n         ON n.ReceiptId  = r.Id
LEFT JOIN Customer cs       ON cs.Id        = r.CustomerId
LEFT JOIN Currency cr       ON cr.Id        = r.CurrencyId
LEFT JOIN Section sc        ON sc.Id        = r.SectionId
LEFT JOIN Category rc       ON rc.Id        = r.ReceiptCategoryId
LEFT JOIN Category ec       ON ec.Id        = r.ExcludeCategoryId 
WHERE r.Id IN (SELECT Id FROM @ReceiptId)
";

        public Task<IEnumerable<Receipt>> SearchReceiptByIdAsync(IEnumerable<long> ReceiptId, CancellationToken token = default(CancellationToken))
            => dbHelper.GetItemsAsync<Receipt>(GetQuerySelectReceipt(), new { ReceiptId = ReceiptId.GetTableParameter() }, token);

        public Task<IEnumerable<Receipt>> GetMatchedReceiptsForCancelAsync(MatchingHeader MatchingHeader, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT r.Id
     , MAX( r.ReceiptAmount     ) [ReceiptAmount]
     , MAX( r.RemainAmount      ) [RemainAmount]
     , MAX( r.DeleteAt          ) [DeleteAt]
     , SUM( m.Amount + CASE WHEN m.TaxDifference > 0 THEN m.TaxDifference ELSE 0 END ) [AssignmentAmount]
  FROM Receipt r
 INNER JOIN Matching m  ON r.Id                 = m.ReceiptId
                       AND m.MatchingHeaderId   = @MatchingHeaderId
 GROUP BY r.Id
";
            return dbHelper.GetItemsAsync<Receipt>(query, new { MatchingHeaderId = MatchingHeader.Id, }, token);
        }

        public Task<IEnumerable<Matching>> GetByHeaderIdAsync(long MatchingHeaderId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT m.*
  FROM Matching m 
 WHERE m.MatchingHeaderId = @MatchingHeaderId
";
            return dbHelper.GetItemsAsync<Matching>(query, new { MatchingHeaderId }, token);
        }

        public Task<Receipt> UpdateReceiptForCancelMatchingAsync(Receipt Receipt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Receipt
   SET RemainAmount     = RemainAmount + @RemainAmount
     , AssignmentAmount = AssignmentAmount - @AssignmentAmount
     , AssignmentFlag   = CASE ReceiptAmount WHEN RemainAmount + @RemainAmount THEN 0 ELSE 1 END
     , UpdateBy         = @UpdateBy
     , UpdateAt         = @UpdateAt
OUTPUT inserted.*
 WHERE Id               = @Id";
            return dbHelper.ExecuteAsync<Receipt>(query, Receipt, token);
        }

        public Task<Billing> UpdateBillingForCancelMatchingAsync(Billing Billing, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Billing
   SET RemainAmount     = RemainAmount      + @RemainAmount
     , AssignmentAmount = AssignmentAmount  - @AssignmentAmount
     , AssignmentFlag   = CASE BillingAmount WHEN RemainAmount + @RemainAmount THEN 0 ELSE 1 END
     , UpdateBy         = @UpdateBy
     , UpdateAt         = @UpdateAt
OUTPUT inserted.*
 WHERE Id               = @Id";
            return dbHelper.ExecuteAsync<Billing>(query, Billing, token);
        }

        public Task<IEnumerable<Matching>> UpdatePreviousBillingLogsAsync(long MatchingHeaderId, long BillingId, decimal Amount, int UpdateBy, DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Matching
   SET BillingRemain    = BillingRemain + @Amount
     , UpdateBy         = @UpdateBy
     , UpdateAt         = @UpdateAt
OUTPUT inserted.*
  FROM Matching m
 WHERE m.BillingId      = @BillingId
   AND m.Id             IN (
       SELECT DISTINCT m1.Id
         FROM Matching m1
        WHERE m1.BillingId          = m.BillingId
          AND m1.MatchingHeaderId   = @MatchingHeaderId
          AND m1.Id                 < m.Id )
";
            return dbHelper.GetItemsAsync<Matching>(query, new { MatchingHeaderId, BillingId, Amount, UpdateBy, UpdateAt }, token);
        }

        public Task<IEnumerable<Matching>> UpdatePreviousReceiptLogsAsync(long MatchingHeaderId, long ReceiptId, decimal Amount, int UpdateBy, DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Matching
   SET ReceiptRemain    = ReceiptRemain + @Amount
     , UpdateBy         = @UpdateBy
     , UpdateAt         = @UpdateAt
OUTPUT inserted.*
  FROM Matching m
 WHERE m.ReceiptId      = @ReceiptId
   AND m.Id            IN (
       SELECT DISTINCT m1.Id
         FROM Matching m1
        WHERE m1.ReceiptId          = m.ReceiptId
          AND m1.MatchingHeaderId   = @MatchingHeaderId
          AND m1.Id                 < m.Id )
";
            return dbHelper.GetItemsAsync<Matching>(query, new { MatchingHeaderId, ReceiptId, Amount, UpdateBy, UpdateAt }, token);

        }

        public Task<int> DeleteMatchingOutputedAsync(long MatchingHeaderId, CancellationToken token = default(CancellationToken))
        {
            var query = @"DELETE
                        FROM MatchingOutputed
                        WHERE MatchingHeaderId = @MatchingHeaderId ";
            return dbHelper.ExecuteAsync(query, new { MatchingHeaderId }, token);
        }


        public Task<int> DeleteMatchingAsync(long matchingHeaderId, DateTime updateAt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
IF NOT EXISTS (
    SELECT 1
      FROM Matching
     WHERE MatchingHeaderId = @matchingHeaderId
       AND UpdateAt         > @updateAt )
DELETE
  FROM Matching
 WHERE MatchingHeaderId = @matchingHeaderId ";
            return dbHelper.ExecuteAsync(query, new { matchingHeaderId, updateAt }, token);
        }
        public Task<int> DeleteBillingShceduledIncomeAsync(long BillingId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE
  FROM BillingScheduledIncome
 WHERE BillingId = @BillingId";
            return dbHelper.ExecuteAsync(query, new { BillingId }, token);
        }

        public async Task<bool> ExistAssignmentScheduledIncomeAsync(IEnumerable<long> MatchingIds, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS (
       SELECT 1
         FROM BillingScheduledIncome bs
        INNER JOIN Billing b
           ON bs.MatchingId    IN (SELECT Id FROM @MatchingIds)
          AND b.Id              = bs.BillingId
          AND b.AssignmentFlag  > 0
           )
";
            return (await dbHelper.ExecuteAsync<int?>(query, new { MatchingIds =MatchingIds .GetTableParameter() }, token)).HasValue;
        }

        public Task<BillingScheduledIncome> GetBillingScheduledIncomeAsync(long MatchingId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM BillingScheduledIncome bs
 WHERE bs.MatchingId = @MatchingId";
            return dbHelper.ExecuteAsync<BillingScheduledIncome>(query, new { MatchingId }, token);
        }


        public Task<int> UpdateBillingForMatchingAsync(Billing Billing, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Billing
   SET RemainAmount     = RemainAmount      - @AssignmentAmount
     , AssignmentAmount = AssignmentAmount  + @AssignmentAmount
     , AssignmentFlag   = CASE RemainAmount WHEN @AssignmentAmount + @OffsetAmount THEN 2 ELSE 1 END
     , OffsetAmount     = @OffsetAmount
     , UpdateBy         = @UpdateBy
     , UpdateAt         = @NewUpdateAt
 WHERE Id               = @Id
   AND UpdateAt         = @UpdateAt";
            return dbHelper.ExecuteAsync(query, Billing, token);
        }

        public Task<int> UpdateReceiptForMatchingAsync(Receipt Receipt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE Receipt
   SET RemainAmount     = RemainAmount      - @AssignmentAmount
     , AssignmentAmount = AssignmentAmount  + @AssignmentAmount
     , AssignmentFlag   = CASE RemainAmount WHEN @AssignmentAmount THEN 2 ELSE 1 END
     , UpdateBy         = @UpdateBy
     , UpdateAt         = @NewUpdateAt
 WHERE Id               = @Id
   AND UpdateAt         = @UpdateAt";
            return dbHelper.ExecuteAsync(query, Receipt, token);
        }

        public Task<int> UpdateReceiptHeaderAsync(long Id, int UpdateBy, DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE ReceiptHeader
   SET AssignmentFlag = 1
     , UpdateBy = @UpdateBy
     , UpdateAt = @UpdateAt
 WHERE Id = @Id";
            return dbHelper.ExecuteAsync(query, new
                {
                    Id,
                    UpdateBy,
                    UpdateAt
                }, token);
        }

        public Task<MatchingHeader> SaveMatchingHeaderAsync(MatchingHeader MatchingHeader, CancellationToken token = default(CancellationToken))
        {
            var query = @"INSERT INTO MatchingHeader(
                        CompanyId,
                        CurrencyId,
                        CustomerId,
                        PaymentAgencyId,
                        Approved,
                        ReceiptCount,
                        BillingCount,
                        Amount,
                        BankTransferFee,
                        TaxDifference,
                        MatchingProcessType,
                        Memo,
                        CreateBy,
                        CreateAt,
                        UpdateBy,
                        UpdateAt
                        )
                        OUTPUT inserted.* 
                        VALUES (
                        @CompanyId,
                        @CurrencyId,
                        @CustomerId,
                        @PaymentAgencyId,
                        @Approved, 
                        @ReceiptCount,
                        @BillingCount,
                        @Amount,
                        @BankTransferFee,
                        @TaxDifference,
                        @MatchingProcessType,
                        @Memo,
                        @CreateBy,
                        @CreateAt,
                        @UpdateBy,
                        @UpdateAt
                        )";
            return dbHelper.ExecuteAsync<MatchingHeader>(query, MatchingHeader, token);
        }

        public Task<Matching> SaveMatchingAsync(Matching Matching, CancellationToken token = default(CancellationToken))
        {
            var query = @"INSERT INTO Matching(
                    ReceiptId,
                    BillingId,
                    MatchingHeaderId,
                    BankTransferFee,
                    Amount,
                    BillingRemain,
                    ReceiptRemain,
                    AdvanceReceivedOccured,
                    RecordedAt,
                    TaxDifference,
                    OutputAt,
                    CreateAt,
                    CreateBy,
                    UpdateAt,
                    UpdateBy
                    )
                    OUTPUT inserted.* 
                    VALUES (
                    @ReceiptId,
                    @BillingId,
                    @MatchingHeaderId,
                    @BankTransferFee,
                    @Amount,
                    @BillingRemain,
                    @ReceiptRemain,
                    @AdvanceReceivedOccured,
                    @RecordedAt,
                    @TaxDifference,
                    NULL,
                    @CreateAt,
                    @CreateBy,
                    @UpdateAt,
                    @UpdateBy
                    )";
            return dbHelper.ExecuteAsync<Matching>(query, Matching, token);
        }

        public Task<Billing> SaveMatchingBillingAsync(Billing Billing, CancellationToken token = default(CancellationToken))
        {
            var query = @"
                            INSERT INTO Billing
                                       (CompanyId
                                      ,CurrencyId
                                      ,CustomerId
                                      ,DepartmentId
                                      ,StaffId
                                      ,BillingCategoryId
                                      ,InputType
                                      ,BillingInputId
                                      ,BilledAt
                                      ,ClosingAt
                                      ,SalesAt
                                      ,DueAt
                                      ,BillingAmount
                                      ,TaxAmount
                                      ,AssignmentAmount
                                      ,RemainAmount
                                      ,OffsetAmount
                                      ,AssignmentFlag
                                      ,Approved
                                      ,CollectCategoryId
                                      ,OriginalCollectCategoryId
                                      ,DebitAccountTitleId
                                      ,CreditAccountTitleId
                                      ,OriginalDueAt
                                      ,OutputAt
                                      ,PublishAt
                                      ,InvoiceCode
                                      ,TaxClassId
                                      ,Note1
                                      ,Note2
                                      ,Note3
                                      ,Note4
                                      ,DeleteAt
                                      ,RequestDate
                                      ,ResultCode
                                      ,TransferOriginalDueAt
                                      ,ScheduledPaymentKey
                                      ,Quantity
                                      ,UnitPrice
                                      ,UnitSymbol
                                      ,Price
                                      ,CreateBy
                                      ,CreateAt
                                      ,UpdateBy
                                      ,UpdateAt
                                )
                             OUTPUT inserted.* 
                             VALUES
                                      ( @CompanyId
                                      ,@CurrencyId
                                      ,@CustomerId
                                      ,@DepartmentId
                                      ,@StaffId
                                      ,@BillingCategoryId
                                      ,@InputType
                                      ,@BillingInputId
                                      ,@BilledAt
                                      ,@ClosingAt
                                      ,@SalesAt
                                      ,@DueAt
                                      ,@BillingAmount
                                      ,@TaxAmount
                                      ,@AssignmentAmount
                                      ,@RemainAmount
                                      ,@OffsetAmount
                                      ,@AssignmentFlag
                                      ,@Approved
                                      ,@CollectCategoryId
                                      ,@OriginalCollectCategoryId
                                      ,@DebitAccountTitleId
                                      ,@CreditAccountTitleId
                                      ,@OriginalDueAt
                                      ,@OutputAt
                                      ,@PublishAt
                                      ,@InvoiceCode
                                      ,@TaxClassId
                                      ,@Note1
                                      ,@Note2
                                      ,@Note3
                                      ,@Note4
                                      ,@DeleteAt
                                      ,@RequestDate
                                      ,@ResultCode
                                      ,@TransferOriginalDueAt
                                      ,@ScheduledPaymentKey
                                      ,@Quantity
                                      ,@UnitPrice
                                      ,@UnitSymbol
                                      ,@Price
                                      ,@CreateBy
                                      ,@CreateAt
                                      ,@UpdateBy
                                      ,@UpdateAt
                                )";
            return dbHelper.ExecuteAsync<Billing>(query, Billing, token);
        }

        public Task<int> UpdateMatchingAsync(Matching Matching, CancellationToken token = default(CancellationToken))
        {
            var query =
                @"UPDATE Matching
                         SET AdvanceReceivedOccured = 1
                           , UpdateAt = @UpdateAt
                           , UpdateBy = @UpdateBy
                    WHERE Id = @Id";
            return dbHelper.ExecuteAsync(query, Matching, token);
        }

        public Task<Billing> GetBillingRemainAmountAsync(IEnumerable<long> BillingIds, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT COALESCE(SUM(b.RemainAmount), 0)      RemainAmount
     , COALESCE(SUM(d.DiscountAmount), 0)    DiscountAmount
  FROM Billing b
 LEFT JOIN (
      SELECT BillingId
           , SUM(DiscountAmount)    DiscountAmount
           , SUM(CASE WHEN DiscountType = 1 THEN DiscountAmount ELSE 0 END) DiscountAmount1
           , SUM(CASE WHEN DiscountType = 2 THEN DiscountAmount ELSE 0 END) DiscountAmount2
           , SUM(CASE WHEN DiscountType = 3 THEN DiscountAmount ELSE 0 END) DiscountAmount3
           , SUM(CASE WHEN DiscountType = 4 THEN DiscountAmount ELSE 0 END) DiscountAmount4
           , SUM(CASE WHEN DiscountType = 5 THEN DiscountAmount ELSE 0 END) DiscountAmount5
        FROM BillingDiscount
       WHERE AssignmentFlag = 0
       GROUP BY
             BillingId
      ) d
   ON b.Id = d.BillingId
WHERE b.Id IN (SELECT Id FROM @Id)
";
            return dbHelper.ExecuteAsync<Billing>(query, new { Id = BillingIds.GetTableParameter() }, token);
        }

        public Task<decimal> GetReceiptRemainAmountAsync(IEnumerable<long> ReceiptIds, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT COALESCE(SUM(RemainAmount), 0)
  FROM Receipt
 WHERE Id IN (SELECT Id FROM @Id)
";
            return dbHelper.ExecuteAsync<decimal>(query, new { Id = ReceiptIds.GetTableParameter() }, token);
        }

        public Task<decimal> GetNettingRemainAmountAsync(IEnumerable<long> NettingIds, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT COALESCE(SUM(Amount), 0)
  FROM Netting
 WHERE Id IN (SELECT Id FROM @Id)
   AND AssignmentFlag = 0
";
            return dbHelper.ExecuteAsync<decimal>(query, new { Id = NettingIds.GetTableParameter() }, token);
        }

        public Task<IEnumerable<Receipt>> GetByOriginalIdAsync(long ReceiptId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT r.*
  FROM Receipt r
 WHERE r.OriginalReceiptId = @ReceiptId
   AND r.AssignmentFlag = 0";
            return dbHelper.GetItemsAsync<Receipt>(query, new { ReceiptId }, token);
        }

        private string GetQueryInsertWorkReceiptTarget() => @"
INSERT INTO WorkReceiptTarget
(ClientKey, ReceiptId,  CompanyId, CurrencyId, PayerName, BankCode, BranchCode, PayerCode, SourceBankname, SourceBranchName, CollationKey, CustomerId, CollationType)
VALUES
(@ClientKey, @ReceiptId, @CompanyId, @CurrencyId, @PayerName, @BankCode, @BranchCode, @PayerCode, @SourceBankName, @SourceBranchName, @CollationKey, COALESCE(@CustomerId, 0), 0 )
";

        public Task<int> SaveWorkReceiptTargetAsync(byte[] ClientKey, long ReceiptId,
            int CompanyId, int CurrencyId, string PayerName, string BankCode, string BranchCode, string PayerCode, string SourceBankName, string SourceBranchName, string CollationKey, int? CustomerId, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync(GetQueryInsertWorkReceiptTarget(),
                new {
                    ClientKey, ReceiptId, CompanyId, CurrencyId,
                    PayerName = PayerName ?? "", PayerCode = PayerCode ?? "",
                    BankCode = BankCode ?? "", BranchCode = BranchCode ?? "",
                    SourceBankName = SourceBankName ?? "", SourceBranchName = SourceBranchName ?? "",
                    CollationKey = CollationKey ?? "", CustomerId
                }, token);

        #region query insert work collation
        private string GetQueryInsertWorkCollation() => @"
INSERT INTO [dbo].[WorkCollation]
     ( ClientKey
     , CompanyId
     , CurrencyId
     , ParentCustomerId
     , PaymentAgencyId
     , PayerName
     , BankCode
     , BranchCode
     , PayerCode
     , SourceBankName
     , SourceBranchName
     , CollationKey
     , CustomerId
     , CollationType
     , ParentCustomerName
     , ParentCustomerKana
     , ParentCustomerShareTransferFee
     , BillingAmount
     , BillingRemainAmount
     , BillingCount
     , ReceiptAmount
     , ReceiptAssignmentAmount
     , ReceiptRemainAmount
     , ReceiptCount
     , AdvanceReceivedCount
     , ForceMatchingIndividually )
SELECT TOP 1 @ClientKey
         , c.CompanyId
         , c.CurrencyId
         , c.ParentCustomerId
         , c.PaymentAgencyId
         , @PayerName
         , @BankCode
         , @BranchCode
         , @PayerCode
         , @SourceBankName
         , @SourceBranchName
         , @CollationKey
         , @CustomerId
         , 0
         , c.ParentCustomerName
         , c.ParentCustomerKana
         , c.ParentCustomerShareTransferFee
         , c.BillingAmount
         , c.BillingRemainAmount
         , c.BillingCount
         , @ReceiptAmount
         , 0
         , @ReceiptAmount
         , 1
         , 1
         , 0
        FROM WorkCollation c
        WHERE c.ParentCustomerId = @ParentCustomerId
        AND c.PaymentAgencyId = @PaymentAgencyId
        AND c.ClientKey = @ClientKey
        AND NOT EXISTS(
            SELECT *
            FROM WorkCollation c2
            WHERE c2.ClientKey      = c.ClientKey
            AND c2.CompanyId        = c.CompanyId
            AND c2.CurrencyId       = c.CurrencyId
            AND c2.ParentCustomerId = c.ParentCustomerId
            AND c2.PaymentAgencyId  = c.PaymentAgencyId
            AND c2.PayerName        = @PayerName
            AND c2.PayerCode        = @PayerCode
            AND c2.BankCode         = @BankCode
            AND c2.BranchCode       = @BranchCode
            AND c2.SourceBankName   = @SourceBankName
            AND c2.SourceBranchName = @SourceBranchName
            AND c2.CollationKey     = @CollationKey
            AND c2.CustomerId       = @CustomerId
            AND c2.CollationType    = 0
        )
";
        #endregion

        public Task<int> SaveWorkCollationAsync(byte[] ClientKey, long ReceiptId, int ParentCustomerId, int PaymentAgencyId, int CustomerId,
            string PayerName, string PayerCode, string BankCode, string BranchCode, string SourceBankName, string SourceBranchName, string CollationKey, decimal ReceiptAmount,
            CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync(GetQueryInsertWorkCollation(),
                new { ClientKey, ReceiptId, ParentCustomerId, PaymentAgencyId, CustomerId,
                    PayerName, PayerCode, BankCode, BranchCode, SourceBankName, SourceBranchName, CollationKey, ReceiptAmount });

    }
}
