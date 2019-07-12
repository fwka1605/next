using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ImportResultStaff : ImportResult
    {
        [DataMember] public List<Staff> Staffs { get; set; } = new List<Staff>();
    }
}
