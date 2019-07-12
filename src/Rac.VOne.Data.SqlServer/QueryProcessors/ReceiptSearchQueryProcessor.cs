using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Rac.VOne.Common.Constants;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReceiptSearchQueryProcessor :
        IReceiptSearchQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ReceiptSearchQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<Receipt>> GetAsync(ReceiptSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT
 COALESCE( ba.BankName  , r.BankName   ) [BankName]
,COALESCE( ba.BranchName, r.BranchName ) [BranchName]
,r.*
,r.ExcludeFlag          ExcludeFlagBuffer
,r.ExcludeCategoryId    ExcludeCategoryIdBuffer
,n.ReceiptId NettingId
,bat.Name AccountTypeName
,rc.UseAdvanceReceived
,rc.UseCashOnDueDates
,rc.Code CategoryCode
,rc.Name CategoryName
,exc.Code ExcludeCategoryCode 
,exc.UseCashOnDueDates
,exc.Name ExcludeCategoryName
,cus.Code CustomerCode
,cus.Name CustomerName
,cur.Code CurrencyCode
,sect.Code SectionCode
,sect.Name SectionName
,memo.Memo ReceiptMemo
,memo.Memo TransferMemo
,memo.ReceiptId ReceiptId
,CASE WHEN re.OutputAt IS NULL THEN 0 ELSE 1 END RecExcOutputAt
,CASE WHEN rst.DestinationReceiptId IS NULL THEN 'false' ELSE 'true' END CancelFlag";

            if (option.AdvanceReceivedFlg.HasValue)
            {
                if (option.AdvanceReceivedFlg.Value == 1)
                {
                    query += string.Format(@"
,CASE WHEN r2.DeleteAt IS NOT NULL AND r2.DeleteAt <> '' THEN {0}", (int)ReceiptStatus.Deleted);
                    query += string.Format(@"
      WHEN r.OutputAt IS NOT NULL AND r.OutputAt <> '' THEN {0}", (int)ReceiptStatus.Journalized);
                    query += string.Format(@"
      WHEN r.AssignmentFlag <> 0 THEN {0}", (int)ReceiptStatus.PartOrFullAssigned);
                    query += string.Format(@"
      WHEN arb.Id IS NOT NULL THEN {0}", (int)ReceiptStatus.AdvancedReceived);
                    query += string.Format(@"
      WHEN rst.DestinationReceiptId IS NOT NULL THEN {0}", (int)ReceiptStatus.SectionTransfered);
                    query += string.Format(@"
      ELSE {0} END ReceiptStatusFlag", (int)ReceiptStatus.None);
                    query += @"
,CASE WHEN (r.OutputAt IS NOT NULL AND r.OutputAt <> '')
           AND r2.RemainAmount > 0 
           AND (r2.DeleteAt IS NULL OR r2.DeleteAt = '')
      THEN 1 ELSE 0 END  RemainAmountFlag";
                }
                else if(option.AdvanceReceivedFlg.Value == 0)
                {
                    query += string.Format(@"
,{0} ReceiptStatusFlag", (int)ReceiptStatus.None);
                    query += @"
,0 RemainAmountFlag";
                }
            }

            query += @"
FROM Receipt as r
LEFT JOIN Customer        cus       ON  cus.Id  = r.CustomerId
LEFT JOIN Currency        cur       ON  cur.Id  = r.CurrencyId
LEFT JOIN ReceiptMemo     memo      ON  r.Id    = memo.ReceiptId
LEFT JOIN Section         sect      ON  sect.Id = r.SectionId
LEFT JOIN Netting         n         ON  r.Id    = n.ReceiptId
LEFT JOIN Category        rc        ON  rc.Id   = r.ReceiptCategoryId
LEFT JOIN Category        exc       ON  exc.Id  = r.ExcludeCategoryId
LEFT JOIN ReceiptHeader   rh        ON  rh.Id   = r.ReceiptHeaderId
LEFT JOIN BankAccountType bat       ON  bat.Id  = r.AccountTypeId
LEFT JOIN BankAccount     ba        ON  ba.CompanyId        = r.CompanyId
                                    AND ba.BankCode         = r.BankCode
                                    AND ba.BranchCode       = r.BranchCode
                                    AND ba.AccountTypeId    = r.AccountTypeId
                                    AND ba.AccountNumber    = r.AccountNumber
LEFT JOIN ReceiptSectionTransfer rst ON rst.DestinationReceiptId = r.Id
LEFT JOIN (
     SELECT re.ReceiptId
          , MAX(re.OutputAt) [OutputAt]
       FROM ReceiptExclude re
      GROUP BY re.ReceiptId ) re    ON r.Id = re.ReceiptId";

            if (option.AdvanceReceivedFlg.HasValue)
            {
                if (option.AdvanceReceivedFlg.Value == 1)
                {
                    query += @"
LEFT JOIN Receipt as r2 ON r2.Id = r.OriginalReceiptId
LEFT JOIN AdvanceReceivedBackup as arb ON arb.OriginalReceiptId = r.OriginalReceiptId";
                }
            }

            query += @"
WHERE r.CompanyId = @CompanyId
  AND r.Apportioned = 1 ";


            var whereCondition = new StringBuilder();
            if (option.UseSectionMaster)
            {
                if (option.KobetsuType != "Matching")
                {
                    whereCondition.AppendLine(@"
  AND r.SectionId IN (
    SELECT SectionId
    FROM SectionWithLoginUser su
    WHERE su.LoginUserId = @LoginUserId) ");

                }
            }

            if (option.UseSectionWork)
            {
                whereCondition.AppendLine(@"
   AND r.SectionId IN (
       SELECT DISTINCT wst.SectionId
         FROM WorkSectionTarget wst
        WHERE wst.ClientKey     = @ClientKey
          AND wst.UseCollation  = 1 )");
            }

            if (option.RecordedAtFrom.HasValue)
            {
                option.RecordedAtFrom = option.RecordedAtFrom.Value.Date;
                whereCondition.AppendLine(" AND r.RecordedAt >= @RecordedAtFrom");
            }
            if (option.RecordedAtTo.HasValue)
            {
                option.RecordedAtTo = option.RecordedAtTo.Value.Date;
                whereCondition.AppendLine(" AND r.RecordedAt <= @RecordedAtTo");
            }
            if (!string.IsNullOrEmpty(option.PayerName))
            {
                option.PayerName = Sql.GetWrappedValue(option.PayerName);
                whereCondition.AppendLine(" AND  r.PayerName LIKE @PayerName");
            }
            if (option.UpdateAtFrom.HasValue)
            {
                option.UpdateAtFrom = option.UpdateAtFrom.Value.Date;
                whereCondition.AppendLine(" AND r.UpdateAt >= @UpdateAtFrom");
            }
            if (option.UpdateAtTo.HasValue)
            {
                option.UpdateAtTo = option.UpdateAtTo.Value.Date.AddDays(1).AddMilliseconds(-1);
                whereCondition.AppendLine(" AND r.UpdateAt <= @UpdateAtTo");
            }
            if (option.UpdateBy.HasValue)
            {
                whereCondition.AppendLine(" AND r.UpdateBy = @UpdateBy");
            }
            if (option.UseForeignCurrencyFlg == 1
                && option.CurrencyId != 0)
            {
                whereCondition.AppendLine(" AND cur.Id = @CurrencyId");
            }
            if (!string.IsNullOrEmpty(option.BankCode))
            {
                whereCondition.AppendLine(" AND r.BankCode = @BankCode");
            }

            if (!string.IsNullOrEmpty(option.BranchCode))
            {
                whereCondition.AppendLine(" AND r.BranchCode = @BranchCode");
            }
            if (option.AccountTypeId != 0)
            {
                whereCondition.AppendLine(" AND r.AccountTypeId = @AccountTypeId");
            }
            if (!string.IsNullOrEmpty(option.AccountNumber))
            {
                whereCondition.AppendLine(" AND r.AccountNumber = @AccountNumber");
            }

            if (!string.IsNullOrEmpty(option.PrivateBankCode))
            {
                whereCondition.AppendLine(" AND r.BankCode = @PrivateBankCode");
            }

            if (!string.IsNullOrEmpty(option.PayerCodePrefix))
            {
                whereCondition.AppendLine(" AND SUBSTRING(r.PayerCode,1,3) = @PayerCodePrefix");
            }
            if (!string.IsNullOrEmpty(option.PayerCodeSuffix))
            {
                whereCondition.AppendLine(" AND SUBSTRING(r.PayerCode,4,10) = @PayerCodeSuffix");
            }
            if (!string.IsNullOrEmpty(option.BillNumber))
            {
                option.BillNumber = Sql.GetWrappedValue(option.BillNumber);
                whereCondition.AppendLine(" AND r.BillNumber LIKE @BillNumber");
            }
            if (!string.IsNullOrEmpty(option.BillBankCode))
            {
                whereCondition.AppendLine(" AND r.BillBankCode = @BillBankCode");
            }
            if (!string.IsNullOrEmpty(option.BillBranchCode))
            {
                whereCondition.AppendLine(" AND r.BillBranchCode = @BillBranchCode");
            }
            if (option.BillDrawAtFrom.HasValue)
            {
                option.BillDrawAtFrom = option.BillDrawAtFrom.Value.Date;
                whereCondition.AppendLine(" AND r.BillDrawAt >= @BillDrawAtFrom");
            }
            if (option.BillDrawAtTo.HasValue)
            {
                option.BillDrawAtTo = option.BillDrawAtTo.Value.Date;
                whereCondition.AppendLine(" AND r.BillDrawAt <= @BillDrawAtTo");
            }
            if (!string.IsNullOrEmpty(option.BillDrawer))
            {
                option.BillDrawer = Sql.GetWrappedValue(option.BillDrawer);
                whereCondition.AppendLine(" AND  r.BillDrawer LIKE @BillDrawer");
            }
            if (!string.IsNullOrEmpty(option.CustomerCodeFrom))
            {
                whereCondition.AppendLine(" AND cus.Code >= @CustomerCodeFrom");
            }
            if (!string.IsNullOrEmpty(option.CustomerCodeTo))
            {
                whereCondition.AppendLine(" AND cus.Code <= @CustomerCodeTo");
            }
            if (!string.IsNullOrEmpty(option.SectionCode))
            {
                whereCondition.AppendLine(" AND sect.Code = @SectionCode");
            }
            if (!string.IsNullOrEmpty(option.SectionCodeFrom))
            {
                whereCondition.AppendLine(" AND sect.Code >= @SectionCodeFrom");
            }
            if (!string.IsNullOrEmpty(option.SectionCodeTo))
            {
                whereCondition.AppendLine(" AND sect.Code <= @SectionCodeTo");
            }
            if (option.ExistsMemo == 1)
            {
                whereCondition.AppendLine(" AND memo.Memo IS NOT NULL");
            }
            if (!string.IsNullOrEmpty(option.ReceiptMemo))
            {
                option.ReceiptMemo = Sql.GetWrappedValue(option.ReceiptMemo);
                whereCondition.AppendLine(" AND memo.Memo LIKE @ReceiptMemo");
            }
            if (option.InputType.HasValue)
            {
                whereCondition.AppendLine(" AND r.InputType = @InputType");
            }
            if (!string.IsNullOrEmpty(option.ReceiptCategoryCode))
            {
                whereCondition.AppendLine(" AND rc.Code = @ReceiptCategoryCode");
            }
            if (!string.IsNullOrEmpty(option.ReceiptCategoryCodeFrom))
            {
                whereCondition.AppendLine(" AND rc.Code >= @ReceiptCategoryCodeFrom");
            }
            if (!string.IsNullOrEmpty(option.ReceiptCategoryCodeTo))
            {
                whereCondition.AppendLine(" AND rc.Code <= @ReceiptCategoryCodeTo");
            }

            var flags = (AssignmentFlagChecked)option.AssignmentFlag;
            var selectedValues = new List<int>();
            if (flags.HasFlag(AssignmentFlagChecked.NoAssignment)) selectedValues.Add(0);
            if (flags.HasFlag(AssignmentFlagChecked.PartAssignment)) selectedValues.Add(1);
            if (flags.HasFlag(AssignmentFlagChecked.FullAssignment)) selectedValues.Add(2);
            if (selectedValues.Any() && !flags.HasFlag(AssignmentFlagChecked.All))
            {
                whereCondition.AppendLine($" AND r.AssignmentFlag IN ({(string.Join(", ", selectedValues))})");
            }

            if (option.ExcludeFlag != 2)
            {
                if (option.ExcludeFlag == 0)
                {
                    whereCondition.AppendLine(" AND r.ReceiptAmount <> r.ExcludeAmount");
                }
                else
                {
                    whereCondition.AppendLine(" AND r.ExcludeFlag = @ExcludeFlag");
                }
            }
            if (option.ExcludeCategoryId.HasValue)
            {
                whereCondition.AppendLine(" AND r.ExcludeCategoryId = @ExcludeCategoryId");
            }
            if (option.ReceiptCategoryId != 0)
            {
                whereCondition.AppendLine(" AND r.ReceiptCategoryId= @ReceiptCategoryId");
            }

            if (option.ReceiptAmountFrom.HasValue)
            {
                whereCondition.AppendLine(" AND r.ReceiptAmount >= @ReceiptAmountFrom");
            }
            if (option.ReceiptAmountTo.HasValue)
            {
                whereCondition.AppendLine(" AND r.ReceiptAmount <= @ReceiptAmountTo");
            }
            if (option.RemainAmountFrom.HasValue)
            {
                whereCondition.AppendLine(" AND r.RemainAmount >= @RemainAmountFrom");
            }
            if (option.RemainAmountTo.HasValue)
            {
                whereCondition.AppendLine(" AND r.RemainAmount <= @RemainAmountTo");
            }

            if (!string.IsNullOrEmpty(option.SourceBankName))
            {
                option.SourceBankName = Sql.GetWrappedValue(option.SourceBankName);
                whereCondition.AppendLine(" AND  r.SourceBankName LIKE @SourceBankName");
            }
            if (!string.IsNullOrEmpty(option.SourceBranchName))
            {
                option.SourceBranchName = Sql.GetWrappedValue(option.SourceBranchName);
                whereCondition.AppendLine(" AND  r.SourceBranchName LIKE @SourceBranchName");
            }

            if (option.AdvanceReceivedFlg.HasValue)
            {
                if (option.AdvanceReceivedFlg.Value == 1)
                {
                    whereCondition.AppendLine(" AND rc.UseAdvanceReceived = 1");
                }
                else if (option.AdvanceReceivedFlg.Value == 0)
                {
                    whereCondition.AppendLine(" AND rc.UseAdvanceReceived = 0");
                }
            }
            if (!string.IsNullOrEmpty(option.Note1))
            {
                option.Note1 = Sql.GetWrappedValue(option.Note1);
                whereCondition.AppendLine(" AND  r.Note1 LIKE @Note1");
            }
            if (!string.IsNullOrEmpty(option.Note2))
            {
                option.Note2 = Sql.GetWrappedValue(option.Note2);
                whereCondition.AppendLine(" AND  r.Note2 LIKE @Note2");
            }
            if (!string.IsNullOrEmpty(option.Note3))
            {
                option.Note3 = Sql.GetWrappedValue(option.Note3);
                whereCondition.AppendLine(" AND  r.Note3 LIKE @Note3");
            }
            if (!string.IsNullOrEmpty(option.Note4))
            {
                option.Note4 = Sql.GetWrappedValue(option.Note4);
                whereCondition.AppendLine(" AND  r.Note4 LIKE @Note4");
            }

            if (option.DeleteFlg == 1)
            {
                // IS NOT NULL
                whereCondition.AppendLine(" AND r.DeleteAt = r.DeleteAt ");
                if (option.DeleteAtFrom.HasValue)
                {
                    whereCondition.AppendLine(" AND r.DeleteAt >= @DeleteAtFrom");
                }
                if (option.DeleteAtTo.HasValue)
                {
                    whereCondition.AppendLine(" AND r.DeleteAt <= @DeleteAtTo");
                }
            }
            else
            {
                whereCondition.AppendLine(" AND r.DeleteAt IS NULL ");
            }
            if (option.IsEditable)
            {
                whereCondition.AppendLine(@"
  AND r.ExcludeFlag = 0
  AND r.AssignmentFlag = 0
  AND r.OriginalReceiptId IS NULL
  AND r.OutputAt IS NULL
  AND r.DeleteAt IS NULL");
            }

            if (option.UseForeignCurrencyFlg == 1)
            {
                whereCondition.AppendLine(" ORDER BY cur.DisplayOrder ASC,r.RecordedAt ASC,r.Id ASC");
            }
            if (option.UseForeignCurrencyFlg == 0)
            {
                whereCondition.AppendLine(" ORDER BY r.RecordedAt ASC,r.Id ASC");
            }


            query += whereCondition.ToString();

            //if (ReceiptSearch.SectionId == null)
            //    ReceiptSearch.SectionId = new int[] { };
            return dbHelper.GetItemsAsync<Receipt>(query, option, token);
        }

    }
}
