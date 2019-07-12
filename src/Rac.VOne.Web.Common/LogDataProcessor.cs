using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class LogDataProcessor : ILogDataProcessor
    {
        private readonly ILogDataByCompanyIdQueryProcessor logdataByCompanyIdQueryProcessor;
        private readonly IAddLogDataQueryProcessor addLogDataQueryProcessor;
        private readonly IDeleteByCompanyQueryProcessor<LogData> deleteLogDataByCompanyQueryProcessor;

        public LogDataProcessor(
            ILogDataByCompanyIdQueryProcessor byCompanyId,
            IAddLogDataQueryProcessor addQuery,
            IDeleteByCompanyQueryProcessor<LogData> deleteLogDataByCompanyQueryProcessor
            )
        {
            logdataByCompanyIdQueryProcessor = byCompanyId;
            addLogDataQueryProcessor = addQuery;
            this.deleteLogDataByCompanyQueryProcessor = deleteLogDataByCompanyQueryProcessor;
        }

        public async Task<IEnumerable<LogData>> GetItemsAsync(LogDataSearch option, CancellationToken token = default(CancellationToken))
            => await logdataByCompanyIdQueryProcessor.GetItemsAsync(option, token);

        public async Task<int> LogAsync(LogData LogData, CancellationToken token = default(CancellationToken))
            => await addLogDataQueryProcessor.SaveAsync(LogData, token);

        public async Task<LogData> GetStatsAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await logdataByCompanyIdQueryProcessor.GetStatsAsync(CompanyId, token);

        public async Task<int> DeleteAllAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await deleteLogDataByCompanyQueryProcessor.DeleteAsync(CompanyId, token);




    }
}
