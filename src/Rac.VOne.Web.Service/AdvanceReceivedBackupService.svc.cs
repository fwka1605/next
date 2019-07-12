using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class AdvanceReceivedBackupService : IAdvanceReceivedBackupService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IAdvanceReceivedBackupProcessor advanceReceivedBackupProcessor;
        private readonly ILogger logger;

        public AdvanceReceivedBackupService(
            IAuthorizationProcessor authorizationProcessor,
            IAdvanceReceivedBackupProcessor advanceReceivedBackupProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.advanceReceivedBackupProcessor = advanceReceivedBackupProcessor;
            logger = logManager.GetLogger(typeof(ReceiptService));
        }

        public async Task<AdvanceReceivedBackupResult> GetByOriginalReceiptIdAsync(string SessionKey, long OriginalReceiptId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await advanceReceivedBackupProcessor.GetByOriginalReceiptIdAsync(OriginalReceiptId, token);

                return new AdvanceReceivedBackupResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    AdvanceReceivedBackup = result,
                };
            }, logger);
        }

        public async Task<AdvanceReceivedBackupsResult> GetByIdsAsync(string SessionKey, long[] ids)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await advanceReceivedBackupProcessor.GetByIdAsync(ids, token)).ToList();
                return new AdvanceReceivedBackupsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    AdvanceReceivedBackups = result,
                };
            }, logger);
    }
}
