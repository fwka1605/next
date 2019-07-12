using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class EBExcludeAccountSetting : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public int AccountTypeId { get; set; }
        [DataMember] public string PayerCode { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class EBExcludeAccountSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public EBExcludeAccountSetting EBExcludeAccountSetting { get; set; }
    }

    [DataContract]
    public class EBExcludeAccountSettingListResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<EBExcludeAccountSetting> EBExcludeAccountSettingList { get; set; }
    }
}
