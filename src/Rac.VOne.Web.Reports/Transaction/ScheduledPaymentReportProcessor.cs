using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Client.Reports;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Reports
{
    public class ScheduledPaymentReportProcessor : IScheduledPaymentReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IReportSettingQueryProcessor reportSettingQueryProcessor;
        private readonly IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor;
        private readonly IScheduledPaymentListQueryProcessor scheduledPaymentListQueryProcessor;
        private const string ReportId = nameof(Rac.VOne.Client.Reports.Settings.PC0301);

        public ScheduledPaymentReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IReportSettingQueryProcessor reportSettingQueryProcessor,
            IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor,
            IScheduledPaymentListQueryProcessor scheduledPaymentListQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.reportSettingQueryProcessor = reportSettingQueryProcessor;
            this.columnNameSettingQueryProcessor = columnNameSettingQueryProcessor;
            this.scheduledPaymentListQueryProcessor = scheduledPaymentListQueryProcessor;
        }

        public async Task<byte[]> GetAsync(ScheduledPaymentListSearch option, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId }, token);
            var settingTask = reportSettingQueryProcessor.GetAsync(option.CompanyId, ReportId, token);
            var nameTask = columnNameSettingQueryProcessor.GetAsync(new ColumnNameSetting { CompanyId = option.CompanyId, TableName = nameof(Billing), }, token);
            var loadTask = scheduledPaymentListQueryProcessor.GetAsync(option, token);

            var requireSettingLoad = !(option.ReportSettings?.Any() ?? false);

            var tasks = new List<Task>();
            tasks.Add(companyTask);
            if (requireSettingLoad) tasks.Add(settingTask);
            tasks.Add(nameTask);
            tasks.Add(loadTask);

            await Task.WhenAll(tasks);

            if (requireSettingLoad) option.ReportSettings = settingTask.Result.ToList();
            var company = companyTask.Result.First();
            var naming = nameTask.Result.ToList();
            var items = loadTask.Result.ToList();

            if (!items.Any()) return null;

            GrapeCity.ActiveReports.SectionReport report;

            if (option.CustomerSummaryFlag)
            {
                var reportTemp = new ScheduledPaymentCustomerListSectionReport();

                reportTemp.SetBasicPageSetting(company.Code, company.Name);
                reportTemp.SetData(items, option.Precision, option.ReportSettings);

                report = reportTemp;
            }
            else
            {
                var reportTemp = new ScheduledPaymentListSectionReport();

                reportTemp.SetBasicPageSetting(company.Code, company.Name);
                reportTemp.SetData(items, option.Precision, option.ReportSettings, naming);

                report = reportTemp;
            }

            report.Name = "入金予定明細表" + DateTime.Today.ToString("yyyyMMdd");
            report.Run();

            return report.Convert();
        }
    }
}
