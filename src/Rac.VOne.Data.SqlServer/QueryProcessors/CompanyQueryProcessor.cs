using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CompanyQueryProcessor :
        ICompanyQueryProcessor,
        IAddCompanyQueryProcessor,
        IDeleteCompanyQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public CompanyQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<Company>> GetAsync(CompanySearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        Company cm
WHERE       cm.Id           = cm.Id";
            if (option.Id.HasValue) query += @"
AND         cm.Id           = @Id";
            if (!string.IsNullOrEmpty(option.Code)) query += @"
AND         cm.Code         = @Code";
            if (!string.IsNullOrWhiteSpace(option.Name))
            {
                option.Name = Sql.GetWrappedValue(option.Name);
                query += @"
AND         cm.Name         LIKE @Name";
            }
            query += @"
ORDER BY    cm.Code         ASC";
            return dbHelper.GetItemsAsync<Company>(query, option, token);
        }


        public Task<Company> SaveAsync(Company Company, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO Company AS target 
USING ( 
    SELECT 
        @Id AS Id 
) AS source 
ON ( 
    target.Id = source.Id 
) 
WHEN MATCHED THEN 
    UPDATE SET 
         Code                   = @Code
        ,Name                   = @Name
        ,Kana                   = @Kana
        ,PostalCode             = @PostalCode
        ,Address1               = @Address1
        ,Address2               = @Address2
        ,Tel                    = @Tel
        ,Fax                    = @Fax
        ,BankAccountName        = @BankAccountName
        ,BankAccountKana        = @BankAccountKana
        ,BankName1              = @BankName1
        ,BranchName1            = @BranchName1
        ,AccountType1           = @AccountType1
        ,AccountNumber1         = @AccountNumber1
        ,BankName2              = @BankName2
        ,BranchName2            = @BranchName2
        ,AccountType2           = @AccountType2
        ,AccountNumber2         = @AccountNumber2
        ,BankName3              = @BankName3
        ,BranchName3            = @BranchName3
        ,AccountType3           = @AccountType3
        ,AccountNumber3         = @AccountNumber3
        ,ProductKey             = @ProductKey
        ,ShowConfirmDialog      = @ShowConfirmDialog
        ,PresetCodeSearchDialog = @PresetCodeSearchDialog
        ,ShowWarningDialog      = @ShowWarningDialog
        ,ClosingDay             = @ClosingDay
        ,UpdateBy               = @UpdateBy
        ,UpdateAt               = GETDATE()
        ,TransferAggregate      = @TransferAggregate
        ,AutoCloseProgressDialog = @AutoCloseProgressDialog
WHEN NOT MATCHED THEN 
    INSERT (
         Code
        ,Name
        ,Kana
        ,PostalCode
        ,Address1
        ,Address2
        ,Tel
        ,Fax
        ,BankAccountName
        ,BankAccountKana
        ,BankName1
        ,BranchName1
        ,AccountType1
        ,AccountNumber1
        ,BankName2
        ,BranchName2
        ,AccountType2
        ,AccountNumber2
        ,BankName3
        ,BranchName3
        ,AccountType3
        ,AccountNumber3
        ,ProductKey
        ,ShowConfirmDialog
        ,PresetCodeSearchDialog
        ,ShowWarningDialog
        ,ClosingDay
        ,CreateBy
        ,CreateAt
        ,UpdateBy
        ,UpdateAt
        ,TransferAggregate
        ,AutoCloseProgressDialog
)
    VALUES (
         @Code
        ,@Name
        ,@Kana
        ,@PostalCode
        ,@Address1
        ,@Address2
        ,@Tel
        ,@Fax
        ,@BankAccountName
        ,@BankAccountKana
        ,@BankName1
        ,@BranchName1
        ,@AccountType1
        ,@AccountNumber1
        ,@BankName2
        ,@BranchName2
        ,@AccountType2
        ,@AccountNumber2
        ,@BankName3
        ,@BranchName3
        ,@AccountType3
        ,@AccountNumber3
        ,@ProductKey
        ,@ShowConfirmDialog
        ,@PresetCodeSearchDialog
        ,@ShowWarningDialog
        ,@ClosingDay
        ,@CreateBy
        ,GETDATE()
        ,@UpdateBy
        ,GETDATE()
        ,@TransferAggregate
        ,@AutoCloseProgressDialog
)
OUTPUT inserted.*; ";
            #endregion
            return dbHelper.ExecuteAsync<Company>(query, Company, token);
        }

        public Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
        {
            #region delete query
            var query = @"
DELETE WorkCollation            WHERE CompanyId = @Id;
DELETE WorkBankTransfer         WHERE CompanyId = @Id;
DELETE WorkBilling              WHERE CompanyId = @Id;
DELETE WorkReceipt              WHERE CompanyId = @Id;
DELETE WorkSectionTarget        WHERE CompanyId = @Id;
DELETE WorkDepartmentTarget     WHERE CompanyId = @Id;
DELETE TaskScheduleHistory      WHERE CompanyId = @Id;
DELETE LogData                  WHERE CompanyId = @Id;
DELETE ReportSetting            WHERE CompanyId = @Id;
DELETE GridSetting              WHERE CompanyId = @Id;
DELETE ColumnNameSetting        WHERE CompanyId = @Id;
DELETE ControlColor             WHERE CompanyId = @Id;
DELETE MailTemplate             WHERE CompanyId = @Id;
DELETE MailSetting              WHERE CompanyId = @Id;
DELETE CollationSetting         WHERE CompanyId = @Id;
DELETE CollationOrder           WHERE CompanyId = @Id;
DELETE MatchingOrder            WHERE CompanyId = @Id;
DELETE TaskSchedule             WHERE CompanyId = @Id;
DELETE ExportFieldSetting       WHERE CompanyId = @Id;
DELETE EBFileSetting            WHERE CompanyId = @Id;
DELETE ImporterSetting          WHERE CompanyId = @Id;
DELETE InputControl             WHERE CompanyId = @Id;
DELETE MasterImportSetting      WHERE CompanyId = @Id;
DELETE OperationLoggingSetting  WHERE CompanyId = @Id;
DELETE JuridicalPersonality     WHERE CompanyId = @Id;
DELETE DensaiRemoveWord         WHERE CompanyId = @Id;
DELETE IgnoreKana               WHERE CompanyId = @Id;
DELETE KanaHistoryPaymentAgency WHERE CompanyId = @Id;
DELETE KanaHistoryCustomer      WHERE CompanyId = @Id;
DELETE BankAccount              WHERE CompanyId = @Id;
DELETE Currency                 WHERE CompanyId = @Id;
DELETE Category                 WHERE CompanyId = @Id;
DELETE AccountTitle             WHERE CompanyId = @Id;
DELETE PaymentAgency            WHERE CompanyId = @Id;
DELETE FunctionAuthority        WHERE CompanyId = @Id;
DELETE MenuAuthority            WHERE CompanyId = @Id;
DELETE Tax                      WHERE CompanyId = @Id;
DELETE GeneralSetting           WHERE CompanyId = @Id;
DELETE Section                  WHERE CompanyId = @Id;
DELETE BankBranch               WHERE CompanyId = @Id;
DELETE LoginUser                WHERE CompanyId = @Id;
DELETE Staff                    WHERE CompanyId = @Id;
DELETE Department               WHERE CompanyId = @Id;
DELETE HolidayCalendar          WHERE CompanyId = @Id;
DELETE LoginUserLicense         WHERE CompanyId = @Id;
DELETE PasswordPolicy           WHERE CompanyId = @Id;
DELETE ApplicationControl       WHERE CompanyId = @Id;
DELETE CompanyLogo              WHERE CompanyId = @Id;
DELETE CompanySetting           WHERE CompanyId = @Id;
DELETE StatusMaster             WHERE CompanyId = @Id;
DELETE InvoiceCommonSetting     WHERE CompanyId = @Id;
DELETE InvoiceNumberSetting     WHERE CompanyId = @Id;
DELETE InvoiceTemplateSetting   WHERE CompanyId = @Id;
DELETE ReminderCommonSetting    WHERE CompanyId = @Id;
DELETE ReminderLevelSetting     WHERE CompanyId = @Id;
DELETE ReminderTemplateSetting  WHERE CompanyId = @Id;
DELETE ReminderSummarySetting   WHERE CompanyId = @Id;

DELETE Company WHERE Id = @Id;
";
            #endregion
            return dbHelper.ExecuteAsync(query, new { Id }, token);
        }


    }
}
