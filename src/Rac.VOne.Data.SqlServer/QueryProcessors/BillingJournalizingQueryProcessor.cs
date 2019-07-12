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
    public class BillingJournalizingQueryProcessor : IBillingJournalizingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingJournalizingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<JournalizingSummary>> GetAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            if (option.OutputAt == null) option.OutputAt = new DateTime[] { };
            var query = @"
SELECT        OutputAt
            , COUNT(*)                  [Count]
            , ccy.Code                  [CurrencyCode]
            , SUM( b.BillingAmount )    [Amount]
            , MAX( b.UpdateAt )         [UpdateAt]
FROM        Billing b
INNER JOIN  Currency ccy
ON          b.CurrencyId    = ccy.Id
AND         b.CompanyId     = @CompanyId
AND         b.InputType <> 3
AND         b.DeleteAt IS NULL";
            if (option.CurrencyId.HasValue) query += @"
AND         b.CurrencyId    = @CurrencyId";
            if (option.BilledAtFrom.HasValue) query += @"
AND         b.BilledAt      >= @BilledAtFrom";
            if (option.BilledAtTo.HasValue) query += @"
AND         b.BilledAt      <= @BilledAtTo";
            query += $@"
AND         b.OutputAt      IS {(option.IsOutuptted ? "NOT " : "")}NULL";
            query += @"
GROUP BY      b.OutputAt
            , ccy.Code
ORDER BY      b.OutputAt DESC
            , ccy.Code ASC";
            return dbHelper.GetItemsAsync<JournalizingSummary>(query, option, token);
        }


        public Task<IEnumerable<BillingJournalizing>> ExtractAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            if (option.OutputAt == null) option.OutputAt = new DateTime[] { };
            #region query
            var query = @"
DECLARE
  @BillingDivisionDepartmentCode nvarchar(20)   --長期前受部門コード
, @BillingDivisionDepartmentName nvarchar(40)   --長期前受部門名
, @BillingDivisionAccountTitleCode nvarchar(20) --長期前受科目コード
, @BillingDivisionAccountTitleName nvarchar(40) --長期前受科目名
, @BillingDivisionSubCode nvarchar(20)          --長期前受補助コード
SET @BillingDivisionDepartmentCode   = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'長期前受金部門コード')
SET @BillingDivisionAccountTitleCode = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'長期前受金科目コード')
SET @BillingDivisionSubCode          = (SELECT Value FROM GeneralSetting WHERE CompanyId = @CompanyId AND Code = N'長期前受金補助コード')
SET @BillingDivisionDepartmentName   = (SELECT Name  FROM Department     WHERE CompanyId = @CompanyId AND Code = @BillingDivisionDepartmentCode)
SET @BillingDivisionAccountTitleName = (SELECT Name  FROM AccountTitle   WHERE CompanyId = @CompanyId AND Code = @BillingDivisionAccountTitleCode)
SELECT b.CompanyId
     , cm.Code [CompanyCode]
     , b.BilledAt
     , b.Id [SlipNumber]
     , '' [DebitDepartmentCode]
     , '' [DebitDepartmentName]
     , COALESCE(dat.Code, '') [DebitAccountTitleCode]
     , COALESCE(dat.Name, '') [DebitAccountTitleName]
     , '' [DebitSubCode]
     , '' [DebitSubName]
     , CASE WHEN bc.UseLongTermAdvanceReceived = 1 THEN @BillingDivisionDepartmentCode
       ELSE d.Code END [CreditDepartmentCode]
     , CASE WHEN bc.UseLongTermAdvanceReceived = 1 THEN @BillingDivisionDepartmentName
       ELSE d.Name END [CreditDepartmentName]
     , CASE WHEN bc.UseLongTermAdvanceReceived = 1 THEN @BillingDivisionAccountTitleCode
       ELSE COALESCE(cat.Code, '') END [CreditAccountTitleCode]
     , CASE WHEN bc.UseLongTermAdvanceReceived = 1 THEN @BillingDivisionAccountTitleName
       ELSE COALESCE(cat.Name, '') END [CreditAccountTitleName]
     , CASE WHEN bc.UseLongTermAdvanceReceived = 1 THEN @BillingDivisionSubCode
       ELSE '' END [CreditSubCode]
     , '' [CreditSubName]
     , b.BillingAmount
     , b.Note1 [Note]
     , cs.Code [CustomerCode]
     , cs.Name [CustomerName]
     , b.InvoiceCode
     , st.Code [StaffCode]
     , '' [PayerCode]
     , '' [PayerName]
     , '' [SourceBankName]
     , '' [SourceBranchName]
     , null [DueAt]
     , '' [BankCode]
     , '' [BankName]
     , '' [BranchCode]
     , '' [BranchName]
     , null [AccountType]
     , '' [AccountNumber]
     , COALESCE(ccy.Code, '') [CurrencyCode]
  FROM Billing b
 INNER JOIN Company cm          ON b.CompanyId = cm.Id
 INNER JOIN Customer cs         ON b.CustomerId = cs.Id
 INNER JOIN Department d        ON b.DepartmentId = d.Id
 INNER JOIN Category bc         ON b.BillingCategoryId = bc.Id
 INNER JOIN Staff st            ON b.StaffId = st.Id
  LEFT JOIN AccountTitle dat    ON b.DebitAccountTitleId = dat.Id
  LEFT JOIN AccountTitle cat    ON bc.AccountTitleId = cat.Id
  LEFT JOIN Currency ccy        ON b.CurrencyId = ccy.Id
