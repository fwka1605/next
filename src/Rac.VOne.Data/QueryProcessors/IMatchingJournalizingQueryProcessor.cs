using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IMatchingJournalizingQueryProcessor
    {

        Task<IEnumerable<MatchingJournalizing>> ExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MatchingJournalizing>> MFExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
    }
}
