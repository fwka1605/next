using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Client.Reports;
using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Reports
{
    public class BillingJournalizingReportProcessor : IBillingJournalizingReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly IBillingJournalizingQueryProcessor billingJournalizingQueryProcessor;

        public BillingJournalizingReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            IBillingJournalizingQueryProcessor billingJournalizingQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.columnNameSettingQueryProcessor = columnNameSettingQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.billingJournalizingQueryProcessor = billingJournalizingQueryProcessor;
        }

        public async Task<byte[]> GetAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(option.CompanyId, token);
            var nameTask = columnNameSettingQueryProcessor.GetAsync(new ColumnNameSetting { CompanyId = option.CompanyId, TableName = nameof(Receipt), }, token);
            var loadTask = billingJournalizingQueryProcessor.ExtractAsync(option, token);

            await Task.WhenAll(companyTask, appConTask, nameTask, loadTask);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;
            var naming = nameTask.Result.ToList();
            var items = loadTask.Result.ToList();

            if (!items.Any()) return null;

            var report = new BillingJournalizingSectionReport();

            var reoutput = option.OutputAt?.Any() ?? false;
            var fileName = $"{(reoutput ? "再出力" : "")}請求仕訳{DateTime.Now:yyyyMMdd_HHmmss}";

            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = fileName;
            report.SetData(items, appCon.UseForeignCurrency == 1, option.Precision, naming);

            report.Run();

            return report.Convert();
        }
    }
}
