using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class ImporterSettingDetailProcessor : IImporterSettingDetailProcessor
    {
        private readonly IImporterSettingDetailQueryProcessor importerSettingDetailQueryProcessor;

        public ImporterSettingDetailProcessor(
            IImporterSettingDetailQueryProcessor importerSettingDetailQueryProcessor
            )
        {
            this.importerSettingDetailQueryProcessor = importerSettingDetailQueryProcessor;
        }

        public async Task<IEnumerable<ImporterSettingDetail>> GetAsync(ImporterSetting setting, CancellationToken token = default(CancellationToken))
            => await importerSettingDetailQueryProcessor.GetAsync(setting, token);

    }
}
