using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Staff : IMasterData, IMaster, ISynchronization
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public string Mail { get; set; } = string.Empty;
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; } = DateTime.Now;
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string Tel { get; set; }
        [DataMember] public string Fax { get; set; }

        //to join department
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string StaffName { get; set; }
    }

    [DataContract]
    public class StaffResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Staff Staff { get; set; }
    }

    [DataContract]
    public class StaffsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Staff> Staffs { get; set; }
    }
}
