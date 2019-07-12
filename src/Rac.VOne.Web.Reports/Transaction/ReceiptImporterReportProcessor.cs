using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Client.Reports;
using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Reports
{
    public class ReceiptImporterReportProcessor : IReceiptImporterReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly IImportDataDetailQueryProcessor importDataDetailQueryProcessor;

        private readonly MessagePackSerializer<ReceiptInput> serializer;

        public ReceiptImporterReportProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor,
            IImportDataDetailQueryProcessor importDataDetailQueryProcessor
            )
        {
            this.companyQueryProcessor = companyQueryProcessor;
            this.columnNameSettingQueryProcessor = columnNameSettingQueryProcessor;
            this.applicationControlGetByCompanyQueryProcessor = applicationControlGetByCompanyQueryProcessor;
            this.importDataDetailQueryProcessor = importDataDetailQueryProcessor;

            serializer = MessagePackSerializer.Get<ReceiptInput>();
        }

        public async Task<byte[]> GetAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken))
        {
            var companyId = source.CompanyId;
            var importDataId = source.ImportDataId.Value;
            var objectType = source.IsValidData ? 0 : 1;

            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(companyId, token);
            var nameTask = columnNameSettingQueryProcessor.GetAsync(new ColumnNameSetting
            {
                CompanyId   = companyId,
                TableName   = nameof(Receipt),
                ColumnName  = nameof(Receipt.Note1),
            }, token);
            var itemsTask = importDataDetailQueryProcessor.GetAsync(importDataId, objectType, token);

            await Task.WhenAll(companyTask, appConTask, nameTask, itemsTask);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;
            var note1 = nameTask.Result.FirstOrDefault()?.DisplayColumnName ?? "備考";

            var items = itemsTask.Result.Select(x => serializer.UnpackSingleObject(x.RecordItem)).ToList();

            if (!items.Any()) return null;

            var report = new ReceiptImporterSectionReport();
            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = $"入金フリーインポーター　取込{(source.IsValidData ? "可能" : "不可能")}データ一覧{DateTime.Today:yyyyMMdd}";
            report.SetData(items, source.IsValidData, appCon.UseForeignCurrency, note1);

            report.Run();

            return report.Convert();
        }
    }
}
