using System.Collections.Generic;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class HolidayCalendarProcessor : IHolidayCalendarProcessor
    {
        private readonly IHolidayCalendarQueryProcessor holidayCalendarQueryProcessor;
        private readonly IAddHolidayCalendarQueryProcessor addHolidayCalendarQueryProcessor;
        private readonly IDeleteHolidayCalendarQueryProcessor deleteHolidayCalendarQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public HolidayCalendarProcessor(
            IHolidayCalendarQueryProcessor holidayCalendarQueryProcessor,
            IAddHolidayCalendarQueryProcessor addHolidayCalendarQueryProcessor,
            IDeleteHolidayCalendarQueryProcessor deleteHolidayCalendarQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.holidayCalendarQueryProcessor = holidayCalendarQueryProcessor;
            this.addHolidayCalendarQueryProcessor = addHolidayCalendarQueryProcessor;
            this.deleteHolidayCalendarQueryProcessor = deleteHolidayCalendarQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<HolidayCalendar>> SaveAsync(IEnumerable<HolidayCalendar> days, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new List<HolidayCalendar>();
                foreach (var day in days)
                    result.Add(await addHolidayCalendarQueryProcessor.SaveAsync(day, token));
                scope.Complete();
                return result;
            }
        }


        public async Task<IEnumerable<HolidayCalendar>> GetAsync(HolidayCalendarSearch option, CancellationToken token = default(CancellationToken))
            => await holidayCalendarQueryProcessor.GetAsync(option, token);

        public async Task<int> DeleteAsync(IEnumerable<HolidayCalendar> holidays, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var count = 0;
                foreach (var holiday in holidays)
                    count += await deleteHolidayCalendarQueryProcessor.DeleteAsync(holiday.CompanyId, holiday.Holiday, token);
                scope.Complete();
                return count;
            }
        }

        public async Task<ImportResult> ImportAsync(
            IEnumerable<HolidayCalendar> insert,
            IEnumerable<HolidayCalendar> update,
            IEnumerable<HolidayCalendar> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;
                foreach (var x in delete)
                {
                    await deleteHolidayCalendarQueryProcessor.DeleteAsync(x.CompanyId, x.Holiday, token);
                    ++deleteCount;
                }

                foreach (var x in update)
                {
                    await addHolidayCalendarQueryProcessor.SaveAsync(x, token);
                    ++updateCount;
                }

                foreach (var x in insert)
                {
                    await addHolidayCalendarQueryProcessor.SaveAsync(x, token);
                    ++insertCount;
                }

                scope.Complete();

                return new ImportResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                };
            }
        }

    }
}
