using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class MatchingHistorySearchProcessor : IMatchingHistorySearchProcessor
    {
        private readonly IMatchingHistorySearchQueryProcessor matchingSearchQueryProcessor;

        public MatchingHistorySearchProcessor(
         IMatchingHistorySearchQueryProcessor matchingSearchQueryProcessor)
        {
            this.matchingSearchQueryProcessor = matchingSearchQueryProcessor;
        }

        public async Task<IEnumerable<MatchingHistory>> GetAsync(MatchingHistorySearch searchOption,
            CancellationToken token, IProgressNotifier notifier)
            => await matchingSearchQueryProcessor.GetAsync(searchOption, token, notifier);


    }
}
