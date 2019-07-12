using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class InvoiceCommonSetting : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int ExcludeAmountZero { get; set; }
        [DataMember] public int ExcludeMinusAmount { get; set; }
        [DataMember] public int ExcludeMatchedData { get; set; }
        [DataMember] public int ControlInputCharacter { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class InvoiceCommonSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public InvoiceCommonSetting InvoiceCommonSetting { get; set; }
    }


}
