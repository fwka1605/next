using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class HolidayCalendar : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime Holiday { get; set; }
    }

    [DataContract]
    public class HolidayCalendarResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public HolidayCalendar HolidayCalendar { get; set; }
    }

    [DataContract]
    public class HolidayCalendarsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<HolidayCalendar> HolidayCalendars { get; set; }
    }
}
