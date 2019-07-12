using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class ImportFileLogProcessor : IImportFileLogProcessor
    {
        private readonly IImportFileLogQueryProcessor importFileLogQueryProcessor;

        private readonly IAddImportFileLogQueryProcessor addImportFileLogQueryProcessor;
        private readonly IAddReceiptHeaderQueryProcessor addReceiptHeaderQueryProcessor;
        private readonly IAddReceiptQueryProcessor addReceiptQueryProcessor;
        private readonly IAddReceiptExcludeQueryProcessor addReceiptExcludeQueryProcessor;

        private readonly IDeleteIdenticalEntityQueryProcessor<ImportFileLog> deleteImportFileLogQueryProcessor;
        private readonly IDeleteReceiptHeaderQueryProcessor deleteReceiptHeaderQueryProcessor;
        private readonly IDeleteReceiptQueryProcessor deleteReceiptQueryProcessor;
        private readonly IDeleteReceiptExcludeQueryProcessor deleteReceiptExcludeQueryProcessor;


        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ImportFileLogProcessor(
            IImportFileLogQueryProcessor importFileLogQueryProcessor,
            IAddImportFileLogQueryProcessor addImportFileLogQueryProcessor,
            IAddReceiptHeaderQueryProcessor addReceiptHeaderQueryProcessor,
            IAddReceiptQueryProcessor addReceiptQueryProcessor,
            IAddReceiptExcludeQueryProcessor addReceiptExcludeQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<ImportFileLog> deleteImportFileLogQueryProcessor,
            IDeleteReceiptHeaderQueryProcessor deleteReceiptHeaderQueryProcessor,
            IDeleteReceiptQueryProcessor deleteReceiptQueryProcessor,
            IDeleteReceiptExcludeQueryProcessor deleteReceiptExcludeQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.importFileLogQueryProcessor = importFileLogQueryProcessor;
            this.addImportFileLogQueryProcessor = addImportFileLogQueryProcessor;
            this.addReceiptHeaderQueryProcessor = addReceiptHeaderQueryProcessor;
            this.addReceiptQueryProcessor = addReceiptQueryProcessor;
            this.addReceiptExcludeQueryProcessor = addReceiptExcludeQueryProcessor;
            this.deleteImportFileLogQueryProcessor = deleteImportFileLogQueryProcessor;
            this.deleteReceiptHeaderQueryProcessor = deleteReceiptHeaderQueryProcessor;
            this.deleteReceiptQueryProcessor = deleteReceiptQueryProcessor;
            this.deleteReceiptExcludeQueryProcessor = deleteReceiptExcludeQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }


        public async Task<IEnumerable<ImportFileLog>> GetHistoryAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await importFileLogQueryProcessor.GetHistoryAsync(CompanyId, token);

        public async Task<IEnumerable<ImportFileLog>> SaveAsync(IEnumerable<ImportFileLog> logs, CancellationToken token = default(CancellationToken))
        {
            var result = new List<ImportFileLog>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var log in logs)
                {
                    var resultLog = await addImportFileLogQueryProcessor.SaveAsync(log, token);

                    foreach (var header in log.ReceiptHeaders)
                    {
                        header.ImportFileLogId = resultLog.Id;
                        var resultHeader = await addReceiptHeaderQueryProcessor.SaveAsync(header, token);

                        resultLog.ReceiptHeaders.Add(resultHeader);

                        foreach (var receipt in header.Receipts)
                        {
                            receipt.ReceiptHeaderId = resultHeader.Id;
                            var resultReceipt = await addReceiptQueryProcessor.SaveAsync(receipt, token: token);

                            var exclude = header.ReceiptExcludes.Any(x => x.ReceiptId == receipt.Id)
                                ? header.ReceiptExcludes.First(x => x.ReceiptId == receipt.Id) : null;

                            if (exclude != null)
                            {
                                exclude.ReceiptId = resultReceipt.Id;
                                var resultExclude = await addReceiptExcludeQueryProcessor.SaveAsync(exclude, token);
                                resultHeader.ReceiptExcludes.Add(resultExclude);
                            }
                            resultHeader.Receipts.Add(resultReceipt);
                        }
                    }
                    result.Add(resultLog);
                }
                scope.Complete();
            }
            return result;
        }

        public async Task<int> DeleteAsync(IEnumerable<int> fileLogIds, CancellationToken token = default(CancellationToken))
        {
            var count = 0;
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach ( var id in fileLogIds)
                {
                    await deleteReceiptExcludeQueryProcessor.DeleteByFileLogIdAsync(id, token);
                    await deleteReceiptQueryProcessor.DeleteByFileLogIdAsync(id, token);
                    await deleteReceiptHeaderQueryProcessor.DeleteByFileLogIdAsync(id, token);
                    count += await deleteImportFileLogQueryProcessor.DeleteAsync(id, token);
                }
                scope.Complete();
            }
            return count;
        }

    }
}


