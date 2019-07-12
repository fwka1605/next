using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Section : ISynchronization, IMaster
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string PayerCode { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string UpdateDate { get; set; }
        [DataMember] public string LoginUserName { get; set; }
        //追加インポート
        [DataMember] public string PayerCodeLeft { get; set; }
        [DataMember] public string PayerCodeRight { get; set; }
    }

    [DataContract]
    public class SectionResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Section[] Section { get; set; }
    }

    [DataContract]
    public class SectionsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Section> Sections { get; set; }
    }

    [DataContract]
    public class SectionResults : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Section Section { get; set; }
    }
}
