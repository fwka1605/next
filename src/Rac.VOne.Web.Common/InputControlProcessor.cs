using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System;
using System.Linq;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class InputControlProcessor : IInputControlProcessor
    {
        private readonly IInputControlQueryProcessor inputControlQueryProcessor;
        private readonly IAddInputControlQueryProcessor addInputControlQueryProcessor;
        private readonly IDeleteInputControlQueryProcessor deleteInputControlQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public InputControlProcessor(
            IInputControlQueryProcessor inputControlQueryProcessor,
            IAddInputControlQueryProcessor addInputControlQueryProcessor,
            IDeleteInputControlQueryProcessor deleteInputControlQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.inputControlQueryProcessor = inputControlQueryProcessor;
            this.addInputControlQueryProcessor = addInputControlQueryProcessor;
            this.deleteInputControlQueryProcessor = deleteInputControlQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<InputControl>> GetAsync(InputControl control, CancellationToken token = default(CancellationToken))
            => await inputControlQueryProcessor.GetAsync(control, token);

        public async Task<IEnumerable<InputControl>> SaveAsync(IEnumerable<InputControl> controls, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var first = controls.First();
                var deleteResult = deleteInputControlQueryProcessor.DeleteAsync(first.CompanyId, first.LoginUserId, first.InputGridTypeId, token);
                var result = new List<InputControl>();
                foreach (var control in controls)
                    result.Add(await addInputControlQueryProcessor.SaveAsync(control, token));

                scope.Complete();

                return result;
            }
        }
    }
}
