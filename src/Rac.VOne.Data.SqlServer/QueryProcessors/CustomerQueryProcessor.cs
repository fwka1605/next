using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CustomerQueryProcessor :
        IAddCustomerQueryProcessor,
        IUpdateCustomerQueryProcessor,
        ICustomerQueryProcessor,
        ICustomerMinQueryProcessor,
        ICustomerExistsQueryProcessor,
        ICustomerImportQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CustomerQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<Customer>> GetAsync(CustomerSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        cs.*
            , cpc.*
            , st.Code       [StaffCode]
            , st.Name       [StaffName]
            , cct.Code      [CollectCategoryCode]
            , cct.Name      [CollectCategoryName]
            , eat.Name      [ExclusiveAccountTypeName]
            , tat.Name      [TransferAccountTypeName]
            , csg.ParentCustomerId
            , pcs.Code      [ParentCode]
            , pcs.Name      [ParentName]
            , cct0.Name     [LessThanCollectCategoryName]
            , cct1.Name     [GreaterThanCollectCategoryName1]
            , cct2.Name     [GreaterThanCollectCategoryName2]
            , cct3.Name     [GreaterThanCollectCategoryName3]
            , CASE cs.ReceiveAccountId1 WHEN 1 THEN cm.BankName1 + N'  ' + cm.BranchName1 + N'  ' + cm.AccountType1 + N'  ' + cm.AccountNumber1 ELSE N'' END [CompanyBankInfo1]
            , CASE cs.ReceiveAccountId2 WHEN 1 THEN cm.BankName2 + N'  ' + cm.BranchName2 + N'  ' + cm.AccountType2 + N'  ' + cm.AccountNumber2 ELSE N'' END [CompanyBankInfo2]
            , CASE cs.ReceiveAccountId3 WHEN 1 THEN cm.BankName3 + N'  ' + cm.BranchName3 + N'  ' + cm.AccountType3 + N'  ' + cm.AccountNumber3 ELSE N'' END [CompanyBankInfo3]
            , lu.Name       [LastUpdateUser]
FROM        Customer cs
LEFT JOIN   Staff st                    ON   st.Id      =  cs.StaffId
LEFT JOIN   Category cct                ON  cct.Id      =  cs.CollectCategoryId
LEFT JOIN   BankAccountType eat         ON  eat.Id      =  cs.ExclusiveAccountTypeId
LEFT JOIN   BankAccountType tat         ON  tat.Id      =  cs.TransferAccountTypeId
LEFT JOIN   CustomerGroup csg           ON   cs.Id      = csg.ChildCustomerId
LEFT JOIN   Customer pcs                ON  pcs.Id      = csg.ParentCustomerId
LEFT JOIN   CustomerPaymentContract cpc ON   cs.Id      = cpc.CustomerId
LEFT JOIN   Category cct0               ON cct0.Id      = cpc.LessThanCollectCategoryId
LEFT JOIN   Category cct1               ON cct1.Id      = cpc.GreaterThanCollectCategoryId1
LEFT JOIN   Category cct2               ON cct2.Id      = cpc.GreaterThanCollectCategoryId2
LEFT JOIN   Category cct3               ON cct3.Id      = cpc.GreaterThanCollectCategoryId3
LEFT JOIN   LoginUser lu                ON   lu.Id      =  cs.UpdateBy
LEFT JOIN   Company cm                  ON   cm.Id      =  cs.CompanyId
WHERE       cs.Id                   = cs.Id";
            if (option.CompanyId.HasValue) query += @"
AND         cs.CompanyId            = @CompanyId";
            if (option.Ids?.Any() ?? false) query += @"
AND         cs.Id                   IN (SELECT Id   FROM @Ids)";
            if (option.Codes?.Any() ?? false) query += @"
AND         cs.Code                 IN (SELECT Code FROM @Codes)";
            if (option.ClosingDay.HasValue) query += @"
AND         cs.ClosingDay           = @ClosingDay";
            if (option.ShareTransferFee.HasValue) query += @"
AND         cs.ShareTransferFee     = @ShareTransferFee";
            if (option.IsParent.HasValue) query += @"
AND         cs.IsParent             = @IsParent";
            if (!string.IsNullOrEmpty(option.CustomerCodeFrom)) query += @"
AND         cs.Code                 >= @CustomerCodeFrom";
            if (!string.IsNullOrEmpty(option.CustomerCodeTo)) query += @"
AND         cs.Code                 <= @CustomerCodeTo";
            if (option.UpdateAtFrom.HasValue) query += @"
AND         cs.UpdateAt             >= @UpdateAtFrom";
            if (option.UpdateAtTo.HasValue) query += @"
AND         cs.UpdateAt             <= @UpdateAtTo";
            if (!string.IsNullOrEmpty(option.StaffCodeFrom)) query += @"
AND         st.Code                 >= @StaffCodeFrom";
            if (!string.IsNullOrEmpty(option.StaffCodeTo)) query += @"
AND         st.Code                 <= @StaffCodeTo";
            if (option.ParentCustomerId.HasValue) query += @"
AND         COALESCE(pcs.Id, cs.Id) = @ParentCustomerId";
            if (option.XorParentCustomerId.HasValue) query += @"
AND         (
                NOT EXISTS (
                SELECT      1
                FROM        CustomerGroup csg2
                WHERE       cs.Id           = csg2.ChildCustomerId
                )
            OR  EXISTS (
                SELECT      1
                FROM        CustomerGroup csg2
                WHERE       cs.Id           = csg2.ChildCustomerId
                AND         csg2.ParentCustomerId   = @XorParentCustomerId
                )
            )";
            if (!string.IsNullOrEmpty(option.ExclusiveBankCode)) query += @"
AND         cs.ExclusiveBankCode    = @ExclusiveBankCode";
            if (!string.IsNullOrEmpty(option.ExclusiveBranchCode)) query += @"
AND         cs.ExclusiveBranchCode  = @ExclusiveBranchCode";
            if (!string.IsNullOrEmpty(option.ExclusiveAccountNumber)) query += @"
AND         cs.ExclusiveAccountNumber   = @ExclusiveAccountNumber";
            query += @"
ORDER BY      cs.CompanyId          ASC
            , cs.Code               ASC";
            return dbHelper.GetItemsAsync<Customer>(query, new {
                            option.CompanyId,
                Ids     =   option.Ids.GetTableParameter(),
                Codes   =   option.Codes.GetTableParameter(),
                            option.ClosingDay,
                            option.ShareTransferFee,
                            option.IsParent,
                            option.CustomerCodeFrom,
                            option.CustomerCodeTo,
                            option.UpdateAtFrom,
                            option.UpdateAtTo,
                            option.StaffCodeFrom,
                            option.StaffCodeTo,
                            option.ParentCustomerId,
                            option.XorParentCustomerId,
                            option.ExclusiveBankCode,
                            option.ExclusiveBranchCode,
                            option.ExclusiveAccountNumber,
            }, token);
        }

        public Task<Customer> SaveAsync(Customer Customer, bool requireIsParentUpdate = false, CancellationToken token = default(CancellationToken))
        {
            #region mrege query
            var query = $@"
MERGE Customer AS Target
USING (
    SELECT @CompanyId   AS CompanyId 
         , @Code        AS Code
) AS Source
ON ( 
        Target.CompanyId    = Source.CompanyId
    AND Target.Code         = Source.Code
) 
WHEN MATCHED THEN 
{GetUpdateQuery(requireIsParentUpdate)}
WHEN NOT MATCHED THEN 
{GetInsertQuery()}
OUTPUT inserted.*; ";
            #endregion
            return dbHelper.ExecuteAsync<Customer>(query, Customer, token);
        }

        #region query
        #region insert query
        private string GetInsertQuery()
            => $@"
