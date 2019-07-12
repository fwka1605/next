using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Client.Reports;
using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Reports
{
    public class ArrearagesListReportProcessor : IArrearagesListReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor;
        private readonly IReportSettingQueryProcessor reportSettingQueryProcessor;
        private readonly IArrearagesListQueryProcessor arrearagesListQueryProcessor;
        private const string ReportId = nameof(Rac.VOne.Client.Reports.Settings.PF0401);

        public ArrearagesListReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor,
            IReportSettingQueryProcessor reportSettingQueryProcessor,
            IArrearagesListQueryProcessor arrearagesListQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.columnNameSettingQueryProcessor = columnNameSettingQueryProcessor;
            this.reportSettingQueryProcessor = reportSettingQueryProcessor;
            this.arrearagesListQueryProcessor = arrearagesListQueryProcessor;
        }

        public async Task<byte[]> GetAsync(ArrearagesListSearch option, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var columnTask  = columnNameSettingQueryProcessor.GetAsync(new ColumnNameSetting { CompanyId = option.CompanyId, TableName = nameof(Billing), }, token);
            var settingTask = reportSettingQueryProcessor.GetAsync(option.CompanyId, ReportId, token);
            var loadTask    = arrearagesListQueryProcessor.GetAsync(option, token);

            var tasks = new List<Task>() { companyTask, columnTask, loadTask };
            var requireSetting = !(option.ReportSettings?.Any() ?? false);
            if (requireSetting) tasks.Add(settingTask);

            await Task.WhenAll(tasks);

            var company = companyTask.Result.First();
            var columns = columnTask.Result.ToList();
            var items   = loadTask.Result.ToList();

            if (!items.Any()) return null;

            if (requireSetting) option.ReportSettings = settingTask.Result.ToList();

            GrapeCity.ActiveReports.SectionReport report;
            var title = $"滞留明細一覧表{DateTime.Today:yyyyMMdd}";

            if (option.CustomerSummaryFlag)
            {
                var reportTemp = new ArrearagesCustomerListReport();
                reportTemp.SetBasicPageSetting(company.Code, company.Name);
                reportTemp.SetData(items, option.Precision, option.ReportSettings);

                report = reportTemp;
            }
            else
            {
                var reportTemp = new ArrearagesListReport();
                reportTemp.SetBasicPageSetting(company.Code, company.Name);
                reportTemp.SetData(items, option.Precision, option.ReportSettings, columns);

                report = reportTemp;
            }
            report.Name = title;
            report.Run();

            return report.Convert();
        }
    }
}
