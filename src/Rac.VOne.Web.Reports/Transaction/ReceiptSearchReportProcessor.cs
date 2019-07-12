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
    public class ReceiptSearchReportProcessor : IReceiptSearchReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly IGridSettingQueryProcessor gridSettingQueryProcessor;
        private readonly IReceiptSearchQueryProcessor receiptSearchQueryProcessor;

        public ReceiptSearchReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            IGridSettingQueryProcessor gridSettingQueryProcessor,
            IReceiptSearchQueryProcessor receiptSearchQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.gridSettingQueryProcessor = gridSettingQueryProcessor;
            this.receiptSearchQueryProcessor = receiptSearchQueryProcessor;
        }

        public async Task<byte[]> GetAsync(ReceiptSearch option, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(option.CompanyId, token);
            var gridTask = gridSettingQueryProcessor.GetAsync(new GridSettingSearch { CompanyId = option.CompanyId, GridId = GridId.ReceiptSearch, }, token);
            var loadTask = receiptSearchQueryProcessor.GetAsync(option, token);

            await Task.WhenAll(companyTask, appConTask, gridTask, loadTask);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;
            var gridSettings = gridTask.Result.ToList();
            var items = loadTask.Result.ToList();

            if (!items.Any()) return null;

            var useSection = appCon.UseReceiptSection == 1;
            var precition = 0;

            var report = new ReceiptSearchSectionReport();

            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = "入金データ一覧" + DateTime.Today.ToString("yyyyMMdd");
            report.SetData(items, precition, useSection, option.DeleteFlg == 1, gridSettings);

            report.Run();

            return report.Convert();
        }
    }
}
