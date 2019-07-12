using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Web.Common
{
    public class HatarakuDBJournalizingProcessor :
        IHatarakuDBJournalizingProcessor
    {
        private readonly IDbSystemDateTimeQueryProcessor dbSystemDateTimeQueryProcessor;
        private readonly IUpdateMatchingJournalizingQueryProcessor updateMatchingJournalizingQueryProcessor;
        private readonly IHatarakuDBJournalizingQueryProcessor hatarakuDBJournalizingQueryProcessor;

        public HatarakuDBJournalizingProcessor(
            IDbSystemDateTimeQueryProcessor dbSystemDateTimeQueryProcessor,
            IUpdateMatchingJournalizingQueryProcessor updateMatchingJournalizingQueryProcessor,
            IHatarakuDBJournalizingQueryProcessor hatarakuDBJournalizingQueryProcessor
            )
        {
            this.dbSystemDateTimeQueryProcessor = dbSystemDateTimeQueryProcessor;
            this.updateMatchingJournalizingQueryProcessor = updateMatchingJournalizingQueryProcessor;
            this.hatarakuDBJournalizingQueryProcessor = hatarakuDBJournalizingQueryProcessor;
        }


        public async Task<IEnumerable<HatarakuDBData>> ExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
            => await hatarakuDBJournalizingQueryProcessor.ExtractAsync(option, token);

        public async Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
            => await hatarakuDBJournalizingQueryProcessor.GetSummaryAsync(option, token);

        public async Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            option.UpdateAt = await dbSystemDateTimeQueryProcessor.GetAsync(token);
            return await updateMatchingJournalizingQueryProcessor.UpdateAsync(option, token);
        }

        public async Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            option.UpdateAt = await dbSystemDateTimeQueryProcessor.GetAsync(token);
            return await updateMatchingJournalizingQueryProcessor.CancelAsync(option, token);
        }
    }
}