INSERT
{GetInsertQueryColumns()}
{GetInsertQueryValues()}";
        private string GetInsertQueryColumns()
            => @"
(
  CompanyId
, Code
, Name
, Kana
, PostalCode
, Address1
, Address2
, Tel
, Fax
, CustomerStaffName
, ExclusiveBankCode
, ExclusiveBankName
, ExclusiveBranchCode
, ExclusiveBranchName
, ExclusiveAccountNumber
, ExclusiveAccountTypeId
, ShareTransferFee
, CreditLimit
, ClosingDay
, CollectCategoryId
, CollectOffsetMonth
, CollectOffsetDay
, StaffId
, IsParent
, Note
, SightOfBill
, DensaiCode
, CreditCode
, CreditRank
, TransferBankCode
, TransferBankName
, TransferBranchCode
, TransferBranchName
, TransferAccountNumber
, TransferAccountTypeId
, ReceiveAccountId1
, ReceiveAccountId2
, ReceiveAccountId3
, UseFeeLearning
, UseKanaLearning
, HolidayFlag
, UseFeeTolerance
, StringValue1
, StringValue2
, StringValue3
, StringValue4
, StringValue5
, IntValue1
, IntValue2
, IntValue3
, IntValue4
, IntValue5
, NumericValue1
, NumericValue2
, NumericValue3
, NumericValue4
, NumericValue5
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, TransferCustomerCode
, TransferNewCode
, TransferAccountName
, PrioritizeMatchingIndividually
, CollationKey
, ExcludeInvoicePublish
, ExcludeReminderPublish
, DestinationDepartmentName
, Honorific
  )";
        private string GetInsertQueryValues()
            => @"
  VALUES
  (
  @CompanyId
, @Code
, @Name
, @Kana
, @PostalCode
, @Address1
, @Address2
, @Tel
, @Fax
, @CustomerStaffName
, @ExclusiveBankCode
, @ExclusiveBankName
, @ExclusiveBranchCode
, @ExclusiveBranchName
, @ExclusiveAccountNumber
, @ExclusiveAccountTypeId
, @ShareTransferFee
, @CreditLimit
, @ClosingDay
, @CollectCategoryId
, @CollectOffsetMonth
, @CollectOffsetDay
, @StaffId
, @IsParent
, @Note
, @SightOfBill
, @DensaiCode
, @CreditCode
, @CreditRank
, @TransferBankCode
, @TransferBankName
, @TransferBranchCode
, @TransferBranchName
, @TransferAccountNumber
, @TransferAccountTypeId
, @ReceiveAccountId1
, @ReceiveAccountId2
, @ReceiveAccountId3
, @UseFeeLearning
, @UseKanaLearning
, @HolidayFlag
, @UseFeeTolerance
, @StringValue1
, @StringValue2
, @StringValue3
, @StringValue4
, @StringValue5
, @IntValue1
, @IntValue2
, @IntValue3
, @IntValue4
, @IntValue5
, @NumericValue1
, @NumericValue2
, @NumericValue3
, @NumericValue4
, @NumericValue5
, @CreateBy
, GETDATE()
, @UpdateBy
, GETDATE()
, @TransferCustomerCode
, @TransferNewCode
, @TransferAccountName
, @PrioritizeMatchingIndividually
, @CollationKey
, @ExcludeInvoicePublish
, @ExcludeReminderPublish
, @DestinationDepartmentName
, @Honorific
    )";
        #endregion
        #region update query
        private string GetUpdateQuery(bool requireIsParentUpdate = false)
            => $@"
