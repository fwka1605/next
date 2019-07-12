using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class EBExcludeAccountSettingProcessor : IEBExcludeAccountSettingProcessor
    {
        private readonly IEBExcludeAccountSettingQueryProcessor ebExcludeAccountSettingQueryProcessor;
        private readonly IByCompanyGetEntitiesQueryProcessor<EBExcludeAccountSetting> ebExcludeAccountSettingGetByCompanyQueryProcessor;

        public EBExcludeAccountSettingProcessor(
            IEBExcludeAccountSettingQueryProcessor ebExcludeAccountSettingQueryProcessor,
            IByCompanyGetEntitiesQueryProcessor<EBExcludeAccountSetting> ebExcludeAccountSettingGetByCompanyQueryProcessor
            )
        {
            this.ebExcludeAccountSettingQueryProcessor = ebExcludeAccountSettingQueryProcessor;
            this.ebExcludeAccountSettingGetByCompanyQueryProcessor = ebExcludeAccountSettingGetByCompanyQueryProcessor;
        }

        public async Task<IEnumerable<EBExcludeAccountSetting>> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await ebExcludeAccountSettingGetByCompanyQueryProcessor.GetItemsAsync(CompanyId, token);

        public async Task<EBExcludeAccountSetting> SaveAsync(EBExcludeAccountSetting setting, CancellationToken token = default(CancellationToken))
            => await ebExcludeAccountSettingQueryProcessor.SaveAsync(setting, token);

        public async Task<int> DeleteAsync(EBExcludeAccountSetting setting, CancellationToken token = default(CancellationToken))
            => await ebExcludeAccountSettingQueryProcessor.DeleteAsync(setting, token);



    }
}
