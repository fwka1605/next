using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class LogsProcessor : ILogsProcessor
    {
        private readonly ILogsQueryProcessor logsQueryProcessor;

        public LogsProcessor(ILogsQueryProcessor logsQueryProcessor)
        {
            this.logsQueryProcessor = logsQueryProcessor;
        }

        public async Task<int> SaveAsync(Logs log, CancellationToken token = default(CancellationToken))
        {
            log.Level = "Error";
            return await logsQueryProcessor.SaveAsync(log, token);
        }

    }
}
