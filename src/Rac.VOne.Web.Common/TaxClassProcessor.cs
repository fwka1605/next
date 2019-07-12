using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
namespace Rac.VOne.Web.Common
{
    public class TaxClassProcessor : ITaxClassProcessor
    {
        private readonly ITaxClassQueryProcessor taxClassQueryProcessor;

        public TaxClassProcessor(
            ITaxClassQueryProcessor taxClassQueryProcessor)
        {
            this.taxClassQueryProcessor = taxClassQueryProcessor;
        }

        public async Task<IEnumerable<TaxClass>> GetAsync(CancellationToken token = default(CancellationToken))
            => await taxClassQueryProcessor.GetAsync(token);

    }
}
