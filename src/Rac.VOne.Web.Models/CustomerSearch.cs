using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CustomerSearch
    {
        [DataMember] public int? CompanyId {get; set;}
        [DataMember] public int[] Ids { get; set; }
        [DataMember] public string[] Codes { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string CustomerCodeFrom { get; set; }
        [DataMember] public string CustomerCodeTo { get; set; }
        [DataMember] public int? ClosingDay { get; set; }
        [DataMember] public int? ShareTransferFee { get; set; }
        [DataMember] public string StaffCodeFrom { get; set; }
        [DataMember] public string StaffCodeTo { get; set; }
        [DataMember] public DateTime? UpdateAtFrom { get; set; }
        [DataMember] public DateTime? UpdateAtTo { get; set; }
        [DataMember] public int? IsParent { get; set; }
        /// <summary>債権代表者ID 属しているグループまたは、単独得意先も取得可</summary>
        [DataMember] public int? ParentCustomerId { get; set; }
        /// <summary>属している債権代表者グループの親得意先ID または 孤立している得意先を取得</summary>
        /// <remarks>債権代表者マスターで、データを取得する際に利用</remarks>
        [DataMember] public int? XorParentCustomerId { get; set; }

        [DataMember] public string ExclusiveBankCode { get; set; }
        [DataMember] public string ExclusiveBranchCode { get; set; }
        [DataMember] public string ExclusiveAccountNumber { get; set; }

    }
}
