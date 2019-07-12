using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BillingInvoiceQueryProcessor : IBillingInvoiceQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public BillingInvoiceQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<BillingInvoice>> GetAsync(BillingInvoiceSearch searchOption,
            InvoiceCommonSetting invoiceCommonSetting,
            CancellationToken token = default(CancellationToken))
        {
            return dbHelper.ExecuteQueriesAsync(async connection =>
            {
                await dbHelper.ExecuteAsync(connection,
                    GetQueryInsertWorkBillingInvoices(searchOption),
                    searchOption, token);

                return await dbHelper.QueryAsync<BillingInvoice>(connection,
                    GetQuerySelectBillingInvoices(searchOption, invoiceCommonSetting),
                    searchOption,
                    token);
            });
        }

        private string GetQueryInsertWorkBillingInvoices(BillingInvoiceSearch option)
        {
            var sql = @"
DELETE FROM WorkBillingInvoice
WHERE ClientKey = @ClientKey

INSERT INTO WorkBillingInvoice
( [ClientKey]
 ,[BillingId]
 ,[TemporaryBillingInputId]
 ,[BillingInputId] )

SELECT @ClientKey
     , b.BillingId
     , DENSE_RANK() OVER(ORDER BY b.BillingInputId, b.BillingInputId2) AS TemporaryBillingInputId
     , b.BillingInputId
  FROM (

    ---BillingInputId 有り
    SELECT b.Id AS BillingId
         , b.BillingInputId
         , 0 AS BillingInputId2
      FROM Billing b
      LEFT JOIN Department d
        ON d.Id = b.DepartmentId
      LEFT JOIN Customer c
        ON c.Id = b.CustomerId
      LEFT JOIN BillingInput bi
        ON bi.Id = b.BillingInputId
      LEFT JOIN Staff s
        ON s.Id = b.StaffId
      LEFT JOIN Category cat
        ON cat.id = b.CollectCategoryId
     WHERE b.CompanyId = @CompanyId
       AND b.BillingInputId IS NOT NULL
       AND b.DeleteAt       IS NULL
{0}

UNION ALL

    ---BillingInputId 無し
    SELECT b.Id AS BillingId
         , b.BillingInputId
         , DENSE_RANK()
           OVER(ORDER BY b.CurrencyId
                       , b.CustomerId
                       , b.DepartmentId
                       , b.StaffId
                       , b.BilledAt
                       , b.ClosingAt
                       , b.DueAt
                       , b.CollectCategoryId
                       , b.InvoiceCode
                       , b.DestinationId
               ) AS BillingInputId2
      FROM Billing b
      LEFT JOIN Department d
        ON d.Id = b.DepartmentId
      LEFT JOIN Customer c
        ON c.Id = b.CustomerId
      LEFT JOIN BillingInput bi
        ON bi.Id = b.BillingInputId
      LEFT JOIN Staff s
        ON s.Id = b.StaffId
      LEFT JOIN Category cat
        ON cat.id = b.CollectCategoryId
     WHERE b.CompanyId = @CompanyId
       AND b.BillingInputId IS NULL
       AND b.DeleteAt       IS NULL
{0}

) AS b
";
            //検索条件
            var condition = new StringBuilder();
            if (option.ClosingAt.HasValue)
            {
                condition.AppendLine("  AND b.ClosingAt = @ClosingAt");
            }
            if (option.BilledAtFrom.HasValue)
            {
                condition.AppendLine("  AND b.BilledAt >= @BilledAtFrom");
            }
            if (option.BilledAtTo.HasValue)
            {
                condition.AppendLine("  AND b.BilledAt <= @BilledAtTo");
            }
            if (option.CollectCategoryId != 0)
            {
                condition.AppendLine("  AND b.CollectCategoryId = @CollectCategoryId");
            }
            if (!string.IsNullOrEmpty(option.DepartmentCodeFrom))
            {
                condition.AppendLine("  AND d.Code >= @DepartmentCodeFrom");
            }
            if (!string.IsNullOrEmpty(option.DepartmentCodeTo))
            {
                condition.AppendLine("  AND d.Code <= @DepartmentCodeTo");
            }
            if (!string.IsNullOrEmpty(option.CustomerCodeFrom))
            {
                condition.AppendLine("  AND c.Code >= @CustomerCodeFrom");
            }
            if (!string.IsNullOrEmpty(option.CustomerCodeTo))
            {
                condition.AppendLine("  AND c.Code <= @CustomerCodeTo");
            }
            if (option.IsPublished)
            {
                condition.AppendLine("  AND bi.PublishAt      IS NOT NULL");
            }
            else
            {
                condition.AppendLine("  AND bi.PublishAt      IS NULL");
            }
            if (!string.IsNullOrEmpty(option.StaffCodeFrom))
            {
                condition.AppendLine("  AND s.Code >= @StaffCodeFrom");
            }
            if (!string.IsNullOrEmpty(option.StaffCodeTo))
            {
                condition.AppendLine("  AND s.Code <= @StaffCodeTo");
            }
            if (!string.IsNullOrEmpty(option.InvoiceCodeFrom))
            {
                condition.AppendLine("  AND b.InvoiceCode >= @InvoiceCodeFrom");
            }
            if (!string.IsNullOrEmpty(option.InvoiceCodeTo))
            {
                condition.AppendLine("  AND b.InvoiceCode <= @InvoiceCodeTo");
            }
            if (!string.IsNullOrEmpty(option.InvoiceCode))
            {
                option.InvoiceCode = Sql.GetWrappedValue(option.InvoiceCode);
                condition.AppendLine("  AND b.InvoiceCode like @InvoiceCode");
            }

            //除外設定
            if (!option.IsPublished)
            {
                condition.AppendLine(@"
  AND cat.ExcludeInvoicePublish = 0
  AND c.ExcludeInvoicePublish = 0
");
            }
            return string.Format(sql, condition);
        }

        private string GetQuerySelectBillingInvoices(BillingInvoiceSearch option,
            InvoiceCommonSetting invoiceCommonSetting)
        {
            var select1 = option.IsPublished ?
               "0"
               : "1";
            var select2 = option.IsPublished ?
                "bi.InvoiceTemplateId AS InvoiceTemplateId"
                : "COALESCE(its.Id, 0) AS InvoiceTemplateId";
           
            var builder = new StringBuilder();
            builder.Append($@"
declare @CompanyCode nvarchar(20) = (SELECT Code From Company WHERE Id = @CompanyId)

SELECT inv.TemporaryBillingInputId
     , @CompanyCode AS CompanyCode
     , inv.BillingInputId
     , {select1} AS Checked
     , its.Code AS InvoiceTemplateCode
     , {select2}
     , inv.InvoiceCode
     , inv.DetailsCount
     , inv.CustomerId
     , cus.Code              AS CustomerCode
     , cus.Name              AS CustomerName
     , cus.PostalCode        AS CustomerPostalCode
     , cus.Address1          AS CustomerAddress1
     , cus.Address2          AS CustomerAddress2
     , cus.DestinationDepartmentName AS CustomerDestinationDepartmentName
     , cus.CustomerStaffName AS CustomerStaffName
     , cus.Honorific         AS CustomerHonorific
     , inv.AmountSum
     , inv.RemainAmountSum
     , (cat.code + ':' + cat.name) AS CollectCategoryCodeAndNeme
     , inv.ClosingAt
     , inv.BilledAt
     , dep.Code  AS DepartmentCode
     , dep.Name  AS DepartmentName
     , sta.code  AS StaffCode
     , sta.name  AS StaffName
     , inv.DestinationId
     , dest.code AS DestnationCode
     , dest.Name           AS DestinationName
     , dest.PostalCode     AS DestinationPostalCode
     , dest.Address1       AS DestinationAddress1
     , dest.Address2       AS DestinationAddress2
     , dest.DepartmentName AS DestinationDepartmentName
     , dest.Addressee      AS DestinationAddressee
     , dest.Honorific      AS DestinationHonorific
     , bi.PublishAt
     , bi.PublishAt1st
     , inv.UpdateAt
FROM (

SELECT wbi.TemporaryBillingInputId
     , wbi.BillingInputId
     , b.CompanyId
     , b.CurrencyId
     , b.CustomerId
     , b.DepartmentId
     , b.StaffId
     , b.DestinationId
     , b.BilledAt
     , b.ClosingAt
     , b.DueAt
     , b.InvoiceCode
     , SUM(b.BillingAmount) AS AmountSum
     , SUM(b.RemainAmount)  AS RemainAmountSum
     , b.CollectCategoryId
     , COUNT(*) AS DetailsCount
     , CASE WHEN (MAX(b.AssignmentFlag) = MIN(b.AssignmentFlag) AND MAX(b.AssignmentFlag) = 0) THEN 0
            WHEN (MAX(b.AssignmentFlag) = MIN(b.AssignmentFlag) AND MAX(b.AssignmentFlag) = 2) THEN 2
            ELSE 1
       END AS AssignmentFlagForInvoice
     , MAX(b.UpdateAt) AS UpdateAt
FROM WorkBillingInvoice wbi
INNER JOIN Billing b
ON b.Id = wbi.BillingId
WHERE wbi.ClientKey = @ClientKey
GROUP BY wbi.TemporaryBillingInputId
       , wbi.BillingInputId
       , b.CompanyId
       , b.CurrencyId
       , b.CustomerId
       , b.DepartmentId
       , b.StaffId
       , b.DestinationId
       , b.BilledAt
       , b.ClosingAt
       , b.DueAt
       , b.InvoiceCode
       , b.CollectCategoryId
) AS inv

LEFT JOIN BillingInput bi
  ON bi.Id = inv.BillingInputId
LEFT JOIN Customer cus
  ON cus.Id = inv.CustomerId
LEFT JOIN Category cat
  ON cat.id = inv.CollectCategoryId
LEFT JOIN Department dep
  ON dep.id = inv.departmentId
LEFT JOIN Staff sta
  ON sta.Id = inv.StaffId
LEFT JOIN Destination dest
  ON dest.id = inv.DestinationId
LEFT JOIN InvoiceTemplateSetting its
  ON its.CollectCategoryId = inv.CollectCategoryId

WHERE 1 = 1
");
            //検索条件
            if (option.IsPublished)
            {
                var flags = (AssignmentFlagChecked)option.AssignmentFlg;
                var selectedFlags = new List<int>();
                if (flags.HasFlag(AssignmentFlagChecked.NoAssignment)) selectedFlags.Add(0);
                if (flags.HasFlag(AssignmentFlagChecked.PartAssignment)) selectedFlags.Add(1);
                if (flags.HasFlag(AssignmentFlagChecked.FullAssignment)) selectedFlags.Add(2);
                if (selectedFlags.Any() && !flags.HasFlag(AssignmentFlagChecked.All))
                {
                    builder.AppendLine($" AND inv.AssignmentFlagForInvoice IN ({(string.Join(", ", selectedFlags))})");
                }

                if (option.PublishAtFrom.HasValue)
                {
                    builder.AppendLine("  AND bi.PublishAt >= @PublishAtFrom");
                }
                if (option.PublishAtTo.HasValue)
                {
                    builder.AppendLine("  AND bi.PublishAt <= @PublishAtTo");
                }
                if (option.PublishAtFirstFrom.HasValue)
                {
                    builder.AppendLine("  AND bi.PublishAt1st >= @PublishAtFirstFrom");
                }
                if (option.PublishAtFirstTo.HasValue)
                {
                    builder.AppendLine("  AND bi.PublishAt1st <= @PublishAtFirstTo");
                }
            }

            //除外設定

            if (invoiceCommonSetting.ExcludeAmountZero == 1)
            {
                if (option.ReportInvoiceAmount == 0)
                {
                    builder.AppendLine("  AND inv.AmountSum <> 0");
                }
                else
                {
                    builder.AppendLine("  AND inv.RemainAmountSum <> 0");
                }
            }
            if (invoiceCommonSetting.ExcludeMinusAmount == 1)
            {
                if (option.ReportInvoiceAmount == 0)
                {
                    builder.AppendLine("  AND inv.AmountSum >= 0");
                }
                else
                {
                    builder.AppendLine("  AND inv.RemainAmountSum >= 0");
                }
            }
            if (invoiceCommonSetting.ExcludeMatchedData == 1)
            {
                builder.AppendLine("  AND inv.AssignmentFlagForInvoice <> 2");
            }
            builder.AppendLine("  ORDER BY inv.BilledAt asc, cus.Code asc, dest.code asc, inv.BillingInputId asc, inv.TemporaryBillingInputId asc");
            return builder.ToString();
        }

        public Task<IEnumerable<BillingInvoice>> UpdateBillingForPublishNewInputId(
            BillingInvoiceForPublish billingInvoiceForPublish,
            bool DoUpdateInvoiceCode,
            CancellationToken token = default(CancellationToken))
        {
            return dbHelper.GetItemsAsync<BillingInvoice>(
                GetQueryUpdateBillingForPublishByTemporaryId(DoUpdateInvoiceCode),
                billingInvoiceForPublish, token);
        }

        private string GetQueryUpdateBillingForPublishByTemporaryId(bool UpdateNewInvoiceCode)
        {
            var query = $@"
UPDATE b
   SET b.BillingInputId = @BillingInputId
     , b.DestinationId = @DestinationId
{(UpdateNewInvoiceCode ? "    , b.InvoiceCode    = @InvoiceCode" : string.Empty)}
OUTPUT INSERTED.*
  FROM Billing b
 INNER JOIN WorkBillingInvoice w
    ON w.BillingId = b.Id
   AND w.ClientKey = @ClientKey
   AND w.TemporaryBillingInputId = @TemporaryBillingInputId";

            return query;
        }

        public Task<IEnumerable<BillingInvoiceDetailForPrint>> GetDetailsForPrintAsync(
            BillingInvoiceDetailSearch option,
            InvoiceCommonSetting invoiceCommonSetting, CancellationToken token = default(CancellationToken))
        {
            return dbHelper.GetItemsAsync<BillingInvoiceDetailForPrint>(
                GetQuerySelectBillingInvoiceDetailsForPrint(option, invoiceCommonSetting),
                new
                {
                    option.ClientKey,
                    BillingInputIds = option.BillingInputIds.GetTableParameter(),
                    TemporaryBillingInputIds = option.TemporaryBillingInputIds.GetTableParameter(),
                    option.InvoiceTemplateId
                }, token);
        }

        private string GetQuerySelectBillingInvoiceDetailsForPrint(
            BillingInvoiceDetailSearch billingInvoiceDetailSearch,
            InvoiceCommonSetting invoiceCommonSetting)
        {
            var templateId = billingInvoiceDetailSearch.TemporaryBillingInputIds != null
                ? "@InvoiceTemplateId"
                : "bi.InvoiceTemplateId";

            var builder = new StringBuilder(@"
SELECT b.BillingInputId
     , b.InvoiceCode
     , b.BilledAt
     , b.ClosingAt
     , b.SalesAt
     , b.DueAt
     , b.BillingAmount
     , CASE b.TaxClassId WHEN 0 THEN b.TaxAmount
                         ELSE 0
       END AS TaxAmount
     , CASE b.TaxClassId WHEN 0 THEN (b.BillingAmount - b.TaxAmount)
                         ELSE b.BillingAmount
       END AS TaxExcludedPrice
     , b.RemainAmount
     , CASE b.TaxClassId WHEN 1 THEN '税込'
                         WHEN 0 THEN '税抜'
                         ELSE tc.Name
       END AS TaxClassName
     , b.Note1
     , b.Note2
     , CASE b.Quantity WHEN 0 THEN NULL
                       ELSE b.Quantity
       END AS Quantity
     , CASE b.UnitPrice WHEN 0 THEN NULL
                        ELSE b.UnitPrice
       END AS UnitPrice
     , b.UnitSymbol
     , s.Name AS StaffName
     , it.DisplayStaff
     , '' AS Tel
     , '' AS Fax
     , c.PostalCode        AS CustomerPostalCode
     , c.Address1          AS CustomerAddress1
     , c.Address2          AS CustomerAddress2
     , c.Code              AS CustomerCode
     , c.Name              AS CustomerName
     , c.DestinationDepartmentName AS CustomerDestinationDepartmentName
     , c.CustomerStaffName AS CustomerStaffName
     , c.Honorific         AS CustomerHonorific
     , c.ShareTransferFee
");
            if (billingInvoiceDetailSearch.DestinationId.HasValue)
            {
                builder.AppendFormat(@"     , {0} AS DestinationId
", billingInvoiceDetailSearch.DestinationId.Value);
            }
            else
            {
                builder.AppendLine("     , b.DestinationId");
            }

                builder.AppendLine(@"
     , des.Name           AS DestinationName
     , des.PostalCode     AS DestinationPostalCode
     , des.Address1       AS DestinationAddress1
     , des.Address2       AS DestinationAddress2
     , des.DepartmentName AS DestinationDepartmentName
     , des.Addressee      AS DestinationAddressee
     , des.Honorific      AS DestinationHonorific
     , it.Title
     , it.Greeting
     , it.DueDateComment
     , it.DueDateFormat
     , it.TransferFeeComment
     , c.ReceiveAccountId1
     , c.ReceiveAccountId2
     , c.ReceiveAccountId3
     , ('振込銀行：'
         + c.ExclusiveBankName
         + ' '
         + c.ExclusiveBranchName
         + ' '
         + at.Name
         + ' '
         + RIGHT(c.ExclusiveAccountNumber, 7)
       ) AS ExclusiveAccount
");
            if (billingInvoiceDetailSearch.TemporaryBillingInputIds != null)
            {
                builder.Append(@"
      , (SELECT COUNT(*)
          FROM WorkBillingInvoice wbi2
         WHERE wbi2.ClientKey               = @ClientKey
           AND wbi2.TemporaryBillingInputId = wbi.TemporaryBillingInputId
       ) AS DetailCount
    FROM Billing b
");
            } else
            {
                builder.Append(@"
      , (SELECT COUNT(*)
          FROM Billing b2
         WHERE b2.BillingInputId = b.BillingInputId
       ) AS DetailCount
    FROM Billing b
");
            }

            if (billingInvoiceDetailSearch.TemporaryBillingInputIds != null)
            {
                builder.Append(@"
INNER JOIN WorkBillingInvoice wbi
   ON wbi.ClientKey = @ClientKey
  AND wbi.BillingId = b.Id
");
            }

            builder.Append($@"
 LEFT join Customer c
   ON c.Id = b.CustomerId
 LEFT JOIN TaxClass tc
   ON tc.Id = b.TaxClassId
 LEFT JOIN Staff s
   ON s.Id = b.StaffId
 LEFT JOIN BillingInput bi
   ON bi.Id = b.BillingInputId
 LEFT JOIN InvoiceTemplateSetting it
   ON it.Id = { templateId }
 LEFT JOIN BankAccountType at
   ON at.Id = c.ExclusiveAccountTypeId
 LEFT JOIN Destination des
");
            if (billingInvoiceDetailSearch.DestinationId.HasValue)
            {
                builder.AppendFormat(@"            ON des.Id = {0}
", billingInvoiceDetailSearch.DestinationId.Value);
            }
            else
            {
                builder.AppendLine("            ON des.Id = b.DestinationId");
            }

            if (billingInvoiceDetailSearch.TemporaryBillingInputIds != null)
            {
                builder.AppendLine("WHERE wbi.TemporaryBillingInputId IN (SELECT Id FROM @TemporaryBillingInputIds)");
            }
            else
            {
                builder.AppendLine("WHERE b.BillingInputId IN (SELECT Id FROM @BillingInputIds)");
            }
            if (invoiceCommonSetting.ExcludeMatchedData != 0)
            {
                builder.AppendLine("  AND b.AssignmentFlag <> 2");
            }
            builder.AppendLine("  ORDER BY b.BilledAt asc, c.Code asc, des.Code asc, b.BillingInputId asc");

            return builder.ToString();
        }

        public Task<int> DeleteWorkTableAsync(byte[] ClientKey, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
DELETE WorkBillingInvoice
 WHERE ClientKey = @ClientKey
";
            return dbHelper.ExecuteAsync(query, new { ClientKey }, token);
        }

        public Task<IEnumerable<BillingInvoiceDetailForExport>> GetDetailsForExportAsync(
            IEnumerable<long> BillingInputIds,
            InvoiceCommonSetting invoiceCommonSetting,
            CancellationToken token = default(CancellationToken))
        {
            return dbHelper.GetItemsAsync<BillingInvoiceDetailForExport>(
                GetQuerySelectBillingInvoiceDetailsForExport(invoiceCommonSetting),
                new { BillingInputIds  = BillingInputIds.GetTableParameter() },
                token);
        }

        private string GetQuerySelectBillingInvoiceDetailsForExport(InvoiceCommonSetting invoiceCommonSetting)
        {
            var builder = new StringBuilder();

            builder.Append(@"
SELECT com.Code AS CompanyCode
     , b.BillingInputId
     , b.Id AS BillingId
     , b.BilledAt
     , b.ClosingAt
     , b.SalesAt
     , b.DueAt
     , b.BillingAmount
     , b.TaxAmount
     , b.Price
     , b.RemainAmount
     , b.InvoiceCode
     , b.Note1
     , b.Note2
     , b.Note3
     , b.Note4
     , b.Note5
     , b.Note6
     , b.Note7
     , b.Note8
     , bm.Memo
     , bdc.ContractNumber
     , b.Quantity
     , b.UnitSymbol
     , b.UnitPrice
     , bi.PublishAt
     , bi.PublishAt1st
     , b.AssignmentFlag
     , dp.Code AS DepartmentCode
     , dp.Name AS DepartmentName
     , bc.Code AS BillingCategoryCode
     , bc.Name AS BillingCategoryName
     , bc.ExternalCode AS BillingCategoryExternalCode
     , b.TaxClassId
     , tc.Name AS TaxClassName
     , cc.Code AS CollectCategoryCode
     , cc.Name AS CollectCategoryName
     , cc.ExternalCode AS CollectCategoryExternalCode
     , s.Code AS StaffCode
     , s.Name AS StaffName
     , cus.Code AS CustomerCode
     , cus.Name AS CustomerName
     , cus.ShareTransferFee
     , cus.PostalCode AS CustomerPostalCode
     , COALESCE(de.Address1, cus.Address1) AS CustomerAddress1
     , COALESCE(de.Address2, cus.Address2) AS CustomerAddress2
     , COALESCE(de.DepartmentName, cus.DestinationDepartmentName) AS CustomerDepartmentName
     , COALESCE(de.Addressee, cus.CustomerStaffName) AS CustomerAddressee
     , COALESCE(de.Honorific, cus.Honorific)         AS CustomerHonorific
     , cus.Note AS CustomerNote
     , cus.ExclusiveBankCode
     , cus.ExclusiveBankName
     , cus.ExclusiveBranchCode
     , LEFT(cus.ExclusiveAccountNumber, 3) AS VirtualBranchCode
     , cus.ExclusiveBranchName

     , RIGHT(cus.ExclusiveAccountNumber, 7) AS VirtualAccountNumber
     , cus.ExclusiveAccountTypeId
     , com.BankName1      AS CompanyBankName1
     , com.BranchName1    AS CompanyBranchName1
     , com.AccountType1   AS CompanyAccountType1
     , com.AccountNumber1 AS CompanyAccountNumber1
     , com.BankName2      AS CompanyBankName2
     , com.BranchName2    AS CompanyBranchName2
     , com.AccountType2   AS CompanyAccountType2
     , com.AccountNumber2 AS CompanyAccountNumber2
     , com.BankName3      AS CompanyBankName3
     , com.BranchName3    AS CompanyBranchName3
     , com.AccountType3   AS CompanyAccountType3
     , com.AccountNumber3 AS CompanyAccountNumber3
     , com.BankAccountName AS CompanyBankAccountName
     , com.Name       AS CompanyName
     , com.PostalCode AS CompanyPostalCode
     , com.Address1   AS CompanyAddress1
     , com.Address2   AS CompanyAddress2
     , com.Tel        AS CompanyTel
     , com.Fax        AS CompanyFax
 FROM Billing b
 LEFT JOIN Company com
   ON com.Id = b.CompanyId
 LEFT JOIN BillingMemo bm
   ON bm.BillingId = b.Id
 LEFT JOIN BillingDivisionContract bdc
   ON bdc.BillingId = b.Id
 LEFT JOIN BillingInput bi
   ON bi.Id = b.BillingInputId
 LEFT JOIN Department dp
   ON dp.Id = b.DepartmentId
 LEFT JOIN Category bc
   ON bc.Id = b.BillingCategoryId
 LEFT JOIN TaxClass tc
   ON tc.Id = b.TaxClassId
 LEFT JOIN Category cc
   ON cc.Id = b.CollectCategoryId
 LEFT JOIN Staff s
   ON s.Id = b.StaffId
 LEFT JOiN Customer cus
   ON cus.Id = b.CustomerId
 LEFT JOIN Destination de
   ON de.Id = b.DestinationId
WHERE b.BillingInputId IN (SELECT Id FROM @BillingInputIds)
");
            if (invoiceCommonSetting.ExcludeMatchedData != 0)
            {
                builder.AppendLine("  AND b.AssignmentFlag <> 2");
            }
            builder.AppendLine("  ORDER BY b.BillingInputId, b.SalesAt, b.Id");
            return builder.ToString();
        }

        public Task<DateTime?> GetMaxUpdateAtAsync(byte[] ClientKey, IEnumerable<long> temporaryBillingInputIds, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT MAX(b.UpdateAt) AS MaxUpdateAt
  FROM WorkBillingInvoice wb
 INNER JOIN Billing b
    ON b.Id = wb.BillingId
 WHERE wb.ClientKey = @ClientKey
   AND wb.TemporaryBillingInputId IN (SELECT Id FROM @temporaryBillingInputIds)
";
            return dbHelper.ExecuteAsync<DateTime?>(query, new {
                ClientKey,
                temporaryBillingInputIds = temporaryBillingInputIds.GetTableParameter()
            }, token);
        }

    }
}