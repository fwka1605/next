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
    public class SectionProcessor : ISectionProcessor
    {
        private readonly ISectionQueryProcessor sectionQueryProcessor;
        private readonly IAddSectionQueryProcessor addSectionQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<Section> deleteSectionQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public SectionProcessor(
           ISectionQueryProcessor sectionQueryProcessor,
           IAddSectionQueryProcessor addSectionQueryProcessor,
           IDeleteIdenticalEntityQueryProcessor<Section> deleteSectionQueryProcessor,
           ITransactionScopeBuilder transactionScopeBuilder)
        {
            this.sectionQueryProcessor = sectionQueryProcessor;
            this.addSectionQueryProcessor = addSectionQueryProcessor;
            this.deleteSectionQueryProcessor = deleteSectionQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Section>> GetAsync(SectionSearch option, CancellationToken token = default(CancellationToken))
            => await sectionQueryProcessor.GetAsync(option, token);

        public async Task<Section> SaveAsync(Section Section, CancellationToken token = default(CancellationToken))
            => await addSectionQueryProcessor.SaveAsync(Section, token);

        public async Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
            => await deleteSectionQueryProcessor.DeleteAsync(Id, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsBankAccountAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken))
            => await sectionQueryProcessor.GetImportItemsBankAccountAsync(companyId, codes, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsSectionWithDepartmentAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken))
            => await sectionQueryProcessor.GetImportItemsSectionWithDepartmentAsync(companyId, codes, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsSectionWithLoginUserAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken))
            => await sectionQueryProcessor.GetImportItemsSectionWithLoginUserAsync(companyId, codes, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsReceiptAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken))
            => await sectionQueryProcessor.GetImportItemsReceiptAsync(companyId, codes, token);

        public async Task<IEnumerable<MasterData>> GetImportItemsNettingAsync(int companyId, IEnumerable<string> codes, CancellationToken token = default(CancellationToken))
            => await sectionQueryProcessor.GetImportItemsNettingAsync(companyId, codes, token);

        public async Task<ImportResult> ImportAsync(
            IEnumerable<Section> insert,
            IEnumerable<Section> update,
            IEnumerable<Section> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;
                var items = new List<Section>();
                foreach (var x in delete)
                {
                    await deleteSectionQueryProcessor.DeleteAsync(x.Id, token);
                    deleteCount++;
                }

                foreach (var x in update)
                {
                    var item = await addSectionQueryProcessor.SaveAsync(x, token);
                    items.Add(item);
                    updateCount++;
                }

                foreach (var x in insert)
                {
                    var item = await addSectionQueryProcessor.SaveAsync(x, token);
                    items.Add(item);
                    insertCount++;
                }

                scope.Complete();

                return new ImportResultSection
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                    Section = items,
                };

            }
        }
    }
}
