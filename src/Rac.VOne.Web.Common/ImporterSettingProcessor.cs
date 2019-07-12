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
    public class ImporterSettingProcessor : IImporterSettingProcessor
    {
        private readonly IImporterSettingQueryProcessor importerSettingQueryProcessor;
        private readonly IImporterSettingDetailQueryProcessor importerSettingDetailQueryProcessor;
        private readonly IAddImporterSettingQueryProcessor addImporterSettingQueryProcessor;
        private readonly IAddImporterSettingDetailQueryProcessor addImporterSettingDetailQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<ImporterSetting> deleteImporterSettingByIdQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ImporterSettingProcessor(
           IImporterSettingQueryProcessor importerSettingQueryProcessor,
           IImporterSettingDetailQueryProcessor importerSettingDetailQueryProcessor,
           IAddImporterSettingQueryProcessor addImporterSettingQueryProcessor,
           IAddImporterSettingDetailQueryProcessor addImporterSettingDetailQueryProcessor,
           IDeleteIdenticalEntityQueryProcessor<ImporterSetting> deleteImporterSettingByIdQueryProcessor,
           ITransactionScopeBuilder transactionScopeBuilder
           )
        {
            this.importerSettingQueryProcessor = importerSettingQueryProcessor;
            this.importerSettingDetailQueryProcessor = importerSettingDetailQueryProcessor;
            this.addImporterSettingQueryProcessor = addImporterSettingQueryProcessor;
            this.addImporterSettingDetailQueryProcessor = addImporterSettingDetailQueryProcessor;
            this.deleteImporterSettingByIdQueryProcessor = deleteImporterSettingByIdQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<ImporterSetting>> GetAsync(ImporterSetting setting, CancellationToken token = default(CancellationToken))
            => await importerSettingQueryProcessor.GetAsync(setting, token);


        public async Task<ImporterSetting> SaveAsync(ImporterSetting setting, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = await addImporterSettingQueryProcessor.SaveAsync(setting, token);
                result.Details = new List<ImporterSettingDetail>();
                foreach (var x in setting.Details)
                {
                    x.ImporterSettingId = result.Id;
                    result.Details.Add(await addImporterSettingDetailQueryProcessor.SaveAsync(x, token));
                }
                scope.Complete();
                return result;
            }
        }

        public async Task<int> DeleteAsync(int ImporterSettingId, CancellationToken token = default(CancellationToken))
        {
            var result = 0;
            using (var scope = transactionScopeBuilder.Create())
            {
                result = await deleteImporterSettingByIdQueryProcessor.DeleteAsync(ImporterSettingId, token);
                if (result != 0)
                    scope.Complete();
            }
            return result;
        }

    }
}