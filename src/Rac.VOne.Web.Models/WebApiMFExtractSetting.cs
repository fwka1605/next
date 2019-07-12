using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class WebApiMFExtractSetting
    {
        /// <summary> 請求区分 </summary>
        [DataMember] public int? BillingCategoryId { get; set; }
        /// <summary> 回収方法 </summary>
        [DataMember] public int? CollectCategoryId { get; set; }
        /// <summary> 営業担当者 </summary>
        [DataMember] public int? StaffId { get; set; }
        /// <summary> 締日 </summary>
        [DataMember] public int? ClosingDay { get; set; }
        /// <summary> 回収予定月 </summary>
        [DataMember] public int? CollectOffsetMonth { get; set; }
        /// <summary> 回収予定日 </summary>
        [DataMember] public int? CollectOffsetDay { get; set; }

    }
}
