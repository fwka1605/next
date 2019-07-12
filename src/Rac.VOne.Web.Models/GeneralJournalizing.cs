using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class GeneralJournalizing
    {
        /// <summary>仕訳種別
        /// 0 : 入金仕訳
        /// 1 : 消込仕訳
        /// 2 : 前受発生仕訳
        /// 3 : 前受振替仕訳
        /// 4 : 対象外仕訳
        /// </summary>
        [DataMember] public int JournalizingType { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public int Precision { get; set; }
        [DataMember] public decimal Amount { get; set; }
        [DataMember] public bool Approved { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public long ReceiptId { get; set; }
        [DataMember] public long? BillingId { get; set; }
        [DataMember] public long? MatchingId { get; set; }
        [DataMember] public long? ScheduledIncomeReceiptId { get; set; }
        [DataMember] public long? ScheduledIncomeBillingId { get; set; }
        [DataMember] public long? ScheduledIncomeMatchingHeaderId { get; set; }
        [DataMember] public long? AdvanceReceivedBackupId { get; set; }
        [DataMember] public long? ReceiptExcludeId { get; set; }

        [DataMember] public int? ReceiptCustomerId { get; set; }
        [DataMember] public int? ReceiptParentCustomerId { get; set; }
        [DataMember] public int? BillingCustomerId { get; set; }
        [DataMember] public int? BillingParentCustomerId { get; set; }
        [DataMember] public int? AdvanceReceivedCustomerId { get; set; }
        [DataMember] public int? AdvanceReceivedParentCustomerId { get; set; }
        [DataMember] public int? BillingDepartmentId { get; set; }
        [DataMember] public int? BillingDepartmentStaffId { get; set; }
        [DataMember] public int? BillingStaffId { get; set; }
        [DataMember] public int? BillingStaffDepartmentId { get; set; }

        public Matching Matching { private get; set; }
        public long? MatchingHeaderId { get { return Matching?.MatchingHeaderId; } }
        public MatchingHeader MatchingHeader { private get; set; }
        public Receipt Receipt { private get; set; }
        public long? ReceiptHeaderId { get { return Receipt?.ReceiptHeaderId; } }
        public ReceiptHeader ReceiptHeader { private get; set; }
        public ReceiptMemo ReceiptMemo { private get; set; }

        public Billing Billing { private get; set; }
        public Customer ReceiptCustomer { private get; set; }
        public Customer ReceiptParentCustomer { private get; set; }
        /// <summary>請求側得意先</summary>
        public Customer Customer { private get; set; }
        /// <summary>請求側親得意先</summary>
        public Customer ParentCustomer { private get; set; }
        public BankAccount BankAccount { private get; set; }
        public Category ReceiptCategory { private get; set; }
        public Category BillingCategory { private get; set; }
        public Category CollectCategory { private get; set; }
        public Category OriginalCollectCategory { private get; set; }
        public Category ScheduledReceiptCategory { private get; set; }
        public Customer ScheudledReceiptCustomer { private get; set; }
        public int? AccountReceivableAccountTitleId { get; set; }
        public Receipt ScheduledReceipt { private get; set; }
        public long? ScheduledReceiptHeaderId { get { return ScheduledReceipt?.ReceiptHeaderId; } }
        public ReceiptHeader ScheduledReceiptHeader { private get; set; }
        public ReceiptMemo ScheduledReceiptMemo { private get; set; }

        public AdvanceReceivedBackup AdvanceReceipt { private get; set; }
        public long? AdvanceReceivedHeaderId { get { return AdvanceReceipt?.ReceiptHeaderId; } }
        public ReceiptHeader AdvanceReceivedHeader { private get; set; }
        public Customer AdvanceReceivedCustomer { private get; set; }
        public Customer AdvanceReceivedParentCustomer { private get; set; }
        public Category AdvanceReceiptCategory { private get; set; }

        public ReceiptExclude ReceiptExclude { private get; set; }
        public Category ReceiptExcludeCategory { private get; set; }

        /// <summary>仮受部門</summary>
        public Department SuspentReceiptDepartment { private get; set; }
        /// <summary>仮受科目</summary>
        public AccountTitle SuspentReceiptAccountTitle { private get; set; }
        /// <summary>振込手数料部門</summary>
        public Department TransferFeeDepartment { private get; set; }
        /// <summary>振込手数料科目</summary>
        public AccountTitle TransferFeeAccountTitle { private get; set; }
        /// <summary>借方消費税誤差部門</summary>
        public Department DebitTaxDiffDepartment { private get; set; }
        /// <summary>借方消費税科目</summary>
        public AccountTitle DebitTaxDiffAccountTitle { private get; set; }
        /// <summary>貸方消費税誤差部門</summary>
        public Department CreditTaxDiffDepartment { private get; set; }
        /// <summary>貸方消費税科目</summary>
        public AccountTitle CreditTaxDiffAccountTitle { private get; set; }
        /// <summary>入金部門 Sectionではなく、入金を担当する請求部門</summary>
        public Department ReceiptDepartment { private get; set; }


        /// <summary>管理マスター</summary>
        public Dictionary<string, string> GeneralSettings { private get; set; }
        private string GetGeneralSettingValue(string key)
            => GeneralSettings != null
            && GeneralSettings.ContainsKey(key)
            ? GeneralSettings[key] : string.Empty;

        #region general setting code const
        private const string SuspentReceiptDepartmentCodeKey    = "仮受部門コード";
        private const string SuspentReceiptAccountTitleCodeKey  = "仮受科目コード";
        private const string SuspentReceiptSubCodeKey           = "仮受補助コード";
        private const string TransferFeeDepartmentCodeKey       = "振込手数料部門コード";
        private const string TransferFeeAccountTitleCodeKey     = "振込手数料科目コード";
        private const string TransferFeeSubCodeKey              = "振込手数料補助コード";
        private const string DebitTaxDiffDepartmentCodeKey      = "借方消費税誤差部門コード";
        private const string DebitTaxDiffAccountTitleCodeKey    = "借方消費税誤差科目コード";
        private const string DebitTaxDiffSubCodeKey             = "借方消費税誤差補助コード";
        private const string CreditTaxDiffDepartmentCodeKey     = "貸方消費税誤差部門コード";
        private const string CreditTaxDiffAccountTitleCodeKey   = "貸方消費税誤差科目コード";
        private const string CreditTaxDiffSubCodeKey            = "貸方消費税誤差補助コード";
        private const string ReceiptDepartmentCodeKey           = "入金部門コード";
        private const string NewTaxRate1Key                     = "新消費税率";
        private const string NewTaxRate2Key                     = "新消費税率2";
        private const string NewTaxRate3Key                     = "新消費税率3";
        private const string NewTaxRate1ApplyDateKey            = "新税率開始年月日";
        private const string NewTaxRate2ApplyDateKey            = "新税率開始年月日2";
        private const string NewTaxRate3ApplyDateKey            = "新税率開始年月日3";
        private const string TaxRoundingModeKey                 = "消費税計算端数処理";
        #endregion


        #region matching / matching header
        public decimal? MatchingBankTransferFee { get { return Matching?.BankTransferFee; } }
        public decimal? MatchingAmount { get { return Matching?.Amount; } }
        public string MatchingProcessTypeName
        {
            get
            {
                return MatchingHeader?.MatchingProcessType == 0 ? "一括消込"
                     : MatchingHeader?.MatchingProcessType == 1 ? "個別消込" : "";
            }
        }
        public DateTime? MatchingRecordedAt { get { return Matching?.RecordedAt; } }
        public int? MatchingCreateBy { get { return Matching?.CreateBy; } }
        public int? MatchingUpdateBy { get { return Matching?.UpdateBy; } }
        public decimal? MatchingTaxDifference { get { return Matching?.TaxDifference; } }
        public string MatchingMemo { get { return MatchingHeader?.Memo; } }
        #endregion

        #region receipt
        public DateTime? ReceiptRecordedAt { get { return Receipt?.RecordedAt; } }
        public string ReceiptPayerCode { get { return Receipt?.PayerCode; } }
        public string ReceiptPayerName { get { return Receipt?.PayerName; } }
        public string  ReceiptPayerNameRaw { get { return Receipt?.PayerNameRaw; } }
        public decimal? ReceiptReceiptAmount { get { return Receipt?.ReceiptAmount; } }
        public string ReceiptSourceBankName { get { return Receipt?.SourceBankName; } }
        public string ReceiptSourceBranchName { get { return Receipt?.SourceBranchName; } }
        public int? ReceiptReceiptCategoryId { get { return Receipt?.ReceiptCategoryId; } }
        public string ReceiptNote1 { get { return Receipt?.Note1; } }
        public DateTime? ReceiptDueAt { get { return Receipt?.DueAt; } }
        public int? ReceiptApproved { get { return Receipt?.Approved; } }
        public int? ReceiptInputType { get { return Receipt?.InputType; } }
        public long? ReceiptOriginalReceiptId { get { return Receipt?.OriginalReceiptId; } }
        public int? ReceiptCreateBy { get { return Receipt?.CreateBy; } }
        public int? ReceiptUpdateBy { get { return Receipt?.UpdateBy; } }
        public string ReceiptReceiptMemo { get { return ReceiptMemo?.Memo; } }
        public int? ReceiptSectionId { get { return Receipt?.SectionId; } }
        public string ReceiptNote2 { get { return Receipt?.Note2; } }
        public string ReceiptNote3 { get { return Receipt?.Note3; } }
        public string ReceiptNote4 { get { return Receipt?.Note4; } }
        public string ReceiptBillNumber { get { return Receipt?.BillNumber; } }
        public string ReceiptBillBankCode { get { return Receipt?.BillBankCode; } }
        public string ReceiptBillBranchCode { get { return Receipt?.BillBranchCode; } }
        public DateTime? ReceiptBillDrawAt { get { return Receipt?.BillDrawAt; } }
        public string ReceiptBillDrawer { get { return Receipt?.BillDrawer; } }
        public DateTime? ReceiptProcessingAt { get { return Receipt?.ProcessingAt; } }
        public decimal? ReceiptRemainAmount { get { return Receipt?.RemainAmount; } }
        public int? ReceiptAssignmentFlag { get { return Receipt?.AssignmentFlag; } }
        public string ReceiptBankCode { get { return Receipt?.BankCode; } }
        public string ReceiptBankName { get { return Receipt?.BankName; } }
        public string ReceiptBranchCode { get { return Receipt?.BranchCode; } }
        public string ReceiptBranchName { get { return Receipt?.BranchName; } }
        public int? ReceiptAccountTypeId { get { return Receipt?.AccountTypeId; } }
        public string ReceiptAccountNumber { get { return Receipt?.AccountNumber; } }
        public string ReceiptAccountName { get { return Receipt?.AccountName; } }
        #endregion

        #region billing
        public DateTime? BillingBilledAt { get { return Billing?.BilledAt; } }
        public decimal? BillingBillingAmount { get { return Billing?.BillingAmount; } }
        public decimal? BillingTaxAmount { get { return Billing?.TaxAmount; } }
        public DateTime? BillingClosingAt { get { return Billing?.ClosingAt; } }
        public DateTime? BillingDueAt { get { return Billing?.DueAt; } }
        public DateTime? BillingOriginalDueAt { get { return Billing?.OriginalDueAt; } }
        public int? BillingDebitAccountTitleId { get { return Billing?.DebitAccountTitleId; } }
        public int? BillingCreditAccountTitleId { get { return Billing?.CreditAccountTitleId; } }
        public DateTime? BillingSalesAt { get { return Billing?.SalesAt; } }
        public string BillingInvoiceCode { get { return Billing?.InvoiceCode; } }
        public string BillingNote1 { get { return Billing?.Note1; } }
        public int? BillingBillingCategoryId { get { return Billing?.BillingCategoryId; } }
        public int? BillingInputType { get { return Billing?.InputType; } }
        public string BillingMemo { get { return Billing?.Memo; } }
        public int? BillingCollectCategoryId { get { return Billing?.CollectCategoryId; } }
        public int? BillingOriginalCollectCategoryId { get { return Billing?.OriginalCollectCategoryId; } }
        public int? BillingTaxClassId { get { return Billing?.TaxClassId; } }
        public decimal? BillingBillingAmountExcludingTax { get { return Billing?.BillingAmountExcludingTax; } }
        public DateTime? BillingRequestDate { get { return Billing?.RequestDate; } }
        public int? BillingResultCode { get { return Billing?.ResultCode; } }
        public DateTime? BillingTransferOriginalDueAt { get { return Billing?.TransferOriginalDueAt; } }
        public string BillingNote2 { get { return Billing?.Note2; } }
        public string BillingNote3 { get { return Billing?.Note3; } }
        public string BillingNote4 { get { return Billing?.Note4; } }
        public string BillingNote5 { get { return Billing?.Note5; } }
        public string BillingNote6 { get { return Billing?.Note6; } }
        public string BillingNote7 { get { return Billing?.Note7; } }
        public string BillingNote8 { get { return Billing?.Note8; } }
        public string BillingScheduledPaymentKey { get { return Billing?.ScheduledPaymentKey; } }
        public long? BillingAccountTransferLogId { get { return Billing?.AccountTransferLogId; } }
        public decimal? BillingAssignmentAmount { get { return Billing?.AssignmentAmount; } }
        public decimal? BillingRemainAmount { get { return Billing?.RemainAmount; } }
        public int? BillingAssignmentFlag { get { return Billing?.AssignmentFlag; } }
        #endregion

        #region customer
        public string CustomerCode { get { return Customer?.Code; } }
        public string CustomerName { get { return Customer?.Name; } }
        public string CustomerKana { get { return Customer?.Kana; } }
        public int? CustomerStaffId { get { return Customer?.StaffId; } }
        public int? CustomerIsParent { get { return Customer?.IsParent; } }
        public string CustomerTel { get { return Customer?.Tel; } }
        public string CustomerFax { get { return Customer?.Fax; } }
        public string CustomerCustomerStaffName { get { return Customer?.CustomerStaffName; } }
        public string CustomerNote { get { return Customer?.Note; } }
        public string CustomerDensaiCode { get { return Customer?.DensaiCode; } }
        public string CustomerCreditCode { get { return Customer?.CreditCode; } }
        public string CustomerCreditRank { get { return Customer?.CreditRank; } }
        #endregion

        #region parent customer
        public string ParentCustomerCode { get { return ParentCustomer?.Code; } }
        public string ParentCustomerName { get { return ParentCustomer?.Name; } }
        public string ParentCustomerKana { get { return ParentCustomer?.Kana; } }
        public int?   ParentCustomerStaffId { get { return ParentCustomer?.StaffId; } }
        public int?   ParentCustomerIsParent { get { return ParentCustomer?.IsParent; } }
        public string ParentCustomerTel { get { return ParentCustomer?.Tel; } }
        public string ParentCustomerFax { get { return ParentCustomer?.Fax; } }
        public string ParentCustomerCustomerStaffName { get { return ParentCustomer?.CustomerStaffName; } }
        public string ParentCustomerNote { get { return ParentCustomer?.Note; } }
        public string ParentCustomerDensaiCode { get { return ParentCustomer?.DensaiCode; } }
        public string ParentCustomerCreditCode { get { return ParentCustomer?.CreditCode; } }
        public string ParentCustomerCreditRank { get { return ParentCustomer?.CreditRank; } }
        #endregion

        #region bank account
        public string BankAccountBankName { get { return BankAccount?.BankName; } }
        public string BankAccountBranchName { get { return BankAccount?.BranchName; } }
        public int? BankAccountSectionId { get { return BankAccount?.SectionId; } }
        public int? BankAccountUseValueDate { get { return null; /* BankAccount?.UseValueDate; */ } }
        #endregion

        #region receipt category
        public string ReceiptCategoryCode { get { return ReceiptCategory?.Code; } }
        public string ReceiptCategoryName { get { return ReceiptCategory?.Name; } }
        public int? ReceiptCategoryAccountTitleId { get { return ReceiptCategory?.AccountTitleId; } }
        public string ReceiptCategorySubCode { get { return ReceiptCategory?.SubCode; } }
        public int? ReceiptCategoryUseLimitDate { get { return ReceiptCategory?.UseLimitDate; } }
        public int? ReceiptCategoryUseCashOnDueDates {  get { return ReceiptCategory?.UseCashOnDueDates; } }
        public int? ReceiptCategoryTaxClassId { get { return ReceiptCategory?.TaxClassId; } }
        public string ReceiptCategoryNote { get { return ReceiptCategory?.Note; } }
        public string ReceiptCategoryExternalCode { get { return ReceiptCategory?.ExternalCode; } }
        #endregion

        #region billing category
        public string BillingCategoryCode { get { return BillingCategory?.Code; } }
        public string BillingCategoryName { get { return BillingCategory?.Name; } }
        public int? BillingCategoryAccountTitleId { get { return BillingCategory?.AccountTitleId; } }
        public string BillingCategorySubCode { get { return BillingCategory?.SubCode; } }
        public int? BillingCategoryTaxClassId { get { return BillingCategory?.TaxClassId; } }
        public int? BillingCategoryUseDiscount { get { return BillingCategory?.UseDiscount; } }
        public string BillingCategoryNote { get { return BillingCategory?.Name; } }
        public string BillingCategoryExternalCode { get { return BillingCategory?.Name; } }
        #endregion

        #region collect cateogry
        public string CollectCategoryCode { get { return CollectCategory?.Code; } }
        public string CollectCategoryName {  get { return CollectCategory?.Name; } }
        public int? CollectCategoryUseAccountTransfer {  get { return CollectCategory?.UseAccountTransfer; } }
        public int? CollectCategoryPaymentAgencyId {  get { return CollectCategory?.PaymentAgencyId; } }
        public string CollectCategoryNote {  get { return CollectCategory?.Note; } }
        public string CollectCategoryExternalCode {  get { return CollectCategory?.ExternalCode; } }
        public string OriginalCollectCategoryCode { get { return OriginalCollectCategory?.Code; } }
        #endregion


        #region receipt customer
        public string ReceiptCustomerCode { get { return ReceiptCustomer?.Code; } }
        public string ReceiptCustomerName { get { return ReceiptCustomer?.Name; } }
        public string ReceiptCustomerKana { get { return ReceiptCustomer?.Kana; } }
        public int? ReceiptCustomerStaffId { get { return ReceiptCustomer?.StaffId; } }
        public int? ReceiptCustomerIsParent { get { return ReceiptCustomer?.IsParent; } }
        public string ReceiptCustomerTel { get { return ReceiptCustomer?.Tel; } }
        public string ReceiptCustomerFax { get { return ReceiptCustomer?.Fax; } }
        public string ReceiptCustomerCustomerStaffName { get { return ReceiptCustomer?.CustomerStaffName; } }
        public string ReceiptCustomerNote { get { return ReceiptCustomer?.Note; } }
        public string ReceiptCustomerDensaiCode { get { return ReceiptCustomer?.DensaiCode; } }
        public string ReceiptCustomerCreditCode { get { return ReceiptCustomer?.CreditCode; } }
        public string ReceiptCustomerCreditRank { get { return ReceiptCustomer?.CreditRank; } }
        #endregion

        #region receipt parent customer
        public string ReceiptParentCustomerCode { get { return ReceiptParentCustomer?.Code; } }
        public string ReceiptParentCustomerName { get { return ReceiptParentCustomer?.Name; } }
        public string ReceiptParentCustomerKana { get { return ReceiptParentCustomer?.Kana; } }
        public int? ReceiptParentCustomerStaffId { get { return ReceiptParentCustomer?.StaffId; } }
        public int? ReceiptParentCustomerIsParent { get { return ReceiptParentCustomer?.IsParent; } }
        public string ReceiptParentCustomerTel { get { return ReceiptParentCustomer?.Tel; } }
        public string ReceiptParentCustomerFax { get { return ReceiptParentCustomer?.Fax; } }
        public string ReceiptParentCustomerCustomerStaffName { get { return ReceiptParentCustomer?.CustomerStaffName; } }
        public string ReceiptParentCustomerNote { get { return ReceiptParentCustomer?.Note; } }
        public string ReceiptParentCustomerDensaiCode { get { return ReceiptParentCustomer?.DensaiCode; } }
        public string ReceiptParentCustomerCreditCode { get { return ReceiptParentCustomer?.CreditCode; } }
        public string ReceiptParentCustomerCreditRank { get { return ReceiptParentCustomer?.CreditRank; } }
        #endregion

        #region scheduled receipt
        public DateTime? ScheduledReceiptRecordedAt { get { return ScheduledReceipt?.RecordedAt; } }
        public string ScheduledReceiptPayerCode { get { return ScheduledReceipt?.PayerCode; } }
        public string ScheduledReceiptPayerName { get { return ScheduledReceipt?.PayerName; } }
        public string ScheduledReceiptPayerNameRaw { get { return ScheduledReceipt?.PayerNameRaw; } }
        public decimal? ScheduledReceiptReceiptAmount { get { return ScheduledReceipt?.ReceiptAmount; } }
        public string ScheduledReceiptSourceBankName { get { return ScheduledReceipt?.SourceBankName; } }
        public string ScheduledReceiptSourceBranchName { get { return ScheduledReceipt?.SourceBranchName; } }
        public int? ScheduledReceiptReceiptCategoryId { get { return ScheduledReceipt?.ReceiptCategoryId; } }
        public string ScheduledReceiptNote1 { get { return ScheduledReceipt?.Note1; } }
        public DateTime? ScheduledReceiptDueAt { get { return ScheduledReceipt?.DueAt; } }
        public int? ScheduledReceiptApproved { get { return ScheduledReceipt?.Approved; } }
        public int? ScheduledReceiptInputType { get { return ScheduledReceipt?.InputType; } }
        public int? ScheduledReceiptCustomerId { get { return ScheduledReceipt?.CustomerId; } }
        public string ScheduledReceiptCustomerCode { get { return ScheudledReceiptCustomer?.Code; } }
        public long? ScheduledReceiptOriginalReceiptId { get { return ScheduledReceipt?.OriginalReceiptId; } }
        public int? ScheduledReceiptCreateBy { get { return ScheduledReceipt?.CreateBy; } }
        public int? ScheduledReceiptUpdateBy { get { return ScheduledReceipt?.UpdateBy; } }
        public string ScheduledReceiptReceiptMemo { get { return ScheduledReceiptMemo?.Memo; } }
        public int? ScheduledReceiptSectionId { get { return ScheduledReceipt?.SectionId; } }
        public string ScheduledReceiptNote2 { get { return ScheduledReceipt?.Note2; } }
        public string ScheduledReceiptNote3 { get { return ScheduledReceipt?.Note3; } }
        public string ScheduledReceiptNote4 { get { return ScheduledReceipt?.Note4; } }
        public string ScheduledReceiptBillNumber { get { return ScheduledReceipt?.BillNumber; } }
        public string ScheduledReceiptBillBankCode { get { return ScheduledReceipt?.BillBankCode; } }
        public string ScheduledReceiptBillBranchCode { get { return ScheduledReceipt?.BillBranchCode; } }
        public DateTime? ScheduledReceiptBillDrawAt { get { return ScheduledReceipt?.BillDrawAt; } }
        public string ScheduledReceiptBillDrawer { get { return ScheduledReceipt?.BillDrawer; } }
        public DateTime? ScheduledReceiptProcessingAt { get { return ScheduledReceipt?.ProcessingAt; } }
        public decimal? ScheduledReceiptRemainAmount { get { return ScheduledReceipt?.RemainAmount; } }
        public int? ScheduledReceiptAssignmentFlag { get { return ScheduledReceipt?.AssignmentFlag; } }
        #region scheduled receipt header
        public string ScheduledReceiptHeaderBankCode { get { return ScheduledReceiptHeader?.BankCode; } }
        public string ScheduledReceiptHeaderBankName { get { return ScheduledReceiptHeader?.BankName; } }
        public string ScheduledReceiptHeaderBranchCode { get { return ScheduledReceiptHeader?.BranchCode; } }
        public string ScheduledReceiptHeaderBranchName { get { return ScheduledReceiptHeader?.BranchName; } }
        public int? ScheduledReceiptHeaderAccountTypeId { get { return ScheduledReceiptHeader?.AccountTypeId; } }
        public string ScheduledReceiptHeaderAccountNumber { get { return ScheduledReceiptHeader?.AccountNumber; } }
        public string ScheduledReceiptHeaderAccountName { get { return ScheduledReceiptHeader?.AccountNumber; } }

        #endregion
        #endregion

        #region scheduled receipt category
        public string ScheduledReceiptCategoryCode { get { return ScheduledReceiptCategory?.Code; } }
        public string ScheduledReceiptCategoryName { get { return ScheduledReceiptCategory?.Name; } }
        public int? ScheduledReceiptCategoryAccountTitleId { get { return ScheduledReceiptCategory?.AccountTitleId; } }
        public string ScheduledReceiptCategorySubCode { get { return ScheduledReceiptCategory?.SubCode; } }
        public int? ScheduledReceiptCategoryUseLimitDate { get { return ScheduledReceiptCategory?.UseLimitDate; } }
        public int? ScheduledReceiptCategoryUseCashOnDueDates { get { return ScheduledReceiptCategory?.UseCashOnDueDates; } }
        public int? ScheduledReceiptCategoryTaxClassId { get { return ScheduledReceiptCategory?.TaxClassId; } }
        public string ScheduledReceiptCategoryNote { get { return ScheduledReceiptCategory?.Note; } }
        public string ScheduledReceiptCategoryExternalCode { get { return ScheduledReceiptCategory?.ExternalCode; } }
        #endregion

        #region advance received backup receipt
        public long? AdvanceReceiptId { get { return AdvanceReceipt?.Id; } }
        public DateTime? AdvanceReceiptRecordedAt { get { return AdvanceReceipt?.RecordedAt; } }
        public string AdvanceReceiptPayerCode { get { return AdvanceReceipt?.PayerCode; } }
        public string AdvanceReceiptPayerName { get { return AdvanceReceipt?.PayerName; } }
        public string AdvanceReceiptPayerNameRaw { get { return AdvanceReceipt?.PayerNameRaw; } }
        public decimal? AdvanceReceiptReceiptAmount { get { return AdvanceReceipt?.ReceiptAmount; } }
        public string AdvanceReceiptSourceBankName { get { return AdvanceReceipt?.SourceBankName; } }
        public string AdvanceReceiptSourceBranchName { get { return AdvanceReceipt?.SourceBranchName; } }
        public int? AdvanceReceiptReceiptCategoryId { get { return AdvanceReceipt?.ReceiptCategoryId; } }
        public string AdvanceReceiptNote1 { get { return AdvanceReceipt?.Note1; } }
        public DateTime? AdvanceReceiptDueAt { get { return AdvanceReceipt?.DueAt; } }
        public int? AdvanceReceiptApproved { get { return AdvanceReceipt?.Approved; } }
        public int? AdvanceReceiptInputType { get { return AdvanceReceipt?.InputType; } }
        public int? AdvanceReceiptCustomerId { get { return AdvanceReceipt?.CustomerId; } }
        public long? AdvanceReceiptOriginalReceiptId { get { return AdvanceReceipt?.OriginalReceiptId; } }
        public int? AdvanceReceiptCreateBy { get { return AdvanceReceipt?.CreateBy; } }
        public string AdvanceReceiptMemo { get { return AdvanceReceipt?.Memo; } } // rename
        public int? AdvanceReceiptSectionId { get { return AdvanceReceipt?.SectionId; } }
        public string AdvanceReceiptNote2 { get { return AdvanceReceipt?.Note2; } }
        public string AdvanceReceiptNote3 { get { return AdvanceReceipt?.Note3; } }
        public string AdvanceReceiptNote4 { get { return AdvanceReceipt?.Note4; } }
        public string AdvanceReceiptBillNumber { get { return AdvanceReceipt?.BillNumber; } }
        public string AdvanceReceiptBillBankCode { get { return AdvanceReceipt?.BillBankCode; } }
        public string AdvanceReceiptBillBranchCode { get { return AdvanceReceipt?.BillBranchCode; } }
        public DateTime? AdvanceReceiptBillDrawAt { get { return AdvanceReceipt?.BillDrawAt; } }
        public string AdvanceReceiptBillDrawer { get { return AdvanceReceipt?.BillDrawer; } }
        //public DateTime? AdvanceReceiptProcessingAt { get { return AdvanceReceipt?.ProcessingAt; } }
        public decimal? AdvanceReceiptRemainAmount { get { return AdvanceReceipt?.RemainAmount; } }
        public int? AdvanceReceiptAssignmentFlag { get { return AdvanceReceipt?.AssignmentFlag; } }
        #region scheduled receipt header
        public string AdvanceReceiptHeaderBankCode { get { return AdvanceReceivedHeader?.BankCode; } }
        public string AdvanceReceiptHeaderBankName { get { return AdvanceReceivedHeader?.BankName; } }
        public string AdvanceReceiptHeaderBranchCode { get { return AdvanceReceivedHeader?.BranchCode; } }
        public string AdvanceReceiptHeaderBranchName { get { return AdvanceReceivedHeader?.BranchName; } }
        public int? AdvanceReceiptHeaderAccountTypeId { get { return AdvanceReceivedHeader?.AccountTypeId; } }
        public string AdvanceReceiptHeaderAccountNumber { get { return AdvanceReceivedHeader?.AccountNumber; } }
        public string AdvanceReceiptHeaderAccountName { get { return AdvanceReceivedHeader?.AccountNumber; } }
        #endregion
        #endregion

        #region receipt customer
        public string AdvanceReceiptCustomerCode { get { return AdvanceReceivedCustomer?.Code; } }
        public string AdvanceReceiptCustomerName { get { return AdvanceReceivedCustomer?.Name; } }
        public string AdvanceReceiptCustomerKana { get { return AdvanceReceivedCustomer?.Kana; } }
        public int? AdvanceReceiptCustomerStaffId { get { return AdvanceReceivedCustomer?.StaffId; } }
        public int? AdvanceReceiptCustomerIsParent { get { return AdvanceReceivedCustomer?.IsParent; } }
        public string AdvanceReceiptCustomerTel { get { return AdvanceReceivedCustomer?.Tel; } }
        public string AdvanceReceiptCustomerFax { get { return AdvanceReceivedCustomer?.Fax; } }
        public string AdvanceReceiptCustomerCustomerStaffName { get { return AdvanceReceivedCustomer?.CustomerStaffName; } }
        public string AdvanceReceiptCustomerNote { get { return AdvanceReceivedCustomer?.Note; } }
        public string AdvanceReceiptCustomerDensaiCode { get { return AdvanceReceivedCustomer?.DensaiCode; } }
        public string AdvanceReceiptCustomerCreditCode { get { return AdvanceReceivedCustomer?.CreditCode; } }
        public string AdvanceReceiptCustomerCreditRank { get { return AdvanceReceivedCustomer?.CreditRank; } }
        #endregion

        #region receipt parent customer
        public string AdvanceReceiptParentCustomerCode { get { return AdvanceReceivedParentCustomer?.Code; } }
        public string AdvanceReceiptParentCustomerName { get { return AdvanceReceivedParentCustomer?.Name; } }
        public string AdvanceReceiptParentCustomerKana { get { return AdvanceReceivedParentCustomer?.Kana; } }
        public int? AdvanceReceiptParentCustomerStaffId { get { return AdvanceReceivedParentCustomer?.StaffId; } }
        public int? AdvanceReceiptParentCustomerIsParent { get { return AdvanceReceivedParentCustomer?.IsParent; } }
        public string AdvanceReceiptParentCustomerTel { get { return AdvanceReceivedParentCustomer?.Tel; } }
        public string AdvanceReceiptParentCustomerFax { get { return AdvanceReceivedParentCustomer?.Fax; } }
        public string AdvanceReceiptParentCustomerCustomerStaffName { get { return AdvanceReceivedParentCustomer?.CustomerStaffName; } }
        public string AdvanceReceiptParentCustomerNote { get { return AdvanceReceivedParentCustomer?.Note; } }
        public string AdvanceReceiptParentCustomerDensaiCode { get { return AdvanceReceivedParentCustomer?.DensaiCode; } }
        public string AdvanceReceiptParentCustomerCreditCode { get { return AdvanceReceivedParentCustomer?.CreditCode; } }
        public string AdvanceReceiptParentCustomerCreditRank { get { return AdvanceReceivedParentCustomer?.CreditRank; } }
        #endregion

        #region advance received receipt category
        public string AdvanceReceiptCategoryCode { get { return AdvanceReceiptCategory?.Code; } }
        public string AdvanceReceiptCategoryName { get { return AdvanceReceiptCategory?.Name; } }
        public int? AdvanceReceiptCategoryAccountTitleId { get { return AdvanceReceiptCategory?.AccountTitleId; } }
        public string AdvanceReceiptCategorySubCode { get { return AdvanceReceiptCategory?.SubCode; } }
        public int? AdvanceReceiptCategoryUseLimitDate { get { return AdvanceReceiptCategory?.UseLimitDate; } }
        public int? AdvanceReceiptCategoryUseCashOnDueDates { get { return AdvanceReceiptCategory?.UseCashOnDueDates; } }
        public int? AdvanceReceiptCategoryTaxClassId { get { return AdvanceReceiptCategory?.TaxClassId; } }
        public string AdvanceReceiptCategoryNote { get { return AdvanceReceiptCategory?.Note; } }
        public string AdvanceReceiptCategoryExternalCode { get { return AdvanceReceiptCategory?.ExternalCode; } }
        #endregion

        #region receipt exclude / exclude category
        public decimal? ReceiptExcludeAmount { get { return ReceiptExclude?.ExcludeAmount; } }
        public int? ReceiptExcludeCategoryId { get { return ReceiptExclude?.ExcludeCategoryId; } }
        public int? ReceiptExcludeCreateBy { get { return ReceiptExclude?.CreateBy; } }
        public int? ReceiptExcludeUpdateBy { get { return ReceiptExclude?.UpdateBy; } }
        public string ReceiptExcludeCategoryCode { get { return ReceiptExcludeCategory?.Code; } }
        public string ReceiptExcludeCategoryName { get { return ReceiptExcludeCategory?.Name; } }
        public int? ReceiptExcludeCategoryAccountTitleId { get { return ReceiptExcludeCategory?.AccountTitleId; } }
        public string ReceiptExcludeCategorySubCode { get { return ReceiptExcludeCategory?.SubCode; } }
        public int? ReceiptExcludeCategoryTaxClassId { get { return ReceiptExcludeCategory?.TaxClassId; } }
        public string ReceiptExcludeCategoryNote { get { return ReceiptExcludeCategory?.Note; } }
        #endregion

        #region general settings
        public string SuspentReceiptDepartmentCode { get { return SuspentReceiptDepartment?.Code; } }
        public string SuspentReceiptDepartmentName { get { return SuspentReceiptDepartment?.Name; } }
        public string SuspentReceiptDepartmentNote { get { return SuspentReceiptDepartment?.Note; } }
        public string SuspentReceiptAccountTitleCode { get { return SuspentReceiptAccountTitle?.Code; } }
        public string SuspentReceiptAccountTitleName { get { return SuspentReceiptAccountTitle?.Name; } }
        public string SuspentReceiptAccountTitleContraCode { get { return SuspentReceiptAccountTitle?.ContraAccountCode; } }
        public string SuspentReceiptAccountTitleContraName { get { return SuspentReceiptAccountTitle?.ContraAccountName; } }
        public string SuspentReceiptAccountTitleContraSubCode { get { return SuspentReceiptAccountTitle?.ContraAccountSubCode; } }
        public string SuspentReceiptSubCode { get { return GetGeneralSettingValue(SuspentReceiptSubCodeKey); } }
        public string TransferFeeDepartmentCode { get { return TransferFeeDepartment?.Code; } }
        public string TransferFeeDepartmentName { get { return TransferFeeDepartment?.Name; } }
        public string TransferFeeDepartmentNote { get { return TransferFeeDepartment?.Note; } }
        public string TransferFeeAccountTitleCode { get { return TransferFeeAccountTitle?.Code; } }
        public string TransferFeeAccountTitleName { get { return TransferFeeAccountTitle?.Name; } }
        public string TransferFeeAccountTitleContraCode { get { return TransferFeeAccountTitle?.ContraAccountCode; } }
        public string TransferFeeAccountTitleContraName { get { return TransferFeeAccountTitle?.ContraAccountName; } }
        public string TransferFeeAccountTitleContraSubCode { get { return TransferFeeAccountTitle?.ContraAccountSubCode; } }
        public string TransferFeeSubCode { get { return GetGeneralSettingValue(TransferFeeSubCodeKey); } }
        public string DebitTaxDiffDepartmentCode { get { return DebitTaxDiffDepartment?.Code; } }
        public string DebitTaxDiffDepartmentName { get { return DebitTaxDiffDepartment?.Name; } }
        public string DebitTaxDiffDepartmentNote { get { return DebitTaxDiffDepartment?.Note; } }
        public string DebitTaxDiffAccountTitleCode { get { return DebitTaxDiffAccountTitle?.Code; } }
        public string DebitTaxDiffAccountTitleName { get { return DebitTaxDiffAccountTitle?.Name; } }
        public string DebitTaxDiffAccountTitleContraCode { get { return DebitTaxDiffAccountTitle?.ContraAccountCode; } }
        public string DebitTaxDiffAccountTitleContraName { get { return DebitTaxDiffAccountTitle?.ContraAccountName; } }
        public string DebitTaxDiffAccountTitleContraSubCode { get { return DebitTaxDiffAccountTitle?.ContraAccountSubCode; } }
        public string DebitTaxDiffSubCode { get { return GetGeneralSettingValue(DebitTaxDiffSubCodeKey); } }
        public string CreditTaxDiffDepartmentCode { get { return CreditTaxDiffDepartment?.Code; } }
        public string CreditTaxDiffDepartmentName { get { return CreditTaxDiffDepartment?.Name; } }
        public string CreditTaxDiffDepartmentNote { get { return CreditTaxDiffDepartment?.Note; } }
        public string CreditTaxDiffAccountTitleCode { get { return CreditTaxDiffAccountTitle?.Code; } }
        public string CreditTaxDiffAccountTitleName { get { return CreditTaxDiffAccountTitle?.Name; } }
        public string CreditTaxDiffAccountTitleContraCode { get { return CreditTaxDiffAccountTitle?.ContraAccountCode; } }
        public string CreditTaxDiffAccountTitleContraName { get { return CreditTaxDiffAccountTitle?.ContraAccountName; } }
        public string CreditTaxDiffAccountTitleContraSubCode { get { return CreditTaxDiffAccountTitle?.ContraAccountSubCode; } }
        public string CreditTaxDiffSubCode { get { return GetGeneralSettingValue(CreditTaxDiffSubCodeKey); } }
        public string ReceiptDepartmentCode { get { return ReceiptDepartment?.Code; } }
        public string ReceiptDepartmentName { get { return ReceiptDepartment?.Name; } }
        public string ReceiptDepartmentNote { get { return ReceiptDepartment?.Note; } }
        public string NewTaxRate1Value { get { return GetGeneralSettingValue(NewTaxRate1Key); } }
        public string NewTaxRate2Value { get { return GetGeneralSettingValue(NewTaxRate2Key); } }
        public string NewTaxRate3Value { get { return GetGeneralSettingValue(NewTaxRate3Key); } }
        public string NewTaxRate1ApplyDateValue { get { return GetGeneralSettingValue(NewTaxRate1ApplyDateKey); } }
        public string NewTaxRate2ApplyDateValue { get { return GetGeneralSettingValue(NewTaxRate2ApplyDateKey); } }
        public string NewTaxRate3ApplyDateValue { get { return GetGeneralSettingValue(NewTaxRate3ApplyDateKey); } }
        public string TaxRoundingModeValue { get { return GetGeneralSettingValue(TaxRoundingModeKey); } }
        #endregion

        /// <summary>blank 出力項目</summary>
        public string Dummy { get; set; } = string.Empty;
        public IEnumerable<int> GetCustomerIds()
        {
            if (ReceiptCustomerId.HasValue) yield return ReceiptCustomerId.Value;
            if (ReceiptParentCustomerId.HasValue) yield return ReceiptParentCustomerId.Value;
            if (BillingCustomerId.HasValue) yield return BillingCustomerId.Value;
            if (BillingParentCustomerId.HasValue) yield return BillingParentCustomerId.Value;
            if (AdvanceReceivedCustomerId.HasValue) yield return AdvanceReceivedCustomerId.Value;
            if (AdvanceReceivedParentCustomerId.HasValue) yield return AdvanceReceivedParentCustomerId.Value;
        }

        #region get ids next set transaction data

        public IEnumerable<int> GetCategoryIds()
        {
            if (ReceiptReceiptCategoryId.HasValue) yield return ReceiptReceiptCategoryId.Value;
            if (BillingBillingCategoryId.HasValue) yield return BillingBillingCategoryId.Value;
            if (BillingCollectCategoryId.HasValue) yield return BillingCollectCategoryId.Value;
            if (BillingOriginalCollectCategoryId.HasValue) yield return BillingOriginalCollectCategoryId.Value;
            if (ScheduledReceiptReceiptCategoryId.HasValue) yield return ScheduledReceiptReceiptCategoryId.Value;
            if (AdvanceReceiptReceiptCategoryId.HasValue) yield return AdvanceReceiptReceiptCategoryId.Value;
            if (ReceiptExcludeCategoryId.HasValue) yield return ReceiptExcludeCategoryId.Value;
        }
        public  void SetCategory(Dictionary<int, Category> dic)
        {
            if (ReceiptReceiptCategoryId.HasValue && dic.ContainsKey(ReceiptReceiptCategoryId.Value)) ReceiptCategory = dic[ReceiptReceiptCategoryId.Value];
            if (BillingBillingCategoryId.HasValue && dic.ContainsKey(BillingBillingCategoryId.Value)) BillingCategory = dic[BillingBillingCategoryId.Value];
            if (BillingCollectCategoryId.HasValue && dic.ContainsKey(BillingCollectCategoryId.Value)) CollectCategory = dic[BillingCollectCategoryId.Value];
            if (BillingOriginalCollectCategoryId.HasValue && dic.ContainsKey(BillingOriginalCollectCategoryId.Value)) OriginalCollectCategory = dic[BillingOriginalCollectCategoryId.Value];
            if (ScheduledReceiptReceiptCategoryId.HasValue && dic.ContainsKey(ScheduledReceiptReceiptCategoryId.Value)) ScheduledReceiptCategory = dic[ScheduledReceiptReceiptCategoryId.Value];
            if (AdvanceReceiptReceiptCategoryId.HasValue && dic.ContainsKey(AdvanceReceiptReceiptCategoryId.Value)) AdvanceReceiptCategory = dic[AdvanceReceiptReceiptCategoryId.Value];
            if (ReceiptExcludeCategoryId.HasValue && dic.ContainsKey(ReceiptExcludeCategoryId.Value)) ReceiptExcludeCategory = dic[ReceiptExcludeCategoryId.Value];
        }
        public IEnumerable<int> GetStaffIds()
        {
            if (BillingStaffId.HasValue) yield return BillingStaffId.Value;
            if (BillingDepartmentStaffId.HasValue) yield return BillingDepartmentStaffId.Value;
        }
        public IEnumerable<int> GetDepartmentIds()
        {
            if (BillingDepartmentId.HasValue) yield return BillingDepartmentId.Value;
            if (BillingStaffDepartmentId.HasValue) yield return BillingStaffDepartmentId.Value;
        }
        public IEnumerable<int> GetLoginUserIds()
        {
            if (MatchingCreateBy.HasValue) yield return MatchingCreateBy.Value;
            if (MatchingUpdateBy.HasValue) yield return MatchingUpdateBy.Value;
            if (ReceiptCreateBy.HasValue) yield return ReceiptCreateBy.Value;
            if (ReceiptUpdateBy.HasValue) yield return ReceiptUpdateBy.Value;
            if (AdvanceReceiptCreateBy.HasValue) yield return AdvanceReceiptCreateBy.Value;
            if (ReceiptExcludeCreateBy.HasValue) yield return ReceiptExcludeCreateBy.Value;
            if (ReceiptExcludeUpdateBy.HasValue) yield return ReceiptExcludeUpdateBy.Value;
        }
        public IEnumerable<long> GetReceiptHeaderIds()
        {
            if (ReceiptHeaderId.HasValue) yield return ReceiptHeaderId.Value;
            if (ScheduledReceiptHeaderId.HasValue) yield return ScheduledReceiptHeaderId.Value;
            if (AdvanceReceivedHeaderId.HasValue) yield return AdvanceReceivedHeaderId.Value;
        }
        /// <summary>科目ID 取得
        /// AccountReceivableAccountTitleId は 未出力
        /// 各種 Category が 設定された後に値を返すようになる
        /// </summary>
        /// <returns></returns>

        public IEnumerable<int> GetAccountTitleIds() // last category set after
        {
            if (BillingDebitAccountTitleId.HasValue) yield return BillingDebitAccountTitleId.Value;
            if (BillingCreditAccountTitleId.HasValue) yield return BillingCreditAccountTitleId.Value;
            if (ReceiptCategoryAccountTitleId.HasValue) yield return ReceiptCategoryAccountTitleId.Value;
            if (BillingCategoryAccountTitleId.HasValue) yield return BillingCategoryAccountTitleId.Value;
            if (ScheduledReceiptCategoryAccountTitleId.HasValue) yield return ScheduledReceiptCategoryAccountTitleId.Value;
            if (ReceiptExcludeCategoryAccountTitleId.HasValue) yield return ReceiptExcludeCategoryAccountTitleId.Value;
        }
        /// <summary>入金部門ID 取得 ReceiptHeader -> BankAccount -> の後に取得する必要がある</summary>
        /// <returns></returns>
        public IEnumerable<int> GetSectionIds()
        {
            if (ReceiptSectionId.HasValue) yield return ReceiptSectionId.Value;
            if (ScheduledReceiptSectionId.HasValue) yield return ScheduledReceiptSectionId.Value;
            if (BankAccountSectionId.HasValue) yield return BankAccountSectionId.Value;
        }

        private bool ValidBankAccount()
            => !string.IsNullOrEmpty(ReceiptBankCode)
                && !string.IsNullOrEmpty(ReceiptBranchCode)
                && ReceiptAccountTypeId.HasValue
                && !string.IsNullOrEmpty(ReceiptAccountNumber);
        private Tuple<string, string, int, string> CreateBankAccountKey()
            => Tuple.Create(
                    ReceiptBankCode,
                    ReceiptBranchCode,
                    ReceiptAccountTypeId.Value,
                    ReceiptAccountNumber);
        public void SetBankAccount(Dictionary<Tuple<string,string,int, string>, BankAccount> dicAccount)
        {
            if (!ValidBankAccount()) return;
            var key = CreateBankAccountKey();
            if (!dicAccount.ContainsKey(key)) return;
            BankAccount = dicAccount[key];
        }

        #endregion
    }

    [DataContract] public class GeneralJournalizingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<GeneralJournalizing> GeneralJournalizings { get; set; } = new List<GeneralJournalizing>();

        public Dictionary<int, AccountTitle> AccountTitleDictionary { get; set; } = new Dictionary<int, AccountTitle>();
        public Dictionary<int, Department> DepartmentDictionary { get; set; } = new Dictionary<int, Department>();
        public Dictionary<int, Web.Models.Section> SectionDictionary { get; set; } = new Dictionary<int, Section>();
        public Dictionary<int, Staff> StaffDictionary { get; set; } = new Dictionary<int, Staff>();
        public Dictionary<int, LoginUser> LoginUserDictionary { get; set; } = new Dictionary<int, LoginUser>();
        public Dictionary<int, PaymentAgency> PaymentAgencyDictionary { get; set; } = new Dictionary<int, PaymentAgency>();

    }

}
