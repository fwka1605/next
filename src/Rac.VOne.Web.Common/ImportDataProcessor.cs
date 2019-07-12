using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Web.Common
{
    public class ImportDataProcessor : IImportDataProcessor
    {
        private readonly IAddImportDataQueryProcessor addImportDataQueryProcessor;
        private readonly IAddImportDataDetailQueryProcessor addImportDataDetailQueryProcessor;
        private readonly ITransactionalGetByIdQueryProcessor<ImportData> importDataGetByIdQueryProcessor;
        private readonly IImportDataDetailQueryProcessor importDataDetailQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<ImportData> deleteImportDataQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ImportDataProcessor(
            IAddImportDataQueryProcessor addImportDataQueryProcessor,
            IAddImportDataDetailQueryProcessor addImportDataDetailQueryProcessor,
            ITransactionalGetByIdQueryProcessor<ImportData> importDataGetByIdQueryProcessor,
            IImportDataDetailQueryProcessor importDataDetailQueryProcessor,
            IDeleteTransactionQueryProcessor<ImportData> deleteImportDataQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.addImportDataQueryProcessor = addImportDataQueryProcessor;
            this.addImportDataDetailQueryProcessor = addImportDataDetailQueryProcessor;
            this.importDataGetByIdQueryProcessor = importDataGetByIdQueryProcessor;
            this.importDataDetailQueryProcessor = importDataDetailQueryProcessor;
            this.deleteImportDataQueryProcessor = deleteImportDataQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public Task<int> DeleteAsync(long id, CancellationToken token = default(CancellationToken))
            => deleteImportDataQueryProcessor.DeleteAsync(id, token);


        public async Task<ImportData> GetAsync(long id, int? objectType = null, CancellationToken token = default(CancellationToken))
        {
            var header = await importDataGetByIdQueryProcessor.GetByIdAsync(id, token);
            header.Details = (await importDataDetailQueryProcessor.GetAsync(id, objectType, token)).ToArray();
            return header;
        }

        public async Task<ImportData> SaveAsync(ImportData data, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var header = await addImportDataQueryProcessor.SaveAsync(data, token);
                for (var i = 0; i < data.Details.Length; i++)
                {
                    var detail = data.Details[i];
                    detail.ImportDataId = header.Id;
                    await addImportDataDetailQueryProcessor.SaveAsync(detail, token);
                }
                scope.Complete();

                return header;
            }
        }
    }
}
