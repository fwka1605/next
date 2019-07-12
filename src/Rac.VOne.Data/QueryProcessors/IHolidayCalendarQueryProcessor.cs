using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IHolidayCalendarQueryProcessor
    {
        Task<IEnumerable<HolidayCalendar>> GetAsync(HolidayCalendarSearch option, CancellationToken token = default(CancellationToken));
    }
}
