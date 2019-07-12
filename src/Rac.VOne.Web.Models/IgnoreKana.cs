using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class IgnoreKana : IByCompany
    {
        [DataMember] public int CompanyId { get; set;}
        [DataMember] public string Kana { get; set; }
        [DataMember] public int ExcludeCategoryId { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }

        // Categoryマスターより
        [DataMember] public string ExcludeCategoryCode { get; set; }
        [DataMember] public string ExcludeCategoryName { get; set; }
    }

    [DataContract]
    public class IgnoreKanasResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<IgnoreKana> IgnoreKanas { get; set; }
    }

    [DataContract]
    public class IgnoreKanaResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public IgnoreKana IgnoreKana { get; set; }
    }
}
