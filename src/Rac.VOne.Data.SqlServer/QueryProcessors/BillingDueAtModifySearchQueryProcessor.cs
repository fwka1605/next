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
    public class BillingDueAtModifySearchQueryProcessor :
        IBillingDueAtModifySearchQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public BillingDueAtModifySearchQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<BillingDueAtModify>> GetAsync(BillingSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = new StringBuilder(@"
SELECT b.*
     , cs.Code [CustomerCode]
     , cs.Name [CustomerName]
     , ccy.Code [CurrencyCode]
     , mct.Code [CollectCategoryCode]
     , mct.Name [CollectCategoryName]
     , oct.Code [OriginalCollectCategoryCode]
     , oct.Name [OriginalCollectCategoryName]
  FROM (
SELECT CASE COUNT(b.Id) WHEN 1 THEN MIN(b.Id) ELSE NULL END [Id]
     , CASE COALESCE(b.BillingInputId, -1) WHEN -1 THEN NULL
       ELSE COALESCE(b.BillingInputId, -1)
       END [BillingInputId]
     , MIN(b.CompanyId)                     [CompanyId]
     , MIN(b.CurrencyId)                    [CurrencyId]
     , MIN(b.CustomerId)                    [CustomerId]
     , MIN(b.DepartmentId)                  [DepartmentId]
     , MIN(b.StaffId)                       [StaffId]
     , MAX(b.InvoiceCode)                   [InvoiceCode]
     , MAX(b.BilledAt)                      [BilledAt]
     , MAX(b.ClosingAt)                     [ClosingAt]
     , MAX(b.DueAt)                         [DueAt]
     , COALESCE(MAX(b.OriginalDueAt)
              , MAX(b.DueAt))               [OriginalDueAt]
     , CASE WHEN MAX(b.OriginalDueAt) IS NULL THEN NULL
       ELSE MAX(b.DueAt) END                [ModifiedDueAt]
     , SUM(b.RemainAmount - b.OffsetAmount) [TargetAmount]
     , MAX(b.DeleteAt)                      [DeleteAt]
     , MAX(b.CollectCategoryId)             [CollectCategoryId]
     , COALESCE(MAX(b.OriginalCollectCategoryId)
              , MAX(b.CollectCategoryId))   [OriginalCollectCategoryId]
     , CASE WHEN MAX(b.OriginalCollectCategoryId) IS NULL THEN NULL
       ELSE MAX(b.CollectCategoryId) END    [ModifiedCollectCategoryId]
     , MAX(b.UpdateBy)                      [UpdateBy]
     , MAX(b.UpdateAt)                      [UpdateAt]
     , MIN(b.Id) [MinId]
     , COUNT(b.Id) [RecordCount]
  FROM Billing b
  LEFT JOIN BillingInput bi
    ON bi.Id            = b.BillingInputId
   AND bi.PublishAt     IS NOT NULL
 WHERE b.CompanyId      = @CompanyId
   AND b.Id             = b.Id
   AND bi.Id            IS NULL
 GROUP BY
       COALESCE(b.BillingInputId, b.Id)
     , COALESCE(b.BillingInputId, -1)
 HAVING SUM(b.RemainAmount - b.OffsetAmount) <> 0
    AND MAX(b.DeleteAt)     IS NULL
       ) b
 INNER JOIN Customer cs     ON  cs.Id   = b.CustomerId
 INNER JOIN Currency ccy    ON ccy.Id   = b.CurrencyId
 INNER JOIN Department dp   ON  dp.Id   = b.DepartmentId
 INNER JOIN Staff st        ON  st.Id   = b.StaffId
 INNER JOIN Category oct    ON oct.Id   = b.OriginalCollectCategoryId
  LEFT JOIN Category mct    ON mct.Id   = b.ModifiedCollectCategoryId
 WHERE b.MinId          = b.MinId");
            if (option.BsBilledAtFrom.HasValue) query.Append(@"
   AND b.BilledAt       >= @BsBilledAtFrom");
            if (option.BsBilledAtTo.HasValue) query.Append(@"
   AND b.BilledAt       <= @BsBilledAtTo");
            if (option.BsClosingAtFrom.HasValue) query.Append(@"
   AND b.ClosingAt      >= @BsClosingAtFrom");
            if (option.BsClosingAtTo.HasValue) query.Append(@"
   AND b.ClosingAt      <= @BsClosingAtTo");
            if (option.BsDueAtFrom.HasValue) query.Append(@"
   AND b.DueAt          >= @BsDueAtFrom");
            if (option.BsDueAtTo.HasValue) query.Append(@"
   AND b.DueAt          <= @BsDueAtTo");
            if (!string.IsNullOrEmpty(option.BsInvoiceCodeFrom)) query.Append(@"
   AND b.InvoiceCode    >= @BsInvoiceCodeFrom");
            if (!string.IsNullOrEmpty(option.BsInvoiceCodeTo)) query.Append(@"
   AND b.InvoiceCode    <= @BsInvoiceCodeTo");
            if (!string.IsNullOrEmpty(option.BsInvoiceCode))
            {
                option.BsInvoiceCode = Sql.GetWrappedValue(option.BsInvoiceCode);
                query.Append(@"
   AND b.InvoiceCode    LIKE @BsInvoiceCode");
            }
            if (!string.IsNullOrEmpty(option.BsCustomerCodeFrom)) query.Append(@"
   AND cs.Code          >= @BsCustomerCodeFrom");
            if (!string.IsNullOrEmpty(option.BsCustomerCodeTo)) query.Append(@"
   AND cs.Code          <= @BsCustomerCodeTo");
            if (!string.IsNullOrEmpty(option.BsDepartmentCodeFrom)) query.Append(@"
   AND dp.Code          >= @BsDepartmentCodeFrom");
            if (!string.IsNullOrEmpty(option.BsDepartmentCodeTo)) query.Append(@"
   AND dp.Code          <= @BsDepartmentCodeTo");
            if (!string.IsNullOrEmpty(option.BsStaffCodeFrom)) query.Append(@"
   AND st.Code          >= @BsStaffCodeFrom");
            if (!string.IsNullOrEmpty(option.BsStaffCodeTo)) query.Append(@"
   AND st.Code          <= @BsStaffCodeTo");
            if (!string.IsNullOrEmpty(option.BsCurrencyCode)) query.Append(@"
   AND ccy.Code         = @BsCurrencyCode");
            query.Append(@"
 ORDER BY b.MinId ASC");
            return dbHelper.GetItemsAsync<BillingDueAtModify>(query.ToString(), option, token);
        }

    }
}
