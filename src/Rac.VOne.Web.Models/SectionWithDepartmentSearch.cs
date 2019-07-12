using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class SectionWithDepartmentSearch
    {
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public int? DepartmentId { get; set; }
    }
}
