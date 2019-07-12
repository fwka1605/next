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
    public class BillingAgingListDetailReportProcessor : IBillingAgingListDetailReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor;
        private readonly IBillingAgingListQueryProcessor billingAgingListQueryProcessor;

        public BillingAgingListDetailReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor,
            IBillingAgingListQueryProcessor billingAgingListQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.columnNameSettingQueryProcessor = columnNameSettingQueryProcessor;
            this.billingAgingListQueryProcessor = billingAgingListQueryProcessor;
        }

        public async Task<byte[]> GetAsync(BillingAgingListSearch option, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = option.CompanyId, }, token);
            var columnTask = columnNameSettingQueryProcessor.GetAsync(new ColumnNameSetting {
                CompanyId       = option.CompanyId,
                TableName       = nameof(Billing),
                ColumnName      = nameof(Billing.Note1), }, token);
            var loadTask = billingAgingListQueryProcessor.GetDetailsAsync(option, token);

            await Task.WhenAll(companyTask, columnTask, loadTask);

            var items = loadTask.Result.ToList();

            if (!items.Any()) return null;

            var company = companyTask.Result.First();
            var column = columnTask.Result.First();
            var title = $"請求残高年齢表（明細）{ DateTime.Today:yyyyMMdd}";

            var report = new BillingAgingListDetailSectionReport();

            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = title;
            report.SetData(items, option.Precision, column);

            report.Run();

            return report.Convert();
        }
    }
}
