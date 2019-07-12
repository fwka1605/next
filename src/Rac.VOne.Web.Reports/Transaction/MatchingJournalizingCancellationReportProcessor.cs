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
    public class MatchingJournalizingCancellationReportProcessor : IMatchingJournalizingCancellationReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        public MatchingJournalizingCancellationReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
        }

        public async Task<byte[]> GetAsync(MatchingJournalizingCancellationReportSource source, CancellationToken token = default(CancellationToken))
        {
            if (!(source.Items?.Any() ?? false)) return null;

            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = source.CompanyId, }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(source.CompanyId, token);

            await Task.WhenAll(companyTask, appConTask);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;

            var report = new MatchingJournalizingCancellationReport();
            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = "消込仕訳取消リスト" + DateTime.Today.ToString("yyyyMMdd");
            report.SetData(source.Items, source.Precision, appCon.UseForeignCurrency == 1);

            report.Run();
            return report.Convert();
        }
    }
}
