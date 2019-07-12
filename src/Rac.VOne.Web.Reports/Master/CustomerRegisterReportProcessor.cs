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
    public class CustomerRegisterReportProcessor : ICustomerRegisterReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly ICustomerQueryProcessor customerQueryProcessor;
        public CustomerRegisterReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            ICustomerQueryProcessor customerQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.customerQueryProcessor = customerQueryProcessor;
        }

        public async Task<byte[]> GetAsync(CustomerSearch option, CancellationToken token = default(CancellationToken))
        {

            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(option.CompanyId.Value, token);
            var masterLoadTask = customerQueryProcessor.GetAsync(option, token);

            await Task.WhenAll(companyTask, appConTask, masterLoadTask);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;
            var items = masterLoadTask.Result.ToList();

            if (!items.Any()) return null;

            var report = new CustomerAccountSectionReport();
            report.Name = "得意先台帳" + DateTime.Now.ToString("yyyyMMdd");
            report.SetBasicPageSetting(company.Code, company.Name);
            report.SetData(items,
                appCon.UsePublishInvoice    == 1,
                appCon.UseReminder          == 1);

            report.Run();

            return report.Convert();
        }
    }
}
