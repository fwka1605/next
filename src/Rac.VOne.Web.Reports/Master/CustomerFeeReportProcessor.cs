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
    public class CustomerFeeReportProcessor : ICustomerFeeReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly ICustomerFeeQueryProcessor customerFeeQueryProcessor;

        public CustomerFeeReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            ICustomerFeeQueryProcessor customerFeeQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.customerFeeQueryProcessor = customerFeeQueryProcessor;
        }

        public async Task<byte[]> GetAsync(CustomerFeeSearch option, CancellationToken token = default(CancellationToken))
        {

            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var appControlTask = applicationControlGetByCompanyQueryProcessor.GetAsync(option.CompanyId.Value, token);
            var masterLoadTask = customerFeeQueryProcessor.GetAsync(option, token);

            await Task.WhenAll(companyTask, appControlTask, masterLoadTask);

            var company     = companyTask.Result.First();
            var appCon      = appControlTask.Result;
            var items       = masterLoadTask.Result.ToList();
            var precision   = appCon.UseForeignCurrency == 0 ? 0 : items.Max(x => x.CurrencyPrecision);

            if (!items.Any()) return null;

            var report = new CustomerFeeSectionReport();
            report.Name = "登録手数料一覧" + DateTime.Now.ToString("yyyyMMdd");
            report.SetBasicPageSetting(company.Code, company.Name);
            report.SetData(items, appCon.UseForeignCurrency);
            report.Precision = precision;
            report.Run();

            return report.Convert();
        }
    }
}
