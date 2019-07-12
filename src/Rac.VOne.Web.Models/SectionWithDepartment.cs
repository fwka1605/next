using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class SectionWithDepartment
    {
        [DataMember] public int SectionId { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        //Department
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }

        //Section
        [DataMember] public string SectionCode { get; set; }
        [DataMember] public string SectionName { get; set; }
    }

    [DataContract]
    public class SectionWithDepartmentResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public SectionWithDepartment SectionDepartment { get; set; }
    }

    [DataContract]
    public class SectionWithDepartmentsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<SectionWithDepartment> SectionDepartments { get; set; }
    }
}
