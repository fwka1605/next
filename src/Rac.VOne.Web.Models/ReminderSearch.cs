using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime CalculateBaseDate { get; set; }
        [DataMember] public bool ContainReminderAmountZero { get; set; }
        [DataMember] public bool ExistsMemo { get; set; }
        [DataMember] public string BillingMemo { get; set; }
        [DataMember] public int? ArrearDaysFrom { get; set; }
        [DataMember] public int? ArrearDaysTo { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string DepartmentCodeFrom { get; set; }
        [DataMember] public string DepartmentCodeTo { get; set; }
        [DataMember] public string StaffCodeFrom { get; set; }
        [DataMember] public string StaffCodeTo { get; set; }
        [DataMember] public string CustomerCodeFrom { get; set; }
        [DataMember] public string CustomerCodeTo { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string ReminderMemo { get; set; }
        [DataMember] public int Status { get; set; }
        [DataMember] public int? OutputFlag { get; set; }
        [DataMember] public bool ReminderManaged { get; set; }
        [DataMember] public bool RemoveExcludeReminderPublishCustomer { get; set; }
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
        [DataMember]
        public int AssignmentFlg { get; set; }
        [DataMember] public DateTime? CreateAtFrom { get; set; }
        [DataMember] public DateTime? CreateAtTo { get; set; }
        [DataMember] public string CreateByCode { get; set; }

        [DataMember] public ReminderCommonSetting Setting { get; set; }
        [DataMember] public ReminderSummarySetting[] SummarySettings { get; set; }
    }
}
