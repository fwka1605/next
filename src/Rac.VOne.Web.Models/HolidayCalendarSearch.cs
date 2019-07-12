using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class HolidayCalendarSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime Holiday { get; set; }
        [DataMember] public DateTime? FromHoliday { get; set; }
        [DataMember] public DateTime? ToHoliday { get; set; }
    }
}
