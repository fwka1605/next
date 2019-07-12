using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Rac.VOne.Data;
using Entities = Rac.VOne.Data.Entities;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common.AutoMappingConfiguration;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class ApplicationControlProcessor : IApplicationControlProcessor
    {
        private readonly IAddApplicationControlQueryProcessor addApplicationControlQueryProcessor;
        private readonly IUpdateApplicationControlQueryProcessor updateApplicationControlQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlByCompanyId;

        public ApplicationControlProcessor(
                IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlByCompanyId,
                IAddApplicationControlQueryProcessor addApplicationControlQueryProcessor,
                IUpdateApplicationControlQueryProcessor updateApplicationControlQueryProcessor)
        {
            this.applicationControlByCompanyId = applicationControlByCompanyId;
            this.addApplicationControlQueryProcessor = addApplicationControlQueryProcessor;
            this.updateApplicationControlQueryProcessor = updateApplicationControlQueryProcessor;
        }

        public async Task< ApplicationControl > GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await applicationControlByCompanyId.GetAsync(CompanyId, token);

        public async Task<int> UpdateUseOperationLogAsync(ApplicationControl AppData, CancellationToken token = default(CancellationToken))
            => await updateApplicationControlQueryProcessor.UpdateUseOperationLogDataAsync(AppData, token);

        public async Task<ApplicationControl> AddAsync(ApplicationControl ApplicationControl, CancellationToken token = default(CancellationToken))
            => await addApplicationControlQueryProcessor.AddAsync(ApplicationControl, token);

    }
}
