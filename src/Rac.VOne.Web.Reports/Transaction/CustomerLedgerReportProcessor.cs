using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Client.Reports;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Reports
{
    public class CustomerLedgerReportProcessor : ICustomerLedgerReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly ICustomerLedgerProcessor customerLedgerProcessor;


        public CustomerLedgerReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            ICustomerLedgerProcessor customerLedgerProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.customerLedgerProcessor = customerLedgerProcessor;
        }

        public async Task<byte[]> GetAsync(CustomerLedgerSearch option, IProgressNotifier notifier, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var loadTask = customerLedgerProcessor.GetAsync(option, token, notifier);

            await Task.WhenAll(companyTask, loadTask);

            var company = companyTask.Result.First();
            var items = loadTask.Result.ToList();

            if (!items.Any()) return null;

            var report = new CustomerLedgerSectionReport();
            report.SearchOption = option;
            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = "得意先別消込台帳" + DateTime.Today.ToString("yyyyMMdd");
            report.SetData(items, option.Precision);

            report.Run();

            return report.Convert();
        }
    }
}
