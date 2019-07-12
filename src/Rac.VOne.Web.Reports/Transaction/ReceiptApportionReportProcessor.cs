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
    public class ReceiptApportionReportProcessor : IReceiptApportionReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;

        public ReceiptApportionReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.columnNameSettingQueryProcessor = columnNameSettingQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
        }

        public async Task<byte[]> GetAsync(ReceiptApportionForReportSource source, CancellationToken token = default(CancellationToken))
        {
            if (!(source.Apportions?.Any() ?? false)) return null;

            var companyId = source.CompanyId;

            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(companyId, token);
            var nameTask = columnNameSettingQueryProcessor.GetAsync(new ColumnNameSetting { CompanyId = companyId, TableName = nameof(Receipt), }, token);

            await Task.WhenAll(companyTask, appConTask, nameTask);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;
            var naming = nameTask.Result.ToList();
            var precision = 0;

            var report = new ReceiptApportionSectionReport();

            report.Name = "入金データ振分リスト" + DateTime.Now.ToString("yyyyMMdd");
            report.SetBasicPageSetting(company.Code, company.Name);
            report.SetHeaderSetting(source.Header, source.CategoryName);
            report.SetPageDataSetting(source.Apportions, appCon.UseReceiptSection, precision);

            report.Run();

            return report.Convert();
        }

    }
}
