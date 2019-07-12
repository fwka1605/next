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
    public class BillingScheduledPaymentReportProcessor : IBillingScheduledPaymentReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IBillingSearchQueryProcessor billingSearchQueryProcessor;
        public BillingScheduledPaymentReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IBillingSearchQueryProcessor billingSearchQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.billingSearchQueryProcessor = billingSearchQueryProcessor;
        }

        public async Task<byte[]> GetAsync(BillingSearch option, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var loadTask = billingSearchQueryProcessor.GetAsync(option, token);

            await Task.WhenAll(companyTask, loadTask);

            var precision = 0; // currency related precision
            var company = companyTask.Result.First();
            var items = loadTask.Result.ToList();

            if (!items.Any()) return null;

            var report = new PaymentScheduleInputSectionReport();
            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = "入金予定リスト" + DateTime.Today.ToString("yyyyMMdd");
            report.SetData(items, precision);
            report.Run();

            return report.Convert();
        }
    }
}
