using System;
using System.Collections.Generic;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System.Linq;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class HolidayCalendarMaster : IHolidayCalendarMaster
    {

        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IHolidayCalendarProcessor holidaycalendarProcessor;
        private readonly ILogger logger;

        public HolidayCalendarMaster(
            IAuthorizationProcessor authorizationProcessor,
            IHolidayCalendarProcessor holidaycalendarProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.holidaycalendarProcessor = holidaycalendarProcessor;
            logger = logManager.GetLogger(typeof(HolidayCalendarMaster));
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, DateTime[] Holiday)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var holidays = Holiday.Select(x => new HolidayCalendar { CompanyId = CompanyId, Holiday = x }).ToArray();
                var result = await holidaycalendarProcessor.DeleteAsync(holidays, token); ;

                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<HolidayCalendarsResult> GetItemsAsync(string SessionKey, HolidayCalendarSearch option)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await holidaycalendarProcessor.GetAsync(option, token)).ToList();

                return new HolidayCalendarsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    HolidayCalendars = result,
                };
            }, logger);
        }

        public async Task<HolidayCalendarResult> SaveAsync(string SessionKey, HolidayCalendar HolidayCalendar)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await holidaycalendarProcessor.SaveAsync(new[] { HolidayCalendar }, token)).First();
                return new HolidayCalendarResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    HolidayCalendar = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string SessionKey,
             HolidayCalendar[] InsertList, HolidayCalendar[] UpdateList, HolidayCalendar[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await holidaycalendarProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token);
                return result;
            }, logger);
        }



    }
}
