using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BankBranch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BankKana { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public string BranchKana { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class BankBranchResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public BankBranch BankBranch { get; set; }
    }

    [DataContract]
    public class BankBranchsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BankBranch> BankBranches { get; set; }
    }
}
