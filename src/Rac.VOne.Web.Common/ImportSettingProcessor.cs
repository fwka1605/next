using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class ImportSettingProcessor : IImportSettingProcessor
    {
        private readonly IImportSettingQueryProcessor importSettingQueryProcessor;
        private readonly IUpdateImportSettingQueryProcessor updateImportSettingQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ImportSettingProcessor(
            IImportSettingQueryProcessor importSettingQueryProcessor,
            IUpdateImportSettingQueryProcessor updateImportSettingQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder)
        {
            this.importSettingQueryProcessor = importSettingQueryProcessor;
            this.updateImportSettingQueryProcessor = updateImportSettingQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<ImportSetting>> GetAsync(ImportSettingSearch option, CancellationToken token = default(CancellationToken))
            => await importSettingQueryProcessor.GetAsync(option, token);


        public async Task<IEnumerable<ImportSetting>> SaveAsync(
            IEnumerable<ImportSetting> settings, CancellationToken token = default(CancellationToken))
        {
            var result = new List<ImportSetting>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var setting in settings)
                    result.Add(await updateImportSettingQueryProcessor.SaveAsync(setting, token));
                scope.Complete();
            }
            return result;
        }

    }
}