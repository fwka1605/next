using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class SectionWithLoginUserProcessor : ISectionWithLoginUserProcessor
    {
        private readonly ISectionWithLoginUserQueryProcessor sectionWithLoginUserQueryProcessor;
        private readonly IAddSectionWithLoginUserQueryProcessor addSectionWithUserQueryProcessor;
        private readonly IDeleteSectionWithLoginUserQueryProcessor deleteSectionWithLoginUserQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public SectionWithLoginUserProcessor(
            ISectionWithLoginUserQueryProcessor sectionWithLoginUserQueryProcessor,
            IAddSectionWithLoginUserQueryProcessor addSectionWithUserQueryProcessor,
            IDeleteSectionWithLoginUserQueryProcessor deleteSectionWithLoginUserQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.sectionWithLoginUserQueryProcessor = sectionWithLoginUserQueryProcessor;
            this.addSectionWithUserQueryProcessor = addSectionWithUserQueryProcessor;
            this.deleteSectionWithLoginUserQueryProcessor = deleteSectionWithLoginUserQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<SectionWithLoginUser>> GetAsync(SectionWithLoginUserSearch option, CancellationToken token = default(CancellationToken))
            => await sectionWithLoginUserQueryProcessor.GetAsync(option, token);

        public async Task<bool> ExistLoginUserAsync(int LoginUserId, CancellationToken token = default(CancellationToken))
            => await sectionWithLoginUserQueryProcessor.ExistLoginUserAsync(LoginUserId, token);

        public async Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken))
            => await sectionWithLoginUserQueryProcessor.ExistSectionAsync(SectionId, token);

        public async Task<IEnumerable<SectionWithLoginUser>> SaveAsync(IEnumerable<SectionWithLoginUser> upsert, IEnumerable<SectionWithLoginUser> delete, CancellationToken token = default(CancellationToken))
        {
            var result = new List<SectionWithLoginUser>();

            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var x in delete)
                    await deleteSectionWithLoginUserQueryProcessor.DeleteAsync(x.LoginUserId, x.SectionId, token);

                foreach (var x in upsert)
                    result.Add(await addSectionWithUserQueryProcessor.SaveAsync(x, token));

                scope.Complete();
            }
            return result;
        }

        public async Task<ImportResult> ImportAsync(IEnumerable<SectionWithLoginUser> insert, IEnumerable<SectionWithLoginUser> update, IEnumerable<SectionWithLoginUser> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;
                var items = new List<SectionWithLoginUser>();

                foreach (var x in delete)
                {
                    await deleteSectionWithLoginUserQueryProcessor.DeleteAsync(
                            x.LoginUserId, x.SectionId, token);
                    deleteCount++;
                }

                foreach (var x in update)
                {
                    var item = await addSectionWithUserQueryProcessor.SaveAsync(x, token);
                    items.Add(item);
                    updateCount++;
                }

                foreach (var x in insert)
                {
                    var item = await addSectionWithUserQueryProcessor.SaveAsync(x, token);
                    items.Add(item);
                    insertCount++;
                }
                scope.Complete();

                return new ImportResultSectionWithLoginUser
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = insertCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                    SectionWithLoginUser = items,
                };
            }
        }
    }
}
