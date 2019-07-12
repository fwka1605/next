using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CompanyLogo : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public byte[] Logo { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int LogoType { get; set; }
    }

    [DataContract]
    public class CompanyLogoResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public CompanyLogo CompanyLogo { get; set; }
    }

    [DataContract]
    public class CompanyLogosResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<CompanyLogo> CompanyLogos { get; set; }
    }
}
