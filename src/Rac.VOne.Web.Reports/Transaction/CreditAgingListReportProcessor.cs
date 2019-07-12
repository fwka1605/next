using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Client.Reports;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using System.Threading;

namespace Rac.VOne.Web.Reports
{
    public class CreditAgingListReportProcessor : ICreditAgingListReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly ICreditAgingListProcessor creditAgingListProcessor;

        public CreditAgingListReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            ICreditAgingListProcessor creditAgingListProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.creditAgingListProcessor = creditAgingListProcessor;
        }

        public async Task<byte[]> GetAsync(CreditAgingListSearch option, IProgressNotifier notifier = null, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var loadTask = creditAgingListProcessor.GetAsync(option, notifier, token);

            await Task.WhenAll(companyTask, loadTask);

            var company = companyTask.Result.First();
            var items = loadTask.Result.ToList();

            if (!items.Any()) return null;

            var title = "債権総額管理表";
            var outputName = $"{title}{DateTime.Today:yyyyMMdd}";

            var report = new CreditAgingListReport();
            report.Name = outputName;
            report.SetBasicPageSetting(company.Code, company.Name);

            report.DisplayCustomerCode      = option.DisplayCustomerCode;
            report.ConsiderCustomerGroup    = option.ConsiderCustomerGroup;
            report.RequireDepartmentTotal   = option.RequireDepartmentTotal;
            report.RequireStaffTotal        = option.RequireStaffTotal;
            report.UseMasterStaff           = option.UseMasterStaff;
            report.lblArrivalDueDate1.Text  = option.ArrivalDueDate1;
            report.lblArrivalDueDate2.Text  = option.ArrivalDueDate2;
            report.lblArrivalDueDate3.Text  = option.ArrivalDueDate3;
            report.lblArrivalDueDate4.Text  = option.ArrivalDueDate4;

            report.SetPageDataSetting(items);
            report.Document.Name = outputName;
            report.Run();
            return report.Convert();
        }
    }
}
