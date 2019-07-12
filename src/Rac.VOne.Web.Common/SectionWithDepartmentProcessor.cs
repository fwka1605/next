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
    public class SectionWithDepartmentProcessor : ISectionWithDepartmentProcessor
    {
        private readonly ISectionWithDepartmentQueryProcessor sectionWithDepartmentQueryProcessor;
        private readonly IAddSectionWithDepartmentQueryProcessor addSectionWithDepartmentQueryProcessor;
        private readonly IDeleteSectionWithDepartmentQueryProcessor deleteSectionWithDepartmentQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public SectionWithDepartmentProcessor(
            ISectionWithDepartmentQueryProcessor sectionWithDepartmentQueryProcessor,
            IAddSectionWithDepartmentQueryProcessor addSectionWithDepartmentQueryProcessor,
            IDeleteSectionWithDepartmentQueryProcessor deleteSectionWithDepartmentQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.sectionWithDepartmentQueryProcessor = sectionWithDepartmentQueryProcessor;
            this.addSectionWithDepartmentQueryProcessor = addSectionWithDepartmentQueryProcessor;
            this.deleteSectionWithDepartmentQueryProcessor = deleteSectionWithDepartmentQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }
        public async Task<IEnumerable<SectionWithDepartment>> GetAsync(SectionWithDepartmentSearch option, CancellationToken token = default(CancellationToken))
            => await sectionWithDepartmentQueryProcessor.GetAsync(option, token);

        public async Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken))
            => await sectionWithDepartmentQueryProcessor.ExistDepartmentAsync(DepartmentId, token);

        public async Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken))
            => await sectionWithDepartmentQueryProcessor.ExistSectionAsync(SectionId, token);

        public async Task<IEnumerable<SectionWithDepartment>> SaveAsync(IEnumerable<SectionWithDepartment> upsert, IEnumerable<SectionWithDepartment> delete, CancellationToken token = default(CancellationToken))
        {
            var result = new List<SectionWithDepartment>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var x in delete)
                    await deleteSectionWithDepartmentQueryProcessor.DeleteAsync(x.SectionId, x.DepartmentId, token);
                foreach (var x in upsert)
                    result.Add(await addSectionWithDepartmentQueryProcessor.SaveAsync(x, token));

                scope.Complete();

            }
            return result;
        }

        public async Task<SectionWithDepartment> SaveAsync(SectionWithDepartment SectionWithDepartment, CancellationToken token = default(CancellationToken))
            => await addSectionWithDepartmentQueryProcessor.SaveAsync(SectionWithDepartment, token);

        public async Task<int> DeleteAsync(int SectionId, int DepartmentId, CancellationToken token = default(CancellationToken))
            => await deleteSectionWithDepartmentQueryProcessor.DeleteAsync(SectionId, DepartmentId, token);

        public async Task<ImportResult> ImportAsync(
            IEnumerable<SectionWithDepartment> insert,
            IEnumerable<SectionWithDepartment> update,
            IEnumerable<SectionWithDepartment> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;
                foreach (var x in delete)
                {
                    await deleteSectionWithDepartmentQueryProcessor.DeleteAsync(x.SectionId, x.DepartmentId, token);
                    deleteCount++;
                }

                foreach (var x in insert)
                {
                    await addSectionWithDepartmentQueryProcessor.SaveAsync(x, token);
                    insertCount++;
                }
                foreach (var x in update)
                {
                    await addSectionWithDepartmentQueryProcessor.SaveAsync(x, token);
                    updateCount++;
                }

                scope.Complete();

                return new ImportResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                };
            }

        }

    }
}
