using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class LoginUser :IMasterData, IMaster, ISynchronization
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public string Mail { get; set; } = string.Empty;
        [DataMember] public int MenuLevel { get; set; }
        [DataMember] public int FunctionLevel { get; set; }
        [DataMember] public int UseClient { get; set; }
        [DataMember] public int UseWebViewer { get; set; }
        [DataMember] public int? AssignedStaffId { get; set; }
        [DataMember] public string StringValue1 { get; set; } = string.Empty;
        [DataMember] public string StringValue2 { get; set; } = string.Empty;
        [DataMember] public string StringValue3 { get; set; } = string.Empty;
        [DataMember] public string StringValue4 { get; set; } = string.Empty;
        [DataMember] public string StringValue5 { get; set; } = string.Empty;
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int CheckFlag { get; set; }
        [DataMember] public string InitialPassword { get; set; }
    }

    [DataContract]
    public class UserResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public LoginUser User { get; set; }
    }

    [DataContract]
    public class UsersResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<LoginUser> Users { get; set; }
    }
}
