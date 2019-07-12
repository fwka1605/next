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
    public class ClosingProcessor : IClosingProcessor
    {
        private readonly IClosingQueryProcessor closingQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<Closing> byCompanyGetClosingQueryProcessor;
        private readonly IDeleteByCompanyQueryProcessor<Closing> deleteByCompanyQueryProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;

        public ClosingProcessor(
            IClosingQueryProcessor closingQueryProcessor,
            IByCompanyGetEntityQueryProcessor<Closing> byCompanyGetClosingQueryProcessor,
            IDeleteByCompanyQueryProcessor<Closing> deleteByCompanyQueryProcessor,
            IApplicationControlProcessor applicationControlProcessor)
        {
            this.byCompanyGetClosingQueryProcessor = byCompanyGetClosingQueryProcessor;
            this.closingQueryProcessor = closingQueryProcessor;
            this.deleteByCompanyQueryProcessor = deleteByCompanyQueryProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
        }

        public async Task<ClosingInformation> GetClosingInformationAsync(int companyId, CancellationToken token = default(CancellationToken))
        {
            var closing = new ClosingInformation();
            var applicationControl = await applicationControlProcessor.GetAsync(companyId, token);
            if (applicationControl?.UseClosing == 1)
            {
                closing.UseClosing = true;
                closing.Closing = await byCompanyGetClosingQueryProcessor.GetAsync(companyId, token);
            }
            return closing;
        }
        public async Task<IEnumerable<ClosingHistory>> GetClosingHistoryAsync(int companyId, CancellationToken token = default(CancellationToken))
            => await closingQueryProcessor.GetClosingHistoryAsync(companyId, token);

        public async Task<Closing> SaveAsync(Closing closing, CancellationToken token = default(CancellationToken))
            => await closingQueryProcessor.SaveAsync(closing, token);

        public async Task<int> DeleteAsync(int companyId, CancellationToken token = default(CancellationToken))
            => await deleteByCompanyQueryProcessor.DeleteAsync(companyId, token);



    }
}