UPDATE SET
{GetUpdateQueryItems(requireIsParentUpdate)}";

        private string GetUpdateQueryItems(bool requireIsParentUpdate = false)
            => $@"
  Name                      = @Name
, Kana                      = @Kana
, PostalCode                = @PostalCode
, Address1                  = @Address1
, Address2                  = @Address2
, Tel                       = @Tel
, Fax                       = @Fax
, CustomerStaffName         = @CustomerStaffName
, ExclusiveBankCode         = @ExclusiveBankCode
, ExclusiveBankName         = @ExclusiveBankName
, ExclusiveBranchCode       = @ExclusiveBranchCode
, ExclusiveBranchName       = @ExclusiveBranchName
, ExclusiveAccountNumber    = @ExclusiveAccountNumber
, ExclusiveAccountTypeId    = @ExclusiveAccountTypeId
, ShareTransferFee          = @ShareTransferFee
, CreditLimit               = @CreditLimit
, ClosingDay                = @ClosingDay
, CollectCategoryId         = @CollectCategoryId
, CollectOffsetMonth        = @CollectOffsetMonth
, CollectOffsetDay          = @CollectOffsetDay
, StaffId                   = @StaffId
{(requireIsParentUpdate ? ", IsParent                  = @IsParent" : "")}
, Note                      = @Note
, SightOfBill               = @SightOfBill
, DensaiCode                = @DensaiCode
, CreditCode                = @CreditCode
, CreditRank                = @CreditRank
, TransferBankCode          = @TransferBankCode
, TransferBankName          = @TransferBankName
, TransferBranchCode        = @TransferBranchCode
, TransferBranchName        = @TransferBranchName
, TransferAccountNumber     = @TransferAccountNumber
, TransferAccountTypeId     = @TransferAccountTypeId
, ReceiveAccountId1         = @ReceiveAccountId1
, ReceiveAccountId2         = @ReceiveAccountId2
, ReceiveAccountId3         = @ReceiveAccountId3
, UseFeeLearning            = @UseFeeLearning
, UseKanaLearning           = @UseKanaLearning
, HolidayFlag               = @HolidayFlag
, UseFeeTolerance           = @UseFeeTolerance
, StringValue1              = @StringValue1
, StringValue2              = @StringValue2
, StringValue3              = @StringValue3
, StringValue4              = @StringValue4
, StringValue5              = @StringValue5
, IntValue1                 = @IntValue1
, IntValue2                 = @IntValue2
, IntValue3                 = @IntValue3
, IntValue4                 = @IntValue4
, IntValue5                 = @IntValue5
, NumericValue1             = @NumericValue1
, NumericValue2             = @NumericValue2
, NumericValue3             = @NumericValue3
, NumericValue4             = @NumericValue4
, NumericValue5             = @NumericValue5
, UpdateBy                  = @UpdateBy
, UpdateAt                  = GETDATE()
, TransferCustomerCode      = @TransferCustomerCode
, TransferNewCode           = @TransferNewCode
, TransferAccountName       = @TransferAccountName
, PrioritizeMatchingIndividually = @PrioritizeMatchingIndividually
, CollationKey              = @CollationKey
, ExcludeInvoicePublish     = @ExcludeInvoicePublish
, ExcludeReminderPublish    = @ExcludeReminderPublish
, DestinationDepartmentName = @DestinationDepartmentName
, Honorific                 = @Honorific
";
        #endregion
        #endregion


        #region update isparent
        private string GetQueryUpdateIsParent() => @"
