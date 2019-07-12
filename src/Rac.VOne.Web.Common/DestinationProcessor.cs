using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class DestinationProcessor : IDestinationProcessor
    {
        private readonly IDestinationQueryProcessor destinationQueryProcessor;
        private readonly IAddDestinationQueryProcessor addDestinationQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<Destination> deleteDestinationQueryProcessor;

        public DestinationProcessor(
            IDestinationQueryProcessor destinationQueryProcessor,
            IAddDestinationQueryProcessor addDestinationQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<Destination> deleteDestinationQueryProcessor)
        {
            this.destinationQueryProcessor = destinationQueryProcessor;
            this.addDestinationQueryProcessor = addDestinationQueryProcessor;
            this.deleteDestinationQueryProcessor = deleteDestinationQueryProcessor;
        }

        public async Task<Destination> SaveAsync(Destination Destination, CancellationToken token = default(CancellationToken))
            => await addDestinationQueryProcessor.SaveAsync(Destination, token);

        public async Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
            => await deleteDestinationQueryProcessor.DeleteAsync(Id, token);

        public async Task<IEnumerable<Destination>> GetAsync(DestinationSearch option, CancellationToken token = default(CancellationToken))
            => await destinationQueryProcessor.GetAsync(option, token);



    }
}
