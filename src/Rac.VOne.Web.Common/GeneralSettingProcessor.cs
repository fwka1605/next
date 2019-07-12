using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class GeneralSettingProcessor : IGeneralSettingProcessor
    {
        private readonly IMasterGetByCodesQueryProcessor<GeneralSetting> masterGetByCodesQueryProcessor;
        private readonly IUpdateGeneralSettingQueryProcessor updateGeneralSettingQueryProcessor;

        public GeneralSettingProcessor(
            IMasterGetByCodesQueryProcessor<GeneralSetting> masterGetByCodesQueryProcessor,
            IUpdateGeneralSettingQueryProcessor updateGeneralSettingQueryProcessor
            )
        {
            this.masterGetByCodesQueryProcessor = masterGetByCodesQueryProcessor;
            this.updateGeneralSettingQueryProcessor = updateGeneralSettingQueryProcessor;
        }


        public async Task<GeneralSetting> SaveAsync(GeneralSetting setting, CancellationToken token = default(CancellationToken))
            => await updateGeneralSettingQueryProcessor.SaveAsync(setting, token);


        public async Task<IEnumerable<GeneralSetting>> GetAsync(GeneralSetting setting, CancellationToken token = default(CancellationToken))
        {
            string[] codes = null;
            if (!string.IsNullOrWhiteSpace(setting.Code)) codes = new[] { setting.Code };
            return await masterGetByCodesQueryProcessor.GetByCodesAsync(setting.CompanyId, codes, token);
        }
    }
}
