using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAdvanceReceivedBackupQueryProcessor
    {
        Task<AdvanceReceivedBackup> GetByOriginalReceiptIdAsync(long OriginalReceiptId, CancellationToken token = default(CancellationToken));

        /// <summary>全てのカラム値を指定通りにセットしたレコードを登録する。
        /// IdやCreateAtも指定する必要があるので注意</summary>
        Task<AdvanceReceivedBackup> SaveAsync(AdvanceReceivedBackup AdvanceReceivedBackup, CancellationToken token = default(CancellationToken));


        Task<int> DeleteAsync(long OriginalReceiptId, CancellationToken token = default(CancellationToken));
    }
}
