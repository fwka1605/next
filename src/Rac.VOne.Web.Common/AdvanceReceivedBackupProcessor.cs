using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Rac.VOne.Web.Common
{
    public class AdvanceReceivedBackupProcessor :
        IAdvanceReceivedBackupProcessor
    {
        private readonly IAdvanceReceivedBackupQueryProcessor advanceReceivedBackupQueryProcessor;
        private readonly ITransactionalGetByIdsQueryProcessor<AdvanceReceivedBackup> advanceReceivedBackupGetByIdsQueryProcessor;

        public AdvanceReceivedBackupProcessor(
            IAdvanceReceivedBackupQueryProcessor advanceReceivedBackupQueryProcessor,
            ITransactionalGetByIdsQueryProcessor<AdvanceReceivedBackup> advanceReceivedBackupGetByIdsQueryProcessor
            )
        {
            this.advanceReceivedBackupQueryProcessor = advanceReceivedBackupQueryProcessor;
            this.advanceReceivedBackupGetByIdsQueryProcessor = advanceReceivedBackupGetByIdsQueryProcessor;
        }

        public async Task<AdvanceReceivedBackup> GetByOriginalReceiptIdAsync(long OriginalReceiptId, CancellationToken token = default(CancellationToken))
            => await advanceReceivedBackupQueryProcessor.GetByOriginalReceiptIdAsync(OriginalReceiptId, token);

        public async Task<IEnumerable<AdvanceReceivedBackup>> GetByIdAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken))
            => await advanceReceivedBackupGetByIdsQueryProcessor.GetByIdsAsync(ids, token);
    }
}
