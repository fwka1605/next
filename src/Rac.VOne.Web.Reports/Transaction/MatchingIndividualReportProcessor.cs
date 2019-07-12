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
    public class MatchingIndividualReportProcessor : IMatchingIndividualReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        public MatchingIndividualReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
        }

        public async Task<byte[]> GetAsync(MatchingIndividualReportSource source, CancellationToken token = default(CancellationToken))
        {
            if (!(source.Items?.Any() ?? false)) return null;

            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = source.CompanyId, }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(source.CompanyId, token);

            await Task.WhenAll(companyTask, appConTask);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;

            var moneyFormat = "#,0";

            GrapeCity.ActiveReports.SectionReport report;

            if (source.PriorReceipt)
            {
                var reportTemp = new MatchingIndividualReceiptBillingSectionReport();
                reportTemp.SetBasicPageSetting(company.Code, company.Name);
                reportTemp.SetAmountSetting(source.Precision,
                    source.BillingTaxDiff.ToString(moneyFormat),
                    source.ReceiptTaxDiff.ToString(moneyFormat),
                    source.BankFee.ToString(moneyFormat),
                    source.DiscountAmount.ToString(moneyFormat),
                    appCon.UseDiscount);
                reportTemp.SetPageDataSetting(source.Items, source.BillingGridSettings, source.ReceiptGridSettings);
                reportTemp.shpBilling.BackColor = System.Drawing.Color.LightCyan;

                report = reportTemp;
            }
            else
            {
                var reportTemp = new MatchingIndividualSectionReport();
                reportTemp.SetBasicPageSetting(company.Code, company.Name);
                reportTemp.SetAmountSetting(source.Precision,
                    source.BillingTaxDiff.ToString(moneyFormat),
                    source.ReceiptTaxDiff.ToString(moneyFormat),
                    source.BankFee.ToString(moneyFormat),
                    source.DiscountAmount.ToString(moneyFormat),
                    appCon.UseDiscount);
                reportTemp.SetPageDataSetting(source.Items, source.BillingGridSettings, source.ReceiptGridSettings);
                reportTemp.shpBilling.BackColor = System.Drawing.Color.LightCyan;

                report = reportTemp;
            }

            report.Name = "個別消込画面" + DateTime.Today.ToString("yyyyMMdd");
            report.Run();

            return report.Convert();
        }
    }
}
