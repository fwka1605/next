using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Billing : ITransactionData, ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public int StaffId { get; set; }
        [DataMember] public int BillingCategoryId { get; set; }
        [DataMember] public int InputType { get; set; }
        [DataMember] public int BillingInputTypeId { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public DateTime ClosingAt { get; set; }
        [DataMember] public DateTime SalesAt { get; set; }
        [DataMember] public DateTime DueAt { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public decimal TaxAmount { get; set; }
        [DataMember] public decimal AssignmentAmount { get; set; }
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public decimal TargetAmount { get; set; }
        [DataMember] public decimal OffsetAmount { get; set; }
        [DataMember] public int AssignmentFlag { get; set; }
        [DataMember] public int Approved { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public int? OriginalCollectCategoryId { get; set; }
        [DataMember] public int? DebitAccountTitleId { get; set; }
        [DataMember] public int? CreditAccountTitleId { get; set; }
        [DataMember] public DateTime? OriginalDueAt { get; set; }
        [DataMember] public DateTime? OutputAt { get; set; }
        [DataMember] public DateTime? PublishAt { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public int TaxClassId { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; }
        [DataMember] public string Note4 { get; set; }
        [DataMember] public string Note5 { get; set; } = string.Empty;
        [DataMember] public string Note6 { get; set; } = string.Empty;
        [DataMember] public string Note7 { get; set; } = string.Empty;
        [DataMember] public string Note8 { get; set; } = string.Empty;
        [DataMember] public DateTime? DeleteAt { get; set; }
        [DataMember] public DateTime? RequestDate { get; set; }
        [DataMember] public int? ResultCode { get; set; }
        [DataMember] public DateTime? TransferOriginalDueAt { get; set; }
        [DataMember] public string ScheduledPaymentKey { get; set; }
        [DataMember] public decimal? Quantity { get; set; }
        [DataMember] public decimal? UnitPrice { get; set; }
        [DataMember] public string UnitSymbol { get; set; }
        [DataMember] public decimal? Price { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public long? BillingInputId { get; set; }
        [DataMember] public DateTime BillingDueAt { get; set; }
        [DataMember] public int? DestinationId { get; set; }

        //Other table fields
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string CustomerKana { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string Memo { get; set; }
        [DataMember] public string BillingCategoryCode { get; set; }
        [DataMember] public string BillingCategoryName { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }
        [DataMember] public string LoginUserCode { get; set; }
        [DataMember] public string LoginUserName { get; set; }
        [DataMember] public string ContractNumber { get; set; }
        [DataMember] public int Confirm { get; set; }
        /// <summary>BillingDiscount の登録があるかどうかの判別用 下にある DiscountAmout != 0M で判別できるので不要 mu-no-</summary>
        [DataMember] public long? BillingDiscountId { get; set; }
        [DataMember] public decimal DiscountAmount { get; set; }
        [DataMember] public decimal DiscountAmount1 { get; set; }
        [DataMember] public decimal DiscountAmount2 { get; set; }
        [DataMember] public decimal DiscountAmount3 { get; set; }
        [DataMember] public decimal DiscountAmount4 { get; set; }
        [DataMember] public decimal DiscountAmount5 { get; set; }
        [DataMember] public int BillingId { get; set; }
        [DataMember] public int ParentCustomerId { get; set; }
        [DataMember] public string ParentCustomerCode { get; set; }
        [DataMember] public string ParentCustomerName { get; set; }
        [DataMember] public string CompanyCode { get; set; }
        [DataMember] public string AccountTitleCode { get; set; }

        [DataMember] public decimal PaymentAmount { get; set; }
        [DataMember] public string CategoryCodeAndName { get; set; }
        [DataMember] public string OrgCategoryCodeAndName { get; set; }

        [DataMember] public DateTime? ModifiedDueAt { get; set; }

        [DataMember] public DateTime? FirstRecordedAt { get; set; }
        [DataMember] public DateTime? LastRecordedAt { get; set; }
        [DataMember] public DateTime? BillingInputPublishAt { get; set; }

        // PE0102 個別消込
        [DataMember] public decimal BankTransferFee { get; set; }
        [DataMember] public decimal TaxDifference { get; set; }
        public decimal TargetAmountBuffer { get; set; }
        /// <summary>
        ///  請求データに紐づく得意先の債権代表者フラグ
        /// </summary>
        [DataMember] public int IsParent { get; set; }

        //PC0201
        /// <summary>？？？？</summary>
        [DataMember] public int BillingDivisionContract { get; set; }
        /// <summary>請求データ入力 歩引計算 を行うかどうか</summary>
        [DataMember] public bool UseDiscount { get; set; }
        [DataMember] public int CurrencyPrecision { get; set; }
        [DataMember] public int UseLongTermAdvancedReceived { get; set; }
        [DataMember] public long AccountTransferLogId { get; set; }

        //PC1701定期請求データ登録
        [DataMember] public long PeriodicBillingSettingId { get; set; }
        [DataMember] public int DisplayOrder { get; set; }

        /// <summary>画面用 個別消込/未消込請求データ削除 の選択有無判定用</summary>
        public bool Checked { get; set; }

        /// <summary>消込用 共通のUpdateAt を設定する用途 既存の UpdateAt は元の値のまま</summary>
        public DateTime NewUpdateAt { get; set; }

        /// <summary>請求データ入力用 メモ/備考2..8 いずれかに入力があれば true</summary>
        public bool IsAnyNoteInputted =>
               !string.IsNullOrEmpty(Memo)
            || !string.IsNullOrEmpty(Note2)
            || !string.IsNullOrEmpty(Note3)
            || !string.IsNullOrEmpty(Note4)
            || !string.IsNullOrEmpty(Note5)
            || !string.IsNullOrEmpty(Note6)
            || !string.IsNullOrEmpty(Note7)
            || !string.IsNullOrEmpty(Note8);

        //PE0107
        public string BillCheck { get; set; }

        /// <summary>
        ///  請求区分 コード : 名称 <see cref="BillingCategoryCode"/>:<see cref="BillingCategoryName"/>
        /// </summary>
        public string BillingCategoryCodeAndName { get { return $"{BillingCategoryCode}:{BillingCategoryName}"; } }

        /// <summary>
        ///  回収区分 コード : 名称 <see cref="CollectCategoryCode"/>:<see cref="CollectCategoryName"/>
        /// </summary>
        public string CollectCategoryCodeAndName { get { return $"{CollectCategoryCode}:{CollectCategoryName}"; } }

        /// <summary>入力区分名 <see cref="InputType"/>の値から文字列取得
        ///   1 : 取込, 2 : 入力, 3 : 期日入金予定, 4 : 定期請求</summary>
        public string InputTypeName
        {
            get
            {
                return InputType == (int)BillingInputType.Importer ? "取込"
                     : InputType == (int)BillingInputType.BillingInput ? "入力"
                     : InputType == (int)BillingInputType.CashOnDueDate ? "期日入金予定"
                     : InputType == (int)BillingInputType.PeriodicBilling ? "定期請求"
                     : "";
            }
        }

        /// <summary>入力区分名 <see cref="InputType"/>の値から文字列取得
        ///   1 : 1：取込, 2 : 2：入力, 3 : 3：期日入金予定</summary>
        public string InputTypeNameAndIndex
        {
            get
            {
                return $"{InputType}：{InputTypeName}";
            }
        }

        /// <summary>
        ///  口座振替結果 <see cref="ResultCode"/>の値から文字列取得
        ///   0 : 振替済, (0以外の文字列) : 振替不能, (未設定) : ""(空文字)
        /// </summary>
        public string ResultCodeName
        {
            get
            {
                return ResultCode.HasValue
                     ? ResultCode.Value == 0
                     ? "振替済"
                     : "振替不能"
                     : "";
            }
        }
        public string AssignmentFlagName
        {
            get
            {
                return AssignmentFlag == 0 ? "未消込"
                     : AssignmentFlag == 1 ? "一部消込"
                     : AssignmentFlag == 2 ? "消込済"
                     : string.Empty;
            }
        }
        public string ConfirmName
        {
            get
            {
                return Confirm == 0 ? "未"
                     : Confirm == 1 ? "済"
                     : "";
            }
        }

        /// <summary>税抜き金額</summary>
        public decimal BillingAmountExcludingTax { get { return BillingAmount - TaxAmount; } }


        public Billing ConvertScheduledIncome(Receipt receipt, decimal amount)
        {
            return new Billing
            {
                CompanyId = this.CompanyId,
                CurrencyId = this.CurrencyId,
                CustomerId = this.CustomerId,
                DepartmentId = this.DepartmentId,
                StaffId = this.StaffId,
                BillingCategoryId = this.BillingCategoryId,
                InputType = 3,
                BillingInputId = null,
                BilledAt = this.BilledAt,
                ClosingAt = this.ClosingAt,
                SalesAt = this.SalesAt,
                DueAt = (DateTime)receipt.DueAt,
                BillingAmount = amount,
                RemainAmount = amount,
                OffsetAmount = 0,
                AssignmentFlag = 0,
                Approved = 1,
                CollectCategoryId = this.CollectCategoryId,
                OriginalCollectCategoryId = null,
                DebitAccountTitleId = this.DebitAccountTitleId,
                CreditAccountTitleId = this.CreditAccountTitleId,
                OriginalDueAt = receipt.RecordedAt,
                OutputAt = null,
                PublishAt = null,
                InvoiceCode = this.InvoiceCode,
                TaxClassId = this.TaxClassId,
                Note1 = this.Note1,
                Note2 = this.Note2,
                Note3 = this.Note3,
                Note4 = this.Note4,
                DeleteAt = null,
                RequestDate = null,
                ResultCode = null,
                TransferOriginalDueAt = null,
                ScheduledPaymentKey = "",
                Quantity = null,
                UnitPrice = null,
                UnitSymbol = null,
                Price = null,
            };
        }

    }

    [DataContract]
    public class BillingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Billing[] Billing { get; set; }
    }

    [DataContract]
    public class BillingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Billing> Billings { get; set; }
    }

}
