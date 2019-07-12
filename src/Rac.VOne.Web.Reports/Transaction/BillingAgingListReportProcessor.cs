using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Client.Reports;
using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using static Rac.VOne.Client.Reports.Settings.PF0101;

namespace Rac.VOne.Web.Reports
{
    public class BillingAgingListReportProcessor : IBillingAgingListReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly IReportSettingQueryProcessor reportSettingQueryProcessor;
        private readonly IBillingAgingListProcessor billingAgingListProcessor;


        private const string ReportId = nameof(Rac.VOne.Client.Reports.Settings.PF0101);

        public BillingAgingListReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            IReportSettingQueryProcessor reportSettingQueryProcessor,
            IBillingAgingListProcessor billingAgingListProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.reportSettingQueryProcessor = reportSettingQueryProcessor;
            this.billingAgingListProcessor = billingAgingListProcessor;
        }

        public async Task<byte[]> GetAsync(BillingAgingListSearch option, IProgressNotifier notifier = null, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var appConTask  = applicationControlGetByCompanyQueryProcessor.GetAsync(option.CompanyId, token);
            var settingTask = reportSettingQueryProcessor.GetAsync(option.CompanyId, ReportId, token);
            var loadTask    = billingAgingListProcessor.GetAsync(option, notifier, token);

            await Task.WhenAll(companyTask, appConTask, settingTask, loadTask);

            var company     = companyTask.Result.First();
            var appCon      = appConTask.Result;
            var settings    = settingTask.Result.ToList();
            var items       = loadTask.Result.ToList();

            if (!items.Any()) return null;

            var remianType  = settings.GetReportSetting<ReportAdvanceReceivedType>(BillingRemainType);
            var displayCode = settings.GetReportSetting<ReportDoOrNot>(DisplayCustomerCode) == ReportDoOrNot.Do;

            var title = "請求残高年齢表";
            var outputName = $"{title}{DateTime.Today:yyyyMMdd}";
            GrapeCity.ActiveReports.SectionReport report;
            if (remianType == ReportAdvanceReceivedType.UseMatchingAmount)
            {
                var reportTemp = new BillingAgingListSectionReport1();
                reportTemp.ConsiderCustomerGroup        = option.ConsiderCustomerGroup;
                reportTemp.DisplayCustomerCode          = displayCode;
                reportTemp.RequireStaffSubtotal         = option.RequireStaffSubtotal;
                reportTemp.RequireDepartmentSubtotal    = option.RequireDepartmentSubtotal;

                reportTemp.SetBasicPageSetting(company.Code, company.Name);
                reportTemp.lblMonthlyRemain0.Text       = option.MonthlyRemain0;
                reportTemp.lblMonthlyRemain1.Text       = option.MonthlyRemain1;
                reportTemp.lblMonthlyRemain2.Text       = option.MonthlyRemain2;
                reportTemp.lblMonthlyRemain3.Text       = option.MonthlyRemain3;
                reportTemp.SetData(items, option.Precision, appCon.UseForeignCurrency);
                report = reportTemp;
            }
            else
            {
                var reportTemp = new BillingAgingListSectionReport();
                reportTemp.ConsiderCustomerGroup        = option.ConsiderCustomerGroup;
                reportTemp.DisplayCutsomerCode          = displayCode; // typo
                reportTemp.RequireStaffSubtotal         = option.RequireStaffSubtotal;
                reportTemp.RequireDepartmentSubtotal    = option.RequireDepartmentSubtotal;
                reportTemp.SetBasicPageSetting(company.Code, company.Name);
                reportTemp.lblMonthlyRemain0.Text       = option.MonthlyRemain0;
                reportTemp.lblMonthlyRemain1.Text       = option.MonthlyRemain1;
                reportTemp.lblMonthlyRemain2.Text       = option.MonthlyRemain2;
                reportTemp.lblMonthlyRemain3.Text       = option.MonthlyRemain3;
                reportTemp.SetData(items, option.Precision, appCon.UseForeignCurrency);
                report = reportTemp;
            }
            report.Name = outputName;
            report.Document.Name = outputName;
            report.Run();
            return report.Convert();


        }
    }
}
