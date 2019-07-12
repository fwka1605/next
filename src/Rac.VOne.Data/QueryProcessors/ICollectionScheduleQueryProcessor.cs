using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICollectionScheduleQueryProcessor
    {
        Task<IEnumerable<CollectionSchedule>> GetAsync(CollectionScheduleSearch searchOption, CancellationToken token, IProgressNotifier notifier);
    }
}
