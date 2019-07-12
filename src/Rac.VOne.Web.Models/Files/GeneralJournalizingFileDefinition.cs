using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Models.Files
{
    /// <summary>汎用仕訳出力用
    /// <see cref="GeneralJournalizing"/>モデルを利用する 左記モデルは Id と 共通のいくつかの値のみを保持
    /// Id に基づき、各マスター / トランザクション データを取得して、モデルを付与する
    /// FieldDefinition の IdToCode 変換 では、現状 string 型の プロパティしか参照できないため
    /// int などの値型を持っているものは、Model を付与する必要がある
    /// 各モデルのプロパティへのアクセスは、read only property を介して行う
    /// ※ lambda での null propergation operator が 許可されていないため
    ///    かつ IdToCode 変換での property info 経由での取得が poor なため
    /// VONE G3 と同じになるよう Dummy Field を利用して、項目数を合わせている
    /// </summary>
    public class GeneralJournalizingFileDefinition : RowDefinition<GeneralJournalizing>
    {
        #region common
        public StandardIdToCodeFieldDefinition<GeneralJournalizing, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CurrencyCodeField { get; private set; }
        public NumberFieldDefinition<GeneralJournalizing, decimal> AmountField { get; private set; }
        public NumberFieldDefinition<GeneralJournalizing, DateTime> CreateAtField { get; private set; }
        public NumberFieldDefinition<GeneralJournalizing, long> ReceiptIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptSubIdField { get; private set; } // dummy
        public StringFieldDefinition<GeneralJournalizing> MatchingCountField { get; private set; } // dummy
        public NullableNumberFieldDefinition<GeneralJournalizing, long> BillingIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingSubIdField { get; private set; } // dummy
        #endregion
        #region matching
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> BankTransferFeeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> MatchingAmountField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> MatchingTypeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, long> MatchingHeaderIdField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> MatchingRecordedAtField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> MatchingCreateByCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> MatchingCreateByNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> MatchingUpdateByCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> MatchingUpdateByNameField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> MatchingTaxDifferenceField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> MatchingMemoField { get; private set; }
        #endregion
        #region receipt
        public StringFieldDefinition<GeneralJournalizing> ReceiptBankCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptBankNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptBranchCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptBranchNameField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ReceiptAccountTypeIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptAccountNumberField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptAccountNameField { get; private set; }
        public NumberFieldDefinition<GeneralJournalizing, DateTime> ReceiptRecordedAtField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptPayerCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptPayerNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptPayerNameRawField { get; private set; }
        public NumberFieldDefinition<GeneralJournalizing, decimal> ReceiptAmountField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptSourceBankNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptSourceBranchNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCategoryCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptNote1Field { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> ReceiptDueAtField { get; private set; }
        public NumberFieldDefinition<GeneralJournalizing, int> ReceiptApprovedField { get; private set; }
        public NumberFieldDefinition<GeneralJournalizing, int> ReceiptInputTypeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCustomerCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, long> ReceiptOriginalIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptOriginalSubIdField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> ReceiptCreateByCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> ReceiptCreateByNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> ReceiptUpdateByCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> ReceiptUpdateByNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptMemoField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section> ReceiptSectionCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptNote2Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptNote3Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptNote4Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptBillNumberField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptBillBankCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptBillBranchCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> ReceiptBillDrawAtField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptBillDrawerField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> ReceiptProcessingAtField { get; private set; }
        public NumberFieldDefinition<GeneralJournalizing, decimal> ReceiptRemainAmountField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptMatchingCustomerCodeField { get; private set; } // dummy
        public NumberFieldDefinition<GeneralJournalizing, int> ReceiptAssignmentFlagField { get; private set; }
        #endregion
        #region billing
        public StringFieldDefinition<GeneralJournalizing> BillingCustomerCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> BillingBilledAtField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> BillingBillingAmountField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> BillingTaxAmountField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> BillingClosingAtField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> BillingDueAtField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> BillingOriginalDueAtField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department> BillingDepartmentCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> BillingDebitAccountTitleCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> BillingCreditAccountTitleCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> BillingSalesAtField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingInvoiceCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingNote1Field { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff> BillingStaffCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingBillingCategoryCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> BillingInputTypeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingMemoField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingCollectCategoryCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingOriginalCollectCategoryCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> BillingTaxClassIdField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> BillingBillingAmountExcludingTaxField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, long> BillingScheduledIncomeMatchingBillingIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingScheduledIncomeMatchingBillingSubIdField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, long> BillingScheduledIncomeMatchingReceiptIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingScheduledIncomeMatchingReceiptSubIdField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, long> BillingScheduledIncomeMatchingHeaderIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingScheduledIncomeMatchingCountField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> BillingRequestDateField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> BillingResultCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> BillingTransferOriginalDueAtField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingNote2Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingNote3Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingNote4Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingScheduledPaymentKeyField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, long> BillingAccountTransferLogIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingAccountTransferAccountTitleIdField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> BillingAssignmentAmountField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> BillingRemainAmountField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> BillingAssignmentFlagField { get; private set; }

        public StringFieldDefinition<GeneralJournalizing> BillingNote5Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingNote6Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingNote7Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingNote8Field { get; private set; }
        #endregion
        #region customer
        public StringFieldDefinition<GeneralJournalizing> CustomerNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CustomerKanaField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff> CustomerStaffCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> CustomerIsParentField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CustomerTelField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CustomerFaxField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CustomerCustomerStaffNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CustomerNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CustomerDensaiCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CustomerCreditCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CustomerCreditRankField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ParentCustomerCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ParentCustomerNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ParentCustomerKanaField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff> ParentCustomerStaffCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ParentCustomerIsParentField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ParentCustomerTelField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ParentCustomerFaxField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ParentCustomerCustomerStaffNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ParentCustomerNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ParentCustomerDensaiCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ParentCustomerCreditCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ParentCustomerCreditRankField { get; private set; }
        #endregion
        #region bankaccount
        public StringFieldDefinition<GeneralJournalizing> BankAccountBankNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BankAccountBranchNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BankAccountAccountTitleCodeield { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BankAccountAccountTitleSubCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section> BankAccountSectionCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> BankAccountUseValueDateField { get; private set; }
        #endregion
        #region receipt category
        public StringFieldDefinition<GeneralJournalizing> ReceiptCategoryNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceiptCategoryAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCategorySubCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ReceiptCategoryUseLimitDateField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ReceiptCategoryUseCashOnDueDatesField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ReceiptCategoryTaxClassIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCategoryNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCategoryExternalCodeField { get; private set; }
        #endregion
        #region billing category
        public StringFieldDefinition<GeneralJournalizing> BillingCategoryNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> BillingCategoryAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingCategorySubCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> BillingCategoryTaxClassIdField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> BillingCategoryUseDiscountField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingCategoryNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BillingCategoryExternalCodeField { get; private set; }
        #endregion
        #region collect category
        public StringFieldDefinition<GeneralJournalizing> CollectCategoryNameField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> CollectCategoryUseAccountTransferField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, PaymentAgency> CollectCategoryPaymentAgencyCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CollectCategoryNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CollectCategoryExternalCodeField { get; private set; }
        #endregion
        #region section
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section> ReceiptSectionNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section> ReceiptSectionNoteField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section> ReceiptSectionPayerCodeField { get; private set; }
        #endregion
        #region receipt customer
        public StringFieldDefinition<GeneralJournalizing> ReceiptCustomerNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCustomerKanaField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff> ReceiptCustomerStaffCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ReceiptCustomerIsParentField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCustomerTelField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCustomerFaxField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCustomerCustomerStaffNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCustomerNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCustomerDensaiCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCustomerCreditCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptCustomerCreditRankField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptParentCustomerCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptParentCustomerNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptParentCustomerKanaField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff> ReceiptParentCustomerStaffCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ReceiptParentCustomerIsParentField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptParentCustomerTelField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptParentCustomerFaxField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptParentCustomerCustomerStaffNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptParentCustomerNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptParentCustomerDensaiCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptParentCustomerCreditCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptParentCustomerCreditRankField { get; private set; }
        #endregion
        #region account title receipt / billing / credit(billing debit) / bank(empty) / account receivable
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceiptAccountTitleNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceiptAccountTitleContraCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceiptAccountTitleContraNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceiptAccountTitleContraSubCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> BillingAccountTitleNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> BillingAccountTitleContraCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> BillingAccountTitleContraNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> BillingAccountTitleContraSubCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> CreditAccountTitleNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> CreditAccountTitleContraCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> CreditAccountTitleContraNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> CreditAccountTitleContraSubCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BankAccountTitleNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BankAccountTitleContraCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BankAccountTitleContraNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> BankAccountTitleContraSubCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceivableAccountTitleNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceivableAccountTitleContraCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceivableAccountTitleContraNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceivableAccountTitleContraSubCodeField { get; private set; }

        #endregion
        #region billing department / staff / staff department
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department> BillingDepartmentNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff> BillingDepartmentStaffCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department> BillingDepartmentNoteFiled { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff> BillingStaffNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department> BillingStaffDepartmentCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department> BillingStaffDepartmentNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department> BillingStaffDepartmentNoteField { get; private set; }

        #endregion
        #region billing scheduled income receipt
        public NullableNumberFieldDefinition<GeneralJournalizing, long> ScheduledReceiptIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptSubIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptBankCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptBankNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptBranchCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptBranchNameField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ScheduledReceiptAccountTypeIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptAccountNumberField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptAccountNameField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> ScheduledReceiptRecordedAtField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptPayerCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptPayerNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptPayerNameRawField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> ScheduledReceiptReceiptAmountField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptSourceBankNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptSourceBranchNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptReceiptCategoryCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptNote1Field { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> ScheduledReceiptDueAtField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ScheduledReceiptApprovedField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ScheduledReceiptInputTypeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptCustomerCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, long> ScheduledReceiptOriginalReceiptIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptOriginalReceiptSubIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptMemoField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section> ScheduledReceiptSectionIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptNote2Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptNote3Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptNote4Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptBillNumberField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptBillBankCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptBillBranchCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> ScheduledReceiptBillDrawAtField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptBillDrawerField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> ScheduledReceiptProcessingAtField { get; private set; }
        #region scheduled receipt category
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptCategoryNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ScheduledReceiptCategoryAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptCategorySubCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ScheduledReceiptCategoryUseLimitDateField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ScheduledReceiptCategoryUseCashOnDueDatesField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ScheduledReceiptCategoryTaxClassIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptCategoryNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ScheduledReceiptCategoryExternalCodeField { get; private set; }
        #endregion
        #region scheduled receipt account title
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ScheduledReceiptCategoryAccountTitleNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ScheduledReceiptCategoryAccountTitleContraCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ScheduledReceiptCategoryAccountTitleContraNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ScheduledReceiptCategoryAccountTitleContraSubCodeField { get; private set; }
        #endregion
        #endregion
        #region advance received receipt backup
        public NullableNumberFieldDefinition<GeneralJournalizing, long> AdvanceReceiptIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptSubIdField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> AdvanceReceiptRecordedAtField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptPayerCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptPayerNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptPayerNameRawField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> AdvanceReceiptReceiptAmountField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptSourceBankNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptSourceBranchNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCategoryCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptNote1Field { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> AdvanceReceiptDueAtField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> AdvanceReceiptApprovedField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> AdvanceReceiptInputTypeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCustomerCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, long> AdvanceReceiptOriginalReceiptIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptOrigianlReceiptSubIdField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> AdvanceReceiptCreateByCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> AdvanceReceiptCreateByNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptMemoField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section> AdvanceReceiptSectionIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptNote2Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptNote3Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptNote4Field { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptBillNumberField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptBillBankCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptBillBranchCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, DateTime> AdvanceReceiptBillDrawAtField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptBillDrawerField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptProcessingAtField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> AdvanceReceiptRemainAmountField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptMatchingCustomerCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> AdvanceReceiptAssignmentFlagField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptHeaderBankCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptHeaderBankNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptHeaderBranchCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptHeaderBranchNameField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> AdvanceReceiptHeaderAccountTypeIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptHeaderAccountNumberField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptHeaderAccountNameField { get; private set; }
        #endregion
        #region advance received receipt customer
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCustomerNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCustomerKanaField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff> AdvanceReceiptCustomerStaffIdField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> AdvanceReceiptCustomerIsParentField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCustomerTelField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCustomerFaxField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCustomerCustomerStaffNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCustomerNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCustomerDensaiCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCustomerCreditCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCustomerCreditRankField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptParentCustomerCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptParentCustomerNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptParentCustomerKanaField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff> AdvanceReceiptParentCustomerStaffIdField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> AdvanceReceiptParentCustomerIsParentField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptParentCustomerTelField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptParentCustomerFaxField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptParentCustomerCustomerStaffNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptParentCustomerNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptParentCustomerDensaiCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptParentCustomerCreditCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptParentCustomerCreditRankField { get; private set; }
        #endregion
        #region advance received receipt category / account title
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCategoryNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> AdvanceReceiptCategoryAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCategorySubCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> AdvanceReceiptCategoryUseLimitDateField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> AdvanceReceiptCategoryUseCashOnDueDatesField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> AdvanceReceiptCategoryTaxClassIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCategoryNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> AdvanceReceiptCategoryExternalCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> AdvanceReceiptCategoryAccountTitleNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> AdvanceReceiptCategoryAccountTitleContraCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> AdvanceReceiptCategoryAccountTitleContraNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> AdvanceReceiptCategoryAccountTitleContraSubCodeField { get; private set; }
        #endregion
        #region receipt exclude / exclude category / exclude category account title
        public NullableNumberFieldDefinition<GeneralJournalizing, long> ReceiptExcludeIdField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, decimal> ReceiptExcludeAmountField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptExcludeCategoryCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> ReceiptExcludeCreateByCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> ReceiptExcludeCreateByNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> ReceiptExcludeUpdateByCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser> ReceiptExcludeUpdateByNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptExcludeCategoryNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceiptExcludeCategoryAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptExcludeCategorySubCodeField { get; private set; }
        public NullableNumberFieldDefinition<GeneralJournalizing, int> ReceiptExcludeCategoryTaxClassIdField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptExcludeCategoryNoteField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceiptExcludeCategoryAccountTitleNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceiptExcludeCategoryAccountTitleContraCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceiptExcludeCategoryAccountTitleContraNameField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle> ReceiptExcludeCategoryAccountTitleContraSubCodeField { get; private set; }
        #endregion
        #region general settings
        public StringFieldDefinition<GeneralJournalizing> SuspentReceiptDepartmentCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> SuspentReceiptDepartmentNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> SuspentReceiptDepartmentNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> SuspentReceiptAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> SuspentReceiptAccountTitleNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> SuspentReceiptAccountTitleContraCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> SuspentReceiptAccountTitleContraNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> SuspentReceiptAccountTitleContraSubCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> SuspentReceiptSubCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> TransferFeeDepartmentCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> TransferFeeDepartmentNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> TransferFeeDepartmentNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> TransferFeeAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> TransferFeeAccountTitleNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> TransferFeeAccountTitleContraCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> TransferFeeAccountTitleContraNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> TransferFeeAccountTitleContraSubCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> TransferFeeSubCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> DebitTaxDiffDepartmentCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> DebitTaxDiffDepartmentNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> DebitTaxDiffDepartmentNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> DebitTaxDiffAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> DebitTaxDiffAccountTitleNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> DebitTaxDiffAccountTitleContraCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> DebitTaxDiffAccountTitleContraNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> DebitTaxDiffAccountTitleContraSubCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> DebitTaxDiffSubCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CreditTaxDiffDepartmentCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CreditTaxDiffDepartmentNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CreditTaxDiffDepartmentNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CreditTaxDiffAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CreditTaxDiffAccountTitleNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CreditTaxDiffAccountTitleContraCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CreditTaxDiffAccountTitleContraNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CreditTaxDiffAccountTitleContraSubCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> CreditTaxDiffSubCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptDepartmentCodeField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptDepartmentNameField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> ReceiptDepartmentNoteField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> NewTaxRate1ValueField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> NewTaxRate2ValueField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> NewTaxRate3ValueField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> NewTaxRate1ApplyDateValueField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> NewTaxRate2ApplyDateValueField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> NewTaxRate3ApplyDateValueField { get; private set; }
        public StringFieldDefinition<GeneralJournalizing> TaxRoundingModeValueField { get; private set; }
        public NumberFieldDefinition<GeneralJournalizing, int> PrecisionField { get; private set; }
        public NumberFieldDefinition<GeneralJournalizing, int> JournalizingTypeField { get; private set; }
        #endregion

        #region department master
        private Dictionary<int, Department> GetDepartmentDictionary(int[] ids) => departments;
        private Dictionary<int, Department> departments { get; set; }
        public Dictionary<int, Department> Departments { set { departments = value ?? new Dictionary<int, Department>(); } }
        #endregion
        #region seciont master
        private Dictionary<int, Section> GetSectionDictionary(int[] ids) => sections;
        private Dictionary<int, Section> sections { get; set; }
        public Dictionary<int, Section> Sections { set { sections = value ?? new Dictionary<int, Section>(); } }
        #endregion
        #region staff master
        private Dictionary<int, Staff> GetStaffDictionary(int[] ids) => staffs;
        private  Dictionary<int, Staff> staffs { get; set; }
        public Dictionary<int, Staff> Staffs { set { staffs = value ?? new Dictionary<int, Staff>(); } }
        #endregion
        #region login user master
        private Dictionary<int, LoginUser> GetLoginUserDictionary(int[] ids) => loginUsers;
        private Dictionary<int, LoginUser> loginUsers { get; set; }
        public Dictionary<int, LoginUser> LoginUsers { set { loginUsers = value ?? new Dictionary<int, LoginUser>(); } }
        #endregion
        #region account title master
        private Dictionary<int, AccountTitle> GetAccountTitleDictionary(int[] ids) => accountTitles;
        private Dictionary<int, AccountTitle> accountTitles { get; set; }
        public Dictionary<int, AccountTitle> AccountTitles { set { accountTitles = value ?? new Dictionary<int, AccountTitle>(); } }
        #endregion
        #region payment agency
        private Dictionary<int, PaymentAgency> GetPaymentAgencyDictionary(int[] ids) => paymentAgencies;
        private Dictionary<int, PaymentAgency> paymentAgencies { get; set; }
        public Dictionary<int, PaymentAgency> PaymentAgencies { set { paymentAgencies = value ?? new Dictionary<int, PaymentAgency>(); } }
        #endregion

        private const string DateFormat  = "yyyy/MM/dd";
        private const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        private string MoneyFormat { get; set; } = "0";
        public GeneralJournalizingFileDefinition(DataExpression dataExpression, string moneyFormat = null) : base(dataExpression)
        {
            if (!string.IsNullOrEmpty(moneyFormat)) MoneyFormat = moneyFormat;
            StartLineNumber = 1;
            DataTypeToken = "汎用消込仕訳";
            FileNameToken = DataTypeToken;
            OutputHeader = false;
#if DEBUG
            OutputHeader = true;
#endif
            Fields.AddRange(InitializeCommon());
            Fields.AddRange(InitializeMatching());
            Fields.AddRange(InitializeReceipt());
            Fields.AddRange(InitializeBilling());
            Fields.AddRange(InitializeCustomer());
            Fields.AddRange(InitializeBankAccount());
            Fields.AddRange(InitializeReceiptCategory());
            Fields.AddRange(InitializeBillingCategory());
            Fields.AddRange(InitializeCollectCategory());
            Fields.AddRange(InitializeSection());
            Fields.AddRange(InitializeReceiptCustomer());
            Fields.AddRange(InitializeAccountTitle());
            Fields.AddRange(InitializeBillingDepartment());
            Fields.AddRange(InitializeScheduledIncomeReceipt());
            Fields.AddRange(InitializeAdvanceReceipt());
            Fields.AddRange(InitializeAdvanceReceiptCustomer());
            Fields.AddRange(InitializeAdvanceReceiptCategory());
            Fields.AddRange(InitializeReceiptExclude());
            Fields.AddRange(InitializeGeneralSettings());
            Fields.AddRange(InitializeBillingAdditional());
        }

        #region initialize fields
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeCommon()
        {
            yield return (CompanyIdField = new StandardIdToCodeFieldDefinition<GeneralJournalizing, Company>(
                x => x.CompanyId, x => x.Id, null, x => x.Code,
                "会社コード", 1, accept: x => x.OwnCompanyCode(CompanyIdField)));
            yield return (CurrencyCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CurrencyCode,
                "通貨コード", 2, accept: x => x.StandardString(CurrencyCodeField)));
            yield return (AmountField = new NumberFieldDefinition<GeneralJournalizing, decimal>(x => x.Amount,
                "仕訳金額", 3, accept: x => x.StandardNumber(AmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (CreateAtField = new NumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.CreateAt,
                "処理日", 4, accept: x => x.StandardNumber(CreateAtField), formatter: x => x.ToString(DateTimeFormat)));
            yield return (ReceiptIdField = new NumberFieldDefinition<GeneralJournalizing, long>(x => x.ReceiptId,
                "入金SEQNO", 5, accept: x => x.StandardNumber(ReceiptIdField), formatter: x => x.ToString()));
            yield return (ReceiptSubIdField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "入金GYONO", 6, accept: x => x.StandardString(ReceiptSubIdField)));
            yield return (MatchingCountField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "消込回数", 7, accept: x => x.StandardString(MatchingCountField)));
            yield return (BillingIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.BillingId,
                "請求SEQNO", 8, accept: x => x.StandardNumber(BillingIdField), formatter: x => x.ToString()));
            yield return (BillingSubIdField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "請求GYONO", 9, accept: x => x.StandardString(BillingSubIdField)));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeMatching()
        {
            yield return (BankTransferFeeField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.MatchingBankTransferFee,
                "振込手数料", 10, accept: x => x.StandardNumber(BankTransferFeeField), formatter: x => x.ToString(MoneyFormat)));
            yield return (MatchingAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.MatchingAmount,
                "消込額", 11, accept: x => x.StandardNumber(MatchingAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (MatchingTypeField = new  StringFieldDefinition<GeneralJournalizing>(x => x.MatchingProcessTypeName,
                "消込理由", 12, accept: x => x.StandardString(MatchingTypeField)));
            yield return (MatchingHeaderIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.MatchingHeaderId,
                "消込キー", 13, accept: x => x.StandardNumber(MatchingHeaderIdField), formatter: x => x.ToString()));
            yield return (MatchingRecordedAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.MatchingRecordedAt,
                "伝票日付", 14, accept: x => x.StandardNumber(MatchingRecordedAtField), formatter: x => x.ToString(DateFormat)));
            yield return (MatchingCreateByCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.MatchingCreateBy, x => x.Id, null, x => x.Code,
                "作成者ID", 15, accept: x => x.LoginUserCodeField(MatchingCreateByCodeField), getModelsById: GetLoginUserDictionary));
            yield return (MatchingCreateByNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.MatchingCreateBy, x => x.Id, null, x => x.Name,
                "作成者名", 16, accept: x => x.LoginUserCodeField(MatchingCreateByNameField), getModelsById: GetLoginUserDictionary));
            yield return (MatchingUpdateByCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.MatchingUpdateBy, x => x.Id, null, x => x.Code,
                "更新者ID", 17, accept: x => x.LoginUserCodeField(MatchingUpdateByCodeField), getModelsById: GetLoginUserDictionary));
            yield return (MatchingUpdateByNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.MatchingUpdateBy, x => x.Id, null, x => x.Name,
                "更新者名", 18, accept: x => x.LoginUserCodeField(MatchingUpdateByNameField), getModelsById: GetLoginUserDictionary));
            yield return (MatchingTaxDifferenceField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.MatchingTaxDifference,
                "消費税誤差", 19, accept: x => x.StandardNumber(MatchingTaxDifferenceField), formatter: x => x.ToString(MoneyFormat)));
            yield return (MatchingMemoField = new StringFieldDefinition<GeneralJournalizing>(x => x.MatchingMemo,
                "消込メモ", 20, accept: x => x.StandardString(MatchingMemoField)));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeReceipt()
        {
            yield return (ReceiptBankCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptBankCode,
                "銀行コード", 21, accept: x => x.StandardString(ReceiptBankCodeField)));
            yield return (ReceiptBankNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptBankName,
                "銀行名", 22, accept: x => x.StandardString(ReceiptBankNameField)));
            yield return (ReceiptBranchCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptBranchCode,
                "支店コード", 23, accept: x => x.StandardString(ReceiptBranchCodeField)));
            yield return (ReceiptBranchNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptBranchName,
                "支店名", 24, accept: x => x.StandardString(ReceiptBranchNameField)));
            yield return (ReceiptAccountTypeIdField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ReceiptAccountTypeId,
                "預金種別", 25, accept: x => x.StandardNumber(ReceiptAccountTypeIdField), formatter: x => x.ToString()));
            yield return (ReceiptAccountNumberField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptAccountNumber,
                "口座番号", 26, accept: x => x.StandardString(ReceiptAccountNumberField)));
            yield return (ReceiptAccountNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptAccountName,
                "口座名", 27, accept: x => x.StandardString(ReceiptAccountNameField)));
            yield return (ReceiptRecordedAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.ReceiptRecordedAt,
                "入金日", 28, accept: x => x.StandardNumber(ReceiptRecordedAtField), formatter: x => x.ToString(DateFormat)));
            yield return (ReceiptPayerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptPayerCode,
                "振込依頼人コード", 29, accept: x => x.StandardString(ReceiptPayerCodeField)));
            yield return (ReceiptPayerNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptPayerName,
                "振込依頼人名", 30, accept: x => x.StandardString(ReceiptPayerNameField)));
            yield return (ReceiptPayerNameRawField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptPayerNameRaw,
                "振込依頼人名（未編集）", 31, accept: x => x.StandardString(ReceiptPayerNameRawField)));
            yield return (ReceiptAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.ReceiptReceiptAmount,
                "入金額", 32, accept: x => x.StandardNumber(ReceiptAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (ReceiptSourceBankNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptSourceBankName,
                "仕向銀行", 33, accept: x => x.StandardString(ReceiptSourceBankNameField)));
            yield return (ReceiptSourceBranchNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptSourceBranchName,
                "仕向支店", 34, accept: x => x.StandardString(ReceiptSourceBranchNameField)));
            yield return (ReceiptCategoryCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCategoryCode,
                "入金区分コード", 35, accept: x => x.StandardString(ReceiptCategoryCodeField)));
            yield return (ReceiptNote1Field = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptNote1,
                "備考1", 36, accept: x => x.StandardString(ReceiptNote1Field))); // field name は あとでまとめて設定すること
            yield return (ReceiptDueAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.ReceiptDueAt,
                "入金期日", 37, accept: x => x.StandardNumber(ReceiptDueAtField), formatter: x => x.ToString(DateFormat)));
            yield return (ReceiptApprovedField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ReceiptApproved,
                "承認フラグ", 38, accept: x => x.StandardNumber(ReceiptApprovedField), formatter: x => x.ToString()));
            yield return (ReceiptInputTypeField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ReceiptInputType,
                "入力区分", 39, accept: x => x.StandardNumber(ReceiptInputTypeField), formatter: x => x.ToString()));
            yield return (ReceiptCustomerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCustomerCode,
                "得意先コード（入金）", 40, accept: x => x.StandardString(ReceiptCustomerCodeField)));
            yield return (ReceiptOriginalIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.ReceiptOriginalReceiptId,
                "元入金SEQNO", 41, accept: x => x.StandardNumber(ReceiptOriginalIdField), formatter: x => x.ToString()));
            yield return (ReceiptOriginalSubIdField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "元入金GYONO", 42, accept: x => x.StandardString(ReceiptOriginalSubIdField)));
            yield return (ReceiptCreateByCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.ReceiptCreateBy, x => x.Id, null, x => x.Code,
                "作成者ID", 43, accept: x => x.LoginUserCodeField(ReceiptCreateByCodeField), getModelsById: GetLoginUserDictionary));
            yield return (ReceiptCreateByNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.ReceiptCreateBy, x => x.Id, null, x => x.Name,
                "作成者名", 44, accept: x => x.LoginUserCodeField(ReceiptCreateByNameField), getModelsById: GetLoginUserDictionary));
            yield return (ReceiptUpdateByCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.ReceiptUpdateBy, x => x.Id, null, x => x.Code,
                "更新者ID", 45, accept: x => x.LoginUserCodeField(ReceiptUpdateByCodeField), getModelsById: GetLoginUserDictionary));
            yield return (ReceiptUpdateByNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.ReceiptUpdateBy, x => x.Id, null, x => x.Name,
                "更新者名", 46, accept: x => x.LoginUserCodeField(ReceiptUpdateByNameField), getModelsById: GetLoginUserDictionary));
            yield return (ReceiptMemoField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptReceiptMemo,
                "入金メモ", 47, accept: x => x.StandardString(ReceiptMemoField)));
            yield return (ReceiptSectionCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section>(
                x => x.ReceiptSectionId, x => x.Id, null, x => x.Code,
                "入金部門コード", 48, accept: x => x.SectionCode(ReceiptSectionCodeField), getModelsById: GetSectionDictionary));
            yield return (ReceiptNote2Field = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptNote2,
                "備考2", 49, accept: x => x.StandardString(ReceiptNote2Field)));
            yield return (ReceiptNote3Field = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptNote3,
                "備考3", 50, accept: x => x.StandardString(ReceiptNote3Field)));
            yield return (ReceiptNote4Field = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptNote4,
                "備考4", 51, accept: x => x.StandardString(ReceiptNote4Field)));
            yield return (ReceiptBillNumberField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptBillNumber,
                "手形番号", 52, accept: x => x.StandardString(ReceiptBillNumberField)));
            yield return (ReceiptBillBankCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptBillBankCode,
                "券面銀行コード", 53, accept: x => x.StandardString(ReceiptBillBankCodeField)));
            yield return (ReceiptBillBranchCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptBillBranchCode,
                "券面支店コード", 54, accept: x => x.StandardString(ReceiptBillBranchCodeField)));
            yield return (ReceiptBillDrawAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.ReceiptBillDrawAt,
                "振出日", 55, accept: x => x.StandardNumber(ReceiptBillDrawAtField), formatter: x => x.ToString(DateFormat)));
            yield return (ReceiptBillDrawerField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptBillDrawer,
                "振出人", 56, accept: x => x.StandardString(ReceiptBillDrawerField)));
            yield return (ReceiptProcessingAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.ReceiptProcessingAt,
                "処理予定日", 57, accept: x => x.StandardNumber(ReceiptProcessingAtField), formatter: x => x.ToString(DateFormat)));
            yield return (ReceiptRemainAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.ReceiptRemainAmount,
                "入金残", 58, accept: x => x.StandardNumber(ReceiptRemainAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (ReceiptMatchingCustomerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "消込時得意先コード", 59, accept: x => x.StandardString(ReceiptMatchingCustomerCodeField)));
            yield return (ReceiptAssignmentFlagField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ReceiptAssignmentFlag,
                "消込フラグ", 60, accept: x => x.StandardNumber(ReceiptAssignmentFlagField), formatter: x => x.ToString()));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeBilling()
        {
            yield return (BillingCustomerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CustomerCode,
                "得意先コード（請求）", 61, accept: x => x.StandardString(BillingCustomerCodeField)));
            yield return (BillingBilledAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.BillingBilledAt,
                "請求日", 62, accept: x => x.StandardNumber(BillingBilledAtField), formatter: x => x.ToString(DateFormat)));
            yield return (BillingBillingAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.BillingBillingAmount,
                "請求額", 63, accept: x => x.StandardNumber(BillingBillingAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (BillingTaxAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.BillingTaxAmount,
                "消費税", 64, accept: x => x.StandardNumber(BillingTaxAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (BillingClosingAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.BillingClosingAt,
                "請求締日", 65, accept: x => x.StandardNumber(BillingClosingAtField), formatter: x => x.ToString(DateFormat)));
            yield return (BillingDueAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.BillingDueAt,
                "入金予定日", 66, accept: x => x.StandardNumber(BillingDueAtField), formatter: x => x.ToString(DateFormat)));
            yield return (BillingOriginalDueAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.BillingOriginalDueAt,
                "元入金予定日", 67, accept: x => x.StandardNumber(BillingOriginalDueAtField), formatter: x => x.ToString(DateFormat)));
            yield return (BillingDepartmentCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department>(
                x => x.BillingDepartmentId, x => x.Id, null, x => x.Code,
                "売上部門コード", 68, accept: x => x.DepartmentCode(BillingDepartmentCodeField), getModelsById: GetDepartmentDictionary));
            yield return (BillingDebitAccountTitleCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingDebitAccountTitleId, x => x.Id, null, x => x.Code,
                "売掛金 借方科目コード", 69, accept: x => x.AccountTitleCode(BillingDebitAccountTitleCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (BillingCreditAccountTitleCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingCreditAccountTitleId, x => x.Id, null, x => x.Code,
                "売上 貸方科目コード", 70, accept: x => x.AccountTitleCode(BillingCreditAccountTitleCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (BillingSalesAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.BillingSalesAt,
                "売上日", 71, accept: x => x.StandardNumber(BillingSalesAtField), formatter: x => x.ToString(DateFormat)));
            yield return (BillingInvoiceCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingInvoiceCode,
                "請求書番号", 72, accept: x => x.StandardString(BillingInvoiceCodeField)));
            yield return (BillingNote1Field = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingNote1,
                "備考1", 73, accept: x => x.StandardString(BillingNote1Field)));
            yield return (BillingStaffCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff>(
                x => x.BillingStaffId, x => x.Id, null, x => x.Code,
                "担当者コード", 74, accept: x => x.StaffCode(BillingStaffCodeField), getModelsById: GetStaffDictionary));
            yield return (BillingBillingCategoryCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingCategoryCode,
                "請求区分コード", 75, accept: x => x.StandardString(BillingBillingCategoryCodeField)));
            yield return (BillingInputTypeField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.BillingInputType,
                "入力区分", 76, accept: x => x.StandardNumber(BillingInputTypeField), formatter: x => x.ToString()));
            yield return (BillingMemoField = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingMemo,
                "請求メモ", 77, accept: x => x.StandardString(BillingMemoField)));
            yield return (BillingCollectCategoryCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CollectCategoryCode,
                "回収区分コード", 79, accept: x => x.StandardString(BillingCollectCategoryCodeField)));
            yield return (BillingOriginalCollectCategoryCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.OriginalCollectCategoryCode,
                "元回収区分コード", 79, accept: x => x.StandardString(BillingOriginalCollectCategoryCodeField)));
            yield return (BillingTaxClassIdField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.BillingTaxClassId,
                "消費税属性", 80, accept: x => x.StandardNumber(BillingTaxClassIdField), formatter: x => x.ToString()));
            yield return (BillingBillingAmountExcludingTaxField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.BillingBillingAmountExcludingTax,
                "金額（税抜）", 81, accept: x => x.StandardNumber(BillingBillingAmountExcludingTaxField), formatter: x => x.ToString(MoneyFormat)));
            yield return (BillingScheduledIncomeMatchingBillingIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.ScheduledIncomeBillingId,
                "元請求SEQNO", 82, accept: x => x.StandardNumber(BillingScheduledIncomeMatchingBillingIdField), formatter: x => x.ToString()));
            yield return (BillingScheduledIncomeMatchingBillingSubIdField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "元請求GYONO", 83, accept: x => x.StandardString(BillingScheduledIncomeMatchingBillingSubIdField)));
            yield return (BillingScheduledIncomeMatchingReceiptIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.ScheduledIncomeReceiptId,
                "消込対象入金SEQNO", 84, accept: x => x.StandardNumber(BillingScheduledIncomeMatchingReceiptIdField), formatter: x => x.ToString()));
            yield return (BillingScheduledIncomeMatchingReceiptSubIdField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "消込対象入金GYONO", 85, accept: x => x.StandardString(BillingScheduledIncomeMatchingReceiptSubIdField)));
            yield return (BillingScheduledIncomeMatchingHeaderIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.ScheduledIncomeMatchingHeaderId,
                "消込キー", 86, accept: x => x.StandardNumber(BillingScheduledIncomeMatchingHeaderIdField), formatter: x => x.ToString()));
            yield return (BillingScheduledIncomeMatchingCountField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "消込回数", 87, accept: x => x.StandardString(BillingScheduledIncomeMatchingCountField)));
            yield return (BillingRequestDateField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.BillingRequestDate,
                "口座振替依頼作成日", 88, accept: x => x.StandardNumber(BillingRequestDateField), formatter: x => x.ToString(DateFormat)));
            yield return (BillingResultCodeField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.BillingResultCode,
                "口座振替結果", 89, accept: x => x.StandardNumber(BillingResultCodeField), formatter: x => x.ToString()));
            yield return (BillingTransferOriginalDueAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.BillingTransferOriginalDueAt,
                "元入金予定日（口座振替）", 90, accept: x => x.StandardNumber(BillingTransferOriginalDueAtField), formatter: x => x.ToString(DateFormat)));
            yield return (BillingNote2Field = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingNote2,
                "備考2", 91, accept: x => x.StandardString(BillingNote2Field)));
            yield return (BillingNote3Field = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingNote3,
                "備考3", 92, accept: x => x.StandardString(BillingNote3Field)));
            yield return (BillingNote4Field = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingNote4,
                "備考4", 93, accept: x => x.StandardString(BillingNote4Field)));
            yield return (BillingScheduledPaymentKeyField = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingScheduledPaymentKey,
                "入金予定キー", 94, accept: x => x.StandardString(BillingScheduledPaymentKeyField)));
            yield return (BillingAccountTransferLogIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.BillingAccountTransferLogId,
                "口座振替依頼作成ID", 95, accept: x => x.StandardNumber(BillingAccountTransferLogIdField), formatter: x => x.ToString()));
            yield return (BillingAccountTransferAccountTitleIdField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "未入集金科目コード", 96, accept: x => x.StandardString(BillingAccountTransferAccountTitleIdField)));
            yield return (BillingAssignmentAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.BillingAssignmentAmount,
                "消込額", 97, accept: x => x.StandardNumber(BillingAssignmentAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (BillingRemainAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.BillingRemainAmount,
                "請求残", 98, accept: x => x.StandardNumber(BillingRemainAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (BillingAssignmentFlagField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.BillingAssignmentFlag,
                "消込フラグ", 99, accept: x => x.StandardNumber(BillingAssignmentFlagField), formatter: x => x.ToString()));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeCustomer()
        {
            yield return (CustomerNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.CustomerName,
                "得意先名", 100, accept: x => x.StandardString(CustomerNameField)));
            yield return (CustomerKanaField = new StringFieldDefinition<GeneralJournalizing>(x => x.CustomerKana,
                "得意先カナ", 101, accept: x => x.StandardString(CustomerKanaField)));
            yield return (CustomerStaffCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff>(
                x => x.CustomerStaffId, x => x.Id, null, x => x.Code,
                "営業担当者コード", 102, accept: x => x.StaffCode(CustomerStaffCodeField), getModelsById: GetStaffDictionary));
            yield return (CustomerIsParentField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.CustomerIsParent,
                "債権代表者フラグ", 103, accept: x => x.StandardNumber(CustomerIsParentField), formatter: x => x.ToString()));
            yield return (CustomerTelField = new StringFieldDefinition<GeneralJournalizing>(x => x.CustomerTel,
                "電話番号", 104, accept: x => x.StandardString(CustomerTelField)));
            yield return (CustomerFaxField = new StringFieldDefinition<GeneralJournalizing>(x => x.CustomerFax,
                "FAX番号", 105, accept: x => x.StandardString(CustomerFaxField)));
            yield return (CustomerCustomerStaffNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.CustomerCustomerStaffName,
                "相手先担当者名", 106, accept: x => x.StandardString(CustomerCustomerStaffNameField)));
            yield return (CustomerNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.CustomerNote,
                "備考", 107, accept: x => x.StandardString(CustomerNoteField)));
            yield return (CustomerDensaiCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CustomerDensaiCode,
                "電子手形用企業コード", 108, accept: x => x.StandardString(CustomerDensaiCodeField)));
            yield return (CustomerCreditCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CustomerCreditCode,
                "信用調査業企業コード", 108, accept: x => x.StandardString(CustomerCreditCodeField)));
            yield return (CustomerCreditRankField = new StringFieldDefinition<GeneralJournalizing>(x => x.CustomerCreditRank,
                "与信ランク", 110, accept: x => x.StandardString(CustomerCreditRankField)));
            yield return (ParentCustomerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ParentCustomerCode,
                "得意先コード", 111, accept: x => x.StandardString(ParentCustomerCodeField)));
            yield return (ParentCustomerNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ParentCustomerName,
                "得意先名", 112, accept: x => x.StandardString(ParentCustomerNameField)));
            yield return (ParentCustomerKanaField = new StringFieldDefinition<GeneralJournalizing>(x => x.ParentCustomerKana,
                "得意先カナ", 113, accept: x => x.StandardString(ParentCustomerKanaField)));
            yield return (ParentCustomerStaffCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff>(
                x => x.ParentCustomerStaffId, x => x.Id, null, x => x.Code,
                "営業担当者コード", 114, accept: x => x.StaffCode(ParentCustomerStaffCodeField), getModelsById: GetStaffDictionary));
            yield return (ParentCustomerIsParentField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ParentCustomerIsParent,
                "債権代表者フラグ", 115, accept: x => x.StandardNumber(ParentCustomerIsParentField), formatter: x => x.ToString()));
            yield return (ParentCustomerTelField = new StringFieldDefinition<GeneralJournalizing>(x => x.ParentCustomerTel,
                "電話番号", 116, accept: x => x.StandardString(ParentCustomerTelField)));
            yield return (ParentCustomerFaxField = new StringFieldDefinition<GeneralJournalizing>(x => x.ParentCustomerFax,
                "FAX番号", 117, accept: x => x.StandardString(ParentCustomerFaxField)));
            yield return (ParentCustomerCustomerStaffNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ParentCustomerCustomerStaffName,
                "相手先担当者名", 118, accept: x => x.StandardString(ParentCustomerCustomerStaffNameField)));
            yield return (ParentCustomerNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.ParentCustomerNote,
                "備考", 119, accept: x => x.StandardString(ParentCustomerNoteField)));
            yield return (ParentCustomerDensaiCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ParentCustomerDensaiCode,
                "電子手形用企業コード", 120, accept: x => x.StandardString(ParentCustomerDensaiCodeField)));
            yield return (ParentCustomerCreditCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ParentCustomerCreditCode,
                "信用調査用企業コード", 121, accept: x => x.StandardString(ParentCustomerCreditCodeField)));
            yield return (ParentCustomerCreditRankField = new StringFieldDefinition<GeneralJournalizing>(x => x.ParentCustomerCreditRank,
                "与信ランク", 122, accept: x => x.StandardString(ParentCustomerCreditRankField)));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeBankAccount()
        {
            yield return (BankAccountBankNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.BankAccountBankName,
                "銀行名", 123, accept: x => x.StandardString(BankAccountBankNameField)));
            yield return (BankAccountBranchNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.BankAccountBranchName,
                "支店名", 124, accept: x => x.StandardString(BankAccountBranchNameField)));
            yield return (BankAccountAccountTitleCodeield = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "科目コード", 125, accept: x => x.StandardString(BankAccountAccountTitleCodeield)));
            yield return (BankAccountAccountTitleSubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "補助コード", 126, accept: x => x.StandardString(BankAccountAccountTitleSubCodeField)));
            yield return (BankAccountSectionCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section>(
                x => x.BankAccountSectionId, x => x.Id, null, x => x.Code,
                "入金部門コード", 127, accept: x => x.SectionCode(BankAccountSectionCodeField), getModelsById: GetSectionDictionary));
            yield return (BankAccountUseValueDateField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.BankAccountUseValueDate,
                "入金日指定", 128, accept: x => x.StandardNumber(BankAccountUseValueDateField), formatter: x => x.ToString()));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeReceiptCategory()
        {
            yield return (ReceiptCategoryNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCategoryName,
                "入金区分名", 129, accept: x => x.StandardString(ReceiptCategoryNameField)));
            yield return (ReceiptCategoryAccountTitleCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ReceiptCategoryAccountTitleId, x => x.Id, null, x => x.Code,
                "科目コード", 130, accept: x => x.AccountTitleCode(ReceiptCategoryAccountTitleCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceiptCategorySubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCategorySubCode,
                "補助コード", 131, accept: x => x.StandardString(ReceiptCategorySubCodeField)));
            yield return (ReceiptCategoryUseLimitDateField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ReceiptCategoryUseLimitDate,
                "期日入力可否", 132, accept: x => x.StandardNumber(ReceiptCategoryUseLimitDateField), formatter: x => x.ToString()));
            yield return (ReceiptCategoryUseCashOnDueDatesField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ReceiptCategoryUseCashOnDueDates,
                "期日入金管理使用可否", 133, accept: x => x.StandardNumber(ReceiptCategoryUseCashOnDueDatesField), formatter: x => x.ToString()));
            yield return (ReceiptCategoryTaxClassIdField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ReceiptCategoryTaxClassId,
                "消費税属性", 134, accept: x => x.StandardNumber(ReceiptCategoryTaxClassIdField), formatter: x => x.ToString()));
            yield return (ReceiptCategoryNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCategoryNote,
                "備考", 135, accept: x => x.StandardString(ReceiptCategoryNoteField)));
            yield return (ReceiptCategoryExternalCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCategoryExternalCode,
                "外部コード", 136, accept: x => x.StandardString(ReceiptCategoryExternalCodeField)));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeBillingCategory()
        {
            yield return (BillingCategoryNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingCategoryName,
                "請求区分名", 137, accept: x => x.StandardString(BillingCategoryNameField)));
            yield return (BillingCategoryAccountTitleCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingCategoryAccountTitleId, x => x.Id, null, x => x.Code,
                "科目コード", 138, accept: x => x.AccountTitleCode(BillingCategoryAccountTitleCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (BillingCategorySubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingCategorySubCode,
                "補助コード", 139, accept: x => x.StandardString(BillingCategorySubCodeField)));
            yield return (BillingCategoryTaxClassIdField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.BillingCategoryTaxClassId,
                "消費税属性", 140, accept: x => x.StandardNumber(BillingCategoryTaxClassIdField), formatter: x => x.ToString()));
            yield return (BillingCategoryUseDiscountField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.BillingCategoryUseDiscount,
                "歩引利用フラグ", 141, accept: x => x.StandardNumber(BillingCategoryUseDiscountField), formatter: x => x.ToString()));
            yield return (BillingCategoryNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingCategoryNote,
                "備考", 142, accept: x => x.StandardString(BillingCategoryNoteField)));
            yield return (BillingCategoryExternalCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingCategoryExternalCode,
                "外部コード", 143, accept: x => x.StandardString(BillingCategoryExternalCodeField)));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeCollectCategory()
        {
            yield return (CollectCategoryNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.CollectCategoryName,
                "回収区分名", 144, accept: x => x.StandardString(CollectCategoryNameField)));
            yield return (CollectCategoryUseAccountTransferField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.CollectCategoryUseAccountTransfer,
                "口座振替使用可否", 145, accept: x => x.StandardNumber(CollectCategoryUseAccountTransferField), formatter: x => x.ToString()));
            yield return (CollectCategoryPaymentAgencyCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, PaymentAgency>(
                x => x.CollectCategoryPaymentAgencyId, x => x.Id, null, x => x.Code,
                "決済代行会社コード", 146, accept: x => x.PaymentAgencyCode(CollectCategoryPaymentAgencyCodeField), getModelsById: GetPaymentAgencyDictionary));
            yield return (CollectCategoryNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.CollectCategoryNote,
                "備考", 147, accept: x => x.StandardString(CollectCategoryNoteField)));
            yield return (CollectCategoryExternalCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CollectCategoryExternalCode,
                "外部コード", 148, accept: x => x.StandardString(CollectCategoryExternalCodeField)));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeSection()
        {
            yield return (ReceiptSectionNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section>(
                x => x.ReceiptSectionId, x => x.Id, null, x => x.Name,
                "入金部門名", 149, accept: x => x.SectionCode(ReceiptSectionNameField), getModelsById: GetSectionDictionary));
            yield return (ReceiptSectionNoteField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section>(
                x => x.ReceiptSectionId, x => x.Id, null, x => x.Note,
                "備考", 150, accept: x => x.SectionCode(ReceiptSectionNoteField), getModelsById: GetSectionDictionary));
            yield return (ReceiptSectionPayerCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section>(
                x => x.ReceiptSectionId, x => x.Id, null, x => x.PayerCode,
                "振込依頼人コード", 151, accept: x => x.SectionCode(ReceiptSectionPayerCodeField), getModelsById: GetSectionDictionary));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeReceiptCustomer()
        {
            yield return (ReceiptCustomerNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCustomerName,
                "得意先名", 152, accept: x => x.StandardString(ReceiptCustomerNameField)));
            yield return (ReceiptCustomerKanaField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCustomerKana,
                "得意先カナ", 153, accept: x => x.StandardString(ReceiptCustomerKanaField)));
            yield return (ReceiptCustomerStaffCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff>(
                x => x.ReceiptCustomerStaffId, x => x.Id, null, x => x.Code,
                "営業担当者コード", 154, accept: x => x.StaffCode(ReceiptCustomerStaffCodeField), getModelsById: GetStaffDictionary));
            yield return (ReceiptCustomerIsParentField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ReceiptCustomerIsParent,
                "債権代表者フラグ", 155, accept: x => x.StandardNumber(ReceiptCustomerIsParentField), formatter: x => x.ToString()));
            yield return (ReceiptCustomerTelField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCustomerTel,
                "電話番号", 156, accept: x => x.StandardString(ReceiptCustomerTelField)));
            yield return (ReceiptCustomerFaxField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCustomerFax,
                "FAX番号", 157, accept: x => x.StandardString(ReceiptCustomerFaxField)));
            yield return (ReceiptCustomerCustomerStaffNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCustomerCustomerStaffName,
                "相手先担当者名", 158, accept: x => x.StandardString(ReceiptCustomerCustomerStaffNameField)));
            yield return (ReceiptCustomerNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCustomerNote,
                "備考", 159, accept: x => x.StandardString(ReceiptCustomerNoteField)));
            yield return (ReceiptCustomerDensaiCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCustomerDensaiCode,
                "電子手形用企業コード", 160, accept: x => x.StandardString(ReceiptCustomerDensaiCodeField)));
            yield return (ReceiptCustomerCreditCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCustomerCreditCode,
                "信用調査用企業コード", 161, accept: x => x.StandardString(ReceiptCustomerCreditCodeField)));
            yield return (ReceiptCustomerCreditRankField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptCustomerCreditRank,
                "与信ランク", 162, accept: x => x.StandardString(ReceiptCustomerCreditRankField)));
            yield return (ReceiptParentCustomerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptParentCustomerCode,
                "得意先コード", 163, accept: x => x.StandardString(ReceiptParentCustomerCodeField)));
            yield return (ReceiptParentCustomerNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptParentCustomerName,
                "得意先名", 164, accept: x => x.StandardString(ReceiptParentCustomerNameField)));
            yield return (ReceiptParentCustomerKanaField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptParentCustomerKana,
                "得意先カナ", 165, accept: x => x.StandardString(ReceiptParentCustomerKanaField)));
            yield return (ReceiptParentCustomerStaffCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff>(
                x => x.ReceiptParentCustomerStaffId, x => x.Id, null, x => x.Code,
                "営業担当者コード", 166, accept: x => x.StaffCode(ReceiptParentCustomerStaffCodeField), getModelsById: GetStaffDictionary));
            yield return (ReceiptParentCustomerIsParentField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ReceiptParentCustomerIsParent,
                "債権代表者フラグ", 167, accept: x => x.StandardNumber(ReceiptParentCustomerIsParentField), formatter: x => x.ToString()));
            yield return (ReceiptParentCustomerTelField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptParentCustomerTel,
                "電話番号", 168, accept: x => x.StandardString(ReceiptParentCustomerTelField)));
            yield return (ReceiptParentCustomerFaxField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptParentCustomerFax,
                "FAX番号", 169, accept: x => x.StandardString(ReceiptParentCustomerFaxField)));
            yield return (ReceiptCustomerCustomerStaffNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptParentCustomerCustomerStaffName,
                "相手先担当者名", 170, accept: x => x.StandardString(ReceiptCustomerCustomerStaffNameField)));
            yield return (ReceiptParentCustomerNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptParentCustomerNote,
                "備考", 171, accept: x => x.StandardString(ReceiptParentCustomerNoteField)));
            yield return (ReceiptParentCustomerDensaiCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptParentCustomerDensaiCode,
                "電子手形用企業コード", 172, accept: x => x.StandardString(ReceiptParentCustomerDensaiCodeField)));
            yield return (ReceiptParentCustomerCreditCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptParentCustomerCreditCode,
                "信用調査用企業コード", 173, accept: x => x.StandardString(ReceiptParentCustomerCreditCodeField)));
            yield return (ReceiptParentCustomerCreditRankField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptParentCustomerCreditRank,
                "与信ランク", 174, accept: x => x.StandardString(ReceiptParentCustomerCreditRankField)));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeAccountTitle()
        {
            yield return (ReceiptAccountTitleNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ReceiptCategoryAccountTitleId, x => x.Id, null, x => x.Name,
                "入金区分科目名", 175, accept: x => x.AccountTitleCode(ReceiptAccountTitleNameField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceiptAccountTitleContraCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ReceiptCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountCode,
                "入金区分相手先科目コード", 176, accept: x => x.AccountTitleCode(ReceiptAccountTitleContraCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceiptAccountTitleContraNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ReceiptCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountName,
                "入金区分相手先科目名", 177, accept: x => x.AccountTitleCode(ReceiptAccountTitleContraNameField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceiptAccountTitleContraSubCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ReceiptCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountSubCode,
                "入金区分相手先補助コード", 178, accept: x => x.AccountTitleCode(ReceiptAccountTitleContraSubCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (BillingAccountTitleNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingCategoryAccountTitleId, x => x.Id, null, x => x.Name,
                "請求区分科目名", 179, accept: x => x.AccountTitleCode(BillingAccountTitleNameField), getModelsById: GetAccountTitleDictionary));
            yield return (BillingAccountTitleContraCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountCode,
                "請求区分相手先科目コード", 180, accept: x => x.AccountTitleCode(BillingAccountTitleContraCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (BillingAccountTitleContraNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountName,
                "請求区分相手先科目名", 181, accept: x => x.AccountTitleCode(BillingAccountTitleContraNameField), getModelsById: GetAccountTitleDictionary));
            yield return (BillingAccountTitleContraSubCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountSubCode,
                "請求区分相手先補助コード", 182, accept: x => x.AccountTitleCode(BillingAccountTitleContraSubCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (CreditAccountTitleNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingDebitAccountTitleId, x => x.Id, null, x => x.Name,
                "回収区分科目名", 183, accept: x => x.AccountTitleCode(CreditAccountTitleNameField), getModelsById: GetAccountTitleDictionary));
            yield return (CreditAccountTitleContraCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingDebitAccountTitleId, x => x.Id, null, x => x.ContraAccountCode,
                "回収区分相手先科目コード", 184, accept: x => x.AccountTitleCode(CreditAccountTitleContraCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (CreditAccountTitleContraNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingDebitAccountTitleId, x => x.Id, null, x => x.ContraAccountName,
                "回収区分相手先科目名", 185, accept: x => x.AccountTitleCode(CreditAccountTitleContraNameField), getModelsById: GetAccountTitleDictionary));
            yield return (CreditAccountTitleContraSubCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.BillingDebitAccountTitleId, x => x.Id, null, x => x.ContraAccountSubCode,
                "回収区分相手先補助コード", 186, accept: x => x.AccountTitleCode(CreditAccountTitleContraSubCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (BankAccountTitleNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "銀行口座マスター科目名", 187, accept: x => x.StandardString(BankAccountTitleNameField)));
            yield return (BankAccountTitleContraCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "銀行口座マスター相手先科目コード", 188, accept: x => x.StandardString(BankAccountTitleContraCodeField)));
            yield return (BankAccountTitleContraNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "銀行口座マスター相手先科目名", 189, accept: x => x.StandardString(BankAccountTitleContraNameField)));
            yield return (BankAccountTitleContraSubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "銀行口座マスター相手先補助コード", 190, accept: x => x.StandardString(BankAccountTitleContraSubCodeField)));
            yield return (ReceivableAccountTitleNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.AccountReceivableAccountTitleId, x => x.Id, null, x => x.Name,
                "未収入金科目名", 191, accept: x => x.AccountTitleCode(ReceivableAccountTitleNameField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceivableAccountTitleContraCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.AccountReceivableAccountTitleId, x => x.Id, null, x => x.ContraAccountCode,
                "未収入金相手先科目コード", 192, accept: x => x.AccountTitleCode(ReceivableAccountTitleContraCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceivableAccountTitleContraNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.AccountReceivableAccountTitleId, x => x.Id, null, x => x.ContraAccountName,
                "未収入金相手先科目名", 193, accept: x => x.AccountTitleCode(ReceivableAccountTitleContraNameField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceivableAccountTitleContraSubCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.AccountReceivableAccountTitleId, x => x.Id, null, x => x.ContraAccountSubCode,
                "未収入金相手先補助コード", 194, accept: x => x.AccountTitleCode(ReceivableAccountTitleContraSubCodeField), getModelsById: GetAccountTitleDictionary));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeBillingDepartment()
        {
            yield return (BillingDepartmentNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department>(
                x => x.BillingDepartmentId, x => x.Id, null, x => x.Name,
                "請求部門名", 195, accept: x => x.DepartmentCode(BillingDepartmentNameField), getModelsById: GetDepartmentDictionary));
            yield return (BillingDepartmentStaffCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff>(
                x => x.BillingDepartmentStaffId, x => x.Id, null, x => x.Code,
                "回収責任者コード", 196, accept: x => x.StaffCode(BillingDepartmentStaffCodeField), getModelsById: GetStaffDictionary));
            yield return (BillingDepartmentNoteFiled = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department>(
                x => x.BillingDepartmentId, x => x.Id, null, x => x.Note,
                "備考", 197, accept: x => x.DepartmentCode(BillingDepartmentNoteFiled), getModelsById: GetDepartmentDictionary));
            yield return (BillingStaffNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff>(
                x => x.BillingStaffId, x => x.Id, null, x => x.Name,
                "担当者名", 198, accept: x => x.StaffCode(BillingStaffNameField), getModelsById: GetStaffDictionary));
            yield return (BillingStaffDepartmentCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department>(
                x => x.BillingStaffDepartmentId, x => x.Id, null, x => x.Code,
                "請求部門コード", 199, accept: x => x.DepartmentCode(BillingStaffDepartmentCodeField), getModelsById: GetDepartmentDictionary));
            yield return (BillingStaffDepartmentNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department>(
                x => x.BillingStaffDepartmentId, x => x.Id, null, x => x.Name,
                "請求部門名", 200, accept: x => x.DepartmentCode(BillingStaffDepartmentNameField), getModelsById: GetDepartmentDictionary));
            yield return (BillingStaffDepartmentNoteField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Department>(
                x => x.BillingStaffDepartmentId, x => x.Id, null, x => x.Note,
                "備考", 201, accept: x => x.DepartmentCode(BillingStaffDepartmentNoteField), getModelsById: GetDepartmentDictionary));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeScheduledIncomeReceipt()
        {
            yield return (ScheduledReceiptIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.ScheduledIncomeReceiptId,
                "入金SEQNO", 202, accept: x => x.StandardNumber(ScheduledReceiptIdField), formatter: x => x.ToString()));
            yield return (ScheduledReceiptSubIdField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "入金GYONO", 203, accept: x => x.StandardString(ScheduledReceiptSubIdField)));
            yield return (ScheduledReceiptBankCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptHeaderBankCode,
                "銀行コード", 204, accept: x => x.StandardString(ScheduledReceiptBankCodeField)));
            yield return (ScheduledReceiptBankNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptHeaderBankName,
                "銀行名", 205, accept: x => x.StandardString(ScheduledReceiptBankNameField)));
            yield return (ScheduledReceiptBranchCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptHeaderBranchCode,
                "支店コード", 206, accept: x => x.StandardString(ScheduledReceiptBranchCodeField)));
            yield return (ScheduledReceiptBranchNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptHeaderBranchName,
                "支店名", 207, accept: x => x.StandardString(ScheduledReceiptBranchNameField)));
            yield return (ScheduledReceiptAccountTypeIdField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ScheduledReceiptHeaderAccountTypeId,
                "預金種別", 208, accept: x => x.StandardNumber(ScheduledReceiptAccountTypeIdField), formatter: x => x.ToString()));
            yield return (ScheduledReceiptAccountNumberField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptHeaderAccountNumber,
                "口座番号", 209, accept: x => x.StandardString(ScheduledReceiptAccountNumberField)));
            yield return (ScheduledReceiptAccountNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptHeaderAccountName,
                "口座名", 210, accept: x => x.StandardString(ScheduledReceiptAccountNameField)));
            yield return (ScheduledReceiptRecordedAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.ScheduledReceiptRecordedAt,
                "入金日", 211, accept: x => x.StandardNumber(ScheduledReceiptRecordedAtField), formatter: x => x.ToString(DateFormat)));
            yield return (ScheduledReceiptPayerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptPayerCode,
                "振込依頼人コード", 212, accept: x => x.StandardString(ScheduledReceiptPayerCodeField)));
            yield return (ScheduledReceiptPayerNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptPayerName,
                "振込依頼人名", 213, accept: x => x.StandardString(ScheduledReceiptPayerNameField)));
            yield return (ScheduledReceiptPayerNameRawField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptPayerNameRaw,
                "振込依頼人名（未編集）", 214, accept: x => x.StandardString(ScheduledReceiptPayerNameRawField)));
            yield return (ScheduledReceiptReceiptAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.ScheduledReceiptReceiptAmount,
                "入金額", 215, accept: x => x.StandardNumber(ScheduledReceiptReceiptAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (ScheduledReceiptSourceBankNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptSourceBankName,
                "仕向銀行", 216, accept: x => x.StandardString(ScheduledReceiptSourceBankNameField)));
            yield return (ScheduledReceiptSourceBranchNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptSourceBranchName,
                "仕向支店", 217, accept: x => x.StandardString(ScheduledReceiptSourceBranchNameField)));
            yield return (ScheduledReceiptReceiptCategoryCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptCategoryCode,
                "入金区分コード", 218, accept: x => x.StandardString(ScheduledReceiptReceiptCategoryCodeField)));
            yield return (ScheduledReceiptNote1Field = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptNote1,
                "備考1", 219, accept: x => x.StandardString(ScheduledReceiptNote1Field)));
            yield return (ScheduledReceiptDueAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.ScheduledReceiptDueAt,
                "入金期日", 220, accept: x => x.StandardNumber(ScheduledReceiptDueAtField), formatter: x => x.ToString(DateFormat)));
            yield return (ScheduledReceiptApprovedField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ScheduledReceiptApproved,
                "承認フラグ", 221, accept: x => x.StandardNumber(ScheduledReceiptApprovedField), formatter: x => x.ToString()));
            yield return (ScheduledReceiptInputTypeField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ScheduledReceiptInputType,
                "入力区分", 222, accept: x => x.StandardNumber(ScheduledReceiptInputTypeField), formatter: x => x.ToString()));
            yield return (ScheduledReceiptCustomerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptCustomerCode,
                "得意先コード（入金）", 223, accept: x => x.StandardString(ScheduledReceiptCustomerCodeField)));
            yield return (ScheduledReceiptOriginalReceiptIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.ScheduledReceiptOriginalReceiptId,
                "元入金SEQNO", 224, accept: x => x.StandardNumber(ScheduledReceiptOriginalReceiptIdField), formatter: x => x.ToString()));
            yield return (ScheduledReceiptOriginalReceiptSubIdField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "元入金GYONO", 225, accept: x => x.StandardString(ScheduledReceiptOriginalReceiptSubIdField)));
            yield return (ScheduledReceiptMemoField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptReceiptMemo,
                "入金メモ", 226, accept: x => x.StandardString(ScheduledReceiptMemoField)));
            yield return (ScheduledReceiptSectionIdField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section>(
                x => x.ScheduledReceiptSectionId, x => x.Id, null, x => x.Code,
                "入金部門コード", 227, accept: x => x.SectionCode(ScheduledReceiptSectionIdField), getModelsById: GetSectionDictionary));
            yield return (ScheduledReceiptNote2Field = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptNote2,
                "備考2", 228, accept: x => x.StandardString(ScheduledReceiptNote2Field)));
            yield return (ScheduledReceiptNote3Field = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptNote3,
                "備考3", 229, accept: x => x.StandardString(ScheduledReceiptNote3Field)));
            yield return (ScheduledReceiptNote4Field = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptNote4,
                "備考4", 230, accept: x => x.StandardString(ScheduledReceiptNote4Field)));
            yield return (ScheduledReceiptBillNumberField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptBillNumber,
                "手形番号", 231, accept: x => x.StandardString(ScheduledReceiptBillNumberField)));
            yield return (ScheduledReceiptBillBankCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptBillBankCode,
                "券面銀行コード", 232, accept: x => x.StandardString(ScheduledReceiptBillBankCodeField)));
            yield return (ScheduledReceiptBillBranchCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptBillBranchCode,
                "券面支店コード", 233, accept: x => x.StandardString(ScheduledReceiptBillBranchCodeField)));
            yield return (ScheduledReceiptBillDrawAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.ScheduledReceiptBillDrawAt,
                "振出日", 234, accept: x => x.StandardNumber(ScheduledReceiptBillDrawAtField), formatter: x => x.ToString(DateFormat)));
            yield return (ScheduledReceiptBillDrawerField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptBillDrawer,
                "振出人", 235, accept: x => x.StandardString(ScheduledReceiptBillDrawerField)));
            yield return (ScheduledReceiptProcessingAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.ScheduledReceiptProcessingAt,
                "処理予定日", 236, accept: x => x.StandardNumber(ScheduledReceiptProcessingAtField), formatter: x => x.ToString(DateFormat)));
            yield return (ScheduledReceiptCategoryNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptCategoryName,
                "期日現金元入金区分名", 237, accept: x => x.StandardString(ScheduledReceiptCategoryNameField)));
            yield return (ScheduledReceiptCategoryAccountTitleCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ScheduledReceiptCategoryAccountTitleId, x => x.Id, null, x => x.Code,
                "期日現金元入金区分科目コード", 238, accept: x => x.AccountTitleCode(ScheduledReceiptCategoryAccountTitleCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (ScheduledReceiptCategorySubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptCategorySubCode,
                "期日現金元入金区分補助コード", 239, accept: x => x.StandardString(ScheduledReceiptCategorySubCodeField)));
            yield return (ScheduledReceiptCategoryUseLimitDateField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ScheduledReceiptCategoryUseLimitDate,
                "期日現金元入金区分期日入力可否", 240, accept: x => x.StandardNumber(ScheduledReceiptCategoryUseLimitDateField), formatter: x => x.ToString()));
            yield return (ScheduledReceiptCategoryUseCashOnDueDatesField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ScheduledReceiptCategoryUseCashOnDueDates,
                "期日現金元入金区分期日入金管理使用可否", 241, accept: x => x.StandardNumber(ScheduledReceiptCategoryUseCashOnDueDatesField), formatter: x => x.ToString()));
            yield return (ScheduledReceiptCategoryTaxClassIdField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ScheduledReceiptCategoryTaxClassId,
                "期日現金元入金区分消費税属性", 242, accept: x => x.StandardNumber(ScheduledReceiptCategoryTaxClassIdField), formatter: x => x.ToString()));
            yield return (ScheduledReceiptCategoryNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptCategoryNote,
                "期日現金元入金区分備考", 243, accept: x => x.StandardString(ScheduledReceiptCategoryNoteField)));
            yield return (ScheduledReceiptCategoryExternalCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ScheduledReceiptCategoryExternalCode,
                "期日現金元入金区分外部コード", 244, accept: x => x.StandardString(ScheduledReceiptCategoryExternalCodeField)));
            yield return (ScheduledReceiptCategoryAccountTitleNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ScheduledReceiptCategoryAccountTitleId, x => x.Id, null, x => x.Name,
                "期日現金元入金区分科目名", 245, accept: x => x.AccountTitleCode(ScheduledReceiptCategoryAccountTitleNameField), getModelsById: GetAccountTitleDictionary));
            yield return (ScheduledReceiptCategoryAccountTitleContraCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ScheduledReceiptCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountCode,
                "期日現金元入金区分相手先科目コード", 246, accept: x => x.AccountTitleCode(ScheduledReceiptCategoryAccountTitleContraCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (ScheduledReceiptCategoryAccountTitleContraNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ScheduledReceiptCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountName,
                "期日現金元入金区分相手先科目名", 247, accept: x => x.AccountTitleCode(ScheduledReceiptCategoryAccountTitleContraNameField), getModelsById: GetAccountTitleDictionary));
            yield return (ScheduledReceiptCategoryAccountTitleContraSubCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ScheduledReceiptCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountSubCode,
                "期日現金元入金区分相手先補助コード", 248, accept: x => x.AccountTitleCode(ScheduledReceiptCategoryAccountTitleContraSubCodeField), getModelsById: GetAccountTitleDictionary));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeAdvanceReceipt()
        {
            yield return (AdvanceReceiptIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.AdvanceReceiptId,
                "入金SEQNO", 249, accept: x => x.StandardNumber(AdvanceReceiptIdField), formatter: x => x.ToString()));
            yield return (AdvanceReceiptSubIdField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "入金GYONO", 250, accept: x => x.StandardString(AdvanceReceiptSubIdField)));
            yield return (AdvanceReceiptHeaderBankCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptHeaderBankCode,
                "銀行コード", 251, accept: x => x.StandardString(AdvanceReceiptHeaderBankCodeField)));
            yield return (AdvanceReceiptHeaderBankNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptHeaderBankName,
                "銀行名", 252, accept: x => x.StandardString(AdvanceReceiptHeaderBankNameField)));
            yield return (AdvanceReceiptHeaderBranchCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptHeaderBranchCode,
                "支店コード", 253, accept: x => x.StandardString(AdvanceReceiptHeaderBranchCodeField)));
            yield return (AdvanceReceiptHeaderBranchNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptHeaderBranchName,
                "支店名", 254, accept: x => x.StandardString(AdvanceReceiptHeaderBranchNameField)));
            yield return (AdvanceReceiptHeaderAccountTypeIdField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.AdvanceReceiptHeaderAccountTypeId,
                "預金種別", 255, accept: x => x.StandardNumber(AdvanceReceiptHeaderAccountTypeIdField), formatter: x => x.ToString()));
            yield return (AdvanceReceiptHeaderAccountNumberField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptHeaderAccountNumber,
                "口座番号", 256, accept: x => x.StandardString(AdvanceReceiptHeaderAccountNumberField)));
            yield return (AdvanceReceiptHeaderAccountNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptHeaderAccountName,
                "口座名", 257, accept: x => x.StandardString(AdvanceReceiptHeaderAccountNameField)));
            yield return (AdvanceReceiptRecordedAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.AdvanceReceiptRecordedAt,
                "入金日", 258, accept: x => x.StandardNumber(AdvanceReceiptRecordedAtField), formatter: x => x.ToString(DateFormat)));
            yield return (AdvanceReceiptPayerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptPayerCode,
                "振込依頼人コード", 259, accept: x => x.StandardString(AdvanceReceiptPayerCodeField)));
            yield return (AdvanceReceiptPayerNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptPayerName,
                "振込依頼人名", 260, accept: x => x.StandardString(AdvanceReceiptPayerNameField)));
            yield return (AdvanceReceiptPayerNameRawField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptPayerNameRaw,
                "振込依頼人名（未編集）", 261, accept: x => x.StandardString(AdvanceReceiptPayerNameRawField)));
            yield return (AdvanceReceiptReceiptAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.AdvanceReceiptReceiptAmount,
                "入金額", 262, accept: x => x.StandardNumber(AdvanceReceiptReceiptAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (AdvanceReceiptSourceBankNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptSourceBankName,
                "仕向銀行", 263, accept: x => x.StandardString(AdvanceReceiptSourceBankNameField)));
            yield return (AdvanceReceiptSourceBranchNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptSourceBranchName,
                "仕向支店", 264, accept: x => x.StandardString(AdvanceReceiptSourceBranchNameField)));
            yield return (AdvanceReceiptCategoryCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCategoryCode,
                "入金区分コード", 265, accept: x => x.StandardString(AdvanceReceiptCategoryCodeField)));
            yield return (AdvanceReceiptNote1Field = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptNote1,
                "備考", 266, accept: x => x.StandardString(AdvanceReceiptNote1Field)));
            yield return (AdvanceReceiptDueAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.AdvanceReceiptDueAt,
                "入金期日", 267, accept: x => x.StandardNumber(AdvanceReceiptDueAtField), formatter: x => x.ToString(DateFormat)));
            yield return (AdvanceReceiptApprovedField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.AdvanceReceiptApproved,
                "承認フラグ", 268, accept: x => x.StandardNumber(AdvanceReceiptApprovedField), formatter: x => x.ToString()));
            yield return (AdvanceReceiptInputTypeField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.AdvanceReceiptInputType,
                "入力区分", 269, accept: x => x.StandardNumber(AdvanceReceiptInputTypeField), formatter: x => x.ToString()));
            yield return (AdvanceReceiptCustomerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCustomerCode,
                "得意先コード（入金）", 270, accept: x => x.StandardString(AdvanceReceiptCustomerCodeField)));
            yield return (AdvanceReceiptOriginalReceiptIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.AdvanceReceiptOriginalReceiptId,
                "元入金SEQNO", 271, accept: x => x.StandardNumber(AdvanceReceiptOriginalReceiptIdField), formatter: x => x.ToString()));
            yield return (AdvanceReceiptOrigianlReceiptSubIdField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "元入金GYONO", 272, accept: x => x.StandardString(AdvanceReceiptOrigianlReceiptSubIdField)));
            yield return (AdvanceReceiptCreateByCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.AdvanceReceiptCreateBy, x => x.Id, null, x => x.Code,
                "作成者ID", 273, accept: x => x.LoginUserCodeField(AdvanceReceiptCreateByCodeField), getModelsById: GetLoginUserDictionary));
            yield return (AdvanceReceiptCreateByNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.AdvanceReceiptCreateBy, x => x.Id, null, x => x.Name,
                "作成者名", 274, accept: x => x.LoginUserCodeField(AdvanceReceiptCreateByNameField), getModelsById: GetLoginUserDictionary));
            yield return (AdvanceReceiptMemoField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptMemo,
                "入金メモ", 275, accept: x => x.StandardString(AdvanceReceiptMemoField)));
            yield return (AdvanceReceiptSectionIdField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Section>(
                x => x.AdvanceReceiptSectionId, x => x.Id, null, x => x.Code,
                "入金部門コード", 276, accept: x => x.SectionCode(AdvanceReceiptSectionIdField), getModelsById: GetSectionDictionary));
            yield return (AdvanceReceiptNote2Field = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptNote2,
                "備考2", 277, accept: x => x.StandardString(AdvanceReceiptNote2Field)));
            yield return (AdvanceReceiptNote3Field = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptNote3,
                "備考3", 278, accept: x => x.StandardString(AdvanceReceiptNote3Field)));
            yield return (AdvanceReceiptNote4Field = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptNote4,
                "備考4", 279, accept: x => x.StandardString(AdvanceReceiptNote4Field)));
            yield return (AdvanceReceiptBillNumberField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptBillNumber,
                "手形番号", 280, accept: x => x.StandardString(AdvanceReceiptBillNumberField)));
            yield return (AdvanceReceiptBillBankCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptBillBankCode,
                "券面銀行コード", 281, accept: x => x.StandardString(AdvanceReceiptBillBankCodeField)));
            yield return (AdvanceReceiptBillBranchCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptBillBranchCode,
                "券面支店コード", 282, accept: x => x.StandardString(AdvanceReceiptBillBranchCodeField)));
            yield return (AdvanceReceiptBillDrawAtField = new NullableNumberFieldDefinition<GeneralJournalizing, DateTime>(x => x.AdvanceReceiptBillDrawAt,
                "振出日", 283, accept: x => x.StandardNumber(AdvanceReceiptBillDrawAtField), formatter: x => x.ToString(DateFormat)));
            yield return (AdvanceReceiptBillDrawerField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptBillDrawer,
                "振出人", 284, accept: x => x.StandardString(AdvanceReceiptBillDrawerField)));
            yield return (AdvanceReceiptProcessingAtField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "処理予定日", 285, accept: x => x.StandardString(AdvanceReceiptProcessingAtField)));
            yield return (AdvanceReceiptRemainAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.AdvanceReceiptRemainAmount,
                "入金残", 286, accept: x => x.StandardNumber(AdvanceReceiptRemainAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (AdvanceReceiptMatchingCustomerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.Dummy,
                "消込時得意先コード", 287, accept: x => x.StandardString(AdvanceReceiptMatchingCustomerCodeField)));
            yield return (AdvanceReceiptAssignmentFlagField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.AdvanceReceiptAssignmentFlag,
                "消込フラグ", 288, accept: x => x.StandardNumber(AdvanceReceiptAssignmentFlagField), formatter: x => x.ToString()));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeAdvanceReceiptCustomer()
        {
            yield return (AdvanceReceiptCustomerNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCustomerName,
                "得意先名", 289, accept: x => x.StandardString(AdvanceReceiptCustomerNameField)));
            yield return (AdvanceReceiptCustomerKanaField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCustomerKana,
                "得意先カナ", 290, accept: x => x.StandardString(AdvanceReceiptCustomerKanaField)));
            yield return (AdvanceReceiptCustomerStaffIdField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff>(
                x => x.AdvanceReceiptCustomerStaffId, x => x.Id, null, x => x.Code,
                "営業担当者コード", 291, accept: x => x.StaffCode(AdvanceReceiptCustomerStaffIdField), getModelsById: GetStaffDictionary));
            yield return (AdvanceReceiptCustomerIsParentField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.AdvanceReceiptCustomerIsParent,
                "債権代表者フラグ", 292, accept: x => x.StandardNumber(AdvanceReceiptCustomerIsParentField), formatter: x => x.ToString()));
            yield return (AdvanceReceiptCustomerTelField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCustomerTel,
                "電話番号", 293, accept: x => x.StandardString(AdvanceReceiptCustomerTelField)));
            yield return (AdvanceReceiptCustomerFaxField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCustomerFax,
                "FAX番号", 294, accept: x => x.StandardString(AdvanceReceiptCustomerFaxField)));
            yield return (AdvanceReceiptCustomerCustomerStaffNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCustomerCustomerStaffName,
                "相手先担当者名", 295, accept: x => x.StandardString(AdvanceReceiptCustomerCustomerStaffNameField)));
            yield return (AdvanceReceiptCustomerNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCustomerNote,
                "備考", 296, accept: x => x.StandardString(AdvanceReceiptCustomerNoteField)));
            yield return (AdvanceReceiptCustomerDensaiCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCustomerDensaiCode,
                "電子手形用企業コード", 297, accept: x => x.StandardString(AdvanceReceiptCustomerDensaiCodeField)));
            yield return (AdvanceReceiptCustomerCreditCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCustomerCreditCode,
                "信用調査用企業コード", 298, accept: x => x.StandardString(AdvanceReceiptCustomerCreditCodeField)));
            yield return (AdvanceReceiptCustomerCreditRankField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCustomerCreditRank,
                "与信ランク", 299, accept: x => x.StandardString(AdvanceReceiptCustomerCreditRankField)));
            yield return (AdvanceReceiptParentCustomerCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptParentCustomerCode,
                "得意先コード", 300, accept: x => x.StandardString(AdvanceReceiptParentCustomerCodeField)));
            yield return (AdvanceReceiptParentCustomerNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptParentCustomerName,
                "得意先名", 301, accept: x => x.StandardString(AdvanceReceiptParentCustomerNameField)));
            yield return (AdvanceReceiptParentCustomerKanaField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptParentCustomerKana,
                "得意先カナ", 302, accept: x => x.StandardString(AdvanceReceiptParentCustomerKanaField)));
            yield return (AdvanceReceiptParentCustomerStaffIdField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, Staff>(
                x => x.AdvanceReceiptParentCustomerStaffId, x => x.Id, null, x => x.Code,
                "営業担当者コード", 303, accept: x => x.StaffCode(AdvanceReceiptBillBankCodeField), getModelsById: GetStaffDictionary));
            yield return (AdvanceReceiptParentCustomerIsParentField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.AdvanceReceiptParentCustomerIsParent,
                "債権代表者フラグ", 304, accept: x => x.StandardNumber(AdvanceReceiptParentCustomerIsParentField), formatter: x => x.ToString()));
            yield return (AdvanceReceiptParentCustomerTelField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptParentCustomerTel,
                "電話番号", 305, accept: x => x.StandardString(AdvanceReceiptParentCustomerTelField)));
            yield return (AdvanceReceiptParentCustomerFaxField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptParentCustomerFax,
                "FAX番号", 306, accept: x => x.StandardString(AdvanceReceiptParentCustomerFaxField)));
            yield return (AdvanceReceiptParentCustomerCustomerStaffNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptParentCustomerCustomerStaffName,
                "相手先担当者名", 307, accept: x => x.StandardString(AdvanceReceiptParentCustomerCustomerStaffNameField)));
            yield return (AdvanceReceiptParentCustomerNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptParentCustomerNote,
                "備考", 308, accept: x => x.StandardString(AdvanceReceiptParentCustomerNoteField)));
            yield return (AdvanceReceiptParentCustomerDensaiCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptParentCustomerDensaiCode,
                "電子手形用企業コード", 309, accept: x => x.StandardString(AdvanceReceiptParentCustomerDensaiCodeField)));
            yield return (AdvanceReceiptParentCustomerCreditCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptParentCustomerCreditCode,
                "信用調査用企業コード", 310, accept: x => x.StandardString(AdvanceReceiptParentCustomerCreditCodeField)));
            yield return (AdvanceReceiptParentCustomerCreditRankField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptParentCustomerCreditRank,
                "与信ランク", 311, accept: x => x.StandardString(AdvanceReceiptParentCustomerCreditRankField)));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeAdvanceReceiptCategory()
        {
            yield return (AdvanceReceiptCategoryNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCategoryName,
                "入金区分名", 312, accept: x => x.StandardString(AdvanceReceiptCategoryNameField)));
            yield return (AdvanceReceiptCategoryAccountTitleCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.AdvanceReceiptCategoryAccountTitleId, x => x.Id, null, x => x.Code,
                "科目コード", 313, accept: x => x.AccountTitleCode(AdvanceReceiptCategoryAccountTitleCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (AdvanceReceiptCategorySubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCategorySubCode,
                "補助コード", 314, accept: x => x.StandardString(AdvanceReceiptCategorySubCodeField)));
            yield return (AdvanceReceiptCategoryUseLimitDateField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.AdvanceReceiptCategoryUseLimitDate,
                "期日入力可否", 315, accept: x => x.StandardNumber(AdvanceReceiptCategoryUseLimitDateField), formatter: x => x.ToString()));
            yield return (AdvanceReceiptCategoryUseCashOnDueDatesField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.AdvanceReceiptCategoryUseCashOnDueDates,
                "期日入金管理使用可否", 316, accept: x => x.StandardNumber(AdvanceReceiptCategoryUseCashOnDueDatesField), formatter: x => x.ToString()));
            yield return (AdvanceReceiptCategoryTaxClassIdField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.AdvanceReceiptCategoryTaxClassId,
                "消費税属性", 317, accept: x => x.StandardNumber(AdvanceReceiptCategoryTaxClassIdField), formatter: x => x.ToString()));
            yield return (AdvanceReceiptCategoryNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCategoryNote,
                "備考", 318, accept: x => x.StandardString(AdvanceReceiptCategoryNoteField)));
            yield return (AdvanceReceiptCategoryExternalCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.AdvanceReceiptCategoryExternalCode,
                "外部コード", 319, accept: x => x.StandardString(AdvanceReceiptCategoryExternalCodeField)));
            yield return (AdvanceReceiptCategoryAccountTitleNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.AdvanceReceiptCategoryAccountTitleId, x => x.Id, null, x => x.Name,
                "科目名", 320, accept: x => x.AccountTitleCode(AdvanceReceiptCategoryAccountTitleNameField), getModelsById: GetAccountTitleDictionary));
            yield return (AdvanceReceiptCategoryAccountTitleContraCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.AccountReceivableAccountTitleId, x => x.Id, null, x => x.ContraAccountCode,
                "相手先科目コード", 321, accept: x => x.AccountTitleCode(AdvanceReceiptCategoryAccountTitleContraCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (AdvanceReceiptCategoryAccountTitleContraNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.AccountReceivableAccountTitleId, x => x.Id, null, x => x.ContraAccountName,
                "相手先科目名", 322, accept: x => x.AccountTitleCode(AdvanceReceiptCategoryAccountTitleContraNameField), getModelsById: GetAccountTitleDictionary));
            yield return (AdvanceReceiptCategoryAccountTitleContraSubCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.AccountReceivableAccountTitleId, x => x.Id, null, x => x.ContraAccountSubCode,
                "相手先補助コード", 323, accept: x => x.AccountTitleCode(AdvanceReceiptCategoryAccountTitleContraSubCodeField), getModelsById: GetAccountTitleDictionary));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeReceiptExclude()
        {
            yield return (ReceiptExcludeIdField = new NullableNumberFieldDefinition<GeneralJournalizing, long>(x => x.ReceiptExcludeId,
                "対象外SEQ", 324, accept: x => x.StandardNumber(ReceiptExcludeIdField), formatter: x => x.ToString()));
            yield return (ReceiptExcludeAmountField = new NullableNumberFieldDefinition<GeneralJournalizing, decimal>(x => x.ReceiptExcludeAmount,
                "対象外金額", 325, accept: x => x.StandardNumber(ReceiptExcludeAmountField), formatter: x => x.ToString(MoneyFormat)));
            yield return (ReceiptExcludeCategoryCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptExcludeCategoryCode,
                "対象外区分コード", 326, accept: x => x.StandardString(ReceiptExcludeCategoryCodeField)));
            yield return (ReceiptExcludeCreateByCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.ReceiptExcludeCreateBy, x => x.Id, null, x => x.Code,
                "作成者ID", 327, accept: x => x.LoginUserCodeField(ReceiptExcludeCreateByCodeField), getModelsById: GetLoginUserDictionary));
            yield return (ReceiptExcludeCreateByNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.ReceiptExcludeCreateBy, x => x.Id, null, x => x.Name,
                "作成者名", 328, accept: x => x.LoginUserCodeField(ReceiptExcludeCreateByNameField), getModelsById: GetLoginUserDictionary));
            yield return (ReceiptExcludeUpdateByCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.ReceiptExcludeUpdateBy, x => x.Id, null, x => x.Code,
                "更新者ID", 329, accept: x => x.LoginUserCodeField(ReceiptExcludeUpdateByCodeField), getModelsById: GetLoginUserDictionary));
            yield return (ReceiptExcludeUpdateByNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, LoginUser>(
                x => x.ReceiptExcludeUpdateBy, x => x.Id, null, x => x.Name,
                "更新者名", 330, accept: x => x.LoginUserCodeField(ReceiptExcludeUpdateByNameField), getModelsById: GetLoginUserDictionary));
            yield return (ReceiptExcludeCategoryNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptExcludeCategoryName,
                "対象外区分名", 331, accept: x => x.StandardString(ReceiptExcludeCategoryNameField)));
            yield return (ReceiptExcludeCategoryAccountTitleCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ReceiptExcludeCategoryAccountTitleId, x => x.Id, null, x => x.Code,
                "対象外区分科目コード", 332, accept: x => x.AccountTitleCode(ReceiptExcludeCategoryAccountTitleCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceiptExcludeCategorySubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptExcludeCategorySubCode,
                "対象外区分補助コード", 333, accept: x => x.StandardString(ReceiptExcludeCategorySubCodeField)));
            yield return (ReceiptExcludeCategoryNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptExcludeCategoryNote,
                "対象外区分備考", 334, accept: x => x.StandardString(ReceiptExcludeCategoryNoteField)));
            yield return (ReceiptExcludeCategoryTaxClassIdField = new NullableNumberFieldDefinition<GeneralJournalizing, int>(x => x.ReceiptExcludeCategoryTaxClassId,
                "対象外区分消費税属性", 335, accept: x => x.StandardNumber(ReceiptExcludeCategoryTaxClassIdField), formatter: x => x.ToString()));
            yield return (ReceiptExcludeCategoryAccountTitleNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ReceiptExcludeCategoryAccountTitleId, x => x.Id, null, x => x.Name,
                "対象外区分科目名", 336, accept: x => x.AccountTitleCode(ReceiptExcludeCategoryAccountTitleNameField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceiptExcludeCategoryAccountTitleContraCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ReceiptExcludeCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountCode,
                "対象外区分相手先科目コード", 337, accept: x => x.AccountTitleCode(ReceiptExcludeCategoryAccountTitleContraCodeField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceiptExcludeCategoryAccountTitleContraNameField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ReceiptExcludeCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountName,
                "対象外区分相手先科目名", 338, accept: x => x.AccountTitleCode(ReceiptExcludeCategoryAccountTitleContraNameField), getModelsById: GetAccountTitleDictionary));
            yield return (ReceiptExcludeCategoryAccountTitleContraSubCodeField = new StandardNullableIdToCodeFieldDefinition<GeneralJournalizing, AccountTitle>(
                x => x.ReceiptExcludeCategoryAccountTitleId, x => x.Id, null, x => x.ContraAccountSubCode,
                "対象外区分相手先補助コード", 339, accept: x => x.AccountTitleCode(ReceiptExcludeCategoryAccountTitleContraSubCodeField), getModelsById: GetAccountTitleDictionary));
        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeGeneralSettings()
        {
            yield return (SuspentReceiptDepartmentCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.SuspentReceiptDepartmentCode,
                "仮受部門コード", 340, accept: x => x.StandardString(SuspentReceiptDepartmentCodeField)));
            yield return (SuspentReceiptDepartmentNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.SuspentReceiptDepartmentName,
                "仮受部門名", 341, accept: x => x.StandardString(SuspentReceiptDepartmentNameField)));
            yield return (SuspentReceiptDepartmentNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.SuspentReceiptDepartmentNote,
                "仮受部門備考", 342, accept: x => x.StandardString(SuspentReceiptDepartmentNoteField)));
            yield return (SuspentReceiptAccountTitleCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.SuspentReceiptAccountTitleCode,
                "仮受科目コード", 343, accept: x => x.StandardString(SuspentReceiptAccountTitleCodeField)));
            yield return (SuspentReceiptAccountTitleNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.SuspentReceiptAccountTitleName,
                "仮受科目名", 344, accept: x => x.StandardString(SuspentReceiptAccountTitleNameField)));
            yield return (SuspentReceiptAccountTitleContraCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.SuspentReceiptAccountTitleContraCode,
                "仮受相手先科目コード", 345, accept: x => x.StandardString(SuspentReceiptAccountTitleContraCodeField)));
            yield return (SuspentReceiptAccountTitleContraNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.SuspentReceiptAccountTitleContraName,
                "仮受相手先科目名", 346, accept: x => x.StandardString(SuspentReceiptAccountTitleContraNameField)));
            yield return (SuspentReceiptAccountTitleContraSubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.SuspentReceiptAccountTitleContraSubCode,
                "仮受相手先補助コード", 347, accept: x => x.StandardString(SuspentReceiptAccountTitleContraSubCodeField)));
            yield return (SuspentReceiptSubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.SuspentReceiptSubCode,
                "仮受補助コード", 348, accept: x => x.StandardString(SuspentReceiptSubCodeField)));
            yield return (TransferFeeDepartmentCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.TransferFeeDepartmentCode,
                "振込手数料部門コード", 349, accept: x => x.StandardString(TransferFeeDepartmentCodeField)));
            yield return (TransferFeeDepartmentNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.TransferFeeDepartmentName,
                "振込手数料部門名", 350, accept: x => x.StandardString(TransferFeeDepartmentNameField)));
            yield return (TransferFeeDepartmentNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.TransferFeeDepartmentNote,
                "振込手数料部門備考", 351, accept: x => x.StandardString(TransferFeeDepartmentNoteField)));
            yield return (TransferFeeAccountTitleCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.TransferFeeAccountTitleCode,
                "振込手数料科目コード", 352, accept: x => x.StandardString(TransferFeeAccountTitleCodeField)));
            yield return (TransferFeeAccountTitleNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.TransferFeeAccountTitleName,
                "振込手数料科目名", 353, accept: x => x.StandardString(TransferFeeAccountTitleNameField)));
            yield return (TransferFeeAccountTitleContraCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.TransferFeeAccountTitleContraCode,
                "振込手数料相手先科目コード", 354, accept: x => x.StandardString(TransferFeeAccountTitleContraCodeField)));
            yield return (TransferFeeAccountTitleContraNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.TransferFeeAccountTitleContraName,
                "振込手数料相手先科目名", 355, accept: x => x.StandardString(TransferFeeAccountTitleContraNameField)));
            yield return (TransferFeeAccountTitleContraSubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.TransferFeeAccountTitleContraSubCode,
                "振込手数料相手先補助コード", 356, accept: x => x.StandardString(TransferFeeAccountTitleContraSubCodeField)));
            yield return (TransferFeeSubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.TransferFeeSubCode,
                "振込手数料補助コード", 357, accept: x => x.StandardString(TransferFeeSubCodeField)));
            yield return (DebitTaxDiffDepartmentCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.DebitTaxDiffDepartmentCode,
                "借方消費税誤差部門コード", 358, accept: x => x.StandardString(DebitTaxDiffDepartmentCodeField)));
            yield return (DebitTaxDiffDepartmentNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.DebitTaxDiffDepartmentName,
                "借方消費税誤差部門名", 359, accept: x => x.StandardString(DebitTaxDiffDepartmentNameField)));
            yield return (DebitTaxDiffDepartmentNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.DebitTaxDiffDepartmentNote,
                "借方消費税誤差部門備考", 360, accept: x => x.StandardString(DebitTaxDiffDepartmentNoteField)));
            yield return (DebitTaxDiffAccountTitleCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.DebitTaxDiffAccountTitleCode,
                "借方消費税誤差科目コード", 361, accept: x => x.StandardString(DebitTaxDiffAccountTitleCodeField)));
            yield return (DebitTaxDiffAccountTitleNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.DebitTaxDiffAccountTitleName,
                "借方消費税誤差科目名", 362, accept: x => x.StandardString(DebitTaxDiffAccountTitleNameField)));
            yield return (DebitTaxDiffAccountTitleContraCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.DebitTaxDiffAccountTitleContraCode,
                "借方消費税誤差相手先科目コード", 363, accept: x => x.StandardString(DebitTaxDiffAccountTitleContraCodeField)));
            yield return (DebitTaxDiffAccountTitleContraNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.DebitTaxDiffAccountTitleContraName,
                "借方消費税誤差相手先科目名", 364, accept: x => x.StandardString(DebitTaxDiffAccountTitleContraNameField)));
            yield return (DebitTaxDiffAccountTitleContraSubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.DebitTaxDiffAccountTitleContraSubCode,
                "借方消費税誤差相手先補助コード", 365, accept: x => x.StandardString(DebitTaxDiffAccountTitleContraSubCodeField)));
            yield return (DebitTaxDiffSubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.DebitTaxDiffSubCode,
                "借方消費税誤差補助コード", 366, accept: x => x.StandardString(DebitTaxDiffSubCodeField)));
            yield return (CreditTaxDiffDepartmentCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CreditTaxDiffDepartmentCode,
                "貸方消費税誤差部門コード", 367, accept: x => x.StandardString(CreditTaxDiffDepartmentCodeField)));
            yield return (CreditTaxDiffDepartmentNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.CreditTaxDiffDepartmentName,
                "貸方消費税誤差部門名", 368, accept: x => x.StandardString(CreditTaxDiffDepartmentNameField)));
            yield return (CreditTaxDiffDepartmentNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.CreditTaxDiffDepartmentNote,
                "貸方消費税誤差部門備考", 369, accept: x => x.StandardString(CreditTaxDiffDepartmentNoteField)));
            yield return (CreditTaxDiffAccountTitleCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CreditTaxDiffAccountTitleCode,
                "貸方消費税誤差科目コード", 370, accept: x => x.StandardString(CreditTaxDiffAccountTitleCodeField)));
            yield return (CreditTaxDiffAccountTitleNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.CreditTaxDiffAccountTitleName,
                "貸方消費税誤差科目名", 371, accept: x => x.StandardString(CreditTaxDiffAccountTitleNameField)));
            yield return (CreditTaxDiffAccountTitleContraCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CreditTaxDiffAccountTitleContraCode,
                "貸方消費税誤差相手先科目コード", 372, accept: x => x.StandardString(CreditTaxDiffAccountTitleContraCodeField)));
            yield return (CreditTaxDiffAccountTitleContraNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.CreditTaxDiffAccountTitleContraName,
                "貸方消費税誤差相手先科目名", 373, accept: x => x.StandardString(CreditTaxDiffAccountTitleContraNameField)));
            yield return (CreditTaxDiffAccountTitleContraSubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CreditTaxDiffAccountTitleContraSubCode,
                "貸方消費税誤差相手先補助コード", 374, accept: x => x.StandardString(CreditTaxDiffAccountTitleContraSubCodeField)));
            yield return (CreditTaxDiffSubCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.CreditTaxDiffSubCode,
                "貸方消費税誤差補助コード", 375, accept: x => x.StandardString(CreditTaxDiffSubCodeField)));
            yield return (ReceiptDepartmentCodeField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptDepartmentCode,
                "入金部門コード", 376, accept: x => x.StandardString(ReceiptDepartmentCodeField)));
            yield return (ReceiptDepartmentNameField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptDepartmentName,
                "入金部門名", 377, accept: x => x.StandardString(ReceiptDepartmentNameField)));
            yield return (ReceiptDepartmentNoteField = new StringFieldDefinition<GeneralJournalizing>(x => x.ReceiptDepartmentNote,
                "入金部門備考", 378, accept: x => x.StandardString(ReceiptDepartmentNoteField)));
            yield return (NewTaxRate1ValueField = new StringFieldDefinition<GeneralJournalizing>(x => x.NewTaxRate1Value,
                "新消費税率", 379, accept: x => x.StandardString(NewTaxRate1ValueField)));
            yield return (NewTaxRate2ValueField = new StringFieldDefinition<GeneralJournalizing>(x => x.NewTaxRate2Value,
                "新消費税率2", 380, accept: x => x.StandardString(NewTaxRate2ValueField)));
            yield return (NewTaxRate3ValueField = new StringFieldDefinition<GeneralJournalizing>(x => x.NewTaxRate3Value,
                "新消費税率3", 381, accept: x => x.StandardString(NewTaxRate3ValueField)));
            yield return (NewTaxRate1ApplyDateValueField = new StringFieldDefinition<GeneralJournalizing>(x => x.NewTaxRate1ApplyDateValue,
                "新税率開始年月日", 382, accept: x => x.StandardString(NewTaxRate1ApplyDateValueField)));
            yield return (NewTaxRate2ApplyDateValueField = new StringFieldDefinition<GeneralJournalizing>(x => x.NewTaxRate2ApplyDateValue,
                "新税率開始年月日2", 383, accept: x => x.StandardString(NewTaxRate2ApplyDateValueField)));
            yield return (NewTaxRate3ApplyDateValueField = new StringFieldDefinition<GeneralJournalizing>(x => x.NewTaxRate3ApplyDateValue,
                "新税率開始年月日3", 384, accept: x => x.StandardString(NewTaxRate3ApplyDateValueField)));
            yield return (TaxRoundingModeValueField = new StringFieldDefinition<GeneralJournalizing>(x => x.TaxRoundingModeValue,
                "消費税計算端数処理", 385, accept: x => x.StandardString(TaxRoundingModeValueField)));
            yield return (PrecisionField = new NumberFieldDefinition<GeneralJournalizing, int>(x => x.Precision,
                "小数点以下桁数", 386, accept: x => x.StandardNumber(PrecisionField), formatter: x => x.ToString()));
            yield return (JournalizingTypeField = new NumberFieldDefinition<GeneralJournalizing, int>(x => x.JournalizingType,
                "仕訳種別", 387, accept: x => x.StandardNumber(JournalizingTypeField), formatter: x => x.ToString()));

        }
        private IEnumerable<IFieldDefinition<GeneralJournalizing>> InitializeBillingAdditional()
        {
            yield return (BillingNote5Field = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingNote5,
                "備考5", 389, accept: x => x.StandardString(BillingNote5Field)));
            yield return (BillingNote6Field = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingNote6,
                "備考6", 389, accept: x => x.StandardString(BillingNote6Field)));
            yield return (BillingNote7Field = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingNote7,
                "備考7", 389, accept: x => x.StandardString(BillingNote7Field)));
            yield return (BillingNote8Field = new StringFieldDefinition<GeneralJournalizing>(x => x.BillingNote8,
                "備考8", 389, accept: x => x.StandardString(BillingNote8Field)));
        }
        #endregion
    }
}
