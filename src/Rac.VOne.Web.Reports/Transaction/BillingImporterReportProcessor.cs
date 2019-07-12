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
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Reports
{
    public class BillingImporterReportProcessor : IBillingImporterReportProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyQueryProcessor;
        private readonly IColumnNameSettingQueryProcessor columnNameSettingQueryProcessor;
        private readonly IImportDataDetailQueryProcessor importDataDetailQueryProcessor;

        private readonly MessagePackSerializer<BillingImport> serializer;

        public BillingImporterReportProcessor(
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
            serializer = MessagePackSerializer.Get<BillingImport>();
        }

        public async Task<byte[]> GetAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken))
        {
            var companyId = source.CompanyId;
            var importDataId = source.ImportDataId.Value;
            var objectType = source.IsValidData ? 0 : 1;

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

            var useForeignCurrency = appCon.UseForeignCurrency == 1;

            var report = new BillingImporterSectionReport();

            var title = $"請求フリーインポーター　取込{(source.IsValidData ? "可能" : "不可能")}データ一覧{DateTime.Now:yyyyMMdd}";
            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = title;
            report.SetData(items, title, useForeignCurrency, note1);
            report.Run();

            return report.Convert();
        }

        public async Task<byte[]> GetCustomersAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken))
        {
            var companyId = source.CompanyId;
            var importDataId = source.ImportDataId.Value;

            var companyTask = companyQueryProcessor.GetAsync(new CompanySearch { Id = companyId, }, token);
            var appConTask = applicationControlGetByCompanyQueryProcessor.GetAsync(companyId, token);
            var columnNameTask = columnNameSettingQueryProcessor.GetAsync(new ColumnNameSetting
            {
                CompanyId = companyId,
                TableName = nameof(Billing),
                ColumnName = nameof(Billing.Note1),
            }, token);
            var itemsTask = importDataDetailQueryProcessor.GetAsync(importDataId, token: token);

            await Task.WhenAll(companyTask, appConTask, columnNameTask, itemsTask);

            var company = companyTask.Result.First();
            var appCon = appConTask.Result;
            var note1 = columnNameTask.Result.FirstOrDefault()?.DisplayColumnName ?? "備考";
            var details = itemsTask.Result.ToArray();

            var items = details.Select(x => serializer.UnpackSingleObject(x.RecordItem))
                .ToLookupCustomerCode()
                .ToList();

            var useForeignCurrency = appCon.UseForeignCurrency == 1;

            var report = new BillingImporterNewCustomerSectionReport();

            var title = $"請求フリーインポーター　新規得意先一覧{DateTime.Now:yyyyMMdd}";
            report.SetBasicPageSetting(company.Code, company.Name);
            report.Name = title;
            report.SetData(items, title, useForeignCurrency, note1);
            report.Run();

            return report.Convert();
        }
    }
}
