using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime? BsBilledAtFrom { get; set; }
        [DataMember] public DateTime? BsBilledAtTo { get; set; }
        [DataMember] public DateTime? BsSalesAtFrom { get; set; }
        [DataMember] public DateTime? BsSalesAtTo { get; set; }
        [DataMember] public DateTime? BsDueAtFrom { get; set; }
        [DataMember] public DateTime? BsDueAtTo { get; set; }
        [DataMember] public string BsScheduledPaymentKey { get; set; }
        [DataMember] public string BsInvoiceCode { get; set; }
        [DataMember] public string BsInvoiceCodeFrom { get; set; }
        [DataMember] public string BsInvoiceCodeTo { get; set; }
        [DataMember] public string BsCustomerCode { get; set; }
        [DataMember] public string BsCustomerName { get; set; }
        [DataMember] public string BsCurrencyCode { get; set; }
        [DataMember] public DateTime? BsClosingAtFrom { get; set; }
        [DataMember] public DateTime? BsClosingAtTo { get; set; }
        [DataMember] public string BsDepartmentCodeFrom { get; set; }
        [DataMember] public string BsDepartmentCodeTo { get; set; }
        [DataMember] public string BsStaffCodeFrom { get; set; }
        [DataMember] public string BsStaffCodeTo { get; set; }
        [DataMember] public string BsCustomerCodeFrom { get; set; }
        [DataMember] public string BsCustomerCodeTo { get; set; }
        [DataMember] public string BsCustomerNameKana { get; set; }
        [DataMember] public string BsPayerName { get; set; }
        [DataMember] public decimal? BsRemaingAmountFrom { get; set; }
        [DataMember] public decimal? BsRemaingAmountTo { get; set; }
        [DataMember] public decimal? BsBillingAmountFrom { get; set; }
        [DataMember] public decimal? BsBillingAmountTo { get; set; }
        [DataMember] public int BsInputType { get; set; }
        [DataMember] public string BsMemo { get; set; }
        [DataMember] public int BsBillingCategoryId { get; set; }
        [DataMember] public DateTime? BsUpdateAtFrom { get; set; }
        [DataMember] public DateTime? BsUpdateAtTo { get; set; }
        [DataMember] public int? BsUpdateBy { get; set; }
        [DataMember] public string BsNote1 { get; set; }
        [DataMember] public string BsNote2 { get; set; }
        [DataMember] public string BsNote3 { get; set; }
        [DataMember] public string BsNote4 { get; set; }
        [DataMember] public string BsNote5 { get; set; }
        [DataMember] public string BsNote6 { get; set; }
        [DataMember] public string BsNote7 { get; set; }
        [DataMember] public string BsNote8 { get; set; }
        [DataMember] public int ParentCustomerId { get; set; }
        /// <summary>
        /// 実際の消込フラグ <see cref="Billing.AssignmentFlg"/>と異なる
        /// <see cref="Rac.VOne.Common.Constants.AssignmentFlagChecked"/>を参照すること
        /// 0b000 何も選択されていない
        /// 0b001 2 ^ 0 未消込   の選択有無
        /// 0b010 2 ^ 1 一部消込 の選択有無
        /// 0b100 2 ^ 2 消込済   の選択有無
        /// なにも選択なし            0x00 0b000
        ///                   未消込  0x01 0b001
        ///         一部消込          0x02 0b010
        ///         一部消込  未消込  0x03 0b011
        /// 消込済                    0x04 0b100
        /// 消込済            未消込  0x05 0b101
        /// 消込済  一部消込          0x06 0b110
        /// すべて選択                0x07 0b111
        /// </summary>
        [DataMember] public int AssignmentFlg { get; set; }
        /// <summary>
        /// 入金・請求部門対応マスターを参照 /請求データ検索のみで利用/個別消込は別
        /// </summary>
        [DataMember] public bool UseSectionMaster { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public string LoginUserCode { get; set; }
        [DataMember] public int UserId { get; set; }
        [DataMember] public bool ExistsMemo { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public int RequestDate { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int IsParentFlg { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public byte[] ClientKey { get; set; }
        /// <summary>
        /// 請求絞込ワークを利用 個別消込のみ利用
        /// </summary>
        [DataMember] public bool UseDepartmentWork { get; set; }
        [DataMember] public bool IsDeleted { get; set; }
        [DataMember] public DateTime? BsDeleteAtFrom { get; set; }
        [DataMember] public DateTime? BsDeleteAtTo { get; set; }
        [DataMember] public string KobetsuType { get; set; }

        /// <summary>検索時にMinRecordedAtとMaxRecordedAtを取得するか否か</summary>
        [DataMember] public bool RequireRecordedAt { get; set; }

        //for Billing DuplicateCheck
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public DateTime? BilledAt { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public decimal TaxAmount { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public decimal Price { get; set; }
        [DataMember] public int? TaxClassId { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; }
        [DataMember] public string Note4 { get; set; }
        [DataMember] public string Note5 { get; set; }
        [DataMember] public string Note6 { get; set; }
        [DataMember] public string Note7 { get; set; }
        [DataMember] public string Note8 { get; set; }
        [DataMember] public string CurrencyCode { get; set; }

        [DataMember] public long? BillingId { get; set; }
        [DataMember] public long? BillingInputId { get; set; }
    }
}
