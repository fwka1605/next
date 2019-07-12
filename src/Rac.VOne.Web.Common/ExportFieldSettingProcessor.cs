using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class ExportFieldSettingProcessor: IExportFieldSettingProcessor
    {
        private readonly IExportFieldSettingQueryProcessor exportFieldSettingQueryProcessor;
        private readonly IAddExportFieldSettingQueryProcessor addExportFieldSettingQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ExportFieldSettingProcessor(
            IExportFieldSettingQueryProcessor exportFieldSettingQueryProcessor,
            IAddExportFieldSettingQueryProcessor addExportFieldSettingQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.exportFieldSettingQueryProcessor = exportFieldSettingQueryProcessor;
            this.addExportFieldSettingQueryProcessor = addExportFieldSettingQueryProcessor;
            this.applicationControlQueryProcessor = applicationControlQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<ExportFieldSetting>> GetAsync(ExportFieldSetting setting, CancellationToken token = default(CancellationToken))
        {
            var items = (await exportFieldSettingQueryProcessor.GetAsync(setting, token)).ToList();
            var app = await applicationControlQueryProcessor.GetAsync(setting.CompanyId, token);
            foreach (var item in items)
            {
                if (app.UseForeignCurrency == 0 && item.ColumnName == "CurrencyCode")
                    item.AllowExport = 0;
                if (app.UseReceiptSection == 0
                    && (item.ColumnName == "SectionCode" || item.ColumnName == "SectionName"))
                {
                    item.AllowExport = 0;
                }
            }

            return items;
        }

        public async Task<IEnumerable<ExportFieldSetting>> SaveAsync(IEnumerable<ExportFieldSetting> settings, CancellationToken token = default(CancellationToken))
        {
            var result = new List<ExportFieldSetting>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var setting in settings)
                    result.Add(await addExportFieldSettingQueryProcessor.SaveAsync(setting, token));

                scope.Complete();
            }
            return result;
        }
    }
}
