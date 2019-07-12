using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Web.Common
{
    public class DataMaintenanceProcessor : IDataMaintenanceProcessor
    {
        private readonly IDataMaintenanceQueryProcessor dataMaintenanceQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public DataMaintenanceProcessor(
            IDataMaintenanceQueryProcessor dataMaintenanceQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.dataMaintenanceQueryProcessor = dataMaintenanceQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<int> DeleteDataAsync(DateTime date, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCountB = await dataMaintenanceQueryProcessor.DeleteBillingBeforeAsync(date, token);
                var deleteCountR = await dataMaintenanceQueryProcessor.DeleteReceiptBeforeAsync(date, token);
                scope.Complete();
                return deleteCountB + deleteCountR;
            }
        }
    }
}
