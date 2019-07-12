using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingInvoiceSearch
    {
        [DataMember] public byte[] ClientKey { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime? BilledAtFrom { get; set; }
        [DataMember] public DateTime? BilledAtTo { get; set; }
        [DataMember] public DateTime? ClosingAt { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public string DepartmentCodeFrom { get; set; }
        [DataMember] public string DepartmentCodeTo { get; set; }
        [DataMember] public string CustomerCodeFrom { get; set; }
        [DataMember] public string CustomerCodeTo { get; set; }
        [DataMember] public bool IsPublished { get; set; }
        [DataMember] public string ReportId { get; set; }
        /// <summary>
        /// 0:請求額を使用 1:請求残を使用
        /// </summary>
        [DataMember] public int ReportInvoiceAmount { get; set; }
        [DataMember] public DateTime? PublishAtFrom { get; set; }
        [DataMember] public DateTime? PublishAtTo { get; set; }
        [DataMember] public DateTime? PublishAtFirstFrom { get; set; }
        [DataMember] public DateTime? PublishAtFirstTo { get; set; }
        [DataMember] public string InvoiceCodeFrom { get; set; }
        [DataMember] public string InvoiceCodeTo { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public string StaffCodeFrom { get; set; }
        [DataMember] public string StaffCodeTo { get; set; }
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

        [DataMember] public string ConnectionId { get; set; }
        [DataMember] public long[] BillingIds { get; set; }
        [DataMember] public long[] BillingInputIds { get; set; }

    }
}
