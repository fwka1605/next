using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class GridSettingProcessor : IGridSettingProcessor
    {
        private readonly IGridSettingQueryProcessor gridSettingQueryProcessor;
        private readonly IAddGridSettingQueryProcessor addGridSettingQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlByCompanyIdQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public GridSettingProcessor(
            IGridSettingQueryProcessor gridSettingQueryProcessor,
            IAddGridSettingQueryProcessor addGridSettingQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlByCompanyIdQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.gridSettingQueryProcessor = gridSettingQueryProcessor;
            this.addGridSettingQueryProcessor = addGridSettingQueryProcessor;
            this.applicationControlByCompanyIdQueryProcessor = applicationControlByCompanyIdQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        private string[] UseLongTermAdvanceReceived { get; } =
        {
            "Confirm",
            "ContractNumber"
        };

        private string[] UseDiscount { get; } =
        {
            "DiscountAmount1",
            "DiscountAmount2",
            "DiscountAmount3",
            "DiscountAmount4",
            "DiscountAmount5",
            "DiscountAmountSummary",
        };

        private string[] UseForeginCurrency { get; } =
        {
            "CurrencyCode",
        };


        private string[] UseReceiptSection { get; } =
        {
            "SectionCode",
            "SectionName",
        };

        private string[] UseScheduledPayment { get; } =
        {
            "ScheduledPaymentKey",
            "NettingState",
        };

        private string[] UseAccountTransfer { get; } =
        {
            "RequestDate",
            "ResultCode",
        };

        public async Task<IEnumerable<GridSetting>> GetAsync(GridSettingSearch option, CancellationToken token = default(CancellationToken))
        {
            var settings = await gridSettingQueryProcessor.GetAsync(option, token);
            var control = await applicationControlByCompanyIdQueryProcessor.GetAsync(option.CompanyId, token);
            return SetApplicationControlValues(settings, control);
        }


        private IEnumerable<GridSetting> SetApplicationControlValues(IEnumerable<GridSetting> entities, ApplicationControl applicationControl)
        {
            foreach (var setting in entities)
            {
                if (applicationControl.UseLongTermAdvanceReceived == 0 && UseLongTermAdvanceReceived.Contains(setting.ColumnName)
                 || applicationControl.UseDiscount                == 0 && UseDiscount               .Contains(setting.ColumnName)
                 || applicationControl.UseForeignCurrency         == 0 && UseForeginCurrency        .Contains(setting.ColumnName)
                 || applicationControl.UseReceiptSection          == 0 && UseReceiptSection         .Contains(setting.ColumnName)
                 || applicationControl.UseScheduledPayment        == 0 && UseScheduledPayment       .Contains(setting.ColumnName)
                 || applicationControl.UseAccountTransfer         == 0 && UseAccountTransfer        .Contains(setting.ColumnName)
                 )
                    setting.DisplayWidth = 0;
            }
            return entities;
        }
        public async Task<IEnumerable<GridSetting>> SaveAsync(IEnumerable<GridSetting> settings, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new List<GridSetting>();
                var first = settings.First();
                var updateAllUsers = first.AllLoginUser;
                var companyId = first.CompanyId;

                var control = await applicationControlByCompanyIdQueryProcessor.GetAsync(companyId, token);
                foreach (var x in settings)
                    result.Add(await addGridSettingQueryProcessor.SaveAsync(x, updateAllUsers, token));

                scope.Complete();

                return SetApplicationControlValues(result, control);
            }
        }

    }
}
