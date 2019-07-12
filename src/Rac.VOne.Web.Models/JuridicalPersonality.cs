using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class JuridicalPersonality
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Kana { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class JuridicalPersonalityResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public JuridicalPersonality JuridicalPersonality { get; set; }
    }

    [DataContract]
    public class JuridicalPersonalitysResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<JuridicalPersonality> JuridicalPersonalities { get; set; }
    }
}
