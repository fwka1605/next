using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class ImportFileLogService : IImportFileLogService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IImportFileLogProcessor importFileLogProcessor;
        private readonly ILogger logger;

        public ImportFileLogService(
            IAuthorizationProcessor authorizationProcessor,
            IImportFileLogProcessor importFileLogProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.importFileLogProcessor = importFileLogProcessor;
            logger = logManager.GetLogger(typeof(ImportFileLogService));
        }

        public async Task<ImportFileLogsResult> GetHistoryAsync(string Sessionkey, int ComapnyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(Sessionkey, async token =>
            {
                var result = (await importFileLogProcessor.GetHistoryAsync(ComapnyId, token)).ToList();
                return new ImportFileLogsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ImportFileLogs = result
                };
            }, logger);
        }

        public async Task<ImportFileLogsResult> SaveImportFileLogAsync(string Sessionkey, ImportFileLog[] ImportFileLog)
        {
            return await authorizationProcessor.DoAuthorizeAsync(Sessionkey, async token =>
            {
                var result = (await importFileLogProcessor.SaveAsync(ImportFileLog)).ToList();

                return new ImportFileLogsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ImportFileLogs = result,
                };
            }, logger);
        }

        public async Task<ImportFileLogsResult> DeleteItemsAsync(string SessionKey, int[] Ids)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                await importFileLogProcessor.DeleteAsync(Ids, token);
                return new ImportFileLogsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ImportFileLogs = null
                };
            }, logger);
        }
    }
}
