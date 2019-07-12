using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Web.Common
{
    public class EBFormatProcessor : IEBFormatProcessor
    {
        private readonly IEBFormatQueryProcessor ebFormatQueryProcessor;

        public EBFormatProcessor(
            IEBFormatQueryProcessor ebFormatQueryProcessor
            )
        {
            this.ebFormatQueryProcessor = ebFormatQueryProcessor;
        }
        public async Task<IEnumerable<EBFormat>> GetAsync(CancellationToken token = default(CancellationToken))
            => await ebFormatQueryProcessor.GetAsync(token);

    }
}
