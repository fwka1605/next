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
using static Rac.VOne.Client.Reports.Settings.PC0301;

namespace Rac.VOne.Web.Reports
{
    public class BillingSearchReportProcessor : IBillingSearchReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly IGridSettingQueryProcessor gridSettingQueryProcessor;
        private readonly IReportSettingQueryProcessor reportSettingQueryProcessor;
        private readonly IBillingSearchQueryProcessor billingSearchQueryProcessor;

        private const string ReportId = nameof(Rac.VOne.Client.Reports.Settings.PC0301);

        public BillingSearchReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            IGridSettingQueryProcessor gridSettingQueryProcessor,
            IReportSettingQueryProcessor reportSettingQueryProcessor,
            IBillingSearchQueryProcessor billingSearchQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.gridSettingQueryProcessor = gridSettingQueryProcessor;
            this.reportSettingQueryProcessor = reportSettingQueryProcessor;
            this.billingSearchQueryProcessor = billingSearchQueryProcessor;
        }

        public async Task<byte[]> GetAsync(BillingSearch option, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var loadTask = billingSearchQueryProcessor.GetAsync(option, token);
            var reportTask = reportSettingQueryProcessor.GetAsync(option.CompanyId, ReportId, token);
            var gridTask = gridSettingQueryProcessor.GetAsync(new GridSettingSearch { CompanyId = option.CompanyId, GridId = GridId.BillingSearch, }, token);

            await Task.WhenAll(companyTask, gridTask, reportTask, loadTask);

            var company     = companyTask.Result.First();
            var items       = loadTask.Result.ToList();

            if (!items.Any()) return null;

            var settings    = reportTask.Result.ToList();
            var gridDic     = gridTask.Result.ToDictionary(x => x.ColumnName);
            var orders      = items.AsQueryable().OrderBy(x => 0);


            var departmentSubtotal  = settings.GetReportSetting<ReportDoOrNot>(DepartmentSubtotal)  == ReportDoOrNot.Do;
            var staffSubtotal       = settings.GetReportSetting<ReportDoOrNot>(StaffSubtotal)       == ReportDoOrNot.Do;
            var customerSubtotal    = settings.GetReportSetting<ReportDoOrNot>(CustomerSubtotal)    == ReportDoOrNot.Do;
            var unitPrice           = settings.GetReportSetting<ReportUnitPrice>(UnitPrice);
            var outputOrder         = settings.GetReportSetting<ReportOutputOrder>(OutputOrder);
            var orderDateType       = settings.GetReportSetting<ReportBaseDate>(OrderDateType);


            var billReport = new BillingServiceSearchSectionReport();
            billReport.SetBasicPageSetting(company.Code, company.Name);
            billReport.Name = $"請求データ一覧{DateTime.Today:yyyyMMdd}";
            billReport.lblNote1.Text = gridDic["Note1"].ColumnNameJp;

            switch (unitPrice)
            {
                case ReportUnitPrice.Per1000:       billReport.UnitPrice =    1000M;    break;
                case ReportUnitPrice.Per10000:      billReport.UnitPrice =   10000M;    break;
                case ReportUnitPrice.Per1000000:    billReport.UnitPrice = 1000000M;    break;
                default:                            billReport.UnitPrice =       1M;    break;
            }

            if (departmentSubtotal)
                orders = orders.ThenBy(x => x.DepartmentCode);
            else
                billReport.gfDepartmentTotal.Visible = false;

            if (staffSubtotal)
                orders = orders.ThenBy(x => x.StaffCode);
            else
                billReport.gfStaffTotal.Visible = false;

            if (customerSubtotal)
                orders = orders.ThenBy(x => x.CustomerCode);
            else
                billReport.gfCustomerTotal.Visible = false;

            switch (outputOrder)
            {
                case ReportOutputOrder.ByCustomerCode:      orders = orders.ThenBy(x => x.CustomerCode); break;
                case ReportOutputOrder.ByDate:
                    switch (orderDateType)
                    {
                        case ReportBaseDate.BilledAt:       orders = orders.ThenBy(x => x.BilledAt);    break;
                        case ReportBaseDate.SalesAt:        orders = orders.ThenBy(x => x.SalesAt);     break;
                        case ReportBaseDate.ClosingAt:      orders = orders.ThenBy(x => x.ClosingAt);   break;
                        case ReportBaseDate.DueAt:          orders = orders.ThenBy(x => x.DueAt);       break;
                    }
                    break;
                case ReportOutputOrder.ById:                orders = orders.ThenBy(x => x.Id);          break;
            }

            billReport.DataSource = orders.ToList();
            billReport.Run();

            return billReport.Convert();
        }
    }
}
