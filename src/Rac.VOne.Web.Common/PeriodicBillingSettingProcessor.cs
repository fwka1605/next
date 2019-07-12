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
    public class PeriodicBillingSettingProcessor :
        IPeriodicBillingSettingProcessor
    {
        private readonly IPeriodicBillingSettingQueryProcessor periodicBillingSettingQueryProcessor;
        private readonly IPeriodicBillingSettingDetailQueryProcessor periodicBillingSettingDetailQueryProcessor;
        private readonly IAddPeriodicBillingSettingQueryProcessor addPeriodicBillingSettingQueryProcessor;
        private readonly IAddPeriodicBillingSettingDetailQueryProcessor addPeriodicBillingSettingDetailQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<PeriodicBillingSetting> deletePeriodicBillingSettingQueryProcessor;
        private readonly IDeletePeriodicBillingSettingDetailQueryProcessor deletePeriodicBillingSettingDetailQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public PeriodicBillingSettingProcessor(
            IPeriodicBillingSettingQueryProcessor periodicBillingSettingQueryProcessor,
            IAddPeriodicBillingSettingQueryProcessor addPeriodicBillingSettingQueryProcessor,
            IPeriodicBillingSettingDetailQueryProcessor periodicBillingSettingDetailQueryProcessor,
            IAddPeriodicBillingSettingDetailQueryProcessor addPeriodicBillingSettingDetailQueryProcessor,
            IDeleteTransactionQueryProcessor<PeriodicBillingSetting> deletePeriodicBillingSettingQueryProcessor,
            IDeletePeriodicBillingSettingDetailQueryProcessor deletePeriodicBillingSettingDetailQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.periodicBillingSettingQueryProcessor = periodicBillingSettingQueryProcessor;
            this.addPeriodicBillingSettingQueryProcessor = addPeriodicBillingSettingQueryProcessor;
            this.periodicBillingSettingDetailQueryProcessor = periodicBillingSettingDetailQueryProcessor;
            this.addPeriodicBillingSettingDetailQueryProcessor = addPeriodicBillingSettingDetailQueryProcessor;
            this.deletePeriodicBillingSettingQueryProcessor = deletePeriodicBillingSettingQueryProcessor;
            this.deletePeriodicBillingSettingDetailQueryProcessor = deletePeriodicBillingSettingDetailQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }


        public async Task<IEnumerable<PeriodicBillingSetting>> GetAsync(PeriodicBillingSettingSearch option, CancellationToken token = default(CancellationToken))
        {
            // データ量的に perfomance で問題になる場合は処理を分ける
            var settings = (await periodicBillingSettingQueryProcessor.GetAsync(option, token)).ToArray();
            option.Ids = settings.Select(x => x.Id).Distinct().ToArray();

            var details = (await periodicBillingSettingDetailQueryProcessor.GetAsync(option, token))
                .GroupBy(x => x.PeriodicBillingSettingId)
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach (var setting in settings)
                setting.Details = details[setting.Id];
            return settings;
        }

        public async Task<PeriodicBillingSetting> SaveAsync(PeriodicBillingSetting setting, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = await addPeriodicBillingSettingQueryProcessor.SaveAsync(setting, token);
                if (setting.Id != 0L)
                    await deletePeriodicBillingSettingDetailQueryProcessor.DeleteAsync(setting.Id, token);
                foreach (var detail in setting.Details.Select((detail, index) => {
                    detail.DisplayOrder = index + 1;
                    return detail;
                }))
                {
                    detail.PeriodicBillingSettingId = result.Id;
                    result.Details.Add(await addPeriodicBillingSettingDetailQueryProcessor.SaveAsync(detail, token));
                }
                scope.Complete();

                return result;
            }
        }

        public async Task<int> DeleteAsync(long id, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                await deletePeriodicBillingSettingDetailQueryProcessor.DeleteAsync(id, token);
                var result = await deletePeriodicBillingSettingQueryProcessor.DeleteAsync(id, token);
                scope.Complete();
                return result;
            }
        }

    }
}
