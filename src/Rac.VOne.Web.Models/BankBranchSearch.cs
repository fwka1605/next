using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BankBranchSearch
    {
        [DataMember] public bool UseCommonSearch { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string[] BankCodes { get; set; }
        [DataMember] public string[] BranchCodes { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BankKana { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public string BranchKana { get; set; }
    }
}
