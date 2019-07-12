using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class NettingSearchProcessor :INettingSearchProcessor
    {
        private readonly INettingSearchQueryProcessor nettingSearchQueryProcessor;

        public NettingSearchProcessor(
          INettingSearchQueryProcessor nettingSearchQueryProcessor
            )
        {
            this.nettingSearchQueryProcessor = nettingSearchQueryProcessor;
        }

        public async Task<IEnumerable<Netting>> GetAsync(int CompanyId, int CustomerId, int CurrencyId, CancellationToken token = default(CancellationToken))
            => await nettingSearchQueryProcessor.GetAsync(CompanyId, CustomerId,CurrencyId, token);

    }
}
