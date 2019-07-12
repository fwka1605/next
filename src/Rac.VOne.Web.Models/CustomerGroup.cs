using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CustomerGroup
    {
        [DataMember] public int ParentCustomerId { get; set; }
        [DataMember] public int ChildCustomerId { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        [DataMember] public string ParentCustomerCode { get; set; }
        [DataMember] public string ParentCustomerName { get; set; }
        [DataMember] public string ParentCustomerKana { get; set; }

        [DataMember] public string ChildCustomerCode { get; set; }
        [DataMember] public string ChildCustomerName { get; set; }

    }

    [DataContract]
    public class CustomerGroupResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public CustomerGroup CustomerGroup { get; set; }
    }

    [DataContract]
    public class CustomerGroupsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<CustomerGroup> CustomerGroups { get; set; }
    }
}
