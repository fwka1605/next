using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class AccountTransferImportData
    {
        [DataMember] public IEnumerable<long> BillingIdList { get; set; }
        [DataMember] public int ResultCode { get; set; }
        [DataMember] public int? DueDateOffset { get; set; }
        [DataMember] public int? CollectCategoryId { get; set; }
        [DataMember] public int[] CustomerIds { get; set; }

        /// <summary> ゆうちょ形式専用 再振替する場合は更新しない </summary>
        [DataMember] public bool DoUpdateAccountTransferLogId { get; set; } = true;
        /// <summary> ゆうちょ形式専用 再振替する場合は入金予定日を更新 </summary>
        [DataMember] public DateTime? DueDate { get; set; }
        [DataMember] public int UpdateBy { get; set; }
    }
}