UPDATE Customer
   SET IsParent     = @isParent
     , UpdateBy     = @loginUserId
     , UpdateAt     = GETDATE()
 WHERE Id IN (SELECT Id FROM @ids)
";


        public Task<int> UpdateIsParentAsync(int isParent, int loginUserId, IEnumerable<int> ids, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync(GetQueryUpdateIsParent(), new { isParent, loginUserId, ids = ids.GetTableParameter() }, token);

        #endregion





        public async Task<bool> ExistCompanyAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = "SELECT 1 WHERE EXISTS ( SELECT 1 FROM Customer WHERE CompanyId = @CompanyId )";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CompanyId }, token)).HasValue;
        }

        public async Task<bool> ExistCategoryAsync(int CollectCategoryId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1 
         FROM Customer
        WHERE CollectCategoryId = @CollectCategoryId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CollectCategoryId }, token)).HasValue;
        }

        public async Task<bool> ExistStaffAsync(int StaffId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
        SELECT 1 
          FROM Customer
         WHERE StaffId = @StaffId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { StaffId })).HasValue;
        }



        public Task<IEnumerable<MasterData>> GetImportItemsParentAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT c.Code, c.Name FROM Customer c
WHERE c.CompanyId = @CompanyId
AND EXISTS (
    SELECT *
        FROM CustomerGroup customerG 
        WHERE customerG.ParentCustomerId = c.Id ) ";
            if (Code.Any()) query += "AND c.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsChildAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT c.Code, c.Name FROM Customer c
