using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>回収予定表 帳票</summary>
    public interface ICollectionScheduleListReportProcessor
    {
        Task<byte[]> GetAsync(CollectionScheduleSearch option, IProgressNotifier notifier, CancellationToken token = default(CancellationToken));
    }
}
