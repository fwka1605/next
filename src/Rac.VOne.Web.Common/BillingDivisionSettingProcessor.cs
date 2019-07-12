using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class BillingDivisionSettingProcessor :
        IBillingDivisionSettingProcessor
    {
        private readonly IByCompanyGetEntityQueryProcessor<BillingDivisionSetting> billingDivisionSettingQueryProcessor;
        public BillingDivisionSettingProcessor(
            IByCompanyGetEntityQueryProcessor<BillingDivisionSetting> billingDivisionSettingQueryProcessor
            )
        {
            this.billingDivisionSettingQueryProcessor = billingDivisionSettingQueryProcessor;
        }

        public async Task<BillingDivisionSetting> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await billingDivisionSettingQueryProcessor.GetAsync(CompanyId, token);
    }
}