WHERE       b.CompanyId     = @CompanyId
AND         b.DeleteAt      IS NULL
AND         b.InputType     <> 3";
            if (option.CurrencyId.HasValue) query += @"
AND         b.CurrencyId    = @CurrencyId";
            if (option.BilledAtFrom.HasValue) query += @"
AND         b.BilledAt      >= @BilledAtFrom";
            if (option.BilledAtTo.HasValue) query += @"
AND         b.BilledAt      <= @BilledAtTo";
            if (option.OutputAt.Any()) query += @"
AND         b.OutputAt      IN @OutputAt";
            query += $@"
AND         b.OutputAt      IS {(option.IsOutuptted ? "NOT " : "")}NULL";
            query += @"
ORDER BY    b.Id, b.BilledAt";
            #endregion
            return dbHelper.GetItemsAsync<BillingJournalizing>(query, option, token);
        }


        public Task<IEnumerable<Billing>> UpdateAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE      Billing
SET           OutputAt  = GETDATE()
            , UpdateAt  = GETDATE()
            , UpdateBy  = @LoginUserId
WHERE       CompanyId   = @CompanyId
AND         OutputAt IS NULL
AND         InputType <> 3
AND         DeleteAt IS NULL";
            if (option.CurrencyId.HasValue) query += @"
AND         CurrencyId  = @CurrencyId";
            if (option.BilledAtFrom.HasValue) query += @"
AND         BilledAt    >= @BilledAtFrom";
            if (option.BilledAtTo.HasValue) query += @"
AND         BilledAt    <= @BilledAtTo";
            return dbHelper.GetItemsAsync<Billing>(query, option, token);
        }


        public Task<IEnumerable<Billing>> CancelAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            if (option.OutputAt == null) option.OutputAt = new DateTime[] { };
            var query = @"
UPDATE      Billing
SET           OutputAt = NULL
            , UpdateAt = GETDATE()
            , UpdateBy = @LoginUserId
OUTPUT      inserted.*
WHERE       CompanyId   = @CompanyId
AND InputType <> 3";
            if (option.CurrencyId.HasValue) query += @"
AND         CurrencyId  = @CurrencyId";
            if (option.OutputAt?.Any() ?? false)
                query += @"
AND         OutputAt IN @OutputAt";
            return dbHelper.GetItemsAsync<Billing>(query, option, token);
        }

    }
}
