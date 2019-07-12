using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IHolidayCalendarProcessor
    {
        Task<IEnumerable<HolidayCalendar>> GetAsync(HolidayCalendarSearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<HolidayCalendar>> SaveAsync(IEnumerable<HolidayCalendar> days, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(IEnumerable<HolidayCalendar> holidays, CancellationToken token = default(CancellationToken));
        Task<ImportResult> ImportAsync(
            IEnumerable<HolidayCalendar> insert,
            IEnumerable<HolidayCalendar> update,
            IEnumerable<HolidayCalendar> delete, CancellationToken token = default(CancellationToken));

    }
}
