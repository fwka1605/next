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
    public class ClosingSettingProcessor : IClosingSettingProcessor
    {
        private readonly IClosingSettingQueryProcessor closingSettingQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ClosingSetting> closingSettingGetByCompanyIdQueryProcessor;

        public ClosingSettingProcessor(
            IClosingSettingQueryProcessor closingSettingQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ClosingSetting> closingSettingGetByCompanyIdQueryProcessor
            )
        {
            this.closingSettingQueryProcessor = closingSettingQueryProcessor;
            this.closingSettingGetByCompanyIdQueryProcessor = closingSettingGetByCompanyIdQueryProcessor;
        }

        public async Task<ClosingSetting> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
            => await closingSettingGetByCompanyIdQueryProcessor.GetAsync(companyId, token);

        public async Task<ClosingSetting> SaveAsync(ClosingSetting setting, CancellationToken token = default(CancellationToken))
            => await closingSettingQueryProcessor.SaveAsync(setting, token);
    }
}
