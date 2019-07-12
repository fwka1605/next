using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class DbFunctionProcessor : IDbFunctionProcessor
    {
        private readonly ICreateClientKeyQueryProcessor createClientKeyQueryProcessor;
        private readonly IDbSystemDateTimeQueryProcessor getNowQueryProcessor;

        public DbFunctionProcessor(ICreateClientKeyQueryProcessor createClientKeyQueryProcessor,
            IDbSystemDateTimeQueryProcessor getNowQueryProcessor
            )
        {
            this.createClientKeyQueryProcessor = createClientKeyQueryProcessor;
            this.getNowQueryProcessor = getNowQueryProcessor;
        }

        public async Task<byte[]> CreateClientKeyAsync(ClientKeySearch option, CancellationToken token = default(CancellationToken))
            => await createClientKeyQueryProcessor.CreateAsync(option, token);


        public async Task<DateTime> GetDbDateTimeAsync(CancellationToken token = default(CancellationToken))
            => await getNowQueryProcessor.GetAsync(token);

    }
}
