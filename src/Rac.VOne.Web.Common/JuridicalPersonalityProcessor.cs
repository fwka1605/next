using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Web.Common
{
    public class JuridicalPersonalityProcessor : IJuridicalPersonalityProcessor
    {
        private readonly IJuridicalPersonalityQueryProcessor queryProcessor;
        private readonly IAddJuridicalPersonalityQueryProcessor addQueryProcessor;
        private readonly IDeleteJuridicalPersonalityQueryProcessor deleteQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public JuridicalPersonalityProcessor(
            IJuridicalPersonalityQueryProcessor queryProcessor,
            IAddJuridicalPersonalityQueryProcessor addQueryProcessor,
            IDeleteJuridicalPersonalityQueryProcessor deleteQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.queryProcessor = queryProcessor;
            this.addQueryProcessor = addQueryProcessor;
            this.deleteQueryProcessor = deleteQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<JuridicalPersonality>> GetAsync(JuridicalPersonality personality, CancellationToken token = default(CancellationToken))
            => await queryProcessor.GetAsync(personality, token);

        public async Task<int> DeleteAsync(int CompanyId, string Kana, CancellationToken token = default(CancellationToken))
            => await deleteQueryProcessor.DeleteAsync(CompanyId, Kana, token);

        public async Task<ImportResult> ImportAsync(
            IEnumerable<JuridicalPersonality> insert,
            IEnumerable<JuridicalPersonality> update,
            IEnumerable<JuridicalPersonality> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;

                foreach (var x in delete)
                {
                    await deleteQueryProcessor.DeleteAsync(x.CompanyId, x.Kana, token);
                    deleteCount++;
                }

                foreach (var x in update)
                {
                    await addQueryProcessor.SaveAsync(x, token);
                    updateCount++;
                }

                foreach (var x in insert)
                {
                    await addQueryProcessor.SaveAsync(x, token);
                    insertCount++;
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

        public async Task<JuridicalPersonality> SaveAsync(JuridicalPersonality personality, CancellationToken token = default(CancellationToken))
        {
            // confirm string.Empty validation was required.
            //if (string.IsNullOrWhiteSpace(personality.Kana))
            //    throw new ArgumentNullException(nameof(personality.Kana), "カナが未入力です");
            return await addQueryProcessor.SaveAsync(personality, token);
        }
    }
}
