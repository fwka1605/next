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
    public class CollectionScheduleListReportProcessor : ICollectionScheduleListReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly ICollectionScheduleProcessor collectionScheduleProcessor;

        public CollectionScheduleListReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            ICollectionScheduleProcessor collectionScheduleProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.collectionScheduleProcessor = collectionScheduleProcessor;
        }

        public async Task<byte[]> GetAsync(CollectionScheduleSearch option, IProgressNotifier notifier, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var loadTask = collectionScheduleProcessor.GetAsync(option, token, notifier);

            await Task.WhenAll(companyTask, loadTask);

            var company = companyTask.Result.First();
            var items = loadTask.Result.ToList();

            if (!items.Any()) return null;

            var report = new CollectionScheduleSectionReport();
            var title = "回収予定表";
            var reportName = title + DateTime.Today.ToString("yyyyMMdd");
            report.GroupByDepartment                = option.DisplayDepartment;
            report.NewPagePerDepartment             = option.NewPagePerDepartment;
            report.NewPagePerStaff                  = option.NewPagePerStaff;
            report.lblUncollectedAmountLast.Text    = option.UncollectedAmountLast;
            report.lblUncollectAmount0.Text         = option.UncollectedAmount0;
            report.lblUncollectAmount1.Text         = option.UncollectedAmount1;
            report.lblUncollectAmount2.Text         = option.UncollectedAmount2;
            report.lblUncollectAmount3.Text         = option.UncollectedAmount3;
            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = reportName;

            report.SetData(items);

            report.Run();
            return report.Convert();
        }
    }
}
