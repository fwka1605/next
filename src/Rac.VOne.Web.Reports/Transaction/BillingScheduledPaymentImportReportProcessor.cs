using MsgPack.Serialization;
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
    public class BillingScheduledPaymentImportReportProcessor : IBillingScheduledPaymentImportReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor;
        private readonly IImportDataDetailQueryProcessor importDataDetailQueryProcessor;

        private readonly MessagePackSerializer<Models.Files.PaymentSchedule> serializer;

        public BillingScheduledPaymentImportReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor,
            IImportDataDetailQueryProcessor importDataDetailQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.columnNameSettingQueryProcessor = columnNameSettingQueryProcessor;
            this.importDataDetailQueryProcessor = importDataDetailQueryProcessor;

            serializer = MessagePackSerializer.Get<Models.Files.PaymentSchedule>();
        }

        public async Task<byte[]> GetAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken))
        {
            var companyId = source.CompanyId;
            var importDataId = source.ImportDataId.Value;
            var objectType = source.IsValidData ? 1 : 2;

            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId, }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(companyId, token);
            var columnNameTask = columnNameSettingQueryProcessor.GetAsync(new ColumnNameSetting
            {
                CompanyId   = companyId,
                TableName   = nameof(Billing),
                ColumnName  = nameof(Billing.Note1),
            }, token);
            var itemsTask = importDataDetailQueryProcessor.GetAsync(importDataId, objectType, token);

            await Task.WhenAll(companyTask, appConTask, columnNameTask, itemsTask);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;
            var note1 = columnNameTask.Result.FirstOrDefault()?.DisplayColumnName ?? "備考";
            var details = itemsTask.Result.ToArray();
            var items = details.Select(x => serializer.UnpackSingleObject(x.RecordItem)).ToList();

            if (!items.Any()) return null;

            var report = new PaymentScheduleImporterSectionReport();

            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = $"入金予定フリーインポーター　取込{(source.IsValidData ? "可能" : "不可能")}データ一覧{DateTime.Today:yyyyMMdd}";
            report.SetData(items, source.IsValidData, appCon.UseForeignCurrency, note1);
            report.Run();

            return report.Convert();
        }
    }
}
