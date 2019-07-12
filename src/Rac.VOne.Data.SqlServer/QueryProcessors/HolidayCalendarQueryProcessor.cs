using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class HolidayCalendarQueryProcessor :
        IHolidayCalendarQueryProcessor,
        IAddHolidayCalendarQueryProcessor,
        IDeleteHolidayCalendarQueryProcessor

    {
        private readonly IDbHelper dbHelper;

        public HolidayCalendarQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<HolidayCalendar> SaveAsync(HolidayCalendar holiday, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO HolidayCalendar AS Org 
USING (
    SELECT
            @CompanyId  AS CompanyId
          , @Holiday    AS Holiday
) AS Target
ON (
        Org.CompanyId = @CompanyId
    AND Org.Holiday = @Holiday
)
WHEN MATCHED AND Org.Holiday = @Holiday AND  Org.CompanyId = @CompanyId THEN
    UPDATE SET
       Org.Holiday = @Holiday
WHEN NOT MATCHED THEN
    INSERT ( CompanyId,  Holiday)
    VALUES (@CompanyId, @Holiday)
OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<HolidayCalendar>(query, holiday, token);
        }

        public Task<IEnumerable<HolidayCalendar>> GetAsync(HolidayCalendarSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        HolidayCalendar
WHERE       CompanyId       = @CompanyId
AND         (@FromHoliday IS NULL OR Holiday  >= @FromHoliday)
AND         (@ToHoliday   IS NULL OR Holiday  <= @ToHoliday)";
            return dbHelper.GetItemsAsync<HolidayCalendar>(query, option, token);
        }

        public Task<int> DeleteAsync(int CompanyId, DateTime Holiday, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE FROM HolidayCalendar
WHERE CompanyId = @CompanyId
AND  Holiday = @Holiday";
            return dbHelper.ExecuteAsync(query, new { CompanyId, Holiday }, token);
        }

    }
}
