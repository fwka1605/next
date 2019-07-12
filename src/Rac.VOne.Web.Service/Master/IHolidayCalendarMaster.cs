using System;
using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IHolidayCalendarMaster
    {
        [OperationContract]
        Task<HolidayCalendarResult> SaveAsync(string SessionKey, HolidayCalendar HolidayCalendar);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, DateTime[] Holiday);

        [OperationContract]
        Task<HolidayCalendarsResult> GetItemsAsync(string SessionKey, HolidayCalendarSearch option);

        [OperationContract]
        Task<ImportResult> ImportAsync(string SessionKey,
            HolidayCalendar[] InsertList, HolidayCalendar[] UpdateList, HolidayCalendar[] DeleteList);
    }
}