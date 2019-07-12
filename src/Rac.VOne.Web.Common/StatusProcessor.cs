using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class StatusProcessor : IStatusProcessor
    {
        private readonly IStatusQueryProcessor statusQueryProcessor;
        private readonly IAddStatusQueryProcessor addStatusQueryProcessor;
        private readonly IDeleteStatusQueryProcessor deleteStatusQueryProcessor;
        private readonly IStatusExistsQueryProcessor statusExistsQueryProcessor;

        public StatusProcessor(
            IStatusQueryProcessor statusQueryProcessor,
            IAddStatusQueryProcessor addStatusQueryProcessor,
            IDeleteStatusQueryProcessor deleteStatusQueryProcessor,
            IStatusExistsQueryProcessor statusExistsQueryProcessor)
        {
            this.statusQueryProcessor = statusQueryProcessor;
            this.addStatusQueryProcessor = addStatusQueryProcessor;
            this.deleteStatusQueryProcessor = deleteStatusQueryProcessor;
            this.statusExistsQueryProcessor = statusExistsQueryProcessor;
        }
        public async Task<IEnumerable<Status>> GetAsync(StatusSearch option, CancellationToken token = default(CancellationToken))
            => await statusQueryProcessor.GetAsync(option, token);

        public async Task<Status> SaveAsync(Status Status, CancellationToken token = default(CancellationToken))
            => await addStatusQueryProcessor.SaveAsync(Status, token);

        public async Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
            => await deleteStatusQueryProcessor.DeleteAsync(Id, token);

        public async Task<bool> ExistReminderAsync(int StatusId, CancellationToken token = default(CancellationToken))
            => await statusExistsQueryProcessor.ExistReminderAsync(StatusId, token);

        public async Task<bool> ExistReminderHistoryAsync(int StatusId, CancellationToken token = default(CancellationToken))
            => await statusExistsQueryProcessor.ExistReminderHistoryAsync(StatusId, token);

    }
}
