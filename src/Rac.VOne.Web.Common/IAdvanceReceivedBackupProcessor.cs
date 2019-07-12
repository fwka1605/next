using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IAdvanceReceivedBackupProcessor
    {
        Task<AdvanceReceivedBackup> GetByOriginalReceiptIdAsync(long OriginalReceiptId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<AdvanceReceivedBackup>> GetByIdAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken));
    }
}
