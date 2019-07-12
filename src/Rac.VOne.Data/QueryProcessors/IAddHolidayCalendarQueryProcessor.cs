using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddHolidayCalendarQueryProcessor
    {
        Task<HolidayCalendar> SaveAsync(HolidayCalendar holiday, CancellationToken token = default(CancellationToken));
    }
}
