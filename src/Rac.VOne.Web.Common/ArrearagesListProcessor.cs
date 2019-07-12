using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class ArrearagesListProcessor : IArrearagesListProcessor
    {
        private readonly IArrearagesListQueryProcessor arrearagesListQueryProcessor;

        public ArrearagesListProcessor(
            IArrearagesListQueryProcessor arrearagesListQueryProcessor)
        {
            this.arrearagesListQueryProcessor = arrearagesListQueryProcessor;
        }

        public async Task<IEnumerable<ArrearagesList>> GetAsync(ArrearagesListSearch option, CancellationToken token = default(CancellationToken))
            => await arrearagesListQueryProcessor.GetAsync(option);
    }
}