WHERE c.CompanyId = @CompanyId
AND EXISTS (
    SELECT *
        FROM CustomerGroup customerG 
        WHERE customerG.ChildCustomerId = c.Id ) ";
            if (Code.Any()) query += "AND c.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsKanaHistoryAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT c.Code, c.Name FROM Customer c
WHERE c.CompanyId = @CompanyId
AND EXISTS (
    SELECT *
        FROM KanaHistoryCustomer kana
        WHERE kana.CompanyId = @CompanyId
        AND kana.CustomerId = c.Id ) ";
            if (Code.Any()) query += "AND c.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT c.Code, c.Name FROM Customer c
WHERE c.CompanyId = @CompanyId
AND EXISTS (
     SELECT *
       FROM Billing bill
       WHERE bill.CompanyId = @CompanyId
       AND bill.CustomerId = c.Id ) ";
            if (Code.Any()) query += "AND c.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsReceiptAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT c.Code, c.Name FROM Customer c
WHERE c.CompanyId = @CompanyId
AND EXISTS (
    SELECT *
        FROM Receipt receipt
        WHERE receipt.CompanyId = @CompanyId
        AND receipt.CustomerId = c.Id ) ";
            if (Code.Any()) query += "AND c.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsNettingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT c.Code, c.Name FROM Customer c
WHERE c.CompanyId = @CompanyId
AND EXISTS (
     SELECT *
       FROM Netting netting
       WHERE netting.CompanyId = @CompanyId
       AND netting.CustomerId = c.Id ) ";
            if (Code.Any()) query += "AND c.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code?.GetTableParameter() }, token);
        }

        public Task<Customer> UpdateForBilingImportAsync(Customer updateCustomer, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE      Customer
SET           ExclusiveBankCode         = @ExclusiveBankCode
            , ExclusiveBranchCode       = @ExclusiveBranchCode
            , ExclusiveAccountNumber    = @ExclusiveAccountNumber
OUTPUT      inserted.*
WHERE       CompanyId                   = @CompanyId
AND         Code                        = @Code";
            return dbHelper.ExecuteAsync<Customer>(query, updateCustomer, token);
        }

        public Task<Customer> UpdateTransferNewCodeAsync(int id, int loginUserId, string transferNewCode, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE      Customer
SET           TransferNewCode   = @transferNewCode
            , UpdateBy          = @loginUserId
            , UpdateAt          = GETDATE()
OUTPUT      INSERTED.*
WHERE       Id                  = @id
";
            return dbHelper.ExecuteAsync<Customer>(query, new { id, loginUserId, transferNewCode,
            }, token);
        }

        public Task<IEnumerable<CustomerMin>> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT cs.Id
     , cs.CompanyId
     , cs.Code
     , cs.Name
     , cs.Kana
  FROM Customer cs
 WHERE cs.CompanyId     = @CompanyId
 ORDER BY
       cs.CompanyId
     , cs.Code";
            return dbHelper.GetItemsAsync<CustomerMin>(query, new { CompanyId }, token);
        }
    }
}
