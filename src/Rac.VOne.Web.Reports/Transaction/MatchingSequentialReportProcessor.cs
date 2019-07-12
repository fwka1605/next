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
    public class MatchingSequentialReportProcessor : IMatchingSequentialReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        public MatchingSequentialReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
        }

        public async Task<byte[]> GetAsync(MatchingSequentialReportSource source, CancellationToken token = default(CancellationToken))
        {
            if (!(source.Items?.Any() ?? false)) return null;

            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = source.CompanyId, }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(source.CompanyId, token);

            await Task.WhenAll(companyTask, appConTask);
            
            var company = companyTask.Result.First();
            var appCon = appConTask.Result;

            GrapeCity.ActiveReports.SectionReport report;
            if (source.PriorReceipt)
            {
                var reportTemp = new MatchingSequentialReceiptBillingSectionReport();
                reportTemp.SetBasicPageSetting(company.Code, company.Name);
                reportTemp.SetPageDataSetting(source.Items, true, appCon.UseForeignCurrency, source.Precision, null);

                report = reportTemp;
            }
            else
            {
                var reportTemp = new MatchingSequentialSectionReport();
                reportTemp.SetBasicPageSetting(company.Code, company.Name);
                reportTemp.SetPageDataSetting(source.Items, true, appCon.UseForeignCurrency, source.Precision, null);

                report = reportTemp;
            }

            report.Name = "一括消込チェックリスト" + DateTime.Today.ToString("yyyyMMdd");
            report.Run();

            return report.Convert();
        }
    }
}
