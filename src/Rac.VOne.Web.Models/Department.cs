using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Department : IMasterData, IMaster, ISynchronization
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int? StaffId { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string StaffName { get; set; }
    }

    [DataContract]
    public class DepartmentResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Department Department { get; set; }
    }

    [DataContract]
    public class DepartmentsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Department> Departments { get; set; }
    }
}
