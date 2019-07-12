using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteHolidayCalendarQueryProcessor
    {
        Task<int> DeleteAsync(int CompanyId, DateTime Holiday, CancellationToken token = default(CancellationToken));
    }
}
