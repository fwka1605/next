using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Models
{
    [Serializable]
    [DataContract]
    public class Receipt : ITransactionData, ITransactional
    {
        [DataMember] public decimal Amount { get; set; }
        [DataMember] public long Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public string PrivateBankCode { get; set; }
        [DataMember] public long? ReceiptHeaderId { get; set; }
        [DataMember] public int ReceiptCategoryId { get; set; }
        [DataMember] public int? CustomerId { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public int InputType { get; set; }
        [DataMember] public int Apportioned { get; set; }
        [DataMember] public int Approved { get; set; }
        [DataMember] public DateTime Workday { get; set; }
        [DataMember] public DateTime RecordedAt { get; set; }
        [DataMember] public DateTime? OriginalRecordedAt { get; set; }
        [DataMember] public decimal ReceiptAmount { get; set; }
        [DataMember] public decimal AssignmentAmount { get; set; }
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public int AssignmentFlag { get; set; }
        [DataMember] public string PayerCode { get; set; } = string.Empty;
        [DataMember] public string PayerName { get; set; } = string.Empty;
        [DataMember] public string PayerNameRaw { get; set; } = string.Empty;
        [DataMember] public string SourceBankName { get; set; } = string.Empty;
        [DataMember] public string SourceBranchName { get; set; } = string.Empty;
        [DataMember] public DateTime? OutputAt { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public DateTime? MailedAt { get; set; }
        [DataMember] public long? OriginalReceiptId { get; set; }
        [DataMember] public int ExcludeFlag { get; set; }
        [DataMember] public int? ExcludeCategoryId { get; set; }
        [DataMember] public decimal ExcludeAmount { get; set; }
        [DataMember] public string ReferenceNumber { get; set; } = string.Empty;
        [DataMember] public string RecordNumber { get; set; } = string.Empty;
        [DataMember] public DateTime? DensaiRegisterAt { get; set; }
        [DataMember] public string Note1 { get; set; } = string.Empty;
        [DataMember] public string Note2 { get; set; } = string.Empty;
        [DataMember] public string Note3 { get; set; } = string.Empty;
        [DataMember] public string Note4 { get; set; } = string.Empty;
        [DataMember] public string BillNumber { get; set; } = string.Empty;
        [DataMember] public string BillBankCode { get; set; } = string.Empty;
        [DataMember] public string BillBranchCode { get; set; } = string.Empty;
        [DataMember] public DateTime? BillDrawAt { get; set; }
        [DataMember] public string BillDrawer { get; set; } = string.Empty;
        [DataMember] public DateTime? DeleteAt { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public DateTime? ProcessingAt { get; set; }
        [DataMember] public int? StaffId { get; set; }
        [DataMember] public string CollationKey { get; set; } = string.Empty;
        [DataMember] public string BankCode { get; set; } = string.Empty;
        [DataMember] public string BankName { get; set; } = string.Empty;
        [DataMember] public string BranchCode { get; set; } = string.Empty;
        [DataMember] public string BranchName { get; set; } = string.Empty;
        [DataMember] public int? AccountTypeId { get; set; }
        [DataMember] public string AccountNumber { get; set; } = string.Empty;
        [DataMember] public string AccountName { get; set; } = string.Empty;

        //other table fields
        [DataMember] public string CategoryCode { get; set; }
        [DataMember] public string CategoryName { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string ExcludeCategoryCode { get; set; }
        [DataMember] public string ExcludeCategoryName { get; set; }
        [DataMember] public string ExcludeCategoryCodeName { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string SectionCode { get; set; }
        [DataMember] public string SectionName { get; set; }
        [DataMember] public int UseAdvanceReceived { get; set; }
        [DataMember] public int UseForeignCurrency { get; set; }
        [DataMember] public string ReceiptMemo { get; set; }
        [DataMember] public string TransferMemo { get; set; }
        [DataMember] public int UseFeeTolerance { get; set; }
        [DataMember] public int CustomerFee { get; set; }
        [DataMember] public int GeneralFee { get; set; }
        [DataMember] public DateTime OriginalUpdateAt { get; set; }
        [DataMember] public decimal BankTransferFee { get; set; }
        [DataMember] public long? NettingId { get; set; }
        [DataMember] public int RecExcOutputAt { get; set; }
        [DataMember] public int UseCashOnDueDates { get; set; }
        [DataMember] public string AccountTypeName { get; set; }

        [DataMember] public bool CancelFlag { get; set; }
        [DataMember]public int ReceiptStatusFlag { get; set; }
        [DataMember]public int RemainAmountFlag { get; set; }

        #region PD0501 Receipt Search
        public string InputTypeName
        {
            get
            {
                return InputType == 1 ? "EB取込"
                     : InputType == 2 ? "入力"
                     : InputType == 3 ? "インポーター取込" : "電債取込";
            }
        }

        public string AssignmentFlagName
        {
            get
            {
                return AssignmentFlag == 0 ? "未消込"
                     : AssignmentFlag == 1 ? "一部消込" : "消込済";
            }
        }

        public bool IsModified
        {
            get
            {
                return !ExcludeFlag.Equals(ExcludeFlagBuffer)
                    || !ExcludeCategoryId.Equals(ExcludeCategoryIdBuffer);
            }
        }

        public bool CanEditable
        {
            get
            {
                return RemainAmount + ExcludeAmount != 0
                    && RecExcOutputAt == 0;
            }
        }

        /// <summary>消込用 共通のUpdateAt を設定する用途 既存の UpdateAt は元の値のまま</summary>
        public DateTime NewUpdateAt { get; set; }

        [DataMember] public int ExcludeFlagBuffer { get; set; }
        [DataMember] public int? ExcludeCategoryIdBuffer { get; set; }
        [DataMember] public string CustomerCodeBuffer { get; set; }
        #endregion

        #region PE0102 matching individual
        public bool Checked { get; set; }

        #endregion

        #region PD0601 ReceiptAdvanceReceived
        public string ReceiptStatusDisp
        { 
            get
            {
                return ReceiptStatusFlag == (int)ReceiptStatus.Deleted ? "元の入金データが削除されています。"
                     : ReceiptStatusFlag == (int)ReceiptStatus.Journalized ? "仕訳出力済みのデータです。"
                     : ReceiptStatusFlag == (int)ReceiptStatus.PartOrFullAssigned ? "一部消込、もしくは消込済みのデータです。"
                     : ReceiptStatusFlag == (int)ReceiptStatus.AdvancedReceived ? "前受振替・分割が行われています。"
                     : ReceiptStatusFlag == (int)ReceiptStatus.SectionTransfered ? "入金部門振替が行われています。" : "";
            }
        }
        #endregion

        public string CategoryCodeName
        {
            get
            {
                return !string.IsNullOrEmpty(CategoryCode) && !string.IsNullOrEmpty(CategoryName)
                    ? $"{CategoryCode}:{CategoryName}" : string.Empty;
            }
        }
        public string ExcludeCategoryCodeAndName
        { get { return !string.IsNullOrEmpty(ExcludeCategoryCode) ? $"{ExcludeCategoryCode}:{ExcludeCategoryName}" : string.Empty; } }
        public string InputTypeCodeName { get { return $"{InputType}:{InputTypeName}"; } }

        /// <summary>仕向銀行・支店情報 
        /// 個別消込 表示用 仕向銀行/仕向支店 が設定されていれば、間に/（スラッシュ）含めて表示する</summary>

        public string SourceBank
        {
            get
            {
                return !string.IsNullOrEmpty(SourceBankName) && !string.IsNullOrEmpty(SourceBranchName) ? $"{SourceBankName}/{SourceBranchName}"
                     : !string.IsNullOrEmpty(SourceBankName) ? SourceBankName
                     : !string.IsNullOrEmpty(SourceBranchName) ? SourceBranchName
                     : string.Empty;
            }
        }


        public string PayerCodePrefix
        {
            get
            {
                return !string.IsNullOrEmpty(PayerCode) && 3 <= PayerCode.Length
                    ? PayerCode.Substring(0, 3) : string.Empty;
            }
        }
        public string PayerCodeSuffix
        {
            get
            {
                return !string.IsNullOrEmpty(PayerCode) && 3 < PayerCode.Length
                ? PayerCode.Substring(3) : string.Empty;
            }
        }

        public string NettingState { get { return NettingId.HasValue ? "*" : ""; } }

        public AdvanceReceivedBackup ConvertToAdvanceReceivedBackup(Func<long, string> memoSetter = null)
            => new AdvanceReceivedBackup {
                Id                  = Id,
                CompanyId           = CompanyId,
                CurrencyId          = CurrencyId,
                ReceiptHeaderId     = ReceiptHeaderId,
                ReceiptCategoryId   = ReceiptCategoryId,
                CustomerId          = CustomerId,
                SectionId           = SectionId,
                InputType           = InputType,
                Apportioned         = Apportioned,
                Approved            = Approved,
                Workday             = Workday,
                RecordedAt          = RecordedAt,
                OriginalRecordedAt  = OriginalRecordedAt,
                ReceiptAmount       = ReceiptAmount,
                AssignmentAmount    = AssignmentAmount,
                RemainAmount        = RemainAmount,
                AssignmentFlag      = AssignmentFlag,
                PayerCode           = PayerCode,
                PayerName           = PayerName,
                PayerNameRaw        = PayerNameRaw,
                SourceBankName      = SourceBankName,
                SourceBranchName    = SourceBranchName,
                OutputAt            = OutputAt,
                DueAt               = DueAt,
                MailedAt            = MailedAt,
                OriginalReceiptId   = OriginalReceiptId,
                ExcludeFlag         = ExcludeFlag,
                ExcludeCategoryId   = ExcludeCategoryId,
                ExcludeAmount       = ExcludeAmount,
                ReferenceNumber     = ReferenceNumber,
                RecordNumber        = RecordNumber,
                DensaiRegisterAt    = DensaiRegisterAt,
                Note1               = Note1,
                Note2               = Note2,
                Note3               = Note3,
                Note4               = Note4,
                BillNumber          = BillNumber,
                BillBankCode        = BillBankCode,
                BillBranchCode      = BillBranchCode,
                BillDrawAt          = BillDrawAt,
                BillDrawer          = BillDrawer,
                DeleteAt            = DeleteAt,
                CreateBy            = CreateBy,
                CreateAt            = CreateAt,
                Memo                = memoSetter?.Invoke(Id) ?? string.Empty,
                TransferOutputAt    = null,
                StaffId             = StaffId,
                CollationKey        = CollationKey,
                BankCode            = BankCode,
                BankName            = BankName,
                BranchCode          = BranchCode,
                BranchName          = BranchName,
                AccountTypeId       = AccountTypeId,
                AccountNumber       = AccountNumber,
                AccountName         = AccountName,
            };
    }

    [DataContract]
    public class ReceiptResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Receipt[] Receipt { get; set; }
    }

    [DataContract]
    public class ReceiptsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Receipt> Receipts { get; set; }
    }

    [DataContract]
    public class AdvanceReceiptsResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public Receipt OriginalReceipt { get; set; }
        [DataMember]
        public List<Receipt> AdvanceReceipts { get; set; }
    }

    [DataContract]
    public class VoidResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
    }
}
